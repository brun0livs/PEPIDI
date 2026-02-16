namespace PEPIDI.UCs.DGVS
{
    using System.ComponentModel;
    using System.Reflection.Emit;
    using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

    public partial class LinhaItem : UserControl
    {
        // Usamos Hidden para evitar que o Designer tente criar instâncias fixas destas propriedades
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IDEPI { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string M { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string T { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int QD { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int QA { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EstadoLinha { get; set; }
        public event EventHandler QuantidadeAlterada;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int QuantidadeOriginal { get; set; }

        public int QuantidadeSelecionada
        {
            get
            {
                // No modo Aprovado, se não entregar, a quantidade é zero para o SQL
                if (EstadoLinha == "Aprovado" && !chkEntregar.Checked)
                    return 0;

                if (int.TryParse(cmbQuant.Text, out int result))
                    return result;

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

            // Retransmitir o scroll para o avô (pnlScroll)
            this.MouseWheel += (s, e) =>
            {
                if (this.Parent?.Parent != null)
                {
                    SendMessage(this.Parent.Parent.Handle, 0x20a, (IntPtr)(e.Delta << 16), IntPtr.Zero);
                }
            };
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void Linha_Load(object sender, EventArgs e)
        {
            // 1. Preenchimento de dados básico
            lblModelo.Text = M;
            lblTamanho.Text = T;
            lblQuantDisp.Text = QD.ToString();

            // 2. Gestão das Percentagens e Visibilidade
            // Vamos garantir que as 4 colunas estão sempre na proporção certa
            tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanel1.ColumnStyles[0].Width = 40F; // Modelo
            tableLayoutPanel1.ColumnStyles[1].Width = 20F; // Tamanho
            tableLayoutPanel1.ColumnStyles[2].Width = 20F; // Quant. Disp

            if (EstadoLinha == "Aprovado")
            {
                // Modo "Selecionar" para entrega
                cmbQuant.Visible = false;
                chkEntregar.Visible = true;
                chkEntregar.Dock = DockStyle.Fill; // Faz a check ocupar o centro da coluna
                chkEntregar.Checked = true;
                tableLayoutPanel1.ColumnStyles[3].Width = 0F; // Ação (Combo ou Check)
                tableLayoutPanel1.ColumnStyles[4].Width = 20F; // Ação (Combo ou Check)
            }
            else
            {
                // Modo "Quantidade" para pendentes
                cmbQuant.Visible = true;
                chkEntregar.Visible = false;
                cmbQuant.Dock = DockStyle.Top;
                tableLayoutPanel1.ColumnStyles[3].Width = 20F; // Ação (Combo ou Check)
                tableLayoutPanel1.ColumnStyles[4].Width = 0F; // Ação (Combo ou Check)

                ConfigurarModoEdicao(); // Teu método que preenche a combo 0-5
            }
        }

        private void ConfigurarModoEdicao()
        {
            cmbQuant.Visible = true;
            chkEntregar.Visible = false;

            cmbQuant.Items.Clear();
            int limiteMaximo = Math.Min(QD, 5); // Lógica de negócio de limite 5
            for (int i = 0; i <= limiteMaximo; i++) cmbQuant.Items.Add(i);

            cmbQuant.Text = (QA > limiteMaximo) ? limiteMaximo.ToString() : QA.ToString();
        }

        private void CmbQuant_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuantidadeAlterada?.Invoke(this, EventArgs.Empty);
        }

        private void ChkEntregar_CheckedChanged(object sender, EventArgs e)
        {
            // Muda a cor da linha se for desmarcado para feedback visual rápido
            this.BackColor = chkEntregar.Checked ? Color.FromArgb(224, 224, 224) : Color.MistyRose;

            QuantidadeAlterada?.Invoke(this, EventArgs.Empty);
        }
    }
}