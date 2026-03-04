using System.Drawing;

namespace PEPIDI.Utils // Ou a pasta onde metes as tuas classes soltas
{
    public static class GestorTamanhos
    {
        // Variável que guarda o modo atual (pode vir da Base de Dados ou das Settings)
        public static string ModoAtual { get; set; } = "Normal";

        // ==========================================
        // AS TUAS DEFINIÇÕES DE TAMANHOS
        // ==========================================

        public static int LarguraMenuLateral
        {
            get { return ModoAtual == "Surface" ? 450 : 300; }
        }

        public static Size TamanhoIconeMenu
        {
            get { return ModoAtual == "Surface" ? new Size(64, 64) : new Size(43, 43); }
        }

        public static Font FonteTitulos
        {
            get { return ModoAtual == "Surface" ? new Font("Roboto", 30F, FontStyle.Bold) : new Font("Roboto", 20F, FontStyle.Bold); }
        }

        public static Font FonteGeral
        {
            get { return ModoAtual == "Surface" ? new Font("Roboto", 16F, FontStyle.Regular) : new Font("Roboto", 11F, FontStyle.Regular); }
        }

        public static int AlturaLinhasGrelha
        {
            get { return ModoAtual == "Surface" ? 60 : 35; }
        }
    }
}