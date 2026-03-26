using Microsoft.Data.SqlClient;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.FormsSecundarios
{
    public partial class FormImportarFuncionarios : Form
    {
        int IDGestor;
        public FormImportarFuncionarios(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormImportarFuncionarios_Load(object sender, EventArgs e)
        {
            // 1. Configurar o aspeto inicial da Grid (se não o fizeste no Designer)
            ConfigurarColunasGrid();

            // 3. Focar na primeira célula para facilitar o CTRL+V
            dgvImport.Focus();
        }

        private void ConfigurarColunasGrid()
        {
            dgvImport.Columns.Clear();

            // Criamos as colunas que batem certo com o teu Excel/SQL
            dgvImport.Columns.Add("Nr", "Nº Mecanográfico");
            dgvImport.Columns.Add("Nome", "Nome do Funcionário");
            dgvImport.Columns.Add("Funcao", "Função (Ex: Produção)");

            // Design: O Nome ocupa o espaço todo, o resto encolhe
            dgvImport.Columns["Nr"].Width = 120;
            dgvImport.Columns["Funcao"].Width = 150;
            dgvImport.Columns["Nome"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Permitir que o utilizador escreva/cole à vontade
            dgvImport.AllowUserToAddRows = true;
            dgvImport.EditMode = DataGridViewEditMode.EditOnKeystroke;
        }

        private void dgvImport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                try
                {
                    string textoClipboard = Clipboard.GetText();
                    string[] linhas = textoClipboard.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int linhaAtual = dgvImport.CurrentRow != null ? dgvImport.CurrentRow.Index : 0;

                    dgvImport.SuspendLayout();

                    foreach (string linha in linhas)
                    {
                        string[] celulas = linha.Split('\t');
                        if (linhaAtual >= dgvImport.Rows.Count - 1) dgvImport.Rows.Add();

                        // 1. Dados Brutos
                        dgvImport["Nr", linhaAtual].Value = celulas.Length > 0 ? celulas[0].Trim() : "";
                        dgvImport["Nome", linhaAtual].Value = celulas.Length > 1 ? celulas[1].Trim() : "";

                        string funcExcel = celulas.Length > 2 ? celulas[2].Trim() : "";

                        // 2. CHAMADA AO MOTOR IA
                        string funcIA = MotorIA.CorrigirFuncao(funcExcel);

                        dgvImport["Funcao", linhaAtual].Value = funcIA;

                        // Guardamos o que veio do Excel no Tag para o botão "Importar" saber se houve mudança
                        dgvImport["Funcao", linhaAtual].Tag = funcExcel;

                        // 3. FEEDBACK VISUAL (IA em dúvida = Amarelo)
                        if (funcIA == "Verificar Colagem")
                        {
                            dgvImport["Funcao", linhaAtual].Style.BackColor = Color.LightGoldenrodYellow;
                            dgvImport["Funcao", linhaAtual].Value = funcExcel; // Mantém o original para facilitar a correção
                        }
                        else
                        {
                            dgvImport["Funcao", linhaAtual].Style.BackColor = Color.White;
                        }

                        linhaAtual++;
                    }
                    dgvImport.ResumeLayout();
                }
                catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
            }
        }

        // No botão Importar dos Funcionários:
        private void btnImportar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvImport.Rows)
            {
                if (row.IsNewRow) continue;

                string original = row.Cells["Funcao"].Tag?.ToString();
                string corrigido = row.Cells["Funcao"].Value?.ToString();

                if (original != corrigido)
                    MotorIA.AprenderNovaRegra(original, corrigido, "Funcao");
            }
            // Gravar na tabela Funcionarios e LogIn...
            this.DialogResult = DialogResult.OK;
        }
    }
}
