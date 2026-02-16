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
    public partial class MSGBX : Form
    {
        public MSGBX(string _Message, string _Titulo)
        {
            InitializeComponent();
            lblMessage.Text = _Message;
            lblTitulo.Text = "PEPIDI | " + _Titulo;
            this.CancelButton = btnOK; // Permite fechar a caixa de mensagem com a tecla ESC
            this.AcceptButton = btnOK; // Permite fechar a caixa de mensagem com a tecla ENTER
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
