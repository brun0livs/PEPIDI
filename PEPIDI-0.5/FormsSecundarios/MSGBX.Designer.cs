namespace PEPIDI.FormsSecundarios
{
    partial class MSGBX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSGBX));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblFechar = new Label();
            lblTitulo = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnOK = new Guna.UI2.WinForms.Guna2Button();
            btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            lblMessage = new Label();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            txtEntradaDeDados = new Guna.UI2.WinForms.Guna2TextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(800, 245);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(254, 107, 0);
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblFechar, 1, 0);
            tableLayoutPanel2.Controls.Add(lblTitulo, 0, 0);
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
            lblFechar.Click += btnOK_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Roboto Medium", 18.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(10, 0);
            lblTitulo.Margin = new Padding(10, 0, 0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(390, 61);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "PEPIDI";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.White;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(btnOK, 3, 0);
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
            // btnOK
            // 
            btnOK.BorderRadius = 10;
            btnOK.CustomizableEdges = customizableEdges1;
            btnOK.DisabledState.BorderColor = Color.DarkGray;
            btnOK.DisabledState.CustomBorderColor = Color.DarkGray;
            btnOK.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnOK.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnOK.Dock = DockStyle.Fill;
            btnOK.FillColor = Color.FromArgb(243, 108, 33);
            btnOK.Font = new Font("Roboto", 18.75F);
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(610, 10);
            btnOK.Margin = new Padding(10);
            btnOK.Name = "btnOK";
            btnOK.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnOK.Size = new Size(180, 42);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BorderRadius = 10;
            btnCancelar.CustomizableEdges = customizableEdges3;
            btnCancelar.DisabledState.BorderColor = Color.DarkGray;
            btnCancelar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancelar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancelar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancelar.Dock = DockStyle.Fill;
            btnCancelar.Enabled = false;
            btnCancelar.FillColor = Color.Silver;
            btnCancelar.Font = new Font("Roboto", 18.75F);
            btnCancelar.ForeColor = Color.Black;
            btnCancelar.Location = new Point(10, 10);
            btnCancelar.Margin = new Padding(10);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnCancelar.Size = new Size(180, 42);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Visible = false;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackgroundImageLayout = ImageLayout.None;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(lblMessage, 0, 0);
            tableLayoutPanel4.Controls.Add(txtEntradaDeDados, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 61);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(800, 122);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Roboto", 18.75F);
            lblMessage.Location = new Point(20, 0);
            lblMessage.Margin = new Padding(20, 0, 3, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(377, 122);
            lblMessage.TabIndex = 3;
            lblMessage.Text = "Message";
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // txtEntradaDeDados
            // 
            txtEntradaDeDados.AutoRoundedCorners = true;
            txtEntradaDeDados.CustomizableEdges = customizableEdges5;
            txtEntradaDeDados.DefaultText = "";
            txtEntradaDeDados.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEntradaDeDados.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEntradaDeDados.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEntradaDeDados.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEntradaDeDados.Dock = DockStyle.Fill;
            txtEntradaDeDados.FocusedState.BorderColor = Color.FromArgb(243, 108, 33);
            txtEntradaDeDados.Font = new Font("Roboto", 11.25F);
            txtEntradaDeDados.ForeColor = Color.Black;
            txtEntradaDeDados.HoverState.BorderColor = Color.Gray;
            txtEntradaDeDados.Location = new Point(410, 40);
            txtEntradaDeDados.Margin = new Padding(10, 40, 40, 40);
            txtEntradaDeDados.MaxLength = 16;
            txtEntradaDeDados.Name = "txtEntradaDeDados";
            txtEntradaDeDados.PlaceholderForeColor = Color.Silver;
            txtEntradaDeDados.PlaceholderText = "NMEC";
            txtEntradaDeDados.SelectedText = "";
            txtEntradaDeDados.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtEntradaDeDados.Size = new Size(350, 42);
            txtEntradaDeDados.TabIndex = 5;
            txtEntradaDeDados.TextOffset = new Point(10, 0);
            // 
            // MSGBX
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 245);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "MSGBX";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MessageBox";
            TopMost = true;
            Load += MSGBX_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblFechar;
        private Label lblTitulo;
        private TableLayoutPanel tableLayoutPanel3;
        private Guna.UI2.WinForms.Guna2Button btnOK;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private TableLayoutPanel tableLayoutPanel4;
        private Label lblMessage;
        private Guna.UI2.WinForms.Guna2TextBox txtEntradaDeDados;
    }
}