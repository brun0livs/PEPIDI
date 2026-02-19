using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Pedidos : UserControl
    {
        readonly int IDGestor;
        readonly private GestorDePedidos gdp;
        readonly string estado;

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
            else
            {
                lblPedidos.Text = "PEDIDOS APROVADOS";
                lblPedidos.ForeColor = Color.Green;
                if (dgvPedidos.Columns.Contains("Check")) dgvPedidos.Columns["Check"].Visible = true;
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

        private void DgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Busca o ID usando o nome exato do Designer: "ID"
                object cellValue = dgvPedidos.Rows[e.RowIndex].Cells["ID"].Value;

                if (cellValue != null && int.TryParse(cellValue.ToString(), out int idPedidoSelecionado))
                {
                    AbrirControl(new UcsSecundarios.PedidosDetalhes(idPedidoSelecionado, IDGestor, estado));
                }
            }
        }

        public void AbrirControl(UserControl control)
        {
            pnlDetails.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlDetails.Controls.Add(control);
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

            // 2. Query à Base de Dados para SOMAR OS TOTAIS GERAIS
            var listaArmazem = new List<(string Modelo, string Tamanho, int Qtd)>();

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();

                // Como a nossa lista só tem números (int), injetamos direto com Join. É super seguro e rápido.
                string inClause = string.Join(",", idsSelecionados);

                // QUERY NOVA: Só precisamos de saber o que ir buscar à prateleira, não interessa quem pediu.
                string sql = $@"
            SELECT 
                E.Modelo, 
                E.Tamanho, 
                SUM(PP.Quantidade) AS QtdTotal
            FROM PedidoPacote PP
            INNER JOIN EPI E ON PP.IDEPI = E.ID
            WHERE PP.IDPedReg IN ({inClause}) AND PP.Quantidade > 0
            GROUP BY E.Modelo, E.Tamanho
            ORDER BY E.Modelo, E.Tamanho";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaArmazem.Add((
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

            // 3. Gerar o PDF e Abrir
            try
            {
                // Chama o MÉTODO NOVO que criámos no PDFGenerator
                string pdfPath = PDFGenerator.GerarListaRecolhaArmazem(listaArmazem);
                System.Diagnostics.Process.Start("explorer.exe", pdfPath);

                // Opcional: Desmarcar as checkboxes depois de imprimir
                foreach (DataGridViewRow row in dgvPedidos.Rows)
                {
                    if (row.Cells["Check"].Value != null && Convert.ToBoolean(row.Cells["Check"].Value) == true)
                    {
                        row.Cells["Check"].Value = false;
                    }
                }
                ValidarBotaoRelatorio(); // Esconde os botões novamente
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