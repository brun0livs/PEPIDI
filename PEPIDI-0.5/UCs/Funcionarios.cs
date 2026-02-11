using PEPIDI.FormsSecundarios;
using PEPIDI.Models;
using PEPIDI.Organizers;
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
    public partial class Funcionarios : UserControl
    {
        MostrarFuncionarios mf = new MostrarFuncionarios();
        private ContextMenuStrip _menuAcoes;
        private int _rowMenu = -1;
        int IDGestor;

        public Funcionarios(int _IDGestor)
        {
            IDGestor = _IDGestor;
            InitializeComponent();
        }

        private void btnAddFunc_Click(object sender, EventArgs e)
        {
            AbrirFuncionarios(null, IDGestor);
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = mf.CarregarFuncionarios(txtPesquisa.Text);
            dgvFuncs.DataSource = dt;
            Configura(dgvFuncs);
        }

        private void AbrirFuncionarios(int? idFunc, int idGestor)
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
                using (FormFuncionario frm = new FormFuncionario(idFunc, idGestor))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        DataTable dt = mf.CarregarFuncionarios("");
                        dgvFuncs.DataSource = dt;
                        Configura(dgvFuncs);
                    }
                }
            }
        }

        private void Funcionarios_Load(object sender, EventArgs e)
        {
            DataTable dt = mf.CarregarFuncionarios("");
            dgvFuncs.DataSource = dt;
            Configura(dgvFuncs);
            TouchScrollHelper.AtivarScrollPorArrasto(dgvFuncs);

        }

        private void Configura(PEPIDIDataGridView dgvFuncs)
        {
            dgvFuncs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvFuncs.Columns["Nome"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvFuncs.ReadOnly = true;
            dgvFuncs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFuncs.RowHeadersVisible = false;

            _menuAcoes = new ContextMenuStrip();

            _menuAcoes.Items.Add("Editar", null, (s, e) => AcaoSelecionada("Editar"));
            _menuAcoes.Items.Add("Ver histórico", null, (s, e) => AcaoSelecionada("Historico"));
            _menuAcoes.Items.Add(new ToolStripSeparator());
            _menuAcoes.Items.Add("Repor palavra-passe…", null, (s, e) => AcaoSelecionada("ReporPass"));

            dgvFuncs.ShowActionDots = true;
            dgvFuncs.ActionDotsAfterColumn = "Estab";   // ou o nome correto na tua grelha
            dgvFuncs.ActionDotsColor = Color.Black;
            dgvFuncs.ActionDotsFontSize = 13f;

            dgvFuncs.ActionDotsCellClick += (s, ev) =>
            {
                _rowMenu = ev.RowIndex;
                var rect = dgvFuncs.GetCellDisplayRectangle(ev.ColumnIndex, ev.RowIndex, true);
                var pt = dgvFuncs.PointToScreen(new System.Drawing.Point(rect.Left, rect.Bottom + 2));
                _menuAcoes.Show(pt);
            };
        }

        // === Ações do menu ===
        private void AcaoSelecionada(string acao)
        {
            if (_rowMenu < 0) return;

            var row = dgvFuncs.Rows[_rowMenu];
            if (!dgvFuncs.Columns.Contains("Nr")) return;

            int id = Convert.ToInt32(row.Cells["Nr"].Value);

            switch (acao)
            {
                case "Editar":
                    try
                    {
                        AbrirFuncionarios(id, IDGestor);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao editar no formulário: {ex.Message}");
                    }
                    break;
                case "Historico":
                    try
                    {
                        //var frm = new FrmConsumosDetalhados(id);
                        //frm.TopMost = true;

                        //// quando o form fechar → recarrega o UC
                        //frm.FormClosed += (s, e) =>
                        //{
                        //    UCFuncionarios_Load("", EventArgs.Empty);
                        //};

                        //frm.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao analisar Consumos: {ex.Message}");
                    }
                    break;

                case "ReporPass":
                    MessageBox.Show($"Password reposta para o funcionário #{id}", "PEPIDI");
                    break;
            }
        }
    }
}
