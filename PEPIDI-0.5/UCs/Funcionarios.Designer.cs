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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Funcionarios));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            lblTituloFUNCIONARIOS = new Label();
            btnAddFunc = new Guna.UI2.WinForms.Guna2Button();
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
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(txtPesquisa, 1, 0);
            tableLayoutPanel2.Controls.Add(lblTituloFUNCIONARIOS, 0, 0);
            tableLayoutPanel2.Controls.Add(btnAddFunc, 2, 0);
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
            txtPesquisa.CustomizableEdges = customizableEdges5;
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
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges6;
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
            lblTituloFUNCIONARIOS.Size = new Size(606, 60);
            lblTituloFUNCIONARIOS.TabIndex = 1;
            lblTituloFUNCIONARIOS.Text = "FUNCIONÁRIOS";
            lblTituloFUNCIONARIOS.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAddFunc
            // 
            btnAddFunc.BorderRadius = 10;
            btnAddFunc.CustomizableEdges = customizableEdges7;
            btnAddFunc.DisabledState.BorderColor = Color.DarkGray;
            btnAddFunc.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAddFunc.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAddFunc.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAddFunc.Dock = DockStyle.Fill;
            btnAddFunc.FillColor = Color.FromArgb(243, 108, 33);
            btnAddFunc.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddFunc.ForeColor = Color.White;
            btnAddFunc.Location = new Point(1253, 12);
            btnAddFunc.Margin = new Padding(29, 12, 29, 12);
            btnAddFunc.Name = "btnAddFunc";
            btnAddFunc.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnAddFunc.Size = new Size(555, 36);
            btnAddFunc.TabIndex = 5;
            btnAddFunc.Text = "Novo Funcionário";
            btnAddFunc.Click += btnAddFunc_Click;
            // 
            // dgvFuncs
            // 
            dgvFuncs.AllowUserToAddRows = false;
            dgvFuncs.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = Color.Transparent;
            dataGridViewCellStyle6.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvFuncs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dgvFuncs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFuncs.BackgroundColor = Color.White;
            dgvFuncs.BorderStyle = BorderStyle.None;
            dgvFuncs.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvFuncs.CellPadding = new Padding(10);
            dgvFuncs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridViewCellStyle7.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvFuncs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvFuncs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.Transparent;
            dataGridViewCellStyle8.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle8.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle8.SelectionForeColor = Color.Black;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvFuncs.DefaultCellStyle = dataGridViewCellStyle8;
            dgvFuncs.Dock = DockStyle.Fill;
            dgvFuncs.EnableHeadersVisualStyles = false;
            dgvFuncs.GridColor = SystemColors.Control;
            dgvFuncs.HeaderFontSize = 15F;
            dgvFuncs.Location = new Point(11, 72);
            dgvFuncs.Margin = new Padding(11, 12, 11, 0);
            dgvFuncs.MultiSelect = false;
            dgvFuncs.Name = "dgvFuncs";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvFuncs.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvFuncs.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = Color.Transparent;
            dataGridViewCellStyle10.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvFuncs.RowsDefaultCellStyle = dataGridViewCellStyle10;
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
    }
}
