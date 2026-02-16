namespace PEPIDI.UCs.DGVS
{
    partial class LinhaItem
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
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblQuantDisp = new Label();
            lblTamanho = new Label();
            lblModelo = new Label();
            cmbQuant = new Guna.UI2.WinForms.Guna2ComboBox();
            chkEntregar = new CheckBox();
            guna2Panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.Transparent;
            guna2Panel1.BorderRadius = 16;
            guna2Panel1.Controls.Add(tableLayoutPanel1);
            guna2Panel1.CustomizableEdges = customizableEdges3;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.FromArgb(224, 224, 224);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Margin = new Padding(3, 4, 3, 4);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel1.Size = new Size(594, 40);
            guna2Panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.Controls.Add(lblQuantDisp, 2, 0);
            tableLayoutPanel1.Controls.Add(lblTamanho, 1, 0);
            tableLayoutPanel1.Controls.Add(lblModelo, 0, 0);
            tableLayoutPanel1.Controls.Add(cmbQuant, 3, 0);
            tableLayoutPanel1.Controls.Add(chkEntregar, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(594, 40);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // lblQuantDisp
            // 
            lblQuantDisp.AutoSize = true;
            lblQuantDisp.Dock = DockStyle.Fill;
            lblQuantDisp.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuantDisp.ForeColor = Color.FromArgb(64, 64, 64);
            lblQuantDisp.Location = new Point(358, 0);
            lblQuantDisp.Name = "lblQuantDisp";
            lblQuantDisp.Size = new Size(112, 40);
            lblQuantDisp.TabIndex = 2;
            lblQuantDisp.Text = "Quant";
            lblQuantDisp.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTamanho
            // 
            lblTamanho.AutoSize = true;
            lblTamanho.Dock = DockStyle.Fill;
            lblTamanho.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTamanho.ForeColor = Color.FromArgb(64, 64, 64);
            lblTamanho.Location = new Point(240, 0);
            lblTamanho.Name = "lblTamanho";
            lblTamanho.Size = new Size(112, 40);
            lblTamanho.TabIndex = 1;
            lblTamanho.Text = "Tamanho";
            lblTamanho.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Dock = DockStyle.Fill;
            lblModelo.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblModelo.ForeColor = Color.FromArgb(64, 64, 64);
            lblModelo.Location = new Point(3, 0);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(231, 40);
            lblModelo.TabIndex = 0;
            lblModelo.Text = "Modelo";
            lblModelo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbQuant
            // 
            cmbQuant.BackColor = Color.Transparent;
            cmbQuant.BorderThickness = 0;
            cmbQuant.CustomizableEdges = customizableEdges1;
            cmbQuant.Dock = DockStyle.Fill;
            cmbQuant.DrawMode = DrawMode.OwnerDrawFixed;
            cmbQuant.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbQuant.FillColor = Color.FromArgb(224, 224, 224);
            cmbQuant.FocusedColor = Color.FromArgb(64, 64, 64);
            cmbQuant.FocusedState.BorderColor = Color.FromArgb(64, 64, 64);
            cmbQuant.FocusedState.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbQuant.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbQuant.ForeColor = Color.FromArgb(64, 64, 64);
            cmbQuant.IntegralHeight = false;
            cmbQuant.ItemHeight = 29;
            cmbQuant.Location = new Point(498, 3);
            cmbQuant.Margin = new Padding(25, 3, 25, 0);
            cmbQuant.Name = "cmbQuant";
            cmbQuant.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cmbQuant.Size = new Size(9, 35);
            cmbQuant.TabIndex = 3;
            cmbQuant.TextAlign = HorizontalAlignment.Center;
            cmbQuant.SelectedIndexChanged += CmbQuant_SelectedIndexChanged;
            // 
            // chkEntregar
            // 
            chkEntregar.AutoSize = true;
            chkEntregar.CheckAlign = ContentAlignment.MiddleCenter;
            chkEntregar.Dock = DockStyle.Fill;
            chkEntregar.Font = new Font("Roboto", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkEntregar.Location = new Point(532, 0);
            chkEntregar.Margin = new Padding(0);
            chkEntregar.Name = "chkEntregar";
            chkEntregar.Size = new Size(62, 40);
            chkEntregar.TabIndex = 4;
            chkEntregar.UseVisualStyleBackColor = true;
            // 
            // LinhaItem
            // 
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(guna2Panel1);
            Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(3, 4, 3, 2);
            Name = "LinhaItem";
            Size = new Size(594, 40);
            Load += Linha_Load;
            guna2Panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblQuantDisp;
        private Label lblTamanho;
        private Label lblModelo;
        private Guna.UI2.WinForms.Guna2ComboBox cmbQuant;
        private CheckBox chkEntregar;
    }
}
