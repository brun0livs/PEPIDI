using Microsoft.Data.SqlClient;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Pedidos : UserControl
    {
        readonly int IDGestor;
        readonly private GestorDePedidos gdp;
        readonly string estado;
        readonly string funcao;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        private CancellationTokenSource _cts = new();
        private bool _aAtualizar = false;

        public Pedidos(int _IDGestor, string _estado, string _funcao)
        {
            InitializeComponent();
            gdp = new GestorDePedidos();
            IDGestor = _IDGestor;
            estado = _estado;
            funcao = _funcao;
        }

        private void Pedidos_Load(object sender, EventArgs e)
        {
            // 1. Bloqueamos a geração automática para respeitar o que fizeste no Designer
            dgvPedidos.AutoGenerateColumns = false;

            GereEstados();
            TouchScrollHelper.AtivarScrollPorArrasto(dgvPedidos);
            GestorTema.AplicarEstilos(this);
            GereFuncoes(funcao);
        }

        private void GereEstados()
        {
            if (estado == "Pendente")
            {
                lblTituloPedidos.Text = "PEDIDOS PENDENTES";
                lblTituloPedidos.ForeColor = Color.FromArgb(243, 108, 33);
                if (dgvPedidos.Columns.Contains("Check")) dgvPedidos.Columns["Check"].Visible = false;
            }
            else if (estado == "Aprovado")
            {
                lblTituloPedidos.Text = "PEDIDOS APROVADOS";
                lblTituloPedidos.ForeColor = Color.Green;
                if (dgvPedidos.Columns.Contains("Check"))
                {
                    dgvPedidos.Columns["Check"].Visible = true;
                    dgvPedidos.Columns["Check"].HeaderText = "Selecionar Tudo";
                    dgvPedidos.Columns["Check"].Width = 200;
                }
            }
            else
            {
                lblTituloPedidos.Text = "PEDIDOS FINALIZADOS";
                lblTituloPedidos.ForeColor = Color.Black;
                if (dgvPedidos.Columns.Contains("Check")) dgvPedidos.Columns["Check"].Visible = false;
            }

            CarregarPedidosPorEstado(dgvPedidos, estado);
        }

        private void GereFuncoes(string funcao)
        {
            if (funcao == "Programador")
            {
                dgvPedidos.Columns["ID"].Visible = true;
                dgvPedidos.Columns["NomeAprovador"].Visible = true;
                dgvPedidos.Columns["NomeEntrega"].Visible = true;
                dgvPedidos.Columns["PDF"].Visible = true;
                dgvPedidos.Columns["PedidoEstado"].Visible = true;
            }
        }

        private void dgvPedidos_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Apenas quando ENTRA no cabeçalho da coluna Check
            if (e.RowIndex == -1 && e.ColumnIndex >= 0 && dgvPedidos.Columns[e.ColumnIndex].Name == "Check")
            {
                dgvPedidos.Cursor = Cursors.Hand;
            }
        }

        private void dgvPedidos_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            // Apenas quando SAI do cabeçalho da coluna Check
            if (e.RowIndex == -1 && e.ColumnIndex >= 0 && dgvPedidos.Columns[e.ColumnIndex].Name == "Check")
            {
                dgvPedidos.Cursor = Cursors.Default;
            }
        }

        private void dgvPedidos_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            // O RowIndex == -1 significa que estamos na zona do Cabeçalho!
            if (e.RowIndex == -1 && e.ColumnIndex >= 0 && dgvPedidos.Columns[e.ColumnIndex].Name == "Check")
            {
                dgvPedidos.Cursor = Cursors.Hand;
            }
            else
            {
                // Se estivermos noutra coluna qualquer, o cursor volta ao normal
                dgvPedidos.Cursor = Cursors.Default;
            }
        }

        private void CarregarPedidosPorEstado(PEPIDIDataGridView dgv, string estado)
        {
            if (gdp == null) return;

            DataTable dt = gdp.CarregarPedidosPorEstado(estado);
            dgv.DataSource = null;

            if (dt == null || dt.Rows.Count == 0) return;

            // 1. CONFIGURAÇÃO DOS BADGES
            dgv.BadgeColumnName = "Estado"; // HeaderText da coluna PedidoEstado
            dgv.BadgeColorColumnName = "CorHex";

            // 2. MAPEAMENTO TOTAL (SQL -> Designer)
            // Básicos
            if (dgv.Columns.Contains("ID")) dgv.Columns["ID"].DataPropertyName = "ID";
            if (dgv.Columns.Contains("Data")) dgv.Columns["Data"].DataPropertyName = "Data";
            if (dgv.Columns.Contains("NrFunc")) dgv.Columns["NrFunc"].DataPropertyName = "NrFunc";
            if (dgv.Columns.Contains("NomeFunc")) dgv.Columns["NomeFunc"].DataPropertyName = "NomeFunc";

            // O QUE FALTA (Baseado no teu Designer)
            if (dgv.Columns.Contains("PedidoEstado")) dgv.Columns["PedidoEstado"].DataPropertyName = "Funcao";
            if (dgv.Columns.Contains("PedidoEstado")) dgv.Columns["PedidoEstado"].DataPropertyName = "Funcao1";
            if (dgv.Columns.Contains("PedidoEstado")) dgv.Columns["PedidoEstado"].DataPropertyName = "Estado";
            if (dgv.Columns.Contains("NomeAprovador")) dgv.Columns["NomeAprovador"].DataPropertyName = "NomeAprovador";
            if (dgv.Columns.Contains("NomeEntrega")) dgv.Columns["NomeEntrega"].DataPropertyName = "NomeEntrega";
            if (dgv.Columns.Contains("PDF")) dgv.Columns["PDF"].DataPropertyName = "PDF";

            // Coluna da cor (Lógica)
            if (dgv.Columns.Contains("CorHex")) dgv.Columns["CorHex"].DataPropertyName = "Funcao1";

            // 3. ATRIBUIÇÃO
            dgv.DataSource = dt;

            // 4. AJUSTES VISUAIS
            if (dgv.Columns.Contains("PedidoEstado"))
            {
                // Transparência para o badge da classe PEPIDIDataGridView
                dgv.Columns["PedidoEstado"].DefaultCellStyle.ForeColor = Color.Transparent;
                dgv.Columns["PedidoEstado"].DefaultCellStyle.SelectionForeColor = Color.Transparent;
            }

            if (dgv.Columns.Contains("CorHex")) dgv.Columns["CorHex"].Visible = false;
        }

        private async void DgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Cancela o clique anterior se o utilizador clicou noutra linha muito rápido
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                // Aguarda 250ms (um pequeno "fôlego" para o CPU)
                await Task.Delay(250, _cts.Token);

                object cellValue = dgvPedidos.Rows[e.RowIndex].Cells["ID"].Value;

                if (cellValue != null && int.TryParse(cellValue.ToString(), out int idPedidoSelecionado))
                {
                    var ucDetalhes = new UcsSecundarios.PedidosDetalhes(idPedidoSelecionado, IDGestor, estado);

                    // A MÁGICA QUE FALTOU: Ligar o fio para o Chefe ouvir o UC!
                    ucDetalhes.AcaoConcluida += FecharDetalhes_AtualizarLista;

                    AbrirControl(ucDetalhes);
                }
            }
            catch (OperationCanceledException) { /* Ignora se cancelado */ }
        }

        public void AbrirControl(UserControl control)
        {
            // 1. Para o desenho do painel (evita o lag visual de montagem)
            SendMessage(pnlDetails.Handle, WM_SETREDRAW, false, 0);

            pnlDetails.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlDetails.Controls.Add(control);

            // 2. Retoma o desenho e força a placa gráfica a pintar tudo de uma vez
            SendMessage(pnlDetails.Handle, WM_SETREDRAW, true, 0);
            pnlDetails.Refresh();
        }

        private void dgvPedidos_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPedidos.IsCurrentCellDirty && dgvPedidos.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvPedidos.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPedidos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPedidos.Columns[e.ColumnIndex].Name == "Check")
            {
                ValidarBotaoRelatorio();
            }
        }

        private void ValidarBotaoRelatorio()
        {
            int linhasSelecionadas = 0;
            int totalLinhas = dgvPedidos.Rows.Count;

            // 1. Conta quantas linhas têm o visto
            foreach (DataGridViewRow row in dgvPedidos.Rows)
            {
                if (row.Cells["Check"].Value != null && Convert.ToBoolean(row.Cells["Check"].Value) == true)
                {
                    linhasSelecionadas++;
                }
            }

            // 2. Gere o Botão do Relatório (Aparece se houver pelo menos 1 selecionado)
            btnRelatorio.Visible = linhasSelecionadas > 0;
            btnRelatorio.Enabled = linhasSelecionadas > 0;

            // 3. MÁGICA DO CABEÇALHO DA COLUNA: 
            // Se selecionou TODAS, muda o texto para Desmarcar. Caso contrário, mantém Selecionar.
            if (dgvPedidos.Columns.Contains("Check"))
            {
                if (linhasSelecionadas > 0 && linhasSelecionadas == totalLinhas)
                {
                    dgvPedidos.Columns["Check"].HeaderText = "✗ Desmarcar Tudo";
                }
                else
                {
                    dgvPedidos.Columns["Check"].HeaderText = "✓ Selecionar Tudo";
                }
            }
        }

        private void btnRecolhaArmazem_Click(object sender, EventArgs e)
        {
            // 1. Recolher todos os IDs dos pedidos que têm o visto
            List<int> idsSelecionados = new List<int>();
            foreach (DataGridViewRow row in dgvPedidos.Rows)
            {
                if (row.Cells["Check"].Value != null && Convert.ToBoolean(row.Cells["Check"].Value) == true)
                {
                    if (int.TryParse(row.Cells["ID"].Value.ToString(), out int idPedido))
                    {
                        idsSelecionados.Add(idPedido);
                    }
                }
            }

            if (idsSelecionados.Count == 0) return;

            /// 2. Query à Base de Dados para obter Itens COM DADOS DO FUNCIONÁRIO
            var listaArmazem = new List<(string NMEC, string Nome, string Modelo, string Tamanho, int Qtd)>();

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();

                // Injeta os IDs na cláusula IN de forma segura
                string inClause = string.Join(",", idsSelecionados);

                // QUERY: Adicionámos o LEFT JOIN com a tabela Stock para sabermos o Estado (1=Novo, 2=Usado)
                // QUERY: Trocámos o E.ID pelo E.Codigo, que é a verdadeira chave da tua tabela EPI!
                string sql = $@"
                        SELECT 
                            F.Nr AS NrFunc, 
                            F.Nome, 
                            E.Modelo, 
                            E.Tamanho, 
                            S.Estado,
                            SUM(PP.Quantidade) AS QtdTotal
                        FROM PedidoPacote PP
                        INNER JOIN EPI E ON PP.CodigoEPI = E.Codigo
                        INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
                        INNER JOIN Funcionarios F ON PR.NrFunc = F.Nr
                        LEFT JOIN Stock S ON PP.IDStock = S.ID
                        WHERE PP.IDPedReg IN ({inClause}) AND PP.Quantidade > 0
                        GROUP BY F.Nr, F.Nome, E.Modelo, E.Tamanho, S.Estado
                        ORDER BY F.Nome, E.Modelo, E.Tamanho";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string modeloBase = reader["Modelo"]?.ToString() ?? "Artigo";

                            // Lemos o estado (Se vier NULL assumimos 1 - Novo)
                            int idEstado = reader["Estado"] != DBNull.Value ? Convert.ToInt32(reader["Estado"]) : 1;

                            // Colamos a etiqueta [NOVO] ou [USADO] à frente do modelo para o gajo do armazém não se enganar!
                            string etiquetaEstado = (idEstado == 2) ? " [USADO]" : " [NOVO]";
                            string modeloComEstado = modeloBase + etiquetaEstado;

                            listaArmazem.Add((
                                reader["NrFunc"]?.ToString() ?? "0000",
                                reader["Nome"]?.ToString() ?? "Desconhecido",
                                modeloComEstado, // <-- Entra aqui já com a etiqueta colada!
                                reader["Tamanho"]?.ToString() ?? "-",
                                Convert.ToInt32(reader["QtdTotal"])
                            ));
                        }
                    }
                }
            }

            if (listaArmazem.Count == 0)
            {
                EfeitoUI M = new EfeitoUI();
                M.AbrirMensagem("Os pedidos selecionados não têm itens para separar.", "Aviso");
                return;
            }

            // 3. Gerar o PDF de Separação por Funcionário e Abrir
            try
            {
                // CHAMA O MÉTODO QUE TU QUERES (GerarListaSeparacaoPorFuncionario)
                string pdfPath = PEPIDI.Organizers.PDFGenerator.GerarListaSeparacaoPorFuncionario(listaArmazem);
                System.Diagnostics.Process.Start("explorer.exe", pdfPath);

                // Desmarcar as checkboxes depois de imprimir para não gerar 2x sem querer
                foreach (DataGridViewRow row in dgvPedidos.Rows)
                {
                    if (row.Cells["Check"].Value != null && Convert.ToBoolean(row.Cells["Check"].Value) == true)
                    {
                        row.Cells["Check"].Value = false;
                    }
                }
                ValidarBotaoRelatorio(); // Esconde o botão novamente
            }
            catch (Exception ex)
            {
                EfeitoUI M = new EfeitoUI();
                string detalhe = ex.InnerException != null ? $"\nDetalhe: {ex.InnerException.Message}" : "";
                M.AbrirMensagem($"Erro ao gerar relatório: {ex.Message}{detalhe}", "Erro");
            }
        }

        private void FecharDetalhes_AtualizarLista(object sender, EventArgs e)
        {
            // 1. Limpa o painel (fecha os detalhes)
            pnlDetails.Controls.Clear();

            // 2. Agora sim, como a ação já terminou em segurança, recarrega a grelha!
            Pedidos_Load(null, EventArgs.Empty);
        }

        private void dgvPedidos_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // 1. Só nos interessa se o clique for no cabeçalho da coluna "Check"
            if (e.ColumnIndex >= 0 && dgvPedidos.Columns[e.ColumnIndex].Name == "Check")
            {
                // 2. Pausa a atualização visual
                dgvPedidos.SuspendLayout();

                // 3. VERIFICAÇÃO INTELIGENTE: Vamos ver se TODAS estão marcadas
                bool todasJaEstaoMarcadas = true;
                foreach (DataGridViewRow row in dgvPedidos.Rows)
                {
                    if (row.Cells["Check"].Value == null || Convert.ToBoolean(row.Cells["Check"].Value) == false)
                    {
                        // Encontrámos uma linha sem visto! Logo, não estão todas marcadas.
                        todasJaEstaoMarcadas = false;
                        break; // Pára de procurar, já sabemos a resposta.
                    }
                }

                // 4. O novo estado é o inverso: se todas estavam marcadas, desmarca. Caso contrário, marca todas!
                bool novoEstado = !todasJaEstaoMarcadas;

                // 5. Aplica o novo estado a todas as linhas
                foreach (DataGridViewRow row in dgvPedidos.Rows)
                {
                    row.Cells["Check"].Value = novoEstado;
                }

                // 6. Força a grelha a assumir e a desenhar os novos valores
                dgvPedidos.EndEdit();
                dgvPedidos.RefreshEdit(); // OBRIGATÓRIO para forçar os vistos a aparecerem na hora!
                dgvPedidos.ResumeLayout();

                // 7. Valida o botão do Relatório
                ValidarBotaoRelatorio();
            }
        }
    }
}