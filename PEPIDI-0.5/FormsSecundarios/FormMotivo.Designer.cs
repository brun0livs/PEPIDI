namespace PEPIDI.FormsSecundarios
{
    partial class FormMotivo
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMotivo));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            txtMotivo = new Guna.UI2.WinForms.Guna2TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblFechar = new Label();
            lblFuncao = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnGuardar = new Guna.UI2.WinForms.Guna2Button();
            btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(txtMotivo, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(800, 245);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // txtMotivo
            // 
            txtMotivo.BorderColor = Color.FromArgb(224, 224, 224);
            txtMotivo.BorderRadius = 15;
            txtMotivo.CustomizableEdges = customizableEdges1;
            txtMotivo.DefaultText = "";
            txtMotivo.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtMotivo.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtMotivo.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtMotivo.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtMotivo.Dock = DockStyle.Fill;
            txtMotivo.FocusedState.BorderColor = Color.FromArgb(243, 108, 33);
            txtMotivo.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMotivo.HoverState.BorderColor = Color.FromArgb(255, 192, 128);
            txtMotivo.IconRightCursor = Cursors.IBeam;
            txtMotivo.Location = new Point(10, 71);
            txtMotivo.Margin = new Padding(10);
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.PlaceholderForeColor = Color.Silver;
            txtMotivo.PlaceholderText = "Insira o motivo para a reprovação";
            txtMotivo.SelectedText = "";
            txtMotivo.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtMotivo.Size = new Size(780, 102);
            txtMotivo.TabIndex = 6;
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
            tableLayoutPanel2.Size = new Size(800, 61);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblFechar
            // 
            lblFechar.Dock = DockStyle.Right;
            lblFechar.Font = new Font("Roboto", 18.75F);
            lblFechar.ForeColor = Color.White;
            lblFechar.Image = (Image)resources.GetObject("lblFechar.Image");
            lblFechar.Location = new Point(733, 0);
            lblFechar.Margin = new Padding(10, 0, 0, 0);
            lblFechar.Name = "lblFechar";
            lblFechar.Size = new Size(67, 61);
            lblFechar.TabIndex = 1;
            lblFechar.TextAlign = ContentAlignment.MiddleLeft;
            lblFechar.Click += btnCancelar_Click;
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
            lblFuncao.Size = new Size(390, 61);
            lblFuncao.TabIndex = 0;
            lblFuncao.Text = "MOTIVO DE REPROVAÇÃO";
            lblFuncao.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.White;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(btnGuardar, 3, 0);
            tableLayoutPanel3.Controls.Add(btnCancelar, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 183);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(800, 62);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.BorderRadius = 10;
            btnGuardar.CustomizableEdges = customizableEdges3;
            btnGuardar.DisabledState.BorderColor = Color.DarkGray;
            btnGuardar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnGuardar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnGuardar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.FillColor = Color.FromArgb(243, 108, 33);
            btnGuardar.Font = new Font("Roboto", 18.75F);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(610, 10);
            btnGuardar.Margin = new Padding(10);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnGuardar.Size = new Size(180, 42);
            btnGuardar.TabIndex = 5;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BorderRadius = 10;
            btnCancelar.CustomizableEdges = customizableEdges5;
            btnCancelar.DisabledState.BorderColor = Color.DarkGray;
            btnCancelar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancelar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancelar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancelar.Dock = DockStyle.Fill;
            btnCancelar.FillColor = Color.Silver;
            btnCancelar.Font = new Font("Roboto", 18.75F);
            btnCancelar.ForeColor = Color.Black;
            btnCancelar.Location = new Point(10, 10);
            btnCancelar.Margin = new Padding(10);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnCancelar.Size = new Size(180, 42);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FormMotivo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 245);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormMotivo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormMotivo";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblFechar;
        private Label lblFuncao;
        private TableLayoutPanel tableLayoutPanel3;
        private Guna.UI2.WinForms.Guna2Button btnGuardar;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private Guna.UI2.WinForms.Guna2TextBox txtMotivo;
    }
}