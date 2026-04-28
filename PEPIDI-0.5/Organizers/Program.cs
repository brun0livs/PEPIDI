using PEPIDI.Utils;

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

            // 3. LÓGICA DE SELEÇÃO DA BASE DE DADOS (TESTE vs REAL)
            // Lemos a string que criaste nas Settings ("Teste" ou "Real")
            string modoBDAtivo = Properties.Settings.Default.ModoBD;

            string pastaApp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PEPIDI");

            // Se for Teste, lê o 'connteste.bin'. Se for Real (ou qualquer outra coisa), lê o 'conn.bin'
            string nomeFicheiro = (modoBDAtivo == "Teste") ? "connteste.bin" : "conn.bin";
            string caminhoArquivoBin = Path.Combine(pastaApp, nomeFicheiro);

            // Garante que a pasta AppData\PEPIDI existe
            if (!Directory.Exists(pastaApp))
                Directory.CreateDirectory(pastaApp);

            string connString = "";

            // 4. CARREGAR OU CONFIGURAR A STRING DE CONEXÃO
            if (File.Exists(caminhoArquivoBin))
            {
                // Tenta ler o ficheiro encriptado correspondente ao modo ativo
                connString = Criptografia.DesencriptarDeFicheiro(caminhoArquivoBin);
            }
            else
            {
                // Se o ficheiro do modo escolhido não existe, pede ao utilizador para configurar
                using FormConfigDB frm = new();

                // Personaliza o título do form para saberes o que estás a configurar
                frm.Text = $"Configurar Conexão: MODO {modoBDAtivo.ToUpper()}";

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    connString = frm.ConnectionStringFinal;
                    Criptografia.EncriptarParaFicheiro(connString, caminhoArquivoBin);
                }
                else
                {
                    M.AbrirMensagem($"Configuração do modo {modoBDAtivo} cancelada.\nA encerrar...", "Erro");
                    return;
                }
            }

            // 5. ATRIBUIR A CONEXÃO À CLASSE GLOBAL
            GetConn.ConnectionString = connString;
            int nr = 1077;
            Application.Run(new FormGestao(nr, PermissoesPerfil.VerPermissoes(nr)));
            //Application.Run(new FrmLogIn());
        }
    }
}