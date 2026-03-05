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
        // 3. AS REGRAS MULTI-ECRÃ (Usando o Switch moderno do C#)
        // ==========================================

        // LABELS
        public static Font FonteLabel => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto", 9F, FontStyle.Regular),
            TipoEcra.MonitorFullHD => new Font("Roboto", 11F, FontStyle.Regular),
            TipoEcra.Surface => new Font("Roboto", 16F, FontStyle.Regular),
            TipoEcra.Televisao => new Font("Roboto", 24F, FontStyle.Regular),
            _ => new Font("Roboto", 11F, FontStyle.Regular) // Segurança
        };

        public static Font FonteTitulo => ModoAtual switch
        {
            TipoEcra.Portatil => new Font("Roboto", 16F, FontStyle.Bold),
            TipoEcra.MonitorFullHD => new Font("Roboto", 18F, FontStyle.Bold),
            TipoEcra.Surface => new Font("Roboto", 24F, FontStyle.Bold),
            TipoEcra.Televisao => new Font("Roboto", 40F, FontStyle.Bold),
            _ => new Font("Roboto", 18F, FontStyle.Bold)
        };

        // BOTÕES
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
            TipoEcra.Surface => new Size(64, 64),
            TipoEcra.Televisao => new Size(96, 96),
            _ => new Size(43, 43)
        };

        // GRELHAS (DATAGRIDVIEWS)
        public static int AlturaLinhaDgv => ModoAtual switch
        {
            TipoEcra.Portatil => 30,
            TipoEcra.MonitorFullHD => 35,
            TipoEcra.Surface => 55,
            TipoEcra.Televisao => 80,
            _ => 35
        };

        // LARGURAS DO MENU LATERAL
        public static int LarguraMenuExpandido => ModoAtual switch
        {
            TipoEcra.Portatil => 250,
            TipoEcra.MonitorFullHD => 310, // O teu valor normal
            TipoEcra.Surface => 450,       // Maior para o Surface
            TipoEcra.Televisao => 600,
            _ => 310
        };

        public static int LarguraMenuRecolhido => ModoAtual switch
        {
            TipoEcra.Portatil => 60,
            TipoEcra.MonitorFullHD => 79,  // O teu quadrado perfeito no PC
            TipoEcra.Surface => 115,       // O novo quadrado perfeito para o Surface! (Ajusta este valor se precisares)
            TipoEcra.Televisao => 150,
            _ => 79
        };

        // (E continuas a adicionar as tuas Fontes e Alturas para a Grelha da mesma forma...)

        // ==========================================
        // 4. O PINTOR (Fica Exatamente Igual!)
        // ==========================================
        public static void AplicarEstilos(Control pai)
        {
            // O teu código do AplicarEstilos que te dei na mensagem anterior fica aqui.
            // A genialidade disto é que o Pintor NÃO MUDA! 
            // Ele só vai pedir a "FonteLabel" e a propriedade acima dá-lhe o tamanho 
            // correto automaticamente, consoante o "ModoAtual" selecionado.

            // ... (Cola aqui o foreach (Control c in pai.Controls) de antes) ...
        }
    }
}