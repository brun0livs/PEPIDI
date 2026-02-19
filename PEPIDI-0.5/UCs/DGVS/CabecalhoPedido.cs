using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs.UcsSecundarios
{
    public partial class CabecalhoPedido : UserControl
    {
        public CabecalhoPedido(string estado)
        {
            InitializeComponent();
            GereEstado(estado);
        }

        private void GereEstado(string estado)
        {
            if (estado == "Aprovado")
            {
                lblQuantDisp.Text = "Quantidade";
                lblQuant.Text = "Selecionar";
            }
            else
            {
                return;
            }
        }
    }
}
