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
    public partial class FormMotivo : Form
    {
        public string Motivo { get; private set; }

        public FormMotivo()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Por favor, escreva o motivo.");
                return;
            }
            Motivo = txtMotivo.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
