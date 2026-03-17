namespace PEPIDI.FormsSecundarios
{
    partial class FormImportarStock
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            tlpMain = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblFechar = new Label();
            lblTitulo = new Label();
            pnlInstrucoes = new Panel();
            flpFuncoes = new FlowLayoutPanel();
            lblTituloFuncoes = new Label();
            lblDica = new Label();
            dgvImport = new DataGridView();
            tlpBotoes = new TableLayoutPanel();
            btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            btnImportar = new Guna.UI2.WinForms.Guna2Button();
            tlpMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlInstrucoes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvImport).BeginInit();
            tlpBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // tlpMain
            // 
            tlpMain.BackColor = Color.WhiteSmoke;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlHeader, 0, 0);
            tlpMain.Controls.Add(pnlInstrucoes, 0, 1);
            tlpMain.Controls.Add(dgvImport, 0, 2);
            tlpMain.Controls.Add(tlpBotoes, 0, 3);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Margin = new Padding(4, 3, 4, 3);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 4;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 69F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 254F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 81F));
            tlpMain.Size = new Size(1108, 865);
            tlpMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(242, 103, 34);
            pnlHeader.Controls.Add(lblFechar);
            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1108, 69);
            pnlHeader.TabIndex = 0;
            // 
            // lblFechar
            // 
            lblFechar.Cursor = Cursors.Hand;
            lblFechar.Dock = DockStyle.Right;
            lblFechar.Font = new Font("Roboto", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechar.ForeColor = Color.White;
            lblFechar.Image = Properties.Resources.Close;
            lblFechar.Location = new Point(1038, 0);
            lblFechar.Margin = new Padding(4, 0, 4, 0);
            lblFechar.Name = "lblFechar";
            lblFechar.Size = new Size(70, 69);
            lblFechar.TabIndex = 1;
            lblFechar.TextAlign = ContentAlignment.MiddleCenter;
            lblFechar.Click += btnCancelar_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Left;
            lblTitulo.Font = new Font("Roboto Medium", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Margin = new Padding(4, 0, 4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(23, 0, 0, 0);
            lblTitulo.Size = new Size(1030, 69);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "IMPORTAR STOCK EM MASSA";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlInstrucoes
            // 
            pnlInstrucoes.BackColor = Color.White;
            pnlInstrucoes.Controls.Add(flpFuncoes);
            pnlInstrucoes.Controls.Add(lblTituloFuncoes);
            pnlInstrucoes.Controls.Add(lblDica);
            pnlInstrucoes.Dock = DockStyle.Fill;
            pnlInstrucoes.Location = new Point(23, 92);
            pnlInstrucoes.Margin = new Padding(23);
            pnlInstrucoes.Name = "pnlInstrucoes";
            pnlInstrucoes.Padding = new Padding(18, 17, 18, 17);
            pnlInstrucoes.Size = new Size(1062, 208);
            pnlInstrucoes.TabIndex = 1;
            // 
            // flpFuncoes
            // 
            flpFuncoes.AutoScroll = true;
            flpFuncoes.Dock = DockStyle.Fill;
            flpFuncoes.Location = new Point(18, 98);
            flpFuncoes.Margin = new Padding(4, 3, 4, 3);
            flpFuncoes.Name = "flpFuncoes";
            flpFuncoes.Size = new Size(1026, 93);
            flpFuncoes.TabIndex = 2;
            // 
            // lblTituloFuncoes
            // 
            lblTituloFuncoes.Dock = DockStyle.Top;
            lblTituloFuncoes.Font = new Font("Roboto", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloFuncoes.ForeColor = Color.FromArgb(64, 64, 64);
            lblTituloFuncoes.Location = new Point(18, 63);
            lblTituloFuncoes.Margin = new Padding(4, 0, 4, 0);
            lblTituloFuncoes.Name = "lblTituloFuncoes";
            lblTituloFuncoes.Size = new Size(1026, 35);
            lblTituloFuncoes.TabIndex = 1;
            lblTituloFuncoes.Text = "Funções Autorizadas (Aplica-se a toda a lista importada):";
            lblTituloFuncoes.TextAlign = ContentAlignment.BottomLeft;
            // 
            // lblDica
            // 
            lblDica.Dock = DockStyle.Top;
            lblDica.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDica.ForeColor = Color.FromArgb(100, 100, 100);
            lblDica.Location = new Point(18, 17);
            lblDica.Margin = new Padding(4, 0, 4, 0);
            lblDica.Name = "lblDica";
            lblDica.Size = new Size(1026, 46);
            lblDica.TabIndex = 0;
            lblDica.Text = "💡 Dica: Copia os dados do Excel (colunas: Modelo, Tamanho, Quantidade), clica na primeira célula da tabela abaixo e prime Ctrl + V.";
            // 
            // dgvImport
            // 
            dgvImport.BackgroundColor = Color.White;
            dgvImport.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(240, 240, 240);
            dataGridViewCellStyle3.Font = new Font("Roboto", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvImport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvImport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(254, 235, 226);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(242, 103, 34);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvImport.DefaultCellStyle = dataGridViewCellStyle4;
            dgvImport.Dock = DockStyle.Fill;
            dgvImport.Location = new Point(23, 323);
            dgvImport.Margin = new Padding(23, 0, 23, 12);
            dgvImport.Name = "dgvImport";
            dgvImport.RowTemplate.Height = 35;
            dgvImport.Size = new Size(1062, 449);
            dgvImport.TabIndex = 2;
            // 
            // tlpBotoes
            // 
            tlpBotoes.ColumnCount = 3;
            tlpBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 233F));
            tlpBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 233F));
            tlpBotoes.Controls.Add(btnCancelar, 1, 0);
            tlpBotoes.Controls.Add(btnImportar, 2, 0);
            tlpBotoes.Dock = DockStyle.Fill;
            tlpBotoes.Location = new Point(0, 784);
            tlpBotoes.Margin = new Padding(0, 0, 23, 23);
            tlpBotoes.Name = "tlpBotoes";
            tlpBotoes.RowCount = 1;
            tlpBotoes.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBotoes.Size = new Size(1085, 58);
            tlpBotoes.TabIndex = 3;
            // 
            // btnCancelar
            // 
            btnCancelar.BorderRadius = 15;
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.CustomizableEdges = customizableEdges5;
            btnCancelar.Dock = DockStyle.Fill;
            btnCancelar.FillColor = Color.DarkGray;
            btnCancelar.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(631, 6);
            btnCancelar.Margin = new Padding(12, 6, 12, 6);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnCancelar.Size = new Size(209, 46);
            btnCancelar.TabIndex = 0;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnImportar
            // 
            btnImportar.BorderRadius = 15;
            btnImportar.Cursor = Cursors.Hand;
            btnImportar.CustomizableEdges = customizableEdges7;
            btnImportar.Dock = DockStyle.Fill;
            btnImportar.FillColor = Color.FromArgb(242, 103, 34);
            btnImportar.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImportar.ForeColor = Color.White;
            btnImportar.Location = new Point(864, 6);
            btnImportar.Margin = new Padding(12, 6, 12, 6);
            btnImportar.Name = "btnImportar";
            btnImportar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnImportar.Size = new Size(209, 46);
            btnImportar.TabIndex = 1;
            btnImportar.Text = "Importar Stock";
            btnImportar.Click += btnImportar_Click;
            // 
            // FormImportarStock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1108, 865);
            Controls.Add(tlpMain);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FormImportarStock";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Importar Stock";
            Load += FormImportarStock_Load;
            tlpMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlInstrucoes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvImport).EndInit();
            tlpBotoes.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblFechar;
        private System.Windows.Forms.Panel pnlInstrucoes;
        private System.Windows.Forms.Label lblDica;
        private System.Windows.Forms.Label lblTituloFuncoes;
        private System.Windows.Forms.FlowLayoutPanel flpFuncoes;
        private System.Windows.Forms.DataGridView dgvImport;
        private System.Windows.Forms.TableLayoutPanel tlpBotoes;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private Guna.UI2.WinForms.Guna2Button btnImportar;
    }
}