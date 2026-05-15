using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AgentePEPIDI
{
    public class ContextoDoAgente : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Timer timerMonitorizacao;
        private FormDetalhesStock janelinhaDetalhes = null;

        // 2. Muda o nome do construtor para ser igual à classe:
        public ContextoDoAgente()
        {
            // 1. Configurar o Menu de Contexto (botão direito no ícone)
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Sair do Agente", null, Sair_Click);

            // 2. Configurar o Ícone na área de notificação (perto do relógio)
            trayIcon = new NotifyIcon()
            {
                Icon = new Icon("logo.ico"),
                ContextMenuStrip = menu,
                Visible = true,
                Text = "Agente PEPIDI" // Texto que aparece ao passar o rato
            };

            // Dizemos ao Windows: "Quando clicarem no balão, corre este método"
            trayIcon.BalloonTipClicked += TrayIcon_BalloonTipClicked;

            // 3. Configurar o Timer para verificações periódicas (ex: a cada 60 seg)
            timerMonitorizacao = new Timer
            {
                Interval = 300000 // 300.000 milissegundos = 5 MUNUTOS
            };
            timerMonitorizacao.Tick += TimerMonitorizacao_Tick;
            timerMonitorizacao.Start();

            // Vai mostrar o balão mal o agente ligue, para provar que está vivo!
            MostrarAlerta("Agente Iniciado", "O Agente PEPIDI está a correr em segundo plano!");
        }

        private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            // 1. Se a janela ainda não existe ou se foi fechada (Disposed), criamos uma nova
            if (janelinhaDetalhes == null || janelinhaDetalhes.IsDisposed)
            {
                janelinhaDetalhes = new FormDetalhesStock();
                janelinhaDetalhes.Show();
            }
            else
            {
                // 2. Se ela já estiver aberta, apenas a trazemos para a frente
                if (janelinhaDetalhes.WindowState == FormWindowState.Minimized)
                    janelinhaDetalhes.WindowState = FormWindowState.Normal;

                janelinhaDetalhes.BringToFront();
            }
        }

        // Método que vai correr sempre que o relógio (Timer) "bater"
        private async void TimerMonitorizacao_Tick(object sender, EventArgs e)
        {
            try
            {
                timerMonitorizacao.Stop(); // Pausa para não encavalar consultas

                using (SqlConnection conn = CONN.GetConnection())
                {
                    await conn.OpenAsync();

                    // 1. Ir buscar o limite às definições (exemplo de query)
                    int limite = 5; // Valor padrão caso falhe
                    string sqlDef = "SELECT Valor FROM Definicoes WHERE Chave = 'StockMinimo'";
                    using (SqlCommand cmdDef = new SqlCommand(sqlDef, conn))
                    {
                        var result = await cmdDef.ExecuteScalarAsync();
                        if (result != null) limite = Convert.ToInt32(result);
                    }

                    // 2. Verificar quantos itens estão abaixo desse limite
                    string sqlStock = "SELECT COUNT(*) FROM Stock WHERE Quant <= @lim";
                    using (SqlCommand cmdStock = new SqlCommand(sqlStock, conn))
                    {
                        cmdStock.Parameters.AddWithValue("@lim", limite);
                        int contagem = (int)await cmdStock.ExecuteScalarAsync();

                        if (contagem > 0)
                        {
                            // 3. Disparar o alerta com o número real!
                            MostrarAlerta("PEPIDI - Alerta de Stock", $"Atenção: Há {contagem} itens com stock baixo ou em falta!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Se der erro (ex: BD desligada), o agente fica calado para não chatear
                Console.WriteLine(ex.Message);
            }
            finally
            {
                timerMonitorizacao.Start(); // Retoma o ciclo
            }
        }

        private void MostrarAlerta(string titulo, string mensagem)
        {
            // O primeiro parâmetro (ex: 30000) é o tempo em milissegundos (30 segundos) 
            // que o sistema tentará mostrar o balão. 
            // (Nota: O Windows 10 e 11 muitas vezes gerem este tempo pelas suas próprias regras de notificações).

            // ToolTipIcon.Warning mete aquele triângulo amarelo de aviso no balão.
            // Podes mudar para ToolTipIcon.Info ou ToolTipIcon.Error consoante a gravidade.
            trayIcon.ShowBalloonTip(30000, titulo, mensagem, ToolTipIcon.Error);
        }

        // Método para fechar o Agente de forma limpa
        private void Sair_Click(object sender, EventArgs e)
        {
            // É fundamental esconder o ícone antes de sair, senão ele fica lá "fantasma" até passares o rato
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}