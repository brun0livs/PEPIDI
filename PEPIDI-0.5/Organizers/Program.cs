using PEPIDI.Utils;
using System;
using System.IO;
using System.Windows.Forms;

namespace PEPIDI.Organizers
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            EfeitoUI M = new();
            // 1. Inicializações de Sistema
            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 2. CARREGAR PREFERÊNCIAS DE ECRÃ
            string memoriaEcra = Properties.Settings.Default.ModoEcraGuardado;
            if (Enum.TryParse(memoriaEcra, out TipoEcra modoLido))
            {
                GestorTema.ModoAtual = modoLido;
            }
            else
            {
                GestorTema.ModoAtual = TipoEcra.MonitorFullHD; // Fallback seguro
            }

            // 3. DEFINIR CAMINHO DA BASE DE DADOS (Exclusivamente conn.bin)
            string pastaApp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PEPIDI");
            string caminhoArquivoBin = Path.Combine(pastaApp, "conn.bin");

            // Garante que a pasta AppData\PEPIDI existe
            if (!Directory.Exists(pastaApp))
                Directory.CreateDirectory(pastaApp);

            string connString = "";

            // 4. CARREGAR OU CONFIGURAR A STRING DE CONEXÃO
            if (File.Exists(caminhoArquivoBin))
            {
                // Lê o ficheiro encriptado de produção
                connString = Criptografia.DesencriptarDeFicheiro(caminhoArquivoBin);
            }
            else
            {
                // Se o ficheiro não existe, abre o formulário de configuração
                using FormConfigDB frm = new();

                // Título simplificado
                frm.Text = "Configurar Conexão com a Base de Dados";

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    connString = frm.ConnectionStringFinal;
                    Criptografia.EncriptarParaFicheiro(connString, caminhoArquivoBin);
                }
                else
                {
                    // Mensagem de cancelamento genérica e limpa
                    M.AbrirMensagem("Configuração da Base de Dados cancelada.\nA encerrar...", "Erro");
                    return;
                }
            }

            // 5. ATRIBUIR A CONEXÃO À CLASSE GLOBAL
            GetConn.ConnectionString = connString;

            //int nr = 1077;
            //Application.Run(new FormGestao(nr, PermissoesPerfil.VerPermissoes(nr)));
            Application.Run(new FrmLogIn());
        }
    }
}