using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Stock : UserControl
    {
        private readonly string _cs = GetConn.ConnectionString;
        private DataTable _dtQueries;
        private PermissoesPerfil permissoes;
        EfeitoUI M = new EfeitoUI();
        private Font fonteNegrito = new Font("Roboto", 11.25F, FontStyle.Bold);
        string Funcao;

        public Stock(PermissoesPerfil _permissoes, string _Funcao)
        {
            InitializeComponent();
            permissoes = _permissoes;
            Funcao = _Funcao;
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            // Ativa a performance turbo que criaste no Organizer
            HelperPerformance.AtivarDoubleBufferRecursivo(this);
            CarregarCombo();
            TouchScrollHelper.AtivarScrollPorArrasto(dgvStock);
            GestorTema.AplicarEstilos(this);

        }

        private async void CarregarCombo()
        {
            try
            {
                DataTable dtTemp = new DataTable();

                // 1. Busca os dados em background (SQL fora da UI Thread)
                await Task.Run(() =>
                {
                    using (var con = new SqlConnection(_cs))
                    using (var cmd = new SqlCommand(@"SELECT ID, Nome, [Query] FROM dbo.[Query] ORDER BY ID;", con))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtTemp);
                    }
                });

                // 2. Adiciona a linha de gestão se tiver permissões
                if (permissoes.PodeAlterarDefinicoes)
                {
                    DataRow rowGerir = dtTemp.NewRow();
                    rowGerir["ID"] = -1;
                    rowGerir["Nome"] = "- Gerir Filtros -";
                    rowGerir["Query"] = "ACTION_GERIR";
                    dtTemp.Rows.Add(rowGerir);
                }

                //3. Adiciona a linha "Stock Usado" se tiver permissões
                if (permissoes.PodeVerUsados)
                {
                    DataRow rowUsado = dtTemp.NewRow();
                    rowUsado["ID"] = -2;
                    rowUsado["Nome"] = "- Stock Usado -";
                    rowUsado["Query"] = "ACTION_USADO";
                    dtTemp.Rows.Add(rowUsado);
                }

                _dtQueries = dtTemp;

                // 4. Vinculação de dados (UI Thread)
                // Definimos os membros ANTES do DataSource para evitar erros de cast
                cmbVisoes.DisplayMember = "Nome";
                cmbVisoes.ValueMember = "Query";

                // Atribuir o DataSource dispara o SelectedIndexChanged automaticamente
                cmbVisoes.DataSource = _dtQueries;

                // 5. Lógica para encontrar o ID 1 e expor no Load
                if (_dtQueries.Rows.Count > 0)
                {
                    // Lemos o ID que guardaste nas definições do projeto
                    int idDefault = Properties.Settings.Default.VisaoStockDefault;

                    DataRow[] linhasIdDefault = _dtQueries.Select($"ID = {idDefault}");

                    if (linhasIdDefault.Length > 0)
                    {
                        cmbVisoes.SelectedValue = linhasIdDefault[0]["Query"].ToString();
                    }
                    else
                    {
                        // Se o ID das definições não for encontrado, volta para o primeiro da lista
                        cmbVisoes.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro a carregar visões: " + ex.Message, "Erro");
            }
        }

        private void cmbVisoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Proteção essencial para o primeiro carregamento
            if (cmbVisoes.SelectedValue == null || cmbVisoes.SelectedValue is DataRowView)
                return;

            string sql = cmbVisoes.SelectedValue.ToString();

            if (sql == "ACTION_GERIR")
            {
                // 1. Descobrir quem é o "Pai" (O Form principal que está aberto)
                Form pai = this.FindForm();

                // 2. Criar a Sombra (Overlay)
                using (Form overlay = new Form())
                {
                    // Configuração da sombra
                    overlay.StartPosition = FormStartPosition.Manual;
                    overlay.FormBorderStyle = FormBorderStyle.None;
                    overlay.Opacity = 0.50d;
                    overlay.BackColor = Color.Black;
                    overlay.ShowInTaskbar = false;

                    if (pai != null)
                    {
                        overlay.Location = pai.Location;
                        overlay.Size = pai.Size;
                        overlay.Show(pai);
                    }
                    else
                    {
                        overlay.WindowState = FormWindowState.Maximized;
                        overlay.Show();
                    }

                    // 3. Abrir o teu FormGestaoDeFiltros POR CIMA da sombra
                    using (PEPIDI.FormsSecundarios.FormGestaoDeFiltros frm = new PEPIDI.FormsSecundarios.FormGestaoDeFiltros())
                    {
                        // Passamos o 'overlay' para o ShowDialog, para ele saber que tem de ficar colado à sombra
                        frm.ShowDialog(overlay);
                    }

                    // 4. Fechar a sombra logo a seguir ao FormGestaoDeFiltros ser fechado
                    overlay.Close();
                }

                // 5. ATUALIZAR A COMBOBOX DEPOIS DA GESTÃO FECHAR!
                CarregarCombo();

                // 6. Abortar a missão para não tentar enviar o "ACTION_GERIR" para a grelha
                return;
            }

            if (sql == "ACTION_USADO")
            {
                // Se for a ação "Stock Usado", aplica a lógica correspondente
                AplicarQueryNaDgv("SELECT E.Codigo, E.Modelo, E.Tamanho, ISNULL(STRING_AGG(F.Nome, ' | '), 'Sem Função') AS NomeFuncao, ISNULL(STRING_AGG(F.CorHex, ','), '#808080') AS CorFuncao, S.Quant FROM EPI E LEFT JOIN AcessoFuncoes AF ON E.Acesso = AF.AcessoID LEFT JOIN Stock S ON E.Codigo = S.Codigo LEFT JOIN Funcoes F ON AF.FuncaoID = F.ID WHERE S.Estado = 2 GROUP BY E.Modelo, E.Tamanho, S.Quant, E.Acesso, E.Codigo", Funcao);
                return;
            }

            // Se for uma Query normal, aplica na grelha!
            AplicarQueryNaDgv(sql, Funcao);
        }

        private void AplicarQueryNaDgv(string sql, string funcao)
        {
            dgvStock.DataSource = null; // Limpa o DataSource antes de aplicar a nova query
            try
            {
                using (var con = new SqlConnection(_cs))
                using (var da = new SqlDataAdapter(sql, con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    dgvStock.AutoGenerateColumns = true;
                    dgvStock.DataSource = dt;

                    if(funcao == "Programador")
                    {
                        if (dgvStock.Columns.Contains("Codigo"))
                        {
                            dgvStock.Columns["Codigo"].Visible =true; // Mostra a coluna ID para os Programadores
                        }
                    }
                    else
                    {
                        if (dgvStock.Columns.Contains("Codigo"))
                        {
                            dgvStock.Columns["Codigo"].Visible = false; // Esconde a coluna ID para os outros
                        }
                    }

                    // --- LIGAÇÃO À CLASSE PEPIDI ---
                    // O BadgeColumnName tem de ser igual ao TÍTULO da coluna (HeaderText)
                    if (dgvStock.Columns.Contains("NomeFuncao"))
                    {
                        // Mudamos o TÍTULO (HeaderText) para "Departamento"
                        dgvStock.Columns["NomeFuncao"].HeaderText = "Departamento";

                        // Escondemos o texto para a pílula aparecer limpa
                        dgvStock.Columns["NomeFuncao"].DefaultCellStyle.ForeColor = Color.Transparent;
                        dgvStock.Columns["NomeFuncao"].DefaultCellStyle.SelectionForeColor = Color.Transparent;
                        dgvStock.BadgeColumnName = "Departamento";

                        // O BadgeColorColumnName tem de ser igual ao NOME da coluna que traz o HEX (#RRGGBB)
                        dgvStock.BadgeColorColumnName = "CorFuncao";
                    }

                    if (dgvStock.Columns.Contains("Departamento"))
                    {
                        // TRUQUE: Tornamos o texto transparente para a pílula aparecer
                        dgvStock.Columns["Departamento"].DefaultCellStyle.ForeColor = Color.Transparent;
                        dgvStock.Columns["Departamento"].DefaultCellStyle.SelectionForeColor = Color.Transparent;
                        dgvStock.Columns["Departamento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    // Esconde a coluna do código de cor para não aparecer texto feio
                    if (dgvStock.Columns.Contains("CorFuncao")) dgvStock.Columns["CorFuncao"].Visible = false;
                }
            }
            catch (Exception ex) { M.AbrirMensagem(ex.Message, "Erro"); }
            // ==========================================
            // BALANCEAMENTO DAS COLUNAS (55, 15, 15, 15)
            // ==========================================

            // 1. OBRIGATÓRIO: Dizemos à grelha para esticar as colunas para ocuparem 100% da largura total
            dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.Columns["Modelo"].FillWeight = 55; // Modelo / Descrição
            dgvStock.Columns["Tamanho"].FillWeight = 15; // Tamanho
            dgvStock.Columns["Tamanho"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // Centraliza a coluna do Tamanho
            //dgvStock.Columns["NomeFuncao"].FillWeight = 15; // Departamento (Onde aparece o Vários)
            if (dgvStock.Columns.Contains("ID"))
            {
                dgvStock.Columns["ID"].FillWeight = 7; // Quantidade
                dgvStock.Columns["Quant"].FillWeight = 8; // Quantidade
            }
            else
            {
                dgvStock.Columns["Quant"].FillWeight = 15; // Quantidade
            }
        }

        private void dgvStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 1. O Nome interno da coluna que vem da BD (NomeFuncao)
            if (dgvStock.Columns[e.ColumnIndex].Name == "NomeFuncao" && e.Value != null)
            {
                string textoOriginal = e.Value.ToString().Trim();

                // 2. Regra do "Vários" (Se tiver o separador | ou for muito grande)
                if (textoOriginal.Contains("|") || textoOriginal.Length > 15)
                {
                    e.Value = "Vários";
                    e.FormattingApplied = true;

                    // Injetamos a cor Laranja na coluna invisível para a Badge saber que cor usar
                    if (dgvStock.Columns.Contains("CorFuncao"))
                    {
                        dgvStock.Rows[e.RowIndex].Cells["CorFuncao"].Value = "#F26722";
                    }
                }
            }
        }

        private void dgvStock_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            // 1. Ignorar os cabeçalhos das colunas e linhas inválidas
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 2. Só queremos mostrar o balão se o rato estiver por cima da coluna certa!
            if (dgvStock.Columns[e.ColumnIndex].Name == "NomeFuncao")
            {
                // 3. Vamos buscar o texto verdadeiro (o que veio da BD, que tem os '|')
                string textoOriginal = dgvStock.Rows[e.RowIndex].Cells["NomeFuncao"].Value?.ToString();

                // 4. Se for daqueles que tu escondes atrás do "Vários"...
                if (!string.IsNullOrEmpty(textoOriginal) && (textoOriginal.Contains("|") || textoOriginal.Length > 15))
                {
                    // Substituímos o "|" por um "\n" (Quebra de linha) para ficar uma lista impecável
                    e.ToolTipText = "Departamentos:\n" + textoOriginal.Replace("|", "\n");
                }
            }
        }
    }
}
