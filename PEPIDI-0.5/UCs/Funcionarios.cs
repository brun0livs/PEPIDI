using PEPIDI.FormsSecundarios;
using PEPIDI.Models;
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
    public partial class Funcionarios : UserControl
    {
        MostrarFuncionarios mf = new MostrarFuncionarios();
        private ContextMenuStrip _menuAcoes;
        private int _rowMenu = -1;
        int IDGestor;
        EfeitoUI M = new EfeitoUI();

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
            GestorTema.AplicarEstilos(this);

        }

        private void Configura(PEPIDIDataGridView dgvFuncs)
        {
            // 1. MUDANÇA AQUI: Modo Fill para as percentagens funcionarem!
            dgvFuncs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvFuncs.ReadOnly = true;
            dgvFuncs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFuncs.RowHeadersVisible = false;

            // --- ACTIVAR BADGES COLORIDAS ---
            // Isto diz à tua classe personalizada para procurar a coluna com HeaderText "Função"
            dgvFuncs.BadgeColumnName = "Função";
            dgvFuncs.BadgeColorColumnName = "CorHex";

            // --- MENU DE AÇÕES ---
            _menuAcoes = new ContextMenuStrip();
            _menuAcoes.Items.Add("Editar", null, (s, e) => AcaoSelecionada("Editar"));
            _menuAcoes.Items.Add("Ver histórico", null, (s, e) => AcaoSelecionada("Historico"));
            _menuAcoes.Items.Add(new ToolStripSeparator());
            _menuAcoes.Items.Add("Repor palavra-passe…", null, (s, e) => AcaoSelecionada("ReporPass"));

            dgvFuncs.ShowActionDots = true;
            dgvFuncs.ActionDotsAfterColumn = "Estab";
            dgvFuncs.ActionDotsColor = Color.Black;
            dgvFuncs.ActionDotsFontSize = 13f;

            dgvFuncs.ActionDotsCellClick += (s, ev) =>
            {
                _rowMenu = ev.RowIndex;
                var rect = dgvFuncs.GetCellDisplayRectangle(ev.ColumnIndex, ev.RowIndex, true);
                var pt = dgvFuncs.PointToScreen(new System.Drawing.Point(rect.Left, rect.Bottom + 2));
                _menuAcoes.Show(pt);
            };

            // --- FORMATAÇÃO, ALINHAMENTO E TAMANHOS ---

            if (dgvFuncs.Columns.Contains("Nr"))
            {
                dgvFuncs.Columns["Nr"].HeaderText = "Nº";
                dgvFuncs.Columns["Nr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFuncs.Columns["Nr"].FillWeight = 5;
            }

            if (dgvFuncs.Columns.Contains("Nome"))
            {
                dgvFuncs.Columns["Nome"].FillWeight = 23;
            }

            if (dgvFuncs.Columns.Contains("Funcao"))
            {
                dgvFuncs.Columns["Funcao"].HeaderText = "Função";
                dgvFuncs.Columns["Funcao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFuncs.Columns["Funcao"].FillWeight = 13;

                // Magia da Transparência: Esconde o texto normal para a badge desenhada poder brilhar
                dgvFuncs.Columns["Funcao"].DefaultCellStyle.ForeColor = Color.Transparent;
                dgvFuncs.Columns["Funcao"].DefaultCellStyle.SelectionForeColor = Color.Transparent;
            }

            // Esconder a coluna que traz o código HEX da Base de Dados
            if (dgvFuncs.Columns.Contains("CorHex"))
            {
                dgvFuncs.Columns["CorHex"].Visible = false;
            }

            if (dgvFuncs.Columns.Contains("DtAdmiss"))
            {
                dgvFuncs.Columns["DtAdmiss"].HeaderText = "Admissão";
                dgvFuncs.Columns["DtAdmiss"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFuncs.Columns["DtAdmiss"].FillWeight = 8;
            }

            if (dgvFuncs.Columns.Contains("Estab"))
            {
                dgvFuncs.Columns["Estab"].HeaderText = "Estab.";
                dgvFuncs.Columns["Estab"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFuncs.Columns["Estab"].FillWeight = 15;
            }
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
                        M.AbrirMensagem($"Erro ao editar no formulário: {ex.Message}", "Erro");
                    }
                    break;
                case "Historico":
                    try
                    {
                        using (Form formHistorico = new Form())
                        {
                            formHistorico.Text = "PEPIDI - Histórico e Gráficos";
                            formHistorico.WindowState = FormWindowState.Maximized;
                            formHistorico.StartPosition = FormStartPosition.CenterScreen;
                            formHistorico.ShowIcon = false;

                            Graficos ucHistorico = new Graficos(id); // Leva o ID na mochila!
                            ucHistorico.Dock = DockStyle.Fill;

                            formHistorico.Controls.Add(ucHistorico);
                            formHistorico.ShowDialog(); // Fica aqui até o utilizador fechar!
                        }
                    }
                    catch (Exception ex)
                    {
                        M.AbrirMensagem($"Erro ao analisar Consumos: {ex.Message}", "Erro");
                    }
                    break;
                case "ReporPass":
                    //M.AbrirMensagem($"Password reposta para o funcionário #{id}", "PEPIDI");
                    MessageBox.Show(this.Size.ToString());
                    break;
            }
        }

        private void btnAbreGraficos_Click(object sender, EventArgs e)
        {
            using (Form formHistorico = new Form())
            {
                formHistorico.Text = "PEPIDI - Dashboard Global";
                formHistorico.WindowState = FormWindowState.Maximized;
                formHistorico.StartPosition = FormStartPosition.CenterScreen;
                formHistorico.ShowIcon = false;

                // Aqui abre SEM ID, mostra o global!
                Graficos ucGraficos = new Graficos();
                ucGraficos.Dock = DockStyle.Fill;

                formHistorico.Controls.Add(ucGraficos);
                formHistorico.ShowDialog();
            }
        }

        private void btnImportacaoRapida_Click(object sender, EventArgs e)
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
                using (FormImportarFuncionarios frm = new FormImportarFuncionarios(IDGestor))
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
    }
}
