using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
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
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        private CancellationTokenSource _cts = new();

        public Pedidos(int _IDGestor, string _estado)
        {
            InitializeComponent();
            gdp = new GestorDePedidos();
            IDGestor = _IDGestor;
            estado = _estado;
        }

        private void Pedidos_Load(object sender, EventArgs e)
        {
            // 1. Bloqueamos a geração automática para respeitar o que fizeste no Designer
            dgvPedidos.AutoGenerateColumns = false;
            GereEstados();

            TouchScrollHelper.AtivarScrollPorArrasto(dgvPedidos);
        }

        private void GereEstados()
        {
            if (estado == "Pendente")
            {
                lblPedidos.Text = "PEDIDOS PENDENTES";
                lblPedidos.ForeColor = Color.FromArgb(243, 108, 33);
                if (dgvPedidos.Columns.Contains("Check")) dgvPedidos.Columns["Check"].Visible = false;
            }
            else if (estado == "Aprovado")
            {
                lblPedidos.Text = "PEDIDOS APROVADOS";
                lblPedidos.ForeColor = Color.Green;
                if (dgvPedidos.Columns.Contains("Check")) dgvPedidos.Columns["Check"].Visible = true;
            }
            else
            {
                lblPedidos.Text = "PEDIDOS FINALIZADOS";
                lblPedidos.ForeColor = Color.Black;
                if (dgvPedidos.Columns.Contains("Check")) dgvPedidos.Columns["Check"].Visible = false;
            }

            CarregarPedidosPorEstado(dgvPedidos, estado);
        }

        private void CarregarPedidosPorEstado(PEPIDIDataGridView dgv, string estado)
        {
            if (gdp == null) return;

            DataTable dt = gdp.CarregarPedidosPorEstado(estado);
            dgv.DataSource = null;

            if (dt == null || dt.Rows.Count == 0) return;

            // 1. CONFIGURAÇÃO DOS NOMES (Bater exatamente com o Designer)
            // O nome da coluna no Designer é "Funcao" (pelo InitializeComponent)
            // Mas o HeaderText é "Função". A classe usa o HeaderText na comparação.
            dgv.BadgeColumnName = "Função";
            dgv.BadgeColorColumnName = "CorHex";

            // 2. MAPEAMENTO DOS DADOS (Vincular colunas do Designer ao SQL)
            if (dgv.Columns.Contains("ID")) dgv.Columns["ID"].DataPropertyName = "ID";
            if (dgv.Columns.Contains("Data")) dgv.Columns["Data"].DataPropertyName = "Data";

            // De acordo com o teu Designer e DT:
            if (dgv.Columns.Contains("NrFunc")) dgv.Columns["NrFunc"].DataPropertyName = "NrFunc";
            if (dgv.Columns.Contains("NomeFunc")) dgv.Columns["NomeFunc"].DataPropertyName = "NomeFunc";

            // Vincula o texto à coluna "Funcao" e a cor à coluna "CorHex"
            if (dgv.Columns.Contains("Funcao")) dgv.Columns["Funcao"].DataPropertyName = "Funcao";
            if (dgv.Columns.Contains("CorHex")) dgv.Columns["CorHex"].DataPropertyName = "Funcao1";

            // 3. ATRIBUIÇÃO DOS DADOS
            dgv.DataSource = dt;

            // 4. APLICAÇÃO DA TRANSPARÊNCIA (Para o texto não encavalitar)
            if (dgv.Columns.Contains("Funcao"))
            {
                // O ForeColor deve ser transparente para o OnCellPaintingModern da tua classe brilhar
                dgv.Columns["Funcao"].DefaultCellStyle.ForeColor = Color.Transparent;
                dgv.Columns["Funcao"].DefaultCellStyle.SelectionForeColor = Color.Transparent;
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
                    // Criamos a UC. Dica: Se o construtor da UC faz SQL, ela vai lagar.
                    // O ideal é a UC ser criada vazia e carregar os dados num evento 'Load' asíncrono.
                    var ucDetalhes = new UcsSecundarios.PedidosDetalhes(idPedidoSelecionado, IDGestor, estado);

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
            bool temSelecionado = false;

            foreach (DataGridViewRow row in dgvPedidos.Rows)
            {
                if (row.Cells["Check"].Value != null && Convert.ToBoolean(row.Cells["Check"].Value) == true)
                {
                    temSelecionado = true;
                    break;
                }
            }

            // Assumindo que o botão se chama btnRelatorio no teu Designer
            btnRelatorio.Visible = temSelecionado;
            btnRelatorio.Enabled = temSelecionado;
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

            // 2. Query à Base de Dados para obter Itens COM DADOS DO FUNCIONÁRIO
            // (A lista agora pede 5 dados: NMEC, Nome, Modelo, Tamanho, Qtd)
            var listaArmazem = new List<(string NMEC, string Nome, string Modelo, string Tamanho, int Qtd)>();

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();

                // Injeta os IDs na cláusula IN de forma segura
                string inClause = string.Join(",", idsSelecionados);

                // QUERY: Junta a tabela EPI, mas também PedidoRegistos e Funcionários para saber de quem é a roupa
                string sql = $@"
            SELECT 
                F.Nr AS NrFunc, 
                F.Nome, 
                E.Modelo, 
                E.Tamanho, 
                SUM(PP.Quantidade) AS QtdTotal
            FROM PedidoPacote PP
            INNER JOIN EPI E ON PP.IDEPI = E.ID
            INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
            INNER JOIN Funcionarios F ON PR.NrFunc = F.Nr
            WHERE PP.IDPedReg IN ({inClause}) AND PP.Quantidade > 0
            GROUP BY F.Nr, F.Nome, E.Modelo, E.Tamanho
            ORDER BY F.Nome, E.Modelo, E.Tamanho";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaArmazem.Add((
                                reader["NrFunc"]?.ToString() ?? "0000",
                                reader["Nome"]?.ToString() ?? "Desconhecido",
                                reader["Modelo"]?.ToString() ?? "Artigo",
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
    }
}