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

            string escolha = cmbDisplay.SelectedItem.ToString();

            // Mostra um aviso ao utilizador de que o programa vai reiniciar
            DialogResult resposta = MessageBox.Show(
                "Para aplicar o novo tamanho de ecrã, o programa precisa de ser reiniciado. Queres reiniciar agora?",
                "Reiniciar PEPIDI",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
            {
                // Aqui tu guardas a escolha numa Settings file ou na Base de Dados
                // Properties.Settings.Default.ModoEcra = escolha;
                // Properties.Settings.Default.Save();

                // Faz um restart limpo à aplicação!
                Application.Restart();
                Environment.Exit(0); // Garante que o processo atual morre imediatamente
            }
            else
            {
                // Se ele disser que não, voltas a meter a combobox como estava antes
            }
        }
    }
}
