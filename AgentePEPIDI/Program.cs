using System;
using System.IO;
using System.Windows.Forms;
// Adiciona aqui o 'using' para a pasta onde meteste a Criptografia e o CONN, se necessário!

namespace AgentePEPIDI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Procurar a pasta mágica do AppData
            string pastaApp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PEPIDI");

            // Removida a lógica de teste. O Agente aponta sempre para o ficheiro principal.
            string caminhoArquivoBin = Path.Combine(pastaApp, "conn.bin");

            // 2. Verificar se a configuração da BD já foi feita pela App Principal
            if (File.Exists(caminhoArquivoBin))
            {
                try
                {
                    // 3. Desencriptar a morada e injetar na classe de ligação (Ajusta CONN para GetConn se for o caso)
                    string connString = Criptografia.DesencriptarDeFicheiro(caminhoArquivoBin);
                    CONN.ConnectionString = connString;

                    // 4. Se chegou aqui com sucesso, ARRANCAR O MOTOR!
                    ApplicationContext contextoAgente = new ContextoDoAgente();
                    Application.Run(contextoAgente);
                }
                catch (Exception ex)
                {
                    // Se falhar a desencriptar (ex: chave errada), encerra silenciosamente
                    Console.WriteLine("Erro crítico no arranque do Agente: " + ex.Message);
                }
            }
            else
            {
                // Se o ficheiro conn.bin não existe, a App Principal ainda não foi configurada.
                // O Agente morre aqui, sem chatear o utilizador com pop-ups.
                Console.WriteLine("Ficheiro de configuração não encontrado. O Agente vai encerrar.");
            }
        }
    }
}