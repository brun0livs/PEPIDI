using Microsoft.VisualBasic.Logging;
using PEPIDI;
using PEPIDI.Organizers;

namespace PEPIDI.Organizers
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string pastaApp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PEPIDI");
            string caminhoBin = Path.Combine(pastaApp, "conn.bin");

            // Garante que a pasta existe
            if (!Directory.Exists(pastaApp))
                Directory.CreateDirectory(pastaApp);

            string connString = "";

            if (File.Exists(caminhoBin))
            {
                connString = Criptografia.DesencriptarDeFicheiro(caminhoBin);
            }
            else
            {
                using FormConfigDB frm = new();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    connString = frm.ConnectionStringFinal;
                    Criptografia.EncriptarParaFicheiro(connString, caminhoBin);
                }
                else
                {
                    EfeitoUI M = new EfeitoUI();
                    M.AbrirMensagem("Configurção cancelada.\nA encerrar...", "Erro");
                    return;
                }
            }

            GetConn.ConnectionString = connString;
            //int nr = 1077;
            //Application.Run(new FormGestao(nr, PermissoesPerfil.VerPermissoes(nr)));
            //Application.Run(new FrmConsumosDetalhados(1016));
            Application.Run(new FrmLogIn());
            //Application.Run(new Finalizacao(24, 666));
            //Application.Run(new FrmCv());
        }
    }
}