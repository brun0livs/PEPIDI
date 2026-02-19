namespace PEPIDI.UCs.UcsSecundarios
{
    partial class CabecalhoPedido
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
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblModelo = new Label();
            lblTamanho = new Label();
            lblQuantDisp = new Label();
            lblQuant = new Label();
            guna2Panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.Controls.Add(tableLayoutPanel1);
            guna2Panel1.CustomBorderColor = Color.FromArgb(224, 224, 224);
            guna2Panel1.CustomBorderThickness = new Padding(0, 0, 0, 1);
            guna2Panel1.CustomizableEdges = customizableEdges3;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.Location = new Point(0, 10);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel1.Size = new Size(683, 30);
            guna2Panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(lblModelo, 0, 0);
            tableLayoutPanel1.Controls.Add(lblTamanho, 1, 0);
            tableLayoutPanel1.Controls.Add(lblQuantDisp, 2, 0);
            tableLayoutPanel1.Controls.Add(lblQuant, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
            tableLayoutPanel1.Size = new Size(683, 30);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Dock = DockStyle.Fill;
            lblModelo.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblModelo.ForeColor = Color.FromArgb(64, 64, 64);
            lblModelo.Location = new Point(3, 0);
            lblModelo.Margin = new Padding(3, 0, 3, 5);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(267, 25);
            lblModelo.TabIndex = 0;
            lblModelo.Text = "Modelo";
            lblModelo.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lblTamanho
            // 
            lblTamanho.AutoSize = true;
            lblTamanho.Dock = DockStyle.Fill;
            lblTamanho.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblTamanho.ForeColor = Color.FromArgb(64, 64, 64);
            lblTamanho.Location = new Point(276, 0);
            lblTamanho.Margin = new Padding(3, 0, 3, 5);
            lblTamanho.Name = "lblTamanho";
            lblTamanho.Size = new Size(130, 25);
            lblTamanho.TabIndex = 1;
            lblTamanho.Text = "Tamanho";
            lblTamanho.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lblQuantDisp
            // 
            lblQuantDisp.AutoSize = true;
            lblQuantDisp.Dock = DockStyle.Fill;
            lblQuantDisp.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblQuantDisp.ForeColor = Color.FromArgb(64, 64, 64);
            lblQuantDisp.Location = new Point(412, 0);
            lblQuantDisp.Margin = new Padding(3, 0, 3, 5);
            lblQuantDisp.Name = "lblQuantDisp";
            lblQuantDisp.Size = new Size(130, 25);
            lblQuantDisp.TabIndex = 2;
            lblQuantDisp.Text = "Quant. Disp";
            lblQuantDisp.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lblQuant
            // 
            lblQuant.AutoSize = true;
            lblQuant.Dock = DockStyle.Fill;
            lblQuant.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblQuant.ForeColor = Color.FromArgb(64, 64, 64);
            lblQuant.Location = new Point(548, 0);
            lblQuant.Margin = new Padding(3, 0, 3, 5);
            lblQuant.Name = "lblQuant";
            lblQuant.Size = new Size(132, 25);
            lblQuant.TabIndex = 3;
            lblQuant.Text = "Quantidade";
            lblQuant.TextAlign = ContentAlignment.BottomCenter;
            // 
            // CabecalhoPedido
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(guna2Panel1);
            Margin = new Padding(0);
            Name = "CabecalhoPedido";
            Padding = new Padding(0, 10, 0, 0);
            Size = new Size(683, 40);
            guna2Panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblQuant;
        private Label lblQuantDisp;
        private Label lblTamanho;
        private Label lblModelo;
    }
}
