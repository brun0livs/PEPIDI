using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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

        // Evento para avisar o pai que algo mudou
        public event EventHandler QuantidadeAlterada;

        // --- PROPRIEDADES DE ACESSO PÚBLICO (CORRIGIDAS) ---

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

        // 4. Quantidade Selecionada (A CORREÇÃO CRÍTICA ESTÁ AQUI)
        public int QuantidadeSelecionada
        {
            get
            {
                // MODO ENTREGA (APROVADO)
                if (EstadoLinha == "Aprovado")
                {
                    // Se não tiver visto, entrega 0
                    if (!chkEntregar.Checked) return 0;

                    // Se tiver visto, entrega a quantidade que foi aprovada (QA)
                    // NÃO TENTES LER A COMBOBOX AQUI, ELA ESTÁ ESCONDIDA!
                    return this.QA;
                }

                // MODO APROVAÇÃO (PENDENTE)
                // Aqui sim, lemos o que o gestor escolheu na ComboBox
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
            lblQuantDisp.Text = QD.ToString();

            // Configuração das colunas (garante que o TableLayoutPanel existe no designer)
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanel1.ColumnStyles[0].Width = 40F; // Modelo
            tableLayoutPanel1.ColumnStyles[1].Width = 20F; // Tamanho
            tableLayoutPanel1.ColumnStyles[2].Width = 20F; // Quant. Disp

            // 2. Configurar Estado
            if (EstadoLinha == "Aprovado")
            {
                // MODO ENTREGA: Esconde Combo, Mostra Checkbox
                cmbQuant.Visible = false;
                chkEntregar.Visible = true;
                chkEntregar.Dock = DockStyle.Fill;

                // Ajusta layout
                tableLayoutPanel1.ColumnStyles[3].Width = 0F;  // Coluna da Combo (some)
                tableLayoutPanel1.ColumnStyles[4].Width = 20F; // Coluna da Check (aparece)

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

                // Ajusta layout
                tableLayoutPanel1.ColumnStyles[3].Width = 20F; // Coluna da Combo (aparece)
                tableLayoutPanel1.ColumnStyles[4].Width = 0F;  // Coluna da Check (some)

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
        }

        private void CmbQuant_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuantidadeAlterada?.Invoke(this, EventArgs.Empty);
        }

        private void ChkEntregar_CheckedChanged(object sender, EventArgs e)
        {
            // Feedback Visual: Cinza se ativo, Vermelho claro se desativo
            this.BackColor = chkEntregar.Checked ? Color.FromArgb(224, 224, 224) : Color.MistyRose;

            QuantidadeAlterada?.Invoke(this, EventArgs.Empty);
        }
    }
}