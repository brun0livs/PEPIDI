using PEPIDI;
using PEPIDI.Organizers;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Policy;

namespace PEPIDI_0._5_Beta
{
    public partial class FrmLogIn : Form
    {
        readonly PEPIDI.Organizers.Hash hash = new();

        public FrmLogIn()
        {
            InitializeComponent();
        }

        private void BtnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                var userTxt = txtUser.Text;
                var passTxt = pbPass.Text;

                if (!string.IsNullOrEmpty(userTxt) && userTxt.Any(c => c == '|' || c == '\\'))
                {

                    string nomePc = Environment.MachineName;
                    int idFunc = Convert.ToInt32(userTxt);
                    using SqlConnection conn = GetConn.GetConnection();
                    using SqlCommand cmd = new("sp_RegistaLoginDispositivo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDFuncionario", idFunc);
                    cmd.Parameters.AddWithValue("@NomePC", nomePc);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Dispositivo registado com sucesso.");
                }
                 Entrar(Convert.ToInt32(userTxt), passTxt);
            }
            catch { MessageBox.Show("Valores Inválidos"); }
        }

        private void Entrar(int user, string pass)
        {
            string HPass = hash.GerarHashSenha(pass);

            Debug.WriteLine(HPass);

            using SqlConnection connection = new(GetConn.ConnectionString);
            try
            {
                connection.Open();

                // 1. ALTERADO: Em vez de SELECT COUNT(*), fazemos SELECT Nr (ou ID)
                // Assim apanhamos logo o ID do utilizador para usar nas permissões
                string query = "SELECT COUNT(*) FROM LogIn WHERE Nr = @username AND Password = @password";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@username", user);
                command.Parameters.AddWithValue("@password", HPass);

                // O ExecuteScalar devolve a primeira coluna (o ID) ou null se não encontrar
                object result = command.ExecuteScalar();

                if (result != null) // Se result não for null, o login é válido
                {
                    txtUser.Text = "";
                    pbPass.Text = "";

                    // 2. ALTERADO: Chamamos a classe correta e passamos o ID (int)
                    // Certifica-te que adicionaste 'using PEPIDI.Organizers;' no topo
                    var permissoes = PermissoesPerfil.VerPermissoes(user);

                    if (permissoes.PodeSubmeter)
                    {
                        // Nota: Provavelmente vais querer passar o 'idUsuario' aqui também
                        AbreFormUserPedido(user);
                    }
                    else
                    {
                        // E aqui também, para o gestor saber quem está logado
                        AbreFormUserGestor(user, permissoes);
                    }
                }
                else
                {
                    MessageBox.Show("Credenciais inválidas.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    pass = string.Empty;
                    pbPass.Focus();
                    pbPass.Text = "";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro de base de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AbreFormUserPedido(int NrFunc)
        {
            try
            {
                FormPedidos frm = new FormPedidos(NrFunc, this);
                frm.ShowDialog();
                txtUser.Focus(); // Retorna o foco para txtUser após fechar o form
            }
            catch (Exception ex)
            {
                // Exibe o erro ou registra no log para análise
                MessageBox.Show($"Erro ao abrir o formulário: {ex.Message}");
            }
        }

        private void AbreFormUserGestor(int NrFunc, PermissoesPerfil perms)
        {
            try
            {
                Form frm = new FormGestao(NrFunc, perms);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                // Exibe o erro ou registra no log para análise
                MessageBox.Show($"Erro ao abrir o formulário: {ex.Message}");
            }
        }


        private void PbPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogIn.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true; // sem beep
            }
        }

        private void PbPass_IconRightClick(object sender, EventArgs e)
        {
            if (pbPass.UseSystemPasswordChar)
            {
                pbPass.UseSystemPasswordChar = false;
                pbPass.IconRight = PEPIDI.Properties.Resources.eye_off;
                pbPass.Font = new Font("Roboto", 11, FontStyle.Regular);
            }
            else
            {
                pbPass.UseSystemPasswordChar = true;
                pbPass.IconRight = PEPIDI.Properties.Resources.eye_on;
            }
        }
    }
}
