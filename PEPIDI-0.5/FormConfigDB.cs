using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI
{
    public partial class FormConfigDB : Form
    {
        public string ConnectionStringFinal { get; private set; }
        EfeitoUI M = new EfeitoUI();
        public FormConfigDB()
        {
            InitializeComponent();
        }

        private void ChkWinAuth_CheckedChanged(object sender, EventArgs e)
        {
            bool usarWindowsAuth = chkWinAuth.Checked;
            txtUser.Enabled = !usarWindowsAuth;
            txtPass.Enabled = !usarWindowsAuth;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string servidor = txtServidor.Text.Trim();
            string baseDados = txtBaseDados.Text.Trim();

            if (string.IsNullOrWhiteSpace(servidor) || string.IsNullOrWhiteSpace(baseDados))
            {
                M.AbrirMensagem("Servidor e Base de Dados são obrigatórios.", "Informação");
                return;
            }

            if (chkWinAuth.Checked)
            {
                // ADICIONADO O TrustServerCertificate=True AQUI!
                ConnectionStringFinal = $"Server={servidor};Database={baseDados};Integrated Security=True;TrustServerCertificate=True;";
            }
            else
            {
                string utilizador = txtUser.Text.Trim();
                string senha = txtPass.Text.Trim();

                if (string.IsNullOrWhiteSpace(utilizador) || string.IsNullOrWhiteSpace(senha))
                {
                    M.AbrirMensagem("Preencha o utilizador e a palavra-passe.", "Informação");
                    return;
                }

                // ADICIONADO O TrustServerCertificate=True AQUI TAMBÉM!
                ConnectionStringFinal = $"Server={servidor};Database={baseDados};User Id={utilizador};Password={senha};TrustServerCertificate=True;";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
