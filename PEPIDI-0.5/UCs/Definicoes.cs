using PEPIDI.Utils;
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
            TipoEcra novoModo = TipoEcra.MonitorFullHD; // Padrão

            // 1. Descobrir qual foi a escolha
            if (escolha.Contains("Tablet") || escolha.Contains("Surface"))
                novoModo = TipoEcra.Surface;
            else if (escolha.Contains("Portátil"))
                novoModo = TipoEcra.Portatil;
            // Podes adicionar mais IFs se tiveres mais modos

            // 2. Perguntar se quer reiniciar agora
            DialogResult resposta = MessageBox.Show(
                "O programa vai reiniciar para aplicar a nova resolução. Queres continuar?",
                "Alterar Resolução",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
            {
                // 3. GUARDAR NAS SETTINGS!
                Properties.Settings.Default.ModoEcraGuardado = novoModo.ToString();
                Properties.Settings.Default.Save(); // Escreve no disco rígido do utilizador

                // 4. REINICIAR (Fecha e volta a abrir instantaneamente)
                Application.Restart();
                Environment.Exit(0);
            }return;
        }
    }
}
