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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1936, 1048);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(txtPesquisa, 1, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(btnAddFunc, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1936, 73);
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
            txtPesquisa.Font = new Font("Roboto", 18.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPesquisa.ForeColor = Color.Black;
            txtPesquisa.HoverState.BorderColor = Color.Gray;
            txtPesquisa.IconRight = Properties.Resources.Lupa;
            txtPesquisa.IconRightOffset = new Point(10, 0);
            txtPesquisa.Location = new Point(655, 10);
            txtPesquisa.Margin = new Padding(10);
            txtPesquisa.MaxLength = 16;
            txtPesquisa.Name = "txtPesquisa";
            txtPesquisa.PlaceholderForeColor = Color.Silver;
            txtPesquisa.PlaceholderText = "Procurar Funcionário";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPesquisa.Size = new Size(625, 53);
            txtPesquisa.TabIndex = 6;
            txtPesquisa.TextAlign = HorizontalAlignment.Center;
            txtPesquisa.TextOffset = new Point(10, 0);
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(639, 73);
            label1.TabIndex = 1;
            label1.Text = "FUNCIONÁRIOS";
            label1.TextAlign = ContentAlignment.MiddleLeft;
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
            btnAddFunc.Font = new Font("Roboto", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddFunc.ForeColor = Color.White;
            btnAddFunc.Location = new Point(1315, 10);
            btnAddFunc.Margin = new Padding(25, 10, 25, 10);
            btnAddFunc.Name = "btnAddFunc";
            btnAddFunc.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnAddFunc.Size = new Size(596, 53);
            btnAddFunc.TabIndex = 5;
            btnAddFunc.Text = "Novo Funcionário";
            btnAddFunc.Click += btnAddFunc_Click;
            // 
            // dgvFuncs
            // 
            dgvFuncs.AllowUserToAddRows = false;
            dgvFuncs.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvFuncs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvFuncs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFuncs.BackgroundColor = Color.White;
            dgvFuncs.BorderStyle = BorderStyle.None;
            dgvFuncs.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvFuncs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvFuncs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvFuncs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvFuncs.DefaultCellStyle = dataGridViewCellStyle3;
            dgvFuncs.Dock = DockStyle.Fill;
            dgvFuncs.EnableHeadersVisualStyles = false;
            dgvFuncs.GridColor = SystemColors.Control;
            dgvFuncs.Location = new Point(10, 83);
            dgvFuncs.Margin = new Padding(10, 10, 10, 0);
            dgvFuncs.MultiSelect = false;
            dgvFuncs.Name = "dgvFuncs";
            dgvFuncs.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvFuncs.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvFuncs.RowTemplate.Height = 54;
            dgvFuncs.ScrollBars = ScrollBars.None;
            dgvFuncs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFuncs.Size = new Size(1916, 965);
            dgvFuncs.TabIndex = 1;
            // 
            // Funcionarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Name = "Funcionarios";
            Size = new Size(1936, 1048);
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
        private Label label1;
        private PEPIDI_0._5.Models.PEPIDIDataGridView dgvFuncs;
        private Guna.UI2.WinForms.Guna2Button btnAddFunc;
        private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
    }
}
