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
                MessageBox.Show("Servidor e Base de Dados são obrigatórios.");
                return;
            }

            if (chkWinAuth.Checked)
            {
                ConnectionStringFinal = $"Server={servidor};Database={baseDados};Integrated Security=True;";
            }
            else
            {
                string utilizador = txtUser.Text.Trim();
                string senha = txtPass.Text.Trim();

                if (string.IsNullOrWhiteSpace(utilizador) || string.IsNullOrWhiteSpace(senha))
                {
                    MessageBox.Show("Preencha o utilizador e a palavra-passe.");
                    return;
                }

                ConnectionStringFinal = $"Server={servidor};Database={baseDados};User Id={utilizador};Password={senha};";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormConfigDB_Load(object sender, EventArgs e)
        {

        }
    }
}
