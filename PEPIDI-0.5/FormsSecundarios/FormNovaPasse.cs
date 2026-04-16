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
        int IDGestor;
        public FormNovaPasse(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }

        private void Verifica(object sender, EventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        private void Close(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
