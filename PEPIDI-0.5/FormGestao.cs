using Guna.UI2.WinForms;
using PEPIDI.Organizers;
using PEPIDI.Models;
using System;
using System.Drawing; // Necessário para HorizontalAlignment não dar erro de ambiguidade
using System.Windows.Forms;
using PEPIDI.UCs.DGVS;

namespace PEPIDI
{
    public partial class FormGestao : Form
    {
        int IDGestor;
        private readonly PermissoesPerfil permissoes;

        // Configuração do Menu
        bool menuExpandido = true;
        const int larguraMax = 350;
        const int larguraMin = 79;
        const int velocidade = 75;

        public FormGestao(int _idGestor, PermissoesPerfil _permissoes)
        {
            InitializeComponent();
            IDGestor = _idGestor;
            permissoes = _permissoes;
            splitContainer1.SplitterDistance = larguraMax;
        }

        private void FormGestao_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            // Verifica se Details e GetInfoGestor existem no teu projeto
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

        private void Navegar(string key)
        {
            // O switch usa a KEY (Tag), garantindo que funciona mesmo com menu fechado
            switch (key.ToLowerInvariant())
            {
                case "stock":
                    AbrirControl(new UCs.Stock(permissoes));
                    break;
                case "funcionários":
                case "funcionarios":
                    AbrirControl(new UCs.Funcionarios(IDGestor));
                    break;
                case "pedidos pendentes":
                    AbrirControl(new UCs.Pedidos(IDGestor, "Pendente"));
                    break;
                case "pedidos aprovados":
                    AbrirControl(new UCs.Pedidos(IDGestor, "Aprovado"));
                    break;
                case "inserir stock":
                    AbrirControl(new UCs.AddStock(IDGestor));
                    break;
                case "criar artigos":
                    AbrirControl(new UCs.CriarStock());
                    break;
                case "funções":
                    AbrirControl(new UCs.Funcoes(IDGestor));
                    break;
                case "definições":
                    AbrirControl(new UCs.Definicoes(IDGestor));
                    break;
                default:
                    // Caso tenhas botões sem case, não faz nada
                    break;
            }
        }

        public void AbrirControl(UserControl control)
        {
            pnlConteudo.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlConteudo.Controls.Add(control);
        }

        // --- CORREÇÃO 2 e 3: TIMER E VISUAL ---
        private void timerMenu_Tick(object sender, EventArgs e)
        {
            if (menuExpandido)
            {
                // -- RECOLHER --

                // Passo 1: Esconde o texto IMEDIATAMENTE antes de começar a encolher
                // para evitar que o texto fique cortado feio.
                if (splitContainer1.SplitterDistance == larguraMax)
                {
                    ConfigurarBotoes(false);
                }

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
                // -- EXPANDIR --
                splitContainer1.SplitterDistance += velocidade;

                if (splitContainer1.SplitterDistance >= larguraMax)
                {
                    splitContainer1.SplitterDistance = larguraMax;
                    menuExpandido = true;
                    timerMenu.Stop();

                    // Passo 2: Mostra o texto APENAS quando terminar de abrir
                    ConfigurarBotoes(true);
                }
            }
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
    }
}