using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Definicoes : UserControl
    {
        int IDGestor;

        public Definicoes(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }

        private void Definicoes_Load(object sender, EventArgs e)
        {
            if (IDGestor == 1077)
            {
                pnlDefsPrev.Visible = true;
            }
        }

        private void cmbDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDisplay.SelectedItem == null) return;

            Form janelaPrincipal = this.FindForm();
            if (janelaPrincipal == null) return;

            string escolha = cmbDisplay.SelectedItem.ToString();

            // Descobre em que monitor físico o programa está atualmente a ser exibido
            Screen ecraAtual = Screen.FromControl(janelaPrincipal);

            switch (escolha)
            {
                case "Automático (Preencher Ecrã)":
                    // Tira do maximizado para a janela poder ser redimensionada livremente
                    janelaPrincipal.WindowState = FormWindowState.Normal;

                    // O "WorkingArea" é a medida exata do ecrã DESCONTANDO a barra de tarefas do Windows.
                    // Isto faz com que o programa ocupe 100% do ecrã disponível, seja um monitor de 24" ou um ecrã de 15".
                    janelaPrincipal.Bounds = ecraAtual.WorkingArea;
                    break;

                case "Maximizado (Fixo)":
                    // Modo maximizado clássico do Windows
                    janelaPrincipal.WindowState = FormWindowState.Maximized;
                    break;

                case "Portátil (HD)":
                    // Força a resolução típica de portáteis mais pequenos
                    janelaPrincipal.WindowState = FormWindowState.Normal;
                    janelaPrincipal.Size = new Size(1366, 768);

                    // Centrar manualmente no ecrã atual (Substitui o CenterToScreen)
                    janelaPrincipal.Location = new Point(
                        ecraAtual.WorkingArea.Left + (ecraAtual.WorkingArea.Width - janelaPrincipal.Width) / 2,
                        ecraAtual.WorkingArea.Top + (ecraAtual.WorkingArea.Height - janelaPrincipal.Height) / 2
                    );
                    break;

                case "Monitor (Full HD)":
                    // Força a resolução de monitores standard
                    janelaPrincipal.WindowState = FormWindowState.Normal;
                    janelaPrincipal.Size = new Size(1920, 1080);

                    // Centrar manualmente no ecrã atual
                    janelaPrincipal.Location = new Point(
                        ecraAtual.WorkingArea.Left + (ecraAtual.WorkingArea.Width - janelaPrincipal.Width) / 2,
                        ecraAtual.WorkingArea.Top + (ecraAtual.WorkingArea.Height - janelaPrincipal.Height) / 2
                    );
                    break;
            }
        }
    }
}
