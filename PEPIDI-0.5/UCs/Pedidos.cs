using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}