namespace PEPIDI.UCs
{
    partial class Stock
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            dgvStock = new PEPIDI.Models.PEPIDIDataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblTituloSTOCK = new Label();
            cmbVisoes = new Guna.UI2.WinForms.Guna2ComboBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dgvStock, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.Size = new Size(1837, 858);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dgvStock
            // 
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.BackgroundColor = Color.White;
            dgvStock.BorderStyle = BorderStyle.None;
            dgvStock.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvStock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvStock.DefaultCellStyle = dataGridViewCellStyle3;
            dgvStock.Dock = DockStyle.Fill;
            dgvStock.EnableHeadersVisualStyles = false;
            dgvStock.GridColor = SystemColors.Control;
            dgvStock.HeaderFontSize = 15F;
            dgvStock.Location = new Point(13, 73);
            dgvStock.Margin = new Padding(13, 13, 13, 0);
            dgvStock.MultiSelect = false;
            dgvStock.Name = "dgvStock";
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvStock.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvStock.RowTemplate.Height = 54;
            dgvStock.ScrollBars = ScrollBars.None;
            dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Size = new Size(1811, 785);
            dgvStock.TabIndex = 2;
            dgvStock.CellFormatting += dgvStock_CellFormatting;
            dgvStock.CellToolTipTextNeeded += dgvStock_CellToolTipTextNeeded;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblTituloSTOCK, 0, 0);
            tableLayoutPanel2.Controls.Add(cmbVisoes, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1837, 60);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblTituloSTOCK
            // 
            lblTituloSTOCK.AutoSize = true;
            lblTituloSTOCK.Dock = DockStyle.Fill;
            lblTituloSTOCK.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloSTOCK.Location = new Point(4, 0);
            lblTituloSTOCK.Margin = new Padding(4, 0, 4, 0);
            lblTituloSTOCK.Name = "lblTituloSTOCK";
            lblTituloSTOCK.Size = new Size(910, 60);
            lblTituloSTOCK.TabIndex = 1;
            lblTituloSTOCK.Text = "STOCK";
            lblTituloSTOCK.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbVisoes
            // 
            cmbVisoes.AutoRoundedCorners = true;
            cmbVisoes.BackColor = Color.Transparent;
            cmbVisoes.BorderColor = Color.FromArgb(254, 107, 0);
            cmbVisoes.BorderRadius = 18;
            cmbVisoes.CustomizableEdges = customizableEdges1;
            cmbVisoes.DisabledState.BorderColor = Color.FromArgb(254, 107, 0);
            cmbVisoes.DisplayMember = "TESTE1";
            cmbVisoes.Dock = DockStyle.Fill;
            cmbVisoes.DrawMode = DrawMode.OwnerDrawFixed;
            cmbVisoes.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVisoes.FocusedColor = Color.FromArgb(255, 128, 0);
            cmbVisoes.FocusedState.BorderColor = Color.FromArgb(255, 128, 0);
            cmbVisoes.FocusedState.ForeColor = Color.Black;
            cmbVisoes.Font = new Font("Roboto", 15.25F);
            cmbVisoes.ForeColor = Color.Black;
            cmbVisoes.HoverState.BorderColor = Color.FromArgb(254, 107, 0);
            cmbVisoes.HoverState.FillColor = Color.White;
            cmbVisoes.HoverState.ForeColor = Color.Black;
            cmbVisoes.ItemHeight = 32;
            cmbVisoes.Items.AddRange(new object[] { "Teste1", "Teste2", "Teste3", "Teste4", "Teste5", "Teste6", "Teste7", "Teste8", "Teste9", "Teste10" });
            cmbVisoes.Location = new Point(933, 12);
            cmbVisoes.Margin = new Padding(15, 12, 15, 12);
            cmbVisoes.Name = "cmbVisoes";
            cmbVisoes.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbVisoes.Size = new Size(889, 38);
            cmbVisoes.TabIndex = 1;
            cmbVisoes.TextOffset = new Point(10, 0);
            cmbVisoes.SelectedIndexChanged += cmbVisoes_SelectedIndexChanged;
            // 
            // Stock
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "Stock";
            Size = new Size(1837, 858);
            Load += Stock_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblTituloSTOCK;
        private Guna.UI2.WinForms.Guna2ComboBox cmbVisoes;
        private Models.PEPIDIDataGridView dgvStock;
    }
}
