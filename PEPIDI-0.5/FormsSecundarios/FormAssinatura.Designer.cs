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
            System.Windows.Forms.DataGridViewCellStyle cellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAssinatura));

            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelFundo = new System.Windows.Forms.Panel();

            this.lblReceber = new System.Windows.Forms.Label();
            this.dgvReceber = new System.Windows.Forms.DataGridView();

            this.lblDevolver = new System.Windows.Forms.Label();
            this.dgvDevolver = new System.Windows.Forms.DataGridView();

            // NOVO: Texto Legal
            this.txtLegal = new System.Windows.Forms.RichTextBox();

            this.picSignature = new System.Windows.Forms.PictureBox();
            this.lblAssinatura = new System.Windows.Forms.Label();
            this.lblInstrucao = new System.Windows.Forms.Label();
            this.chkAceito = new System.Windows.Forms.CheckBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevolver)).BeginInit();
            this.panelFundo.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(33)))));
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(700, 50);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Confirmação de Movimentos";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ESTILOS GERAIS
            cellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            cellStyle.BackColor = System.Drawing.SystemColors.Control;
            cellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            cellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            cellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            cellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            cellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            // 
            // lblReceber (Y=60)
            // 
            this.lblReceber.AutoSize = true;
            this.lblReceber.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblReceber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblReceber.Location = new System.Drawing.Point(20, 60);
            this.lblReceber.Name = "lblReceber";
            this.lblReceber.Size = new System.Drawing.Size(147, 19);
            this.lblReceber.Text = "A RECEBER (Entrega)";

            // 
            // dgvReceber (Y=82, H=100)
            // 
            this.dgvReceber.AllowUserToAddRows = false;
            this.dgvReceber.AllowUserToDeleteRows = false;
            this.dgvReceber.AllowUserToResizeRows = false;
            this.dgvReceber.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReceber.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvReceber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReceber.ColumnHeadersDefaultCellStyle = cellStyle;
            this.dgvReceber.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReceber.Location = new System.Drawing.Point(20, 82);
            this.dgvReceber.Name = "dgvReceber";
            this.dgvReceber.ReadOnly = true;
            this.dgvReceber.RowHeadersVisible = false;
            this.dgvReceber.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReceber.Size = new System.Drawing.Size(660, 100);
            this.dgvReceber.TabIndex = 11;

            // 
            // lblDevolver (Y=195)
            // 
            this.lblDevolver.AutoSize = true;
            this.lblDevolver.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDevolver.ForeColor = System.Drawing.Color.Firebrick;
            this.lblDevolver.Location = new System.Drawing.Point(20, 195);
            this.lblDevolver.Name = "lblDevolver";
            this.lblDevolver.Size = new System.Drawing.Size(175, 19);
            this.lblDevolver.Text = "A DEVOLVER (Retoma)";

            // 
            // dgvDevolver (Y=217, H=80)
            // 
            this.dgvDevolver.AllowUserToAddRows = false;
            this.dgvDevolver.AllowUserToDeleteRows = false;
            this.dgvDevolver.AllowUserToResizeRows = false;
            this.dgvDevolver.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDevolver.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvDevolver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDevolver.ColumnHeadersDefaultCellStyle = cellStyle;
            this.dgvDevolver.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevolver.Location = new System.Drawing.Point(20, 217);
            this.dgvDevolver.Name = "dgvDevolver";
            this.dgvDevolver.ReadOnly = true;
            this.dgvDevolver.RowHeadersVisible = false;
            this.dgvDevolver.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevolver.Size = new System.Drawing.Size(660, 80);
            this.dgvDevolver.TabIndex = 13;

            // 
            // txtLegal (O TEXTO IMPORTANTE) - Y=310
            // 
            this.txtLegal.BackColor = System.Drawing.Color.White;
            this.txtLegal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLegal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLegal.ForeColor = System.Drawing.Color.DimGray;
            this.txtLegal.Location = new System.Drawing.Point(20, 310);
            this.txtLegal.Name = "txtLegal";
            this.txtLegal.ReadOnly = true;
            this.txtLegal.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLegal.Size = new System.Drawing.Size(660, 120); // Altura generosa para o texto
            this.txtLegal.TabIndex = 14;
            this.txtLegal.Text = resources.GetString("lblTexto.Text"); // Vai buscar o texto aos Resources ou podes por hardcoded abaixo

            // 
            // lblAssinatura (Y=440)
            // 
            this.lblAssinatura.AutoSize = true;
            this.lblAssinatura.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAssinatura.Location = new System.Drawing.Point(20, 440);
            this.lblAssinatura.Name = "lblAssinatura";
            this.lblAssinatura.Text = "Assinatura:";

            this.lblInstrucao.AutoSize = true;
            this.lblInstrucao.ForeColor = System.Drawing.Color.Gray;
            this.lblInstrucao.Location = new System.Drawing.Point(500, 444);
            this.lblInstrucao.Text = "Use o rato ou caneta";

            // 
            // picSignature (Y=462)
            // 
            this.picSignature.BackColor = System.Drawing.Color.White;
            this.picSignature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSignature.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picSignature.Location = new System.Drawing.Point(20, 462);
            this.picSignature.Name = "picSignature";
            this.picSignature.Size = new System.Drawing.Size(660, 140);
            this.picSignature.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSignature_MouseDown);
            this.picSignature.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picSignature_MouseMove);
            this.picSignature.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picSignature_MouseUp);

            // 
            // Botoes e Checkbox (Y ajustados para baixo)
            // 
            this.btnLimpar.BackColor = System.Drawing.Color.Gray;
            this.btnLimpar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpar.ForeColor = System.Drawing.Color.White;
            this.btnLimpar.Location = new System.Drawing.Point(20, 610);
            this.btnLimpar.Size = new System.Drawing.Size(120, 30);
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);

            this.chkAceito.AutoSize = true;
            this.chkAceito.Location = new System.Drawing.Point(160, 660);
            this.chkAceito.Text = "Li e concordo com os termos acima descritos.";

            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(540, 650);
            this.btnConfirmar.Size = new System.Drawing.Size(140, 45);
            this.btnConfirmar.Text = "CONFIRMAR";
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);

            this.btnCancelar.BackColor = System.Drawing.Color.Firebrick;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(20, 650);
            this.btnCancelar.Size = new System.Drawing.Size(120, 45);
            this.btnCancelar.Text = "CANCELAR";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // 
            // panelFundo
            // 
            this.panelFundo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFundo.Controls.Add(this.lblTitulo);
            this.panelFundo.Controls.Add(this.lblReceber);
            this.panelFundo.Controls.Add(this.dgvReceber);
            this.panelFundo.Controls.Add(this.lblDevolver);
            this.panelFundo.Controls.Add(this.dgvDevolver);
            this.panelFundo.Controls.Add(this.txtLegal); // Adicionado
            this.panelFundo.Controls.Add(this.lblAssinatura);
            this.panelFundo.Controls.Add(this.lblInstrucao);
            this.panelFundo.Controls.Add(this.picSignature);
            this.panelFundo.Controls.Add(this.btnLimpar);
            this.panelFundo.Controls.Add(this.chkAceito);
            this.panelFundo.Controls.Add(this.btnConfirmar);
            this.panelFundo.Controls.Add(this.btnCancelar);
            this.panelFundo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFundo.Location = new System.Drawing.Point(0, 0);
            this.panelFundo.Size = new System.Drawing.Size(700, 720); // Aumentei para 720px

            // 
            // FormAssinatura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 720); // Aumentei para 720px
            this.Controls.Add(this.panelFundo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAssinatura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assinatura";
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReceber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevolver)).EndInit();
            this.panelFundo.ResumeLayout(false);
            this.panelFundo.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelFundo;

        // As duas grids e labels
        private System.Windows.Forms.Label lblReceber;
        private System.Windows.Forms.DataGridView dgvReceber;
        private System.Windows.Forms.Label lblDevolver;
        private System.Windows.Forms.DataGridView dgvDevolver;

        private System.Windows.Forms.PictureBox picSignature;
        private System.Windows.Forms.Label lblAssinatura;
        private System.Windows.Forms.Label lblInstrucao;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.CheckBox chkAceito;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.RichTextBox txtLegal;
    }
}