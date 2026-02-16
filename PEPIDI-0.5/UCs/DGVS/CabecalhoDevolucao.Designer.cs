namespace PEPIDI.UCs.DGVS
{
    partial class CabecalhoDevolucao
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
            tableLayoutPanel1 = new TableLayoutPanel();
            lblModelo = new Label();
            lblTamanho = new Label();
            lblQuantDisp = new Label();
            label1 = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
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
            tableLayoutPanel1.Controls.Add(label1, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 10);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
            tableLayoutPanel1.Size = new Size(594, 30);
            tableLayoutPanel1.TabIndex = 6;
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
            lblModelo.Size = new Size(231, 25);
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
            lblTamanho.Location = new Point(240, 0);
            lblTamanho.Margin = new Padding(3, 0, 3, 5);
            lblTamanho.Name = "lblTamanho";
            lblTamanho.Size = new Size(112, 25);
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
            lblQuantDisp.Location = new Point(358, 0);
            lblQuantDisp.Margin = new Padding(3, 0, 3, 5);
            lblQuantDisp.Name = "lblQuantDisp";
            lblQuantDisp.Size = new Size(112, 25);
            lblQuantDisp.TabIndex = 2;
            lblQuantDisp.Text = "Quantidade";
            lblQuantDisp.TextAlign = ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(476, 0);
            label1.Margin = new Padding(3, 0, 3, 5);
            label1.Name = "label1";
            label1.Size = new Size(115, 25);
            label1.TabIndex = 3;
            label1.Text = "Selecionar";
            label1.TextAlign = ContentAlignment.BottomCenter;
            // 
            // CabecalhoDevolucao
            // 
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "CabecalhoDevolucao";
            Padding = new Padding(0, 10, 0, 0);
            Size = new Size(594, 40);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblModelo;
        private Label lblTamanho;
        private Label lblQuantDisp;
        private Label label1;
    }
}
