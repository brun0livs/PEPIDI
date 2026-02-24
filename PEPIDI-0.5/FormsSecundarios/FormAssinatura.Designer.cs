namespace PEPIDI
{
    partial class FormAssinatura
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            panelFundo = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnLimpar = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            chkAceito = new CheckBox();
            btnCancelar = new Button();
            btnConfirmar = new Button();
            txtLegal = new RichTextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblAssinatura = new Label();
            lblInstrucao = new Label();
            picSignature = new PictureBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            label1 = new Label();
            lblTitulo = new Label();
            tableLayoutPanel5 = new TableLayoutPanel();
            lblReceber = new Label();
            lblDevolver = new Label();
            dgvReceber = new PEPIDI.Models.PEPIDIDataGridView();
            dgvDevolver = new PEPIDI.Models.PEPIDIDataGridView();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            panelFundo.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSignature).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceber).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDevolver).BeginInit();
            SuspendLayout();
            // 
            // panelFundo
            // 
            panelFundo.BorderStyle = BorderStyle.FixedSingle;
            panelFundo.Controls.Add(tableLayoutPanel1);
            panelFundo.Dock = DockStyle.Fill;
            panelFundo.Location = new Point(0, 0);
            panelFundo.Margin = new Padding(4, 3, 4, 3);
            panelFundo.Name = "panelFundo";
            panelFundo.Size = new Size(817, 831);
            panelFundo.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(btnLimpar, 0, 5);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 6);
            tableLayoutPanel1.Controls.Add(txtLegal, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 3);
            tableLayoutPanel1.Controls.Add(picSignature, 0, 4);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.012081F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 40.0604057F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15.0226526F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 2.00302029F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.79251F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.094232F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.0151014F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(815, 829);
            tableLayoutPanel1.TabIndex = 22;
            // 
            // btnLimpar
            // 
            btnLimpar.BackColor = Color.Gray;
            btnLimpar.Dock = DockStyle.Fill;
            btnLimpar.FlatStyle = FlatStyle.Flat;
            btnLimpar.ForeColor = Color.White;
            btnLimpar.Location = new Point(4, 681);
            btnLimpar.Margin = new Padding(4, 4, 616, 4);
            btnLimpar.Name = "btnLimpar";
            btnLimpar.Size = new Size(195, 59);
            btnLimpar.TabIndex = 4;
            btnLimpar.Text = "Limpar";
            btnLimpar.UseVisualStyleBackColor = false;
            btnLimpar.Click += btnLimpar_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Controls.Add(chkAceito, 1, 0);
            tableLayoutPanel4.Controls.Add(btnCancelar, 0, 0);
            tableLayoutPanel4.Controls.Add(btnConfirmar, 2, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 754);
            tableLayoutPanel4.Margin = new Padding(0, 10, 0, 10);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(815, 65);
            tableLayoutPanel4.TabIndex = 27;
            // 
            // chkAceito
            // 
            chkAceito.AutoSize = true;
            chkAceito.Dock = DockStyle.Fill;
            chkAceito.Location = new Point(207, 3);
            chkAceito.Margin = new Padding(4, 3, 4, 3);
            chkAceito.Name = "chkAceito";
            chkAceito.Size = new Size(399, 59);
            chkAceito.TabIndex = 5;
            chkAceito.Text = "Li e concordo com os termos acima descritos.";
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Firebrick;
            btnCancelar.Dock = DockStyle.Fill;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(4, 4);
            btnCancelar.Margin = new Padding(4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(195, 57);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "CANCELAR";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.FromArgb(40, 167, 69);
            btnConfirmar.Dock = DockStyle.Fill;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(614, 4);
            btnConfirmar.Margin = new Padding(4);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(197, 57);
            btnConfirmar.TabIndex = 7;
            btnConfirmar.Text = "CONFIRMAR";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // txtLegal
            // 
            txtLegal.BackColor = Color.White;
            txtLegal.BorderStyle = BorderStyle.None;
            txtLegal.Dock = DockStyle.Fill;
            txtLegal.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLegal.ForeColor = Color.DimGray;
            txtLegal.Location = new Point(4, 401);
            txtLegal.Margin = new Padding(4, 3, 4, 3);
            txtLegal.Name = "txtLegal";
            txtLegal.ReadOnly = true;
            txtLegal.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtLegal.Size = new Size(807, 118);
            txtLegal.TabIndex = 3;
            txtLegal.Text = "";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblAssinatura, 0, 0);
            tableLayoutPanel2.Controls.Add(lblInstrucao, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 522);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(815, 16);
            tableLayoutPanel2.TabIndex = 24;
            // 
            // lblAssinatura
            // 
            lblAssinatura.AutoSize = true;
            lblAssinatura.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAssinatura.Location = new Point(4, 0);
            lblAssinatura.Margin = new Padding(4, 0, 4, 0);
            lblAssinatura.Name = "lblAssinatura";
            lblAssinatura.Size = new Size(82, 16);
            lblAssinatura.TabIndex = 16;
            lblAssinatura.Text = "Assinatura:";
            // 
            // lblInstrucao
            // 
            lblInstrucao.AutoSize = true;
            lblInstrucao.ForeColor = Color.Gray;
            lblInstrucao.Location = new Point(411, 0);
            lblInstrucao.Margin = new Padding(4, 0, 4, 0);
            lblInstrucao.Name = "lblInstrucao";
            lblInstrucao.Size = new Size(115, 15);
            lblInstrucao.TabIndex = 16;
            lblInstrucao.Text = "Use o rato ou caneta";
            // 
            // picSignature
            // 
            picSignature.BackColor = Color.White;
            picSignature.BorderStyle = BorderStyle.FixedSingle;
            picSignature.Cursor = Cursors.Cross;
            picSignature.Dock = DockStyle.Fill;
            picSignature.Location = new Point(4, 541);
            picSignature.Margin = new Padding(4, 3, 4, 3);
            picSignature.Name = "picSignature";
            picSignature.Size = new Size(807, 133);
            picSignature.TabIndex = 17;
            picSignature.TabStop = false;
            picSignature.MouseDown += picSignature_MouseDown;
            picSignature.MouseMove += picSignature_MouseMove;
            picSignature.MouseUp += picSignature_MouseUp;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.FromArgb(243, 108, 33);
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 91.90184F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.09816F));
            tableLayoutPanel3.Controls.Add(label1, 1, 0);
            tableLayoutPanel3.Controls.Add(lblTitulo, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(815, 66);
            tableLayoutPanel3.TabIndex = 28;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(243, 108, 33);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Image = Properties.Resources.Close;
            label1.Location = new Point(749, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(66, 66);
            label1.TabIndex = 2;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += btnCancelar_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.FromArgb(243, 108, 33);
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(66, 0);
            lblTitulo.Margin = new Padding(66, 0, 0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(683, 66);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Confirmação de Movimentos";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(lblReceber, 0, 0);
            tableLayoutPanel5.Controls.Add(lblDevolver, 1, 0);
            tableLayoutPanel5.Controls.Add(dgvReceber, 0, 1);
            tableLayoutPanel5.Controls.Add(dgvDevolver, 1, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 69);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 6.74846649F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 93.25153F));
            tableLayoutPanel5.Size = new Size(809, 326);
            tableLayoutPanel5.TabIndex = 29;
            // 
            // lblReceber
            // 
            lblReceber.AutoSize = true;
            lblReceber.Dock = DockStyle.Fill;
            lblReceber.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblReceber.ForeColor = Color.FromArgb(40, 167, 69);
            lblReceber.Location = new Point(4, 0);
            lblReceber.Margin = new Padding(4, 0, 4, 0);
            lblReceber.Name = "lblReceber";
            lblReceber.Size = new Size(396, 22);
            lblReceber.TabIndex = 1;
            lblReceber.Text = "A RECEBER (Entrega)";
            // 
            // lblDevolver
            // 
            lblDevolver.AutoSize = true;
            lblDevolver.Dock = DockStyle.Fill;
            lblDevolver.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDevolver.ForeColor = Color.Firebrick;
            lblDevolver.Location = new Point(408, 0);
            lblDevolver.Margin = new Padding(4, 0, 4, 0);
            lblDevolver.Name = "lblDevolver";
            lblDevolver.Size = new Size(397, 22);
            lblDevolver.TabIndex = 12;
            lblDevolver.Text = "A DEVOLVER (Retoma)";
            // 
            // dgvReceber
            // 
            dgvReceber.AllowUserToAddRows = false;
            dgvReceber.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvReceber.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvReceber.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReceber.BackgroundColor = Color.White;
            dgvReceber.BorderStyle = BorderStyle.None;
            dgvReceber.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvReceber.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvReceber.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvReceber.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvReceber.DefaultCellStyle = dataGridViewCellStyle3;
            dgvReceber.Dock = DockStyle.Fill;
            dgvReceber.EnableHeadersVisualStyles = false;
            dgvReceber.GridColor = SystemColors.Control;
            dgvReceber.HeaderFontSize = 15F;
            dgvReceber.Location = new Point(3, 25);
            dgvReceber.MultiSelect = false;
            dgvReceber.Name = "dgvReceber";
            dgvReceber.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvReceber.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvReceber.RowTemplate.Height = 54;
            dgvReceber.ScrollBars = ScrollBars.None;
            dgvReceber.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReceber.Size = new Size(398, 298);
            dgvReceber.TabIndex = 13;
            // 
            // dgvDevolver
            // 
            dgvDevolver.AllowUserToAddRows = false;
            dgvDevolver.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = Color.Transparent;
            dgvDevolver.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvDevolver.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDevolver.BackgroundColor = Color.White;
            dgvDevolver.BorderStyle = BorderStyle.None;
            dgvDevolver.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvDevolver.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvDevolver.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvDevolver.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.Transparent;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle7.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvDevolver.DefaultCellStyle = dataGridViewCellStyle7;
            dgvDevolver.Dock = DockStyle.Fill;
            dgvDevolver.EnableHeadersVisualStyles = false;
            dgvDevolver.GridColor = SystemColors.Control;
            dgvDevolver.HeaderFontSize = 15F;
            dgvDevolver.Location = new Point(407, 25);
            dgvDevolver.MultiSelect = false;
            dgvDevolver.Name = "dgvDevolver";
            dgvDevolver.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = Color.Transparent;
            dgvDevolver.RowsDefaultCellStyle = dataGridViewCellStyle8;
            dgvDevolver.RowTemplate.Height = 54;
            dgvDevolver.ScrollBars = ScrollBars.None;
            dgvDevolver.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDevolver.Size = new Size(399, 298);
            dgvDevolver.TabIndex = 13;
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // FormAssinatura
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(817, 831);
            Controls.Add(panelFundo);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FormAssinatura";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Assinatura";
            panelFundo.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picSignature).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReceber).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDevolver).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panelFundo;

        // As duas grids e labels
        private System.Windows.Forms.Label lblReceber;
        private System.Windows.Forms.Label lblDevolver;

        private System.Windows.Forms.PictureBox picSignature;
        private System.Windows.Forms.Label lblInstrucao;
        private System.Windows.Forms.RichTextBox txtLegal;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblAssinatura;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox chkAceito;
        private Button btnCancelar;
        private Button btnConfirmar;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Button btnLimpar;
        private TableLayoutPanel tableLayoutPanel3;
        private Label lblTitulo;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel5;
        private Models.PEPIDIDataGridView dgvReceber;
        private Models.PEPIDIDataGridView dgvDevolver;
    }
}