using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Organizers;
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
    public partial class FormNovaPasse : Form
    {
        readonly int IDGestor;
        readonly PEPIDI.Organizers.Hash hash = new();
        public FormNovaPasse(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }


        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Verifica(txtAtual, txtNova, txtConfirma);
        }
        private void Verifica(Guna2TextBox pboxAtual, Guna2TextBox pboxNova, Guna2TextBox pboxConfirma)
        {
            // 1. Seguranças à porta (Early Returns). Faltou preencher algo? Rua!
            if (string.IsNullOrWhiteSpace(pboxAtual.Text))
            {
                MessageBox.Show("Preencha o campo da password atual.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(pboxNova.Text))
            {
                MessageBox.Show("Preencha o campo da nova password.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(pboxConfirma.Text))
            {
                MessageBox.Show("Preencha o campo de confirmação da nova password.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pboxNova.Text != pboxConfirma.Text)
            {
                MessageBox.Show("A nova password e a confirmação não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Pergunta à BD se a pass antiga está certa.
            // Se a função devolver FALSE, faz um "return" imediato para abortar tudo
            if (!VerificaPassword(IDGestor, pboxAtual.Text))
            {
                return;
            }

            // 3. Caminho Livre! Se o código sobreviveu até aqui, é seguro atualizar.
            AtualizarPassword(IDGestor, pboxNova.Text);
        }

        private bool VerificaPassword(int idGestor, string passwordAtual)
        {
            try
            {
                string HPass = hash.GerarHashSenha(passwordAtual);
                string sql = "SELECT COUNT(*) FROM LogIn WHERE Nr = @ID AND Password = @HPass";

                using SqlConnection conn = new(GetConn.ConnectionString);
                conn.Open();
                using SqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("@HPass", HPass);
                cmd.Parameters.AddWithValue("@ID", idGestor);

                int count = (int)cmd.ExecuteScalar();
                if (count == 0)
                {
                    MessageBox.Show("A password atual está incorreta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // A password falhou! Diz à função principal que deu erro.
                }

                return true; // A password está correta! Dá luz verde para avançar.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar a password:\n" + ex.Message, "Erro SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Em caso de erro do servidor, falha por segurança.
            }
        }

        private void AtualizarPassword(int idGestor, string novaPassword)
        {
            try
            {
                // 1. Gera a hash da nova password usando a tua classe
                string HPass = hash.GerarHashSenha(novaPassword);

                // 2. Prepara a query SQL com PARÂMETROS de segurança (@HPass e @ID)
                string sql = "UPDATE LogIn SET Password = @HPass WHERE Nr = @ID";

                // 3. Abre a ligação e executa o comando
                using SqlConnection conn = new(GetConn.ConnectionString);
                conn.Open();

                using SqlCommand cmd = new(sql, conn);
                // 4. Dizemos ao SQL o que vale cada parâmetro
                cmd.Parameters.AddWithValue("@HPass", HPass);
                cmd.Parameters.AddWithValue("@ID", idGestor);

                // 5. Dá a ordem de execução!
                cmd.ExecuteNonQuery();
                MessageBox.Show("Password atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // Se a base de dados falhar (ex: rede foi abaixo), o programa não "rebenta"
                MessageBox.Show("Erro ao atualizar a password na Base de Dados:\n" + ex.Message, "Erro de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostraPass_IconRightClick(object sender, EventArgs e)
        {
            // 1. Converte o sender para o tipo EXATO da TextBox
            var txtBox = (Guna2TextBox)sender;

            // 2. Garante que a conversão funcionou por segurança
            if (txtBox != null)
            {
                if (txtBox.UseSystemPasswordChar)
                {
                    txtBox.UseSystemPasswordChar = false;
                    txtBox.IconRight = Properties.Resources.eye_off;
                }
                else
                {
                    txtBox.UseSystemPasswordChar = true;
                    txtBox.IconRight = PEPIDI.Properties.Resources.eye_on;
                }
            }
        }
    }
}
