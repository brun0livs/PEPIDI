using PEPIDI;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Security.Policy;

namespace PEPIDI
{
    public partial class FrmLogIn : Form
    {
        readonly PEPIDI.Organizers.Hash hash = new();
        EfeitoUI M = new EfeitoUI();
        public FrmLogIn()
        {
            InitializeComponent();
            GestorTema.AplicarEstilos(this);
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
                    M.AbrirMensagem("Dispositivo registado com sucesso.", "Erro");
                }
                Entrar(Convert.ToInt32(userTxt), passTxt);
            }
            catch { M.AbrirMensagem("Valores Inválidos", "Erro"); }
        }

        private void Entrar(int user, string pass)
        {
            // 1. Gera o Hash para comparar com a BD
            string HPass = hash.GerarHashSenha(pass);
            Debug.WriteLine(HPass);

            using SqlConnection connection = new(GetConn.ConnectionString);
            try
            {
                connection.Open();

                // Query optimizada para contar registos que coincidem com User e Pass
                string query = "SELECT COUNT(*) FROM LogIn WHERE Nr = @username AND Password = @password";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@username", user);
                command.Parameters.AddWithValue("@password", HPass);

                // ExecuteScalar devolve o resultado da primeira coluna (o COUNT)
                object result = command.ExecuteScalar();

                // 2. CORREÇÃO CRÍTICA: Convertemos para int e verificamos se é > 0
                // O COUNT(*) nunca é null, é 0 quando falha
                int count = (result != null) ? Convert.ToInt32(result) : 0;

                if (count > 0)
                {
                    // SUCESSO: Limpar campos para segurança antes de mudar de ecrã
                    txtUser.Text = "";
                    pbPass.Text = "";

                    // 3. Carregar permissões e redirecionar conforme o perfil
                    var permissoes = PermissoesPerfil.VerPermissoes(user);

                    if (permissoes.PodeSubmeter)
                    {
                        AbreFormUserPedido(user);
                    }
                    else
                    {
                        AbreFormUserGestor(user, permissoes);
                    }
                }
                else
                {
                    // FALHA: Credenciais erradas
                    M.AbrirMensagem("Credenciais inválidas. Verifique o NMEC e a Password.", "Erro");
                    pbPass.Text = "";
                    pbPass.Focus();
                }
            }
            catch (SqlException ex)
            {
                M.AbrirMensagem("Erro de ligação à base de dados:\n" + ex.Message, "Erro de SQL");
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Ocorreu um erro inesperado:\n" + ex.Message, "Erro Crítico");
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
                M.AbrirMensagem($"Erro ao abrir o formulário: {ex.Message}", "Erro");
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
                M.AbrirMensagem($"Erro ao abrir o formulário: {ex.Message}", "Erro");
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
