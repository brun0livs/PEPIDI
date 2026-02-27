namespace PEPIDI.FormsSecundarios
{
    partial class FormGestaoDeFiltros
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGestaoDeFiltros));
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblFechar = new Label();
            lblFuncao = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 4, 4, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 86F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.Size = new Size(1030, 1003);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(254, 107, 0);
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblFechar, 1, 0);
            tableLayoutPanel2.Controls.Add(lblFuncao, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1030, 70);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lblFechar
            // 
            lblFechar.Dock = DockStyle.Right;
            lblFechar.Font = new Font("Roboto", 18.75F);
            lblFechar.ForeColor = Color.White;
            lblFechar.Image = (Image)resources.GetObject("lblFechar.Image");
            lblFechar.Location = new Point(963, 0);
            lblFechar.Margin = new Padding(10, 0, 0, 0);
            lblFechar.Name = "lblFechar";
            lblFechar.Size = new Size(67, 70);
            lblFechar.TabIndex = 1;
            lblFechar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFuncao
            // 
            lblFuncao.AutoSize = true;
            lblFuncao.Dock = DockStyle.Fill;
            lblFuncao.Font = new Font("Roboto Medium", 18.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFuncao.ForeColor = Color.White;
            lblFuncao.Location = new Point(10, 0);
            lblFuncao.Margin = new Padding(10, 0, 0, 0);
            lblFuncao.Name = "lblFuncao";
            lblFuncao.Size = new Size(505, 70);
            lblFuncao.TabIndex = 0;
            lblFuncao.Text = "FILTROS";
            lblFuncao.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormGestaoDeFiltros
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1030, 1003);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 4, 4, 4);
            Name = "FormGestaoDeFiltros";
            Text = "FormGestaoDeFiltros";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblFechar;
        private Label lblFuncao;
    }
}