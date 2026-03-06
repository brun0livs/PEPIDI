using Guna.UI2.WinForms;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.UCs.DGVS;
using PEPIDI.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PEPIDI
{
    public partial class FormGestao : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        int IDGestor;
        private readonly PermissoesPerfil permissoes;

        // Configuração do Menu
        bool menuAberto;
        int larguraMax;
        int larguraMin;
        const int velocidade = 35; // Velocidade da animação

        public FormGestao(int _idGestor, PermissoesPerfil _permissoes)
        {
            InitializeComponent();

            // Magia Anti-Tremor
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            IDGestor = _idGestor;
            permissoes = _permissoes;
        }

        // --- VACINA CONTRA TREMORES ---
        public static void SetDoubleBuffered(Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession) return;
            System.Reflection.PropertyInfo pi = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            pi.SetValue(c, true, null);
        }

        private void FormGestao_Load(object sender, EventArgs e)
        {
            // Aplicar o Double Buffer ao teu SplitContainer (pnlMenu) e aos painéis dentro dele!
            SetDoubleBuffered(pnlMenu);
            SetDoubleBuffered(pnlMenu.Panel1);
            SetDoubleBuffered(pnlMenu.Panel2);
            SetDoubleBuffered(tlpMenu);
            SetDoubleBuffered(tableLayoutPanel3);
            SetDoubleBuffered(pnlConteudo);

            this.KeyPreview = true;

            // 1. APLICAR O TEMA CENTRALIZADO
            GestorTema.AplicarEstilos(this);

            // 2. LER AS LARGURAS CORRETAS
            this.PerformLayout();
            larguraMin = Nav1.Height; // Fica quadrado perfeito
            larguraMax = GestorTema.ModoAtual == TipoEcra.Surface ? 350 : 220;

            // 3. CARREGAR UTILIZADOR E PERMISSÕES
            var info = Details.GetInfoGestor(IDGestor);
            lblNome.Text = info.Nome + " · " + info.Funcao;
            AplicarPermissoes();

            // Garante que as Tags estão preenchidas no arranque para os nomes não se perderem
            foreach (Control c in tlpMenu.Controls)
            {
                if (c is Guna2Button btn && btn.Tag == null)
                {
                    btn.Tag = btn.Text;
                }
            }

            AbrirPrimeiraPermissao();

            // 4. LER A SETTING E FORÇAR O TAMANHO INICIAL DO SPLITCONTAINER
            menuAberto = Properties.Settings.Default.MenuExpandido;

            if (menuAberto == true)
            {
                pnlMenu.SplitterDistance = larguraMax;
            }
            else
            {
                pnlMenu.SplitterDistance = larguraMin;
            }

            // 5. AJUSTAR OS TEXTOS/ÍCONES LOGO NO ARRANQUE
            ConfigurarBotoes(menuAberto);
        }

        private void AbrirPrimeiraPermissao()
        {
            Guna2Button[] botoesNavegacao = { Nav1, Nav2, Nav3, Nav4, Nav5, Nav6, Nav7, Nav9 };

            foreach (var btn in botoesNavegacao)
            {
                if (btn.Visible)
                {
                    Nav_Clicked(btn, EventArgs.Empty);
                    return;
                }
            }

            EfeitoUI M = new EfeitoUI();
            M.AbrirMensagem("Não tem permissões para aceder a nenhum módulo.", "Aviso de Acesso");
        }

        public bool PodeAceder(string key)
        {
            switch (key.ToLowerInvariant())
            {
                case "stock": return permissoes.PodeVerStock || permissoes.PodeCriarStock;
                case "funcionários":
                case "funcionarios": return permissoes.PodeEditarFunc;
                case "pedidos pendentes": return permissoes.PodeAprovar || permissoes.PodeEntregar;
                case "pedidos aprovados": return permissoes.PodeEntregar || permissoes.PodeAprovar;
                case "histórico":
                case "historico": return permissoes.PodeVerHistorico;
                case "criar artigos": return permissoes.PodeCriarStock;
                case "funções":
                case "funcoes": return permissoes.PodeCriarFuncoes;
                case "definições":
                case "definicoes": return permissoes.PodeAlterarDefinicoes;
                case "sair": return true;
                default: return false;
            }
        }

        private void AplicarPermissoes()
        {
            Nav1.Visible = Nav1.Enabled = false;
            Nav2.Visible = Nav2.Enabled = false;
            Nav3.Visible = Nav3.Enabled = false;
            Nav4.Visible = Nav4.Enabled = false;
            Nav5.Visible = Nav5.Enabled = false;
            Nav6.Visible = Nav6.Enabled = false;
            Nav7.Visible = Nav7.Enabled = false;
            Nav9.Visible = Nav9.Enabled = false;

            Nav10.Visible = true; Nav10.Enabled = true;

            if (PodeAceder("Stock")) { Nav1.Visible = true; Nav1.Enabled = true; }
            if (PodeAceder("Funcionários")) { Nav2.Visible = true; Nav2.Enabled = true; }
            if (PodeAceder("Pedidos Pendentes")) { Nav3.Visible = true; Nav3.Enabled = true; }
            if (PodeAceder("Pedidos Aprovados")) { Nav4.Visible = true; Nav4.Enabled = true; }
            if (PodeAceder("Histórico")) { Nav5.Visible = true; Nav5.Enabled = true; }
            if (PodeAceder("Criar Artigos")) { Nav6.Visible = true; Nav6.Enabled = true; }
            if (PodeAceder("Funções")) { Nav7.Visible = true; Nav7.Enabled = true; }
            if (PodeAceder("Definições")) { Nav9.Visible = true; Nav9.Enabled = true; }
        }

        private void Nav_Clicked(object sender, EventArgs e)
        {
            var clicked = (Guna2Button)sender;
            string chaveNavegacao = clicked.Tag != null ? clicked.Tag.ToString() : clicked.Text;

            if (chaveNavegacao.Equals("Sair", StringComparison.OrdinalIgnoreCase))
            {
                Close();
                return;
            }

            if (chaveNavegacao.Equals("Expandir", StringComparison.OrdinalIgnoreCase))
            {
                menuAberto = !menuAberto;

                // Guarda a definição do utilizador
                Properties.Settings.Default.MenuExpandido = menuAberto;
                Properties.Settings.Default.Save();

                // ESCONDE LOGO OS TEXTOS ANTES DE ANIMAR! (Isto evita o efeito esmagado)
                ConfigurarBotoes(menuAberto);

                timerMenu.Start();
                return;
            }

            if (!clicked.Checked)
            {
                foreach (Control c in tlpMenu.Controls)
                {
                    if (c is Guna2Button nb && nb.Checked)
                    {
                        nb.Checked = false;
                        nb.Image = ImageHelper.InverterCores(nb.Image);
                    }
                }

                clicked.Checked = true;
                clicked.Image = ImageHelper.InverterCores(clicked.Image);
            }

            Navegar(chaveNavegacao);
            //calcular percentagens de cada modulo

        }

        private async void Navegar(string key)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                await Task.Yield();
                UserControl controlParaAbrir = key.ToLowerInvariant() switch
                {
                    "stock" => new UCs.Stock(permissoes),
                    "funcionarios" or "funcionários" => new UCs.Funcionarios(IDGestor),
                    "pedidos pendentes" => new UCs.Pedidos(IDGestor, "Pendente"),
                    "pedidos aprovados" => new UCs.Pedidos(IDGestor, "Aprovado"),
                    "historico" or "histórico" => new UCs.Pedidos(IDGestor, "Finalizado"),
                    "criar artigos" => new UCs.CriarStock(),
                    "funções" => new UCs.Funcoes(IDGestor),
                    "definições" => new UCs.Definicoes(IDGestor),
                    _ => null
                };

                if (controlParaAbrir != null)
                {
                    AbrirControl(controlParaAbrir);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao navegar: {ex.Message}");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public void AbrirControl(UserControl control)
        {
            SendMessage(pnlConteudo.Handle, WM_SETREDRAW, false, 0);
            pnlConteudo.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlConteudo.Controls.Add(control);
            SendMessage(pnlConteudo.Handle, WM_SETREDRAW, true, 0);
            pnlConteudo.Refresh();
        }

        // O MOTOR DA ANIMAÇÃO DO TEU SPLITCONTAINER
        private void timerMenu_Tick(object sender, EventArgs e)
        {
            if (menuAberto) // ANIMAR PARA ABRIR
            {
                if (pnlMenu.SplitterDistance + velocidade < larguraMax)
                {
                    pnlMenu.SplitterDistance += velocidade;
                }
                else
                {
                    pnlMenu.SplitterDistance = larguraMax;
                    timerMenu.Stop();
                }
            }
            else // ANIMAR PARA FECHAR
            {
                if (pnlMenu.SplitterDistance - velocidade > larguraMin)
                {
                    pnlMenu.SplitterDistance -= velocidade;
                }
                else
                {
                    pnlMenu.SplitterDistance = larguraMin;
                    timerMenu.Stop();
                }
            }
        }

        // ESTA É A FUNÇÃO QUE METE OS ÍCONES BONITOS E APAGA O TEXTO
        private void ConfigurarBotoes(bool mostrarTexto)
        {
            foreach (Control c in tlpMenu.Controls)
            {
                if (c is Guna2Button btn)
                {
                    if (mostrarTexto)
                    {
                        // MODO ABERTO: Restaura texto da TAG e alinha à esquerda
                        if (btn.Tag != null) btn.Text = btn.Tag.ToString();
                        btn.ImageAlign = HorizontalAlignment.Left;
                        btn.TextAlign = HorizontalAlignment.Left;
                    }
                    else
                    {
                        // MODO FECHADO: Apaga texto e alinha ícone ao centro
                        if (btn.Tag == null || string.IsNullOrEmpty(btn.Tag.ToString()))
                        {
                            btn.Tag = btn.Text;
                        }
                        btn.Text = "";
                        btn.ImageAlign = HorizontalAlignment.Center;
                        btn.TextAlign = HorizontalAlignment.Center;
                    }
                }
            }
        }

        private void FormGestao_KeyPress(object sender, KeyPressEventArgs e) { }

        private void FormGestao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.CenterToScreen();
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void FormGestao_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            AdaptarDPI();
        }

        private void AdaptarDPI()
        {
            float dpiScale = this.DeviceDpi / 96f;
            if (dpiScale <= 1f) return;

            larguraMax = (int)(300 * dpiScale);
            if (menuAberto) pnlMenu.SplitterDistance = larguraMax;

            int tamanhoIcone = (int)(43 * dpiScale);
            Size novoTamanhoGuna = new Size(tamanhoIcone, tamanhoIcone);

            Nav1.ImageSize = novoTamanhoGuna;
            Nav2.ImageSize = novoTamanhoGuna;
            Nav3.ImageSize = novoTamanhoGuna;
            Nav4.ImageSize = novoTamanhoGuna;
            Nav5.ImageSize = novoTamanhoGuna;
            Nav6.ImageSize = novoTamanhoGuna;
            Nav7.ImageSize = novoTamanhoGuna;
            Nav8.ImageSize = novoTamanhoGuna;
            Nav9.ImageSize = novoTamanhoGuna;
            Nav10.ImageSize = novoTamanhoGuna;
        }

        private void pnlMenu_SplitterMoved(object sender, SplitterEventArgs e)
        {
            MessageBox.Show("Splitter Distance: " + pnlMenu.SplitterDistance);
        }
    }
}