using PEPIDI.FormsSecundarios;
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
    public partial class Funcoes : UserControl
    {
        private int IDGestor;
        public Funcoes(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }

        private void Funcoes_Load(object sender, EventArgs e)
        {
            CarregarDGV(dgvFuncoes);
            TouchScrollHelper.AtivarScrollPorArrasto(dgvFuncoes);
            GestorTema.AplicarEstilos(this);
        }


        private DataTable CarregarDGV(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;

            string query = @"SELECT [ID], [Nome], [PodeVerStock], [PodeCriarStock],
                                    [PodeEditarFunc], [PodeSubmeter], [PodeAprovar], [PodeEntregar],
                                    [PodeCriarFuncoes], [PodeAlterarDefinicoes], [CorHex] FROM Funcoes";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            dgv.DataSource = dt;
            return dt;
        }

        private void dgvFuncoes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvFuncoes.IsCurrentCellDirty)
                dgvFuncoes.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvFuncoes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvFuncoes.Columns[e.ColumnIndex].Name;
            System.Diagnostics.Debug.WriteLine($"CellValueChanged: col={colName}, row={e.RowIndex}");

            if (colName == "Nome" || colName == "ID") return;

            var row = dgvFuncoes.Rows[e.RowIndex];
            System.Diagnostics.Debug.WriteLine($"ID cell value = {row.Cells["ID"].Value}");

            int id = Convert.ToInt32(row.Cells["ID"].Value);

            GuardarPermissoesFuncao(id, row);
        }


        private void GuardarPermissoesFuncao(int idFuncao, DataGridViewRow row)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_AtualizarPermissoesFuncao", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", idFuncao);
                cmd.Parameters.AddWithValue("@PodeVerStock", row.Cells["PodeVerStock"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeInserirStock", row.Cells["PodeInserirStock"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeCriarStock", row.Cells["PodeCriarStock"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeEditarFunc", row.Cells["PodeEditarFunc"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeSubmeter", row.Cells["PodeSubmeter"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeAprovar", row.Cells["PodeAprovar"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeEntregar", row.Cells["PodeEntregar"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeCriarFuncoes", row.Cells["PodeCriarFuncoes"].Value ?? false);
                cmd.Parameters.AddWithValue("@PodeAlterarDefinicoes", row.Cells["PodeAlterarDefinicoes"].Value ?? false);

                cmd.Parameters.AddWithValue("@AlteradoPor", IDGestor);

                conn.Open();
                int linhas = cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"[SP] Linhas afetadas: {linhas}");
            }
        }

        private void btnAddFuncao_Click(object sender, EventArgs e)
        {
            using (Form overlay = new Form())
            {
                // Configurar o formulário "sombra"
                overlay.StartPosition = FormStartPosition.CenterScreen;
                overlay.WindowState = FormWindowState.Maximized;
                overlay.FormBorderStyle = FormBorderStyle.None; // Sem bordas
                overlay.Opacity = 0.50d;                        // 50% transparente
                overlay.BackColor = Color.Black;                // Cor preta
                overlay.ShowInTaskbar = false;                  // Não aparece na barra de tarefas

                // Faz o overlay cobrir exatamente o formulário atual (this)
                overlay.Location = this.Location;
                overlay.Size = this.Size;

                // Mostra a sombra
                overlay.Show(this);
                using (FormFuncao frm = new FormFuncao("", 0, IDGestor, ""))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        CarregarDGV(dgvFuncoes);
                    }
                }
            }
        }

        private void dgvFuncoes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Verificações básicas
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 2. Obter os dados (Usei nomes seguros, ajusta se as tuas colunas tiverem nomes diferentes)
            // Nota: Certifica-te que as colunas na DataGridView se chamam mesmo "Nome", "ID" e "CorHex"
            string textofuncao = dgvFuncoes.Rows[e.RowIndex].Cells["Nome"].Value.ToString();
            int idfuncao = Convert.ToInt32(dgvFuncoes.Rows[e.RowIndex].Cells["ID"].Value);

            var cellHex = dgvFuncoes.Rows[e.RowIndex].Cells["CorHex"].Value;
            string hex = cellHex != null ? cellHex.ToString() : "";

            // =================================================================
            // 3. EFEITO DE ESCURECER O FUNDO (OVERLAY)
            // =================================================================
            using (Form overlay = new Form())
            {
                // Configurar o formulário "sombra"
                overlay.StartPosition = FormStartPosition.CenterScreen;
                overlay.WindowState = FormWindowState.Maximized;
                overlay.FormBorderStyle = FormBorderStyle.None; // Sem bordas
                overlay.Opacity = 0.50d;                        // 50% transparente
                overlay.BackColor = Color.Black;                // Cor preta
                overlay.ShowInTaskbar = false;                  // Não aparece na barra de tarefas

                // Faz o overlay cobrir exatamente o formulário atual (this)
                overlay.Location = this.Location;
                overlay.Size = this.Size;

                // Mostra a sombra
                overlay.Show(this);

                // 4. Abrir o FormFuncao
                // Passamos o 'overlay' como dono (owner) para garantir que o FormFuncao fica por cima da sombra
                using (FormFuncao frm = new FormFuncao(textofuncao, idfuncao, IDGestor, hex))
                {
                    frm.StartPosition = FormStartPosition.CenterParent; // Centraliza na sombra

                    if (frm.ShowDialog(overlay) == DialogResult.OK)
                    {
                        CarregarDGV(dgvFuncoes);
                    }
                }

                // Quando o using acabar, o overlay fecha-se sozinho
            }
        }
    }
}
