using Guna.UI2.WinForms;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.UCs.DGVS;
using PEPIDI.Utils;
using System;
using System.Drawing; // Necessário para HorizontalAlignment não dar erro de ambiguidade
using System.Windows.Forms;

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
        bool menuExpandido = Properties.Settings.Default.MenuEspandido;
        int larguraMax;
        int larguraMin; // <-- Deixou de ser const e perdeu o 79!
        const int velocidade = 75; // A velocidade da animação pode continuar fixa

        public FormGestao(int _idGestor, PermissoesPerfil _permissoes)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            // Força o motor do Windows a usar estilos modernos de desenho
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            IDGestor = _idGestor;
            permissoes = _permissoes;
            splitContainer1.SplitterDistance = larguraMax;
        }

        public static void SetDoubleBuffered(Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession) return;
            System.Reflection.PropertyInfo pi = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            pi.SetValue(c, true, null);
        }



        private void FormGestao_Load(object sender, EventArgs e)
        {
            SetDoubleBuffered(splitContainer1.Panel1);
            SetDoubleBuffered(splitContainer1.Panel2);
            this.KeyPreview = true;

            // ==========================================
            // 1. APLICAR O TEMA CENTRALIZADO (O "CSS")
            // ==========================================
            GestorTema.AplicarEstilos(this);

            // Definir a largura do menu consoante o ecrã
            larguraMax = GestorTema.ModoAtual == TipoEcra.Surface ? 450 : 310;
            splitContainer1.SplitterDistance = larguraMax;
            // ==========================================

            var info = Details.GetInfoGestor(IDGestor);
            lblNome.Text = info.Nome + " · " + info.Funcao;
            AplicarPermissoes();

            // Garante que as Tags estão preenchidas no arranque caso tenhas esquecido no Designer
            foreach (Control c in tlpMenu.Controls)
            {
                if (c is Guna2Button btn && btn.Tag == null)
                {
                    btn.Tag = btn.Text; // Salva o texto inicial na Tag
                }
            }

            AbrirPrimeiraPermissao();
        }

        private void AbrirPrimeiraPermissao()
        {
            // Lista ordenada pela prioridade que queres dar (do mais importante para o menos)
            // Nav1 = Stock, Nav2 = Funcionários, Nav3 = Pendentes, etc.
            Guna2Button[] botoesNavegacao = { Nav1, Nav2, Nav3, Nav4, Nav5, Nav6, Nav7, Nav9 };

            foreach (var btn in botoesNavegacao)
            {
                // Se o botão estiver Visível (significa que tem permissão), clica nele!
                if (btn.Visible)
                {
                    // Simula o clique para ativar a cor, o ícone e carregar a UC
                    Nav_Clicked(btn, EventArgs.Empty);
                    return; // Sai do método assim que encontrar o primeiro
                }
            }

            // Se chegar aqui, é porque não tem permissão nenhuma (exceto Sair)
            // Podes usar a tua classe M (EfeitoUI) se já a tiveres instanciada globalmente
            // ou criar uma nova instância aqui.
            EfeitoUI M = new EfeitoUI();
            M.AbrirMensagem("Não tem permissões para aceder a nenhum módulo.", "Aviso de Acesso");
        }

        public bool PodeAceder(string key)
        {
            switch (key.ToLowerInvariant())
            {
                case "stock":
                    return permissoes.PodeVerStock
                           || permissoes.PodeInserirStock
                           || permissoes.PodeCriarStock;

                case "funcionários":
                case "funcionarios": // segurança ortográfica
                    return permissoes.PodeEditarFunc;

                case "pedidos pendentes":
                    return permissoes.PodeAprovar || permissoes.PodeEntregar;

                case "pedidos aprovados":
                    return permissoes.PodeEntregar || permissoes.PodeAprovar;

                case "inserir stock":
                    return permissoes.PodeInserirStock;

                case "criar artigos":
                    return permissoes.PodeCriarStock;

                case "funções":
                case "funcoes":
                    return permissoes.PodeCriarFuncoes;

                case "definições":
                case "definicoes":
                    return permissoes.PodeAlterarDefinicoes;

                case "sair":
                    return true;

                default:
                    return false;
            }
        }

        private void AplicarPermissoes()
        {
            // 1) Desliga tudo por omissão (menos "Sair")
            Nav1.Visible = Nav1.Enabled = false; // Stock
            Nav2.Visible = Nav2.Enabled = false; // Funcionários
            Nav3.Visible = Nav3.Enabled = false; // Pedidos Pendentes
            Nav4.Visible = Nav4.Enabled = false; // Pedidos Aprovados
            Nav5.Visible = Nav5.Enabled = false; // Inserir Stock
            Nav6.Visible = Nav6.Enabled = false; // Criar Artigos
            Nav7.Visible = Nav7.Enabled = false; // Funções
            Nav9.Visible = Nav9.Enabled = false; // Definições

            Nav10.Visible = true;   // Sair
            Nav10.Enabled = true;

            // 2) Ligar com base nas permissões

            if (PodeAceder("Stock"))
            {
                Nav1.Visible = true;
                Nav1.Enabled = true;
            }

            if (PodeAceder("Funcionários"))
            {
                Nav2.Visible = true;
                Nav2.Enabled = true;
            }

            if (PodeAceder("Pedidos Pendentes"))
            {
                Nav3.Visible = true;
                Nav3.Enabled = true;
            }

            if (PodeAceder("Pedidos Aprovados"))
            {
                Nav4.Visible = true;
                Nav4.Enabled = true;
            }

            if (PodeAceder("Inserir Stock"))
            {
                Nav5.Visible = true;
                Nav5.Enabled = true;
            }

            if (PodeAceder("Criar Artigos"))
            {
                Nav6.Visible = true;
                Nav6.Enabled = true;
            }

            if (PodeAceder("Funções"))
            {
                Nav7.Visible = true;
                Nav7.Enabled = true;
            }

            if (PodeAceder("Definições"))
            {
                Nav9.Visible = true;
                Nav9.Enabled = true;
            }
        }

        // --- CORREÇÃO 1: NAVEGAÇÃO BASEADA NA TAG ---
        private void Nav_Clicked(object sender, EventArgs e)
        {
            var clicked = (Guna2Button)sender;

            // IMPORTANTE: Usa a TAG, pois o Text pode estar vazio se o menu estiver fechado
            string chaveNavegacao = clicked.Tag != null ? clicked.Tag.ToString() : clicked.Text;

            if (chaveNavegacao.Equals("Sair", StringComparison.OrdinalIgnoreCase))
            {
                Close();
                return;
            }

            if (chaveNavegacao.Equals("Expandir", StringComparison.OrdinalIgnoreCase))
            {
                timerMenu.Start();
                Properties.Settings.Default.MenuEspandido = !menuExpandido; // Salva o estado para a próxima vez
                return;
            }

            if (!clicked.Checked)
            {
                // 1. Reseta TODOS os botões para a cor original (ex: Preto)
                foreach (Control c in tlpMenu.Controls)
                {
                    if (c is Guna2Button nb && nb.Checked)
                    {
                        nb.Checked = false;
                        // Se guardaste a imagem original em algum lugar, restaura aqui.
                        // OU, se a imagem atual for branca, inverte de volta para preto:
                        nb.Image = ImageHelper.InverterCores(nb.Image);
                    }
                }

                // 2. Marca o atual como Checked
                clicked.Checked = true;

                // 3. Inverte a cor do ícone atual (ex: vira Branco)
                clicked.Image = ImageHelper.InverterCores(clicked.Image);
            }

            // Passa a TAG para o switch
            Navegar(chaveNavegacao);
        }

        private async void Navegar(string key)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Dá um pequeno fôlego à UI para não congelar o clique do menu
                await Task.Yield();

                UserControl controlParaAbrir = key.ToLowerInvariant() switch
                {
                    "stock" => new UCs.Stock(permissoes),
                    "funcionarios" or "funcionários" => new UCs.Funcionarios(IDGestor),
                    "pedidos pendentes" => new UCs.Pedidos(IDGestor, "Pendente"),
                    "pedidos aprovados" => new UCs.Pedidos(IDGestor, "Aprovado"),
                    "histórico" => new UCs.Stock(permissoes),
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
            // Diz ao Windows: "Para de desenhar este painel agora"
            SendMessage(pnlConteudo.Handle, WM_SETREDRAW, false, 0);

            pnlConteudo.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlConteudo.Controls.Add(control);

            // Diz ao Windows: "Ok, agora desenha tudo de uma vez"
            SendMessage(pnlConteudo.Handle, WM_SETREDRAW, true, 0);
            pnlConteudo.Refresh();
        }

        // --- CORREÇÃO 2 e 3: TIMER E VISUAL ---
        private void timerMenu_Tick(object sender, EventArgs e)
        {
            // Suspendemos o desenho do painel que contém o conteúdo (lado direito)
            // para o Windows não tentar ajustar o CriarStock a cada pixel que o menu move
            splitContainer1.Panel2.SuspendLayout();

            if (menuExpandido)
            {
                if (splitContainer1.SplitterDistance == larguraMax) ConfigurarBotoes(false);

                splitContainer1.SplitterDistance -= velocidade;

                if (splitContainer1.SplitterDistance <= larguraMin)
                {
                    splitContainer1.SplitterDistance = larguraMin;
                    menuExpandido = false;
                    timerMenu.Stop();
                }
            }
            else
            {
                splitContainer1.SplitterDistance += velocidade;

                if (splitContainer1.SplitterDistance >= larguraMax)
                {
                    splitContainer1.SplitterDistance = larguraMax;
                    menuExpandido = true;
                    timerMenu.Stop();
                    ConfigurarBotoes(true);
                }
            }

            // Voltamos a permitir que o painel se desenhe agora que já parou de mexer
            splitContainer1.Panel2.ResumeLayout();
        }

        // Método auxiliar para limpar ou restaurar texto e ícones
        private void ConfigurarBotoes(bool mostrarTexto)
        {
            // Certifica-te que o botão de menu (dummy) ESTÁ dentro do tlpMenu
            foreach (Control c in tlpMenu.Controls)
            {
                if (c is Guna2Button btn)
                {
                    if (mostrarTexto)
                    {
                        // MODO ABERTO (Texto + Ícone na esquerda)

                        // Restaura o texto original da TAG
                        if (btn.Tag != null) btn.Text = btn.Tag.ToString();

                        btn.ImageAlign = HorizontalAlignment.Left;
                        btn.ImageOffset = new Point(0, 0);
                        btn.TextAlign = HorizontalAlignment.Left;
                    }
                    else
                    {
                        // MODO FECHADO (Só ícones centralizados)

                        // Verifica se a Tag está vazia antes de apagar o texto, só por segurança
                        if (btn.Tag == null || string.IsNullOrEmpty(btn.Tag.ToString()))
                        {
                            btn.Tag = btn.Text; // Salva o texto antes de apagar
                        }

                        btn.Text = ""; // Apaga o texto visualmente
                        btn.TextAlign = HorizontalAlignment.Center;
                        btn.ImageAlign = HorizontalAlignment.Center;
                        btn.ImageOffset = new Point(0, 0);
                    }
                }
            }
        }

        private void FormGestao_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void FormGestao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                // Verifica se JÁ ESTÁ em Fullscreen (Sem bordas)
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    // --- VOLTAR AO NORMAL ---
                    this.WindowState = FormWindowState.Normal;      // Desmaximiza primeiro
                    this.FormBorderStyle = FormBorderStyle.Sizable; // Devolve as bordas e barra de título
                    this.CenterToScreen();                          // (Opcional) Centra no ecrã
                }
                else
                {
                    // --- ENTRAR EM FULLSCREEN ---
                    this.FormBorderStyle = FormBorderStyle.None;    // Tira bordas (para tapar a barra de tarefas)
                    this.WindowState = FormWindowState.Maximized;   // Maximiza
                }
            }
        }

        // Adiciona este evento no teu FormGestao
        private void FormGestao_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            AdaptarDPI();
        }

        private void AdaptarDPI()
        {
            float dpiScale = this.DeviceDpi / 96f;

            if (dpiScale <= 1f) return; // Se for um ecrã normal, não mexe em nada!

            // Ajusta a largura do menu lateral
            splitContainer1.SplitterDistance = (int)(300 * dpiScale);

            // Ajusta o tamanho dos ícones do Menu
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
    }
}