namespace PEPIDI.UCs
{
    partial class Funcionarios
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Funcionarios));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            lblTituloFUNCIONARIOS = new Label();
            btnAddFunc = new Guna.UI2.WinForms.Guna2Button();
            btnAbreGraficos = new Guna.UI2.WinForms.Guna2Button();
            dgvFuncs = new PEPIDI.Models.PEPIDIDataGridView();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFuncs).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(dgvFuncs, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.Size = new Size(1837, 858);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666641F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333244F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            tableLayoutPanel2.Controls.Add(txtPesquisa, 2, 0);
            tableLayoutPanel2.Controls.Add(lblTituloFUNCIONARIOS, 0, 0);
            tableLayoutPanel2.Controls.Add(btnAddFunc, 4, 0);
            tableLayoutPanel2.Controls.Add(btnAbreGraficos, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1837, 60);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // txtPesquisa
            // 
            txtPesquisa.AutoRoundedCorners = true;
            txtPesquisa.CustomizableEdges = customizableEdges1;
            txtPesquisa.DefaultText = "";
            txtPesquisa.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPesquisa.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPesquisa.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPesquisa.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPesquisa.Dock = DockStyle.Fill;
            txtPesquisa.FocusedState.BorderColor = Color.FromArgb(243, 108, 33);
            txtPesquisa.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPesquisa.ForeColor = Color.Black;
            txtPesquisa.HoverState.BorderColor = Color.Gray;
            txtPesquisa.IconRight = (Image)resources.GetObject("txtPesquisa.IconRight");
            txtPesquisa.IconRightOffset = new Point(10, 0);
            txtPesquisa.IconRightSize = new Size(25, 25);
            txtPesquisa.Location = new Point(625, 13);
            txtPesquisa.Margin = new Padding(13);
            txtPesquisa.MaxLength = 16;
            txtPesquisa.Name = "txtPesquisa";
            txtPesquisa.PlaceholderForeColor = Color.Silver;
            txtPesquisa.PlaceholderText = "Procurar Funcionário";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPesquisa.Size = new Size(586, 34);
            txtPesquisa.TabIndex = 6;
            txtPesquisa.TextAlign = HorizontalAlignment.Center;
            txtPesquisa.TextOffset = new Point(10, 0);
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;
            // 
            // lblTituloFUNCIONARIOS
            // 
            lblTituloFUNCIONARIOS.AutoSize = true;
            lblTituloFUNCIONARIOS.Dock = DockStyle.Fill;
            lblTituloFUNCIONARIOS.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloFUNCIONARIOS.Location = new Point(3, 0);
            lblTituloFUNCIONARIOS.Name = "lblTituloFUNCIONARIOS";
            lblTituloFUNCIONARIOS.Size = new Size(300, 60);
            lblTituloFUNCIONARIOS.TabIndex = 1;
            lblTituloFUNCIONARIOS.Text = "FUNCIONÁRIOS";
            lblTituloFUNCIONARIOS.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAddFunc
            // 
            btnAddFunc.BorderRadius = 10;
            btnAddFunc.CustomizableEdges = customizableEdges3;
            btnAddFunc.DisabledState.BorderColor = Color.DarkGray;
            btnAddFunc.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAddFunc.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAddFunc.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAddFunc.Dock = DockStyle.Fill;
            btnAddFunc.FillColor = Color.FromArgb(243, 108, 33);
            btnAddFunc.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddFunc.ForeColor = Color.White;
            btnAddFunc.Location = new Point(1559, 12);
            btnAddFunc.Margin = new Padding(29, 12, 29, 12);
            btnAddFunc.Name = "btnAddFunc";
            btnAddFunc.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAddFunc.Size = new Size(249, 36);
            btnAddFunc.TabIndex = 5;
            btnAddFunc.Text = "Novo Funcionário";
            btnAddFunc.Click += btnAddFunc_Click;
            // 
            // btnAbreGraficos
            // 
            btnAbreGraficos.BorderRadius = 10;
            btnAbreGraficos.CustomizableEdges = customizableEdges5;
            btnAbreGraficos.DisabledState.BorderColor = Color.DarkGray;
            btnAbreGraficos.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAbreGraficos.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAbreGraficos.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAbreGraficos.Dock = DockStyle.Fill;
            btnAbreGraficos.FillColor = Color.FromArgb(243, 108, 33);
            btnAbreGraficos.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAbreGraficos.ForeColor = Color.White;
            btnAbreGraficos.Location = new Point(1253, 12);
            btnAbreGraficos.Margin = new Padding(29, 12, 29, 12);
            btnAbreGraficos.Name = "btnAbreGraficos";
            btnAbreGraficos.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnAbreGraficos.Size = new Size(248, 36);
            btnAbreGraficos.TabIndex = 7;
            btnAbreGraficos.Text = "Gráficos";
            btnAbreGraficos.Click += btnAbreGraficos_Click;
            // 
            // dgvFuncs
            // 
            dgvFuncs.AllowUserToAddRows = false;
            dgvFuncs.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dataGridViewCellStyle1.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvFuncs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvFuncs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFuncs.BackgroundColor = Color.White;
            dgvFuncs.BorderStyle = BorderStyle.None;
            dgvFuncs.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvFuncs.CellPadding = new Padding(10);
            dgvFuncs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvFuncs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvFuncs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvFuncs.DefaultCellStyle = dataGridViewCellStyle3;
            dgvFuncs.Dock = DockStyle.Fill;
            dgvFuncs.EnableHeadersVisualStyles = false;
            dgvFuncs.GridColor = SystemColors.Control;
            dgvFuncs.HeaderFontSize = 15F;
            dgvFuncs.Location = new Point(11, 72);
            dgvFuncs.Margin = new Padding(11, 12, 11, 0);
            dgvFuncs.MultiSelect = false;
            dgvFuncs.Name = "dgvFuncs";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvFuncs.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvFuncs.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = Color.Transparent;
            dataGridViewCellStyle5.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvFuncs.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvFuncs.RowTemplate.Height = 54;
            dgvFuncs.ScrollBars = ScrollBars.None;
            dgvFuncs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFuncs.Size = new Size(1815, 786);
            dgvFuncs.TabIndex = 1;
            // 
            // Funcionarios
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Funcionarios";
            Size = new Size(1837, 858);
            Load += Funcionarios_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFuncs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblTituloFUNCIONARIOS;
        private PEPIDI.Models.PEPIDIDataGridView dgvFuncs;
        private Guna.UI2.WinForms.Guna2Button btnAddFunc;
        private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
        private Guna.UI2.WinForms.Guna2Button btnAbreGraficos;
    }
}
