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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            cmbVisoes = new Guna.UI2.WinForms.Guna2ComboBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
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
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(cmbVisoes, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1936, 73);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(962, 73);
            label1.TabIndex = 1;
            label1.Text = "STOCK";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbVisoes
            // 
            cmbVisoes.AutoRoundedCorners = true;
            cmbVisoes.BackColor = Color.Transparent;
            cmbVisoes.BorderColor = Color.FromArgb(254, 107, 0);
            cmbVisoes.BorderRadius = 17;
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
            cmbVisoes.ItemHeight = 30;
            cmbVisoes.Items.AddRange(new object[] { "Teste1", "Teste2", "Teste3", "Teste4", "Teste5", "Teste6", "Teste7", "Teste8", "Teste9", "Teste10" });
            cmbVisoes.Location = new Point(993, 11);
            cmbVisoes.Margin = new Padding(25, 11, 25, 11);
            cmbVisoes.Name = "cmbVisoes";
            cmbVisoes.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbVisoes.Size = new Size(918, 36);
            cmbVisoes.TabIndex = 1;
            cmbVisoes.SelectedIndexChanged += cmbVisoes_SelectedIndexChanged;
            // 
            // Stock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(0);
            Name = "Stock";
            Size = new Size(1936, 1048);
            Load += Stock_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbVisoes;
        private PEPIDI_0._5.Models.PEPIDIDataGridView dgvStock;
    }
}
