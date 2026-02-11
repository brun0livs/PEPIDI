using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Pedidos : UserControl
    {
        int IDGestor;
        private GestorDePedidos gdp;
        string estado;


        public Pedidos(int _IDGestor, string _estado)
        {
            InitializeComponent();
            gdp = new GestorDePedidos();
            IDGestor = _IDGestor;
            estado = _estado;
        }

        private void Pedidos_Load(object sender, EventArgs e)
        {
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
            }
            else
            {
                lblPedidos.Text = "PEDIDOS APROVADOS";
                lblPedidos.ForeColor = Color.Green;
                dgvPedidos.Columns["Check"].Visible = true;
            }

            CarregarPedidosPorEstado(dgvPedidos, estado);
        }

        private void CarregarPedidosPorEstado(PEPIDIDataGridView dgv, string estado)
        {
            if (gdp == null)
            {
                MessageBox.Show("gdp está null!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable dt = gdp.CarregarPedidosPorEstado(estado);
            if (dt == null || dt.Rows.Count == 0)
            {
                Debug.WriteLine("Nenhum pedido encontrado para o estado: " + estado);
                dgv.DataSource = null;
                return;
            }

            dgv.AutoGenerateColumns = false;
            dgv.DataSource = dt;

            // mapear colunas do DataTable para as colunas do DGV
            for (int i = 0; i < dgv.Columns.Count && i < dt.Columns.Count; i++)
            {
                dgv.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
            }

            // 1. Configura a coluna que tem o Texto (ex: "Armazém")
            dgv.BadgeColumnName = "Função";

            // 2. Configura a coluna que tem a Cor (ex: "#FF0000")
            dgv.BadgeColorColumnName = "CorHex";

            // 3. Esconde a coluna da cor para não ficar feio na tabela
            if (dgv.Columns.Contains("CorHex"))
            {
                dgv.Columns["CorHex"].Visible = false;
            }

        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Validar se o clique foi numa linha de dados e não no cabeçalho
            if (e.RowIndex >= 0)
            {
                // 2. Obter o ID da coluna correta. 
                int valorID = Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells["ID"].Value);

                if (valorID != null && int.TryParse(valorID.ToString(), out int idPedidoSelecionado))
                {
                    AbrirControl(new UcsSecundarios.PedidosDetalhes(valorID, IDGestor, estado));
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
