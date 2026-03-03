namespace PEPIDI.UCs.DGVS
{
    partial class LinhaDevolucao
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
            lblQuantDevolvida = new Label();
            lblTamanho = new Label();
            lblModelo = new Label();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            tableLayoutPanel1.SuspendLayout();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(lblQuantDevolvida, 2, 0);
            tableLayoutPanel1.Controls.Add(lblTamanho, 1, 0);
            tableLayoutPanel1.Controls.Add(lblModelo, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(594, 40);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // lblQuantDevolvida
            // 
            lblQuantDevolvida.AutoSize = true;
            lblQuantDevolvida.Dock = DockStyle.Fill;
            lblQuantDevolvida.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuantDevolvida.ForeColor = Color.FromArgb(64, 64, 64);
            lblQuantDevolvida.Location = new Point(448, 0);
            lblQuantDevolvida.Name = "lblQuantDevolvida";
            lblQuantDevolvida.Size = new Size(143, 40);
            lblQuantDevolvida.TabIndex = 2;
            lblQuantDevolvida.Text = "Quant";
            lblQuantDevolvida.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTamanho
            // 
            lblTamanho.AutoSize = true;
            lblTamanho.Dock = DockStyle.Fill;
            lblTamanho.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTamanho.ForeColor = Color.FromArgb(64, 64, 64);
            lblTamanho.Location = new Point(300, 0);
            lblTamanho.Name = "lblTamanho";
            lblTamanho.Size = new Size(142, 40);
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
            lblModelo.Size = new Size(291, 40);
            lblModelo.TabIndex = 0;
            lblModelo.Text = "Modelo";
            lblModelo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.Transparent;
            guna2Panel1.BorderRadius = 16;
            guna2Panel1.Controls.Add(tableLayoutPanel1);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.FromArgb(224, 224, 224);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Margin = new Padding(0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(594, 40);
            guna2Panel1.TabIndex = 3;
            // 
            // LinhaDevolucao
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            Controls.Add(guna2Panel1);
            Name = "LinhaDevolucao";
            Size = new Size(594, 40);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            guna2Panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblQuantDevolvida;
        private Label lblTamanho;
        private Label lblModelo;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
    }
}
