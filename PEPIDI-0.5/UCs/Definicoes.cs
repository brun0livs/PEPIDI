using PEPIDI.Utils;
using System;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Definicoes : UserControl
    {
        int IDGestor;
        bool aCarregar = true; // <-- O nosso travão de segurança!
        EfeitoUI M = new ();

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
            GestorTema.AplicarEstilos(this);

            // 1. Injetar os itens por código (Assim tens a certeza absoluta que o texto bate certo)
            // Se já os tens no Designer, podes apagar de lá para não haver duplicados.
            cmbDisplay.Items.Clear();
            cmbDisplay.Items.Add("Modo Monitor (1080p)");
            cmbDisplay.Items.Add("Modo Portátil (1366x768)");
            cmbDisplay.Items.Add("Modo Tátil (Tablet/Surface)");

            // 2. Ir buscar a memória (ex: "Surface", "MonitorFullHD", etc)
            string modoGuardado = Properties.Settings.Default.ModoEcraGuardado;

            // 3. Traduzir a memória para o texto exato da ComboBox
            if (modoGuardado == "Surface")
                cmbDisplay.SelectedItem = "Modo Tátil (Tablet/Surface)";
            else if (modoGuardado == "Portatil")
                cmbDisplay.SelectedItem = "Modo Portátil (1366x768)";
            else
                cmbDisplay.SelectedItem = "Modo Monitor (1080p)"; // O defeito

            // 4. Tirar o travão de segurança agora que tudo está carregado
            aCarregar = false;

            // 5. Verificar o que está na memória ("Teste" ou "Real")
            string modoAtual = Properties.Settings.Default.ModoBD;

            // 6. Colocar o Switch na posição correta SEM disparar o evento de mensagem
            // Desativamos o evento temporariamente para não abrir a MessageBox ao carregar o ecrã
            switchBD.CheckedChanged -= switchBD_CheckedChanged;

            switchBD.Checked = (modoAtual == "Real");

            switchBD.CheckedChanged += switchBD_CheckedChanged;
        }

        private void cmbDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Se o ecrã ainda estiver a carregar, ignoramos o evento para não pedir para reiniciar à toa!
            if (aCarregar || cmbDisplay.SelectedItem == null) return;

            string escolha = cmbDisplay.SelectedItem.ToString();
            TipoEcra novoModo = TipoEcra.MonitorFullHD; // Padrão

            // 1. Descobrir qual foi a escolha
            if (escolha.Contains("Tablet") || escolha.Contains("Surface"))
                novoModo = TipoEcra.Surface;
            else if (escolha.Contains("Portátil"))
                novoModo = TipoEcra.Portatil;

            // 2. Perguntar se quer reiniciar agora
            DialogResult resposta = MessageBox.Show(
                "O programa vai reiniciar para aplicar a nova resolução. Queres continuar?",
                "Alterar Resolução",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
            {
                // 3. GUARDAR NAS SETTINGS
                Properties.Settings.Default.ModoEcraGuardado = novoModo.ToString();
                Properties.Settings.Default.Save();

                // 4. REINICIAR
                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                // Se ele cancelar, voltamos a meter o travão e revertemos a ComboBox para o que estava guardado
                aCarregar = true;
                Definicoes_Load(null, null); // Re-executa o Load para meter o valor antigo
            }
        }

        private void switchBD_CheckedChanged(object sender, EventArgs e)
        {
            // False = Teste (Local)
            // True = Real (Azure)
            string novoModo = switchBD.Checked ? "Real" : "Teste";

            // 1. Grava a preferência nas Settings do projeto
            Properties.Settings.Default.ModoBD = novoModo;
            Properties.Settings.Default.Save();

            // 2. Feedback visual para o utilizador
            if (switchBD.Checked)
            {
                M.AbrirMensagem("Modo REAL (Azure) selecionado.\n\n" +
                               "Atenção: A aplicação precisa de ser reiniciada para estabelecer a ligação à Nuvem.",
                               "Alteração de Servidor");
            }
            else
            {
                M.AbrirMensagem("Modo TESTE (Local) selecionado.\n\n" +
                               "A aplicação precisa de ser reiniciada para ligar ao servidor local.",
                               "Alteração de Servidor");
            }
        }
    }
}