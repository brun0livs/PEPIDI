using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI_0._5.UCs.DGVS
{
    public partial class LinhaItem : UserControl
    {
        string M;
        string T;
        int QD;
        int QA;

        public LinhaItem(string _M, string _T, int _QD, int _QA)
        {
            InitializeComponent();
            M = _M;
            T = _T;
            QD = _QD;
            QA = _QA;
            // Isto faz com que, ao rodar o rato sobre a linha, o comando passe para o painel pai
            this.MouseWheel += (s, e) => {
                if (this.Parent != null)
                {
                    // Envia a mensagem de scroll para o painel que contém as linhas
                    HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
                    ee.Handled = true; // Impede que o UC tente dar scroll em si mesmo
                                       // Invoca o scroll do painel pai
                    this.Parent.Focus();
                }
            };
        }

        private void Linha_Load(object sender, EventArgs e)
        {
            lblModelo.Text = M;
            lblTamanho.Text = T;
            lblQuantDisp.Text = QD.ToString();

            cmbQuant.Items.Clear();

            // 1. Calculamos o limite: o menor entre QD e 5
            // Se QD for 3, o limite é 3. Se QD for 10, o limite é 5.
            int limiteMaximo = Math.Min(QD, 5);

            // 2. Preenchemos de 0 até esse limite
            for (int i = 0; i <= limiteMaximo; i++)
            {
                cmbQuant.Items.Add(i);
            }

            // 3. Selecionamos a quantidade atual
            // Se por algum motivo QA for maior que o limite, forçamos o limite
            if (QA > limiteMaximo)
                cmbQuant.Text = limiteMaximo.ToString();
            else
                cmbQuant.Text = QA.ToString();
        }
    }
}
