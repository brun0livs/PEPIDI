namespace PEPIDI.UCs
{
    partial class Definicoes
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            pnlDefsPrev = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            cmbDisplay = new Guna.UI2.WinForms.Guna2ComboBox();
            guna2CustomGradientPanel1 = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            tableLayoutPanel1.SuspendLayout();
            pnlDefsPrev.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlDefsPrev, 1, 1);
            tableLayoutPanel1.Controls.Add(guna2CustomGradientPanel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.Size = new Size(1837, 858);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(9, 0);
            label1.Margin = new Padding(9, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(905, 60);
            label1.TabIndex = 2;
            label1.Text = "DEFINIÇÕES";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlDefsPrev
            // 
            pnlDefsPrev.BorderColor = Color.FromArgb(242, 103, 34);
            pnlDefsPrev.BorderRadius = 25;
            pnlDefsPrev.BorderThickness = 1;
            pnlDefsPrev.Controls.Add(tableLayoutPanel2);
            pnlDefsPrev.CustomizableEdges = customizableEdges3;
            pnlDefsPrev.Dock = DockStyle.Fill;
            pnlDefsPrev.Location = new Point(937, 79);
            pnlDefsPrev.Margin = new Padding(19);
            pnlDefsPrev.Name = "pnlDefsPrev";
            pnlDefsPrev.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlDefsPrev.Size = new Size(881, 760);
            pnlDefsPrev.TabIndex = 1;
            pnlDefsPrev.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Transparent;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(label2, 0, 0);
            tableLayoutPanel2.Controls.Add(label3, 0, 1);
            tableLayoutPanel2.Controls.Add(cmbDisplay, 1, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 86F));
            tableLayoutPanel2.Size = new Size(881, 760);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(19, 4);
            label2.Margin = new Padding(19, 4, 4, 4);
            label2.Name = "label2";
            label2.Size = new Size(417, 45);
            label2.TabIndex = 3;
            label2.Text = "DEFINIÇÕES PREVILEGIADAS";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Roboto", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(19, 57);
            label3.Margin = new Padding(19, 4, 4, 4);
            label3.Name = "label3";
            label3.Size = new Size(417, 45);
            label3.TabIndex = 4;
            label3.Text = "Tipo de Dispositivo";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbDisplay
            // 
            cmbDisplay.AutoRoundedCorners = true;
            cmbDisplay.BackColor = Color.Transparent;
            cmbDisplay.BorderColor = Color.FromArgb(254, 107, 0);
            cmbDisplay.BorderRadius = 18;
            cmbDisplay.CustomizableEdges = customizableEdges1;
            cmbDisplay.DisabledState.BorderColor = Color.FromArgb(254, 107, 0);
            cmbDisplay.DisplayMember = "TESTE1";
            cmbDisplay.Dock = DockStyle.Fill;
            cmbDisplay.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDisplay.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDisplay.FocusedColor = Color.FromArgb(255, 128, 0);
            cmbDisplay.FocusedState.BorderColor = Color.FromArgb(255, 128, 0);
            cmbDisplay.FocusedState.ForeColor = Color.Black;
            cmbDisplay.Font = new Font("Roboto", 15.25F);
            cmbDisplay.ForeColor = Color.Black;
            cmbDisplay.HoverState.BorderColor = Color.FromArgb(254, 107, 0);
            cmbDisplay.HoverState.FillColor = Color.White;
            cmbDisplay.HoverState.ForeColor = Color.Black;
            cmbDisplay.ItemHeight = 32;
            cmbDisplay.Items.AddRange(new object[] { "Portatil", "MonitorFullHD", "Surface", "Televisao " });
            cmbDisplay.Location = new Point(472, 67);
            cmbDisplay.Margin = new Padding(32, 14, 32, 14);
            cmbDisplay.Name = "cmbDisplay";
            cmbDisplay.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbDisplay.Size = new Size(377, 38);
            cmbDisplay.TabIndex = 5;
            cmbDisplay.SelectedIndexChanged += cmbDisplay_SelectedIndexChanged;
            // 
            // guna2CustomGradientPanel1
            // 
            guna2CustomGradientPanel1.BorderColor = Color.FromArgb(242, 103, 34);
            guna2CustomGradientPanel1.BorderRadius = 25;
            guna2CustomGradientPanel1.BorderThickness = 1;
            guna2CustomGradientPanel1.CustomizableEdges = customizableEdges5;
            guna2CustomGradientPanel1.Dock = DockStyle.Fill;
            guna2CustomGradientPanel1.Location = new Point(19, 79);
            guna2CustomGradientPanel1.Margin = new Padding(19);
            guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            guna2CustomGradientPanel1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2CustomGradientPanel1.Size = new Size(880, 760);
            guna2CustomGradientPanel1.TabIndex = 0;
            // 
            // Definicoes
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "Definicoes";
            Size = new Size(1837, 858);
            Load += Definicoes_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            pnlDefsPrev.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel pnlDefsPrev;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label2;
        private Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDisplay;
    }
}
