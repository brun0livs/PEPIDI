using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Guna.UI2.WinForms;
using System.Data;

namespace PEPIDI.UCs.DGVS
{
    public partial class LinhaItem : UserControl
    {
        // --- PROPRIEDADES DE DADOS (HIDDEN PARA NÃO DAR ERRO NO DESIGNER) ---
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IDEPI { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string M { get; set; } // Modelo

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string T { get; set; } // Tamanho

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int QD { get; set; } // Quantidade Disponível (Stock)

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int QA { get; set; } // Quantidade Aprovada/Pedida

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EstadoLinha { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int QuantidadeOriginal { get; set; }

        // --- AS DUAS PROPRIEDADES NOVAS PARA O MODO "GESTÃO INTELIGENTE" ---
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ObservacoesAutomaticas { get; set; } = "";

        public string EstadoSelecionado
        {
            get { return cmbEstado != null ? cmbEstado.Text : "Novo"; }
        }
        public bool EstadoForcadoPeloSistema
        {
            get { return cmbEstado != null && !cmbEstado.Enabled; }
        }

        // Evento para avisar o pai que algo mudou
        public event EventHandler QuantidadeAlterada;
        public event EventHandler EstadoAlterado;

        // --- PROPRIEDADES DE ACESSO PÚBLICO ---

        // 1. Descrição do Artigo
        public string DescricaoArtigo
        {
            get { return this.M; }
        }

        // 2. Tamanho Selecionado
        public string TamanhoSelecionado
        {
            get { return this.T; }
        }

        // 3. Se está selecionado (Checkbox)
        public bool Selecionado
        {
            get { return chkEntregar.Checked; }
        }

        // 4. Quantidade Selecionada
        public int QuantidadeSelecionada
        {
            get
            {
                // MODO ENTREGA (APROVADO)
                if (EstadoLinha == "Aprovado")
                {
                    // Se não tem visto, é 0
                    if (!chkEntregar.Checked) return 0;

                    // Se TEM visto, devolve QA. Se QA for 0, devolve 1 (Rede de Segurança!)
                    return (this.QA > 0) ? this.QA : 1;
                }

                // MODO APROVAÇÃO (PENDENTE)
                if (int.TryParse(cmbQuant.Text, out int result))
                {
                    return result;
                }

                return 0;
            }
        }

        public LinhaItem(string _M, string _T, int _QD, int _QA, string _estado)
        {
            InitializeComponent();
            M = _M;
            T = _T;
            QD = _QD;
            QA = _QA;
            EstadoLinha = _estado;
            QuantidadeOriginal = _QA;

            // Retransmitir o scroll para o painel pai (Hack do Scroll)
            this.MouseWheel += (s, e) =>
            {
                if (this.Parent?.Parent != null)
                {
                    SendMessage(this.Parent.Parent.Handle, 0x20a, (IntPtr)(e.Delta << 16), IntPtr.Zero);
                }
            };
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void Linha_Load(object sender, EventArgs e)
        {
            // 1. Preenchimento visual
            lblModelo.Text = M;
            lblTamanho.Text = T;
            lblQuantDisp.Text = (EstadoLinha == "Aprovado") ? QA.ToString() : QD.ToString();

            // 2. Configurar Estado
            if (EstadoLinha == "Aprovado")
            {
                // MODO ENTREGA: Esconde Combo, Mostra Checkbox
                cmbQuant.Visible = false;
                chkEntregar.Visible = true;
                chkEntregar.Dock = DockStyle.Fill;

                // Esconde a Combo de Estado (Novo/Usado) porque na entrega não interessa
                if (cmbEstado != null) cmbEstado.Visible = false;

                // Ajusta layout
                tableLayoutPanel1.ColumnStyles[4].Width = 0F;  // Coluna da Combo Quantidade (some)
                tableLayoutPanel1.ColumnStyles[5].Width = 15F; // Coluna da Check (aparece)

                // Lógica de Stock
                if (QD <= 0)
                {
                    // Sem stock: Desativa e desmarca
                    chkEntregar.Checked = false;
                    chkEntregar.Enabled = false;
                    this.BackColor = Color.FromArgb(255, 235, 235); // Vermelho claro
                }
                else
                {
                    // Com stock: Marca por defeito
                    chkEntregar.Checked = true;
                    chkEntregar.Enabled = true;
                }
            }
            else
            {
                // MODO PENDENTE: Mostra Combo, Esconde Checkbox
                cmbQuant.Visible = true;
                chkEntregar.Visible = false;
                cmbQuant.Dock = DockStyle.Top;

                if (cmbEstado != null) cmbEstado.Visible = true;

                // Ajusta layout
                tableLayoutPanel1.ColumnStyles[4].Width = 15F; // Coluna da Combo (aparece)
                tableLayoutPanel1.ColumnStyles[5].Width = 0F;  // Coluna da Check (some)

                ConfigurarModoEdicao();
            }
        }

        private void ConfigurarModoEdicao()
        {
            cmbQuant.Items.Clear();

            // Define o limite (Stock ou 5, o que for menor)
            int limiteMaximo = Math.Min(QD, 5);

            for (int i = 0; i <= limiteMaximo; i++)
            {
                cmbQuant.Items.Add(i);
            }

            // Seleciona a quantidade pedida (QA) ou o máximo possível se QA > Stock
            int valorParaSelecionar = (QA > limiteMaximo) ? limiteMaximo : QA;
            cmbQuant.Text = valorParaSelecionar.ToString();

            // ATENÇÃO: Apaguei o "Configura(cmbEstado);" daqui porque estava a esmagar a lógica do ConfigurarSugestaoStock!
        }

        private void CmbQuant_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuantidadeAlterada?.Invoke(this, EventArgs.Empty);
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Avisa o Form Principal que o estado mudou!
            EstadoAlterado?.Invoke(this, EventArgs.Empty);
        }

        private void ChkEntregar_CheckedChanged(object sender, EventArgs e)
        {
            // Feedback Visual: Cinza se ativo, Vermelho claro se desativo
            this.BackColor = chkEntregar.Checked ? Color.FromArgb(224, 224, 224) : Color.MistyRose;

            QuantidadeAlterada?.Invoke(this, EventArgs.Empty);
        }

        // =========================================================================
        // O CÉREBRO DA SUGESTÃO DE STOCK (Chamado pelo Formulário Pai)
        // =========================================================================
        public void ConfigurarSugestaoStock(int qtdNovo, int qtdUsado)
        {
            if (cmbEstado == null) return;

            // Desliga os eventos temporariamente se necessário, mas aqui a ordem resolve.
            cmbEstado.DataSource = null;
            cmbEstado.Items.Clear();
            this.ObservacoesAutomaticas = "";

            bool temNovo = qtdNovo > 0;
            bool temUsado = qtdUsado > 0;

            if (temNovo && temUsado)
            {
                // CASO 1: Existem os dois tipos (Padrão Novo)
                cmbEstado.Items.Add("Novo");
                cmbEstado.Items.Add("Usado");
                cmbEstado.Enabled = true;    // 1º Define o estado (Destrancado)
                cmbEstado.SelectedIndex = 0; // 2º Dispara o evento
            }
            else if (temNovo)
            {
                // CASO 2: Só existe Novo
                cmbEstado.Items.Add("Novo");
                cmbEstado.Enabled = false;   // 1º Tranca
                cmbEstado.SelectedIndex = 0; // 2º Dispara o evento
            }
            else if (temUsado)
            {
                // CASO 3: Só existe Usado
                cmbEstado.Items.Add("Usado");

                cmbEstado.Enabled = false; // 1º TRANCA LOGO!
                this.ObservacoesAutomaticas = " (sem stock de novo)"; // 2º PREPARA A MENSAGEM!

                cmbEstado.SelectedIndex = 0; // 3º AGORA SIM, DISPARA O EVENTO! (Ele já vai ler que está trancado)
            }
            else
            {
                // CASO 4: Não há stock de nenhum!
                cmbEstado.Items.Add("SEM STOCK");
                this.BackColor = Color.MistyRose;

                if (cmbEstado.Items.Count > 0) cmbEstado.SelectedIndex = 0;
            }
        }
    }
}