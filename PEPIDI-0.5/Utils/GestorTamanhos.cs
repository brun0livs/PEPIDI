using PEPIDI.Models;
using System.Drawing;
using System.Windows.Forms;

namespace PEPIDI.Utils
{
    // 1. A TUA LISTA DE ECRÃS SUPORTADOS
    public enum TipoEcra
    {
        Portatil,       // Ecrãs pequenos (ex: 1366x768)
        MonitorFullHD,  // O teu PC normal (ex: 1920x1080)
        Surface,        // Ecrãs táteis ou com muito zoom (Letras gigantes)
        Televisao       // Caso no futuro metam o programa numa TV de 55"
    }

    public static class GestorTema
    {
        // 2. A VARIÁVEL QUE DIZ ONDE ESTAMOS A CORRER
        public static TipoEcra ModoAtual { get; set; } = TipoEcra.MonitorFullHD; // Padrão

        // ==========================================
        // 3. AS REGRAS MULTI-ECRÃ
        // ==========================================

        // LABELS
        public static Font FonteLabel => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto", 9F, FontStyle.Regular),
            TipoEcra.MonitorFullHD => new Font("Roboto", 11F, FontStyle.Regular),
            TipoEcra.Surface => new Font("Roboto", 12F, FontStyle.Regular),
            TipoEcra.Televisao => new Font("Roboto", 24F, FontStyle.Regular),
            _ => new Font("Roboto", 11F, FontStyle.Regular)
        };

        public static Font FonteTitulo => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto Medium", 12F, FontStyle.Regular),
            TipoEcra.MonitorFullHD => new Font("Roboto Medium", 16F, FontStyle.Regular),
            TipoEcra.Surface => new Font("Roboto Medium", 16F, FontStyle.Regular),
            TipoEcra.Televisao => new Font("Roboto Medium", 25F, FontStyle.Regular),
            _ => new Font("Roboto Medium", 16F, FontStyle.Regular)
        };

        public static Font FonteNome => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto", 12F, FontStyle.Regular),
            TipoEcra.MonitorFullHD => new Font("Roboto", 16F, FontStyle.Regular),
            TipoEcra.Surface => new Font("Roboto", 16F, FontStyle.Regular),
            TipoEcra.Televisao => new Font("Roboto", 25F, FontStyle.Regular),
            _ => new Font("Roboto", 16F, FontStyle.Regular)
        };

        // BOTÕES
        public static Font FonteBotaoNAV => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto", 12F, FontStyle.Regular),
            TipoEcra.MonitorFullHD => new Font("Roboto", 16F, FontStyle.Regular),
            TipoEcra.Surface => new Font("Roboto", 16F, FontStyle.Regular),
            TipoEcra.Televisao => new Font("Roboto", 25F, FontStyle.Regular),
            _ => new Font("Roboto", 16F, FontStyle.Regular)
        };

        public static Font FonteBotao => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto", 11F, FontStyle.Regular),
            TipoEcra.MonitorFullHD => new Font("Roboto", 12F, FontStyle.Regular),
            TipoEcra.Surface => new Font("Roboto", 12F, FontStyle.Regular),
            TipoEcra.Televisao => new Font("Roboto", 20F, FontStyle.Regular),
            _ => new Font("Roboto", 12F, FontStyle.Regular)
        };

        public static Padding PaddingBotao => ModoAtual switch
        {
            TipoEcra.Portatil => new Padding(5, 2, 5, 2),
            TipoEcra.MonitorFullHD => new Padding(10, 5, 10, 5),
            TipoEcra.Surface => new Padding(20, 15, 20, 15),
            TipoEcra.Televisao => new Padding(30, 20, 30, 20),
            _ => new Padding(10, 5, 10, 5)
        };

        public static Size TamanhoIconeBotao => ModoAtual switch
        {
            TipoEcra.Portatil => new Size(32, 32),
            TipoEcra.MonitorFullHD => new Size(43, 43),
            TipoEcra.Surface => new Size(80, 80), // O teu ícone grande para o Surface!
            TipoEcra.Televisao => new Size(128, 128),
            _ => new Size(43, 43)
        };

        // GRELHAS (DATAGRIDVIEWS)
        public static int AlturaLinhaDgv => ModoAtual switch
        {
            TipoEcra.Portatil => 50,
            TipoEcra.MonitorFullHD => 54,
            TipoEcra.Surface => 80,
            TipoEcra.Televisao => 85,
            _ => 35
        };

        // LARGURAS DO MENU LATERAL
        public static int LarguraMenuExpandido => ModoAtual switch
        {
            TipoEcra.Portatil => 250,
            TipoEcra.MonitorFullHD => 310,
            TipoEcra.Surface => 450,
            TipoEcra.Televisao => 600,
            _ => 310
        };

        public static int LarguraMenuRecolhido => ModoAtual switch
        {
            TipoEcra.Portatil => 60,
            TipoEcra.MonitorFullHD => 79,
            TipoEcra.Surface => 115,
            TipoEcra.Televisao => 150,
            _ => 79
        };

        public static int AlturaCombos => ModoAtual switch
        {
            TipoEcra.Portatil => 28,
            TipoEcra.MonitorFullHD => 36,
            TipoEcra.Surface => 50,
            TipoEcra.Televisao => 50,
            _ => 36
        };

        // ==========================================
        // 4. O PINTOR (O Scanner Corrigido!)
        // ==========================================
        public static void AplicarEstilos(Control pai)
        {
            // O FOREACH É OBRIGATÓRIO! É ele que entra nos painéis e procura os botões.
            foreach (Control c in pai.Controls)
            {
                if (c is Guna.UI2.WinForms.Guna2Button btn)
                {
                    if (btn.Name.StartsWith("Nav"))
                    {
                        btn.Font = FonteBotaoNAV;
                        btn.CheckedState.Font = FonteBotaoNAV;
                        btn.Padding = PaddingBotao;
                        btn.ImageSize = TamanhoIconeBotao;
                    }else if (btn.Name.Contains("btn"))
                    {
                        btn.Font = FonteBotao;
                    }

                }
                else if (c is Label lbl)
                {
                    if (lbl.Name.Contains("lblNome"))
                        lbl.Font = FonteNome;
                    else if (lbl.Name.Contains("Titulo"))
                        lbl.Font = FonteTitulo;
                    else
                        lbl.Font = FonteLabel;
                }else if (c is Guna.UI2.WinForms.Guna2ComboBox cmb)
                {
                    cmb.Font = FonteLabel;
                    cmb.ItemHeight = AlturaCombos;
                }
                else if (c is PEPIDIDataGridView dgv)
                {
                    dgv.RowTemplate.Height = AlturaLinhaDgv;
                }

                // A RECURSIVIDADE: Se for um painel (como o teu menu), ele entra lá para dentro e pinta os botões!
                if (c.HasChildren)
                {
                    AplicarEstilos(c);
                }
            }
        }
    }
}