namespace PEPIDI.FormsSecundarios
{
    partial class FormFuncao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFuncao));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblFechar = new Label();
            lblFuncao = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnGuardar = new Guna.UI2.WinForms.Guna2Button();
            btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
            label2 = new Label();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnColorPicker = new Guna.UI2.WinForms.Guna2Button();
            txtCorHex = new Guna.UI2.WinForms.Guna2TextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
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
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.Size = new Size(800, 450);
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
            tableLayoutPanel2.Size = new Size(800, 56);
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
            lblFechar.Size = new Size(67, 56);
            lblFechar.TabIndex = 1;
            lblFechar.TextAlign = ContentAlignment.MiddleLeft;
            lblFechar.Click += LblFechar_Click;
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
            lblFuncao.Size = new Size(390, 56);
            lblFuncao.TabIndex = 0;
            lblFuncao.Text = "FUNÇÃO";
            lblFuncao.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = SystemColors.Control;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(btnGuardar, 3, 0);
            tableLayoutPanel3.Controls.Add(btnCancelar, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 393);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(800, 57);
            tableLayoutPanel3.TabIndex = 1;
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
            btnGuardar.Font = new Font("Roboto", 18.75F);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(610, 10);
            btnGuardar.Margin = new Padding(10);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnGuardar.Size = new Size(180, 37);
            btnGuardar.TabIndex = 5;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += Guardar_Click;
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
            btnCancelar.FillColor = Color.Silver;
            btnCancelar.Font = new Font("Roboto", 18.75F);
            btnCancelar.ForeColor = Color.Black;
            btnCancelar.Location = new Point(10, 10);
            btnCancelar.Margin = new Padding(10);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnCancelar.Size = new Size(180, 37);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += LblFechar_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tableLayoutPanel4.Controls.Add(txtNome, 1, 0);
            tableLayoutPanel4.Controls.Add(label1, 0, 0);
            tableLayoutPanel4.Controls.Add(label2, 0, 1);
            tableLayoutPanel4.Controls.Add(tableLayoutPanel5, 1, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 56);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(800, 337);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // txtNome
            // 
            txtNome.AutoRoundedCorners = true;
            txtNome.CustomizableEdges = customizableEdges5;
            txtNome.DefaultText = "";
            txtNome.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtNome.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtNome.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtNome.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtNome.Dock = DockStyle.Fill;
            txtNome.FocusedState.BorderColor = Color.FromArgb(243, 108, 33);
            txtNome.Font = new Font("Roboto", 18.75F);
            txtNome.ForeColor = Color.Black;
            txtNome.HoverState.BorderColor = Color.Gray;
            txtNome.Location = new Point(203, 60);
            txtNome.Margin = new Padding(3, 60, 3, 60);
            txtNome.MaxLength = 16;
            txtNome.Name = "txtNome";
            txtNome.PlaceholderForeColor = Color.Silver;
            txtNome.PlaceholderText = "";
            txtNome.SelectedText = "";
            txtNome.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtNome.Size = new Size(594, 48);
            txtNome.TabIndex = 2;
            txtNome.TextOffset = new Point(10, 0);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 18.75F);
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(200, 168);
            label1.TabIndex = 0;
            label1.Text = "Nome :";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Roboto", 18.75F);
            label2.Location = new Point(0, 168);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(200, 169);
            label2.TabIndex = 1;
            label2.Text = "Cor (Cód. Hex.):";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.61074F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3892632F));
            tableLayoutPanel5.Controls.Add(btnColorPicker, 1, 0);
            tableLayoutPanel5.Controls.Add(txtCorHex, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(200, 168);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(600, 169);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // btnColorPicker
            // 
            btnColorPicker.BorderRadius = 10;
            btnColorPicker.CustomizableEdges = customizableEdges7;
            btnColorPicker.DisabledState.BorderColor = Color.DarkGray;
            btnColorPicker.DisabledState.CustomBorderColor = Color.DarkGray;
            btnColorPicker.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnColorPicker.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnColorPicker.Dock = DockStyle.Fill;
            btnColorPicker.FillColor = Color.FromArgb(243, 108, 33);
            btnColorPicker.Font = new Font("Roboto", 18.75F);
            btnColorPicker.ForeColor = Color.White;
            btnColorPicker.Location = new Point(409, 60);
            btnColorPicker.Margin = new Padding(10, 60, 10, 60);
            btnColorPicker.Name = "btnColorPicker";
            btnColorPicker.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnColorPicker.Size = new Size(181, 49);
            btnColorPicker.TabIndex = 6;
            btnColorPicker.Text = "Escolher";
            btnColorPicker.Click += EscolherCor_Click;
            // 
            // txtCorHex
            // 
            txtCorHex.AutoRoundedCorners = true;
            txtCorHex.CustomizableEdges = customizableEdges9;
            txtCorHex.DefaultText = "";
            txtCorHex.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtCorHex.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtCorHex.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtCorHex.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtCorHex.Dock = DockStyle.Fill;
            txtCorHex.FocusedState.BorderColor = Color.FromArgb(243, 108, 33);
            txtCorHex.Font = new Font("Roboto", 18.75F);
            txtCorHex.ForeColor = Color.Black;
            txtCorHex.HoverState.BorderColor = Color.Gray;
            txtCorHex.Location = new Point(3, 60);
            txtCorHex.Margin = new Padding(3, 60, 3, 60);
            txtCorHex.MaxLength = 16;
            txtCorHex.Name = "txtCorHex";
            txtCorHex.PlaceholderForeColor = Color.Silver;
            txtCorHex.PlaceholderText = "#FE6B00 (Laranja)";
            txtCorHex.SelectedText = "";
            txtCorHex.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtCorHex.Size = new Size(393, 49);
            txtCorHex.TabIndex = 3;
            txtCorHex.TextOffset = new Point(10, 0);
            // 
            // FormFuncao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormFuncao";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Formfuncao";
            Load += FormFuncao_Load;
            KeyPress += FormFuncao_KeyPress;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblFechar;
        private Label lblFuncao;
        private TableLayoutPanel tableLayoutPanel3;
        private Guna.UI2.WinForms.Guna2Button btnGuardar;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label1;
        private Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtNome;
        private TableLayoutPanel tableLayoutPanel5;
        private Guna.UI2.WinForms.Guna2Button btnColorPicker;
        private Guna.UI2.WinForms.Guna2TextBox txtCorHex;
    }
}