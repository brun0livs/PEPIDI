namespace PEPIDI
{
    partial class FormConfigDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigDB));
            tableLayoutPanel1 = new TableLayoutPanel();
            label5 = new Label();
            label1 = new Label();
            txtServidor = new TextBox();
            txtBaseDados = new TextBox();
            chkWinAuth = new CheckBox();
            label2 = new Label();
            txtUser = new TextBox();
            label3 = new Label();
            txtPass = new TextBox();
            btnGuardar = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 5F));
            tableLayoutPanel1.Controls.Add(label5, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(txtServidor, 2, 0);
            tableLayoutPanel1.Controls.Add(txtBaseDados, 2, 1);
            tableLayoutPanel1.Controls.Add(chkWinAuth, 2, 2);
            tableLayoutPanel1.Controls.Add(label2, 1, 3);
            tableLayoutPanel1.Controls.Add(txtUser, 2, 3);
            tableLayoutPanel1.Controls.Add(label3, 1, 4);
            tableLayoutPanel1.Controls.Add(txtPass, 2, 4);
            tableLayoutPanel1.Controls.Add(btnGuardar, 2, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.0095024F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.0095024F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.0095024F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.0095024F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.0095024F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 19.9524937F));
            tableLayoutPanel1.Size = new Size(434, 311);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Roboto", 11.25F);
            label5.Location = new Point(24, 49);
            label5.Name = "label5";
            label5.Size = new Size(145, 49);
            label5.TabIndex = 7;
            label5.Text = "Base de Dados :";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 11.25F);
            label1.Location = new Point(24, 0);
            label1.Name = "label1";
            label1.Size = new Size(145, 49);
            label1.TabIndex = 0;
            label1.Text = "Servidor :";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtServidor
            // 
            txtServidor.AcceptsReturn = true;
            txtServidor.Cursor = Cursors.IBeam;
            txtServidor.Dock = DockStyle.Fill;
            txtServidor.Font = new Font("Roboto", 11.25F);
            txtServidor.Location = new Point(175, 10);
            txtServidor.Margin = new Padding(3, 10, 3, 15);
            txtServidor.Name = "txtServidor";
            txtServidor.Size = new Size(232, 26);
            txtServidor.TabIndex = 1;
            // 
            // txtBaseDados
            // 
            txtBaseDados.AcceptsReturn = true;
            txtBaseDados.Cursor = Cursors.IBeam;
            txtBaseDados.Dock = DockStyle.Fill;
            txtBaseDados.Font = new Font("Roboto", 11.25F);
            txtBaseDados.Location = new Point(175, 59);
            txtBaseDados.Margin = new Padding(3, 10, 3, 15);
            txtBaseDados.Name = "txtBaseDados";
            txtBaseDados.Size = new Size(232, 26);
            txtBaseDados.TabIndex = 1;
            // 
            // chkWinAuth
            // 
            chkWinAuth.AutoSize = true;
            chkWinAuth.Dock = DockStyle.Fill;
            chkWinAuth.Location = new Point(175, 101);
            chkWinAuth.Name = "chkWinAuth";
            chkWinAuth.Size = new Size(232, 43);
            chkWinAuth.TabIndex = 8;
            chkWinAuth.Text = "Autenticação do Windows";
            chkWinAuth.UseVisualStyleBackColor = true;
            chkWinAuth.CheckedChanged += ChkWinAuth_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Roboto", 11.25F);
            label2.Location = new Point(24, 147);
            label2.Name = "label2";
            label2.Size = new Size(145, 49);
            label2.TabIndex = 7;
            label2.Text = "Utilizador :";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUser
            // 
            txtUser.AcceptsReturn = true;
            txtUser.Cursor = Cursors.IBeam;
            txtUser.Dock = DockStyle.Fill;
            txtUser.Font = new Font("Roboto", 11.25F);
            txtUser.Location = new Point(175, 157);
            txtUser.Margin = new Padding(3, 10, 3, 15);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(232, 26);
            txtUser.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Roboto", 11.25F);
            label3.Location = new Point(24, 196);
            label3.Name = "label3";
            label3.Size = new Size(145, 49);
            label3.TabIndex = 7;
            label3.Text = "Password :";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPass
            // 
            txtPass.AcceptsReturn = true;
            txtPass.Cursor = Cursors.IBeam;
            txtPass.Dock = DockStyle.Fill;
            txtPass.Font = new Font("Roboto", 11.25F);
            txtPass.Location = new Point(175, 206);
            txtPass.Margin = new Padding(3, 10, 3, 15);
            txtPass.Name = "txtPass";
            txtPass.PasswordChar = '*';
            txtPass.Size = new Size(232, 26);
            txtPass.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.BorderRadius = 10;
            btnGuardar.CustomizableEdges = customizableEdges1;
            btnGuardar.DisabledState.BorderColor = Color.DarkGray;
            btnGuardar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnGuardar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnGuardar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.FillColor = Color.FromArgb(243, 108, 33);
            btnGuardar.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(172, 245);
            btnGuardar.Margin = new Padding(0, 0, 0, 27);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnGuardar.Size = new Size(238, 39);
            btnGuardar.TabIndex = 9;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += BtnGuardar_Click;
            // 
            // FormConfigDB
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.Control;
            ClientSize = new Size(434, 311);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MaximumSize = new Size(450, 350);
            MinimumSize = new Size(450, 350);
            Name = "FormConfigDB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PEPIDI | Configuração da Base de Dados";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label5;
        private Label label1;
        private TextBox txtServidor;
        private TextBox txtBaseDados;
        private CheckBox chkWinAuth;
        private Label label2;
        private TextBox txtUser;
        private Label label3;
        private TextBox txtPass;
        private Guna.UI2.WinForms.Guna2Button btnGuardar;
    }
}