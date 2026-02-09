namespace PEPIDI.UCs.UcsSecundarios
{
    partial class PedidosDetalhes
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
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
            Curvas = new Guna.UI2.WinForms.Guna2Panel();
            Orientacao1 = new TableLayoutPanel();
            dgvDevolucoes = new PEPIDI_0._5.Models.PEPIDIDataGridView();
            dgvPedidos = new PEPIDI_0._5.Models.PEPIDIDataGridView();
            Devolucao = new Guna.UI2.WinForms.Guna2Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label4 = new Label();
            pnlPedidos = new Guna.UI2.WinForms.Guna2Panel();
            OrientacaoPedidos = new TableLayoutPanel();
            label3 = new Label();
            close = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnAprovar = new Guna.UI2.WinForms.Guna2Button();
            btnReprovar = new Guna.UI2.WinForms.Guna2Button();
            txtObs = new Guna.UI2.WinForms.Guna2TextBox();
            lblClose = new Label();
            Curvas.SuspendLayout();
            Orientacao1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDevolucoes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
            Devolucao.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            pnlPedidos.SuspendLayout();
            OrientacaoPedidos.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // Curvas
            // 
            Curvas.BorderColor = Color.Silver;
            Curvas.BorderRadius = 15;
            Curvas.BorderThickness = 1;
            Curvas.Controls.Add(Orientacao1);
            Curvas.CustomizableEdges = customizableEdges11;
            Curvas.Dock = DockStyle.Fill;
            Curvas.Location = new Point(0, 0);
            Curvas.Name = "Curvas";
            Curvas.ShadowDecoration.CustomizableEdges = customizableEdges12;
            Curvas.Size = new Size(600, 850);
            Curvas.TabIndex = 0;
            // 
            // Orientacao1
            // 
            Orientacao1.BackColor = Color.Transparent;
            Orientacao1.ColumnCount = 1;
            Orientacao1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Orientacao1.Controls.Add(dgvDevolucoes, 0, 3);
            Orientacao1.Controls.Add(dgvPedidos, 0, 1);
            Orientacao1.Controls.Add(Devolucao, 0, 2);
            Orientacao1.Controls.Add(pnlPedidos, 0, 0);
            Orientacao1.Controls.Add(tableLayoutPanel1, 0, 5);
            Orientacao1.Controls.Add(txtObs, 0, 4);
            Orientacao1.Dock = DockStyle.Fill;
            Orientacao1.Font = new Font("Roboto Medium", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Orientacao1.ForeColor = Color.White;
            Orientacao1.Location = new Point(0, 0);
            Orientacao1.Margin = new Padding(0);
            Orientacao1.Name = "Orientacao1";
            Orientacao1.RowCount = 6;
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.705883F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.64705849F));
            Orientacao1.Size = new Size(600, 850);
            Orientacao1.TabIndex = 3;
            // 
            // dgvDevolucoes
            // 
            dgvDevolucoes.AllowUserToAddRows = false;
            dgvDevolucoes.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvDevolucoes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvDevolucoes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDevolucoes.BackgroundColor = Color.White;
            dgvDevolucoes.BorderStyle = BorderStyle.None;
            dgvDevolucoes.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvDevolucoes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvDevolucoes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDevolucoes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvDevolucoes.DefaultCellStyle = dataGridViewCellStyle3;
            dgvDevolucoes.Dock = DockStyle.Fill;
            dgvDevolucoes.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDevolucoes.EnableHeadersVisualStyles = false;
            dgvDevolucoes.GridColor = SystemColors.Control;
            dgvDevolucoes.HeaderFontSize = 11F;
            dgvDevolucoes.Location = new Point(10, 382);
            dgvDevolucoes.Margin = new Padding(10, 10, 10, 0);
            dgvDevolucoes.MultiSelect = false;
            dgvDevolucoes.Name = "dgvDevolucoes";
            dgvDevolucoes.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvDevolucoes.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvDevolucoes.RowTemplate.Height = 54;
            dgvDevolucoes.ScrollBars = ScrollBars.None;
            dgvDevolucoes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDevolucoes.Size = new Size(580, 244);
            dgvDevolucoes.TabIndex = 7;
            // 
            // dgvPedidos
            // 
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = Color.Transparent;
            dgvPedidos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.BackgroundColor = Color.White;
            dgvPedidos.BorderStyle = BorderStyle.None;
            dgvPedidos.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPedidos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Roboto", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.Black;
            dataGridViewCellStyle6.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvPedidos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvPedidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.Transparent;
            dataGridViewCellStyle7.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.Black;
            dataGridViewCellStyle7.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle7.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle7.SelectionForeColor = Color.Black;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvPedidos.DefaultCellStyle = dataGridViewCellStyle7;
            dgvPedidos.Dock = DockStyle.Fill;
            dgvPedidos.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvPedidos.EnableHeadersVisualStyles = false;
            dgvPedidos.GridColor = SystemColors.Control;
            dgvPedidos.HeaderFontSize = 9F;
            dgvPedidos.Location = new Point(10, 69);
            dgvPedidos.Margin = new Padding(10, 10, 10, 0);
            dgvPedidos.MultiSelect = false;
            dgvPedidos.Name = "dgvPedidos";
            dgvPedidos.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = Color.Transparent;
            dgvPedidos.RowsDefaultCellStyle = dataGridViewCellStyle8;
            dgvPedidos.RowTemplate.Height = 54;
            dgvPedidos.ScrollBars = ScrollBars.None;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.Size = new Size(580, 244);
            dgvPedidos.TabIndex = 6;
            // 
            // Devolucao
            // 
            Devolucao.BorderRadius = 15;
            Devolucao.Controls.Add(tableLayoutPanel2);
            customizableEdges1.BottomLeft = false;
            customizableEdges1.BottomRight = false;
            Devolucao.CustomizableEdges = customizableEdges1;
            Devolucao.Dock = DockStyle.Fill;
            Devolucao.FillColor = Color.Gray;
            Devolucao.Location = new Point(0, 313);
            Devolucao.Margin = new Padding(0);
            Devolucao.Name = "Devolucao";
            Devolucao.ShadowDecoration.CustomizableEdges = customizableEdges2;
            Devolucao.Size = new Size(600, 59);
            Devolucao.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Transparent;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel2.Controls.Add(label4, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(10, 0, 10, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(600, 59);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(10, 0);
            label4.Margin = new Padding(10, 0, 0, 0);
            label4.Name = "label4";
            label4.Size = new Size(590, 59);
            label4.TabIndex = 3;
            label4.Text = "DEVOLUÇÃO DE ROUPA";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlPedidos
            // 
            pnlPedidos.BorderRadius = 15;
            pnlPedidos.Controls.Add(OrientacaoPedidos);
            customizableEdges3.BottomLeft = false;
            customizableEdges3.BottomRight = false;
            pnlPedidos.CustomizableEdges = customizableEdges3;
            pnlPedidos.Dock = DockStyle.Fill;
            pnlPedidos.FillColor = Color.Gray;
            pnlPedidos.Location = new Point(0, 0);
            pnlPedidos.Margin = new Padding(0);
            pnlPedidos.Name = "pnlPedidos";
            pnlPedidos.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlPedidos.Size = new Size(600, 59);
            pnlPedidos.TabIndex = 0;
            // 
            // OrientacaoPedidos
            // 
            OrientacaoPedidos.BackColor = Color.Transparent;
            OrientacaoPedidos.ColumnCount = 2;
            OrientacaoPedidos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85F));
            OrientacaoPedidos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            OrientacaoPedidos.Controls.Add(label3, 0, 0);
            OrientacaoPedidos.Controls.Add(close, 1, 0);
            OrientacaoPedidos.Dock = DockStyle.Fill;
            OrientacaoPedidos.Location = new Point(0, 0);
            OrientacaoPedidos.Margin = new Padding(0);
            OrientacaoPedidos.Name = "OrientacaoPedidos";
            OrientacaoPedidos.RowCount = 1;
            OrientacaoPedidos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            OrientacaoPedidos.Size = new Size(600, 59);
            OrientacaoPedidos.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(0, 0);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(510, 59);
            label3.TabIndex = 3;
            label3.Text = " PEDIDO DE ROUPA";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // close
            // 
            close.AutoSize = true;
            close.BackColor = Color.Transparent;
            close.Cursor = Cursors.Hand;
            close.Dock = DockStyle.Fill;
            close.Image = Properties.Resources.Close;
            close.Location = new Point(510, 0);
            close.Margin = new Padding(0);
            close.Name = "close";
            close.Size = new Size(90, 59);
            close.TabIndex = 4;
            close.TextAlign = ContentAlignment.MiddleCenter;
            close.Click += close_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.Controls.Add(btnAprovar, 2, 0);
            tableLayoutPanel1.Controls.Add(btnReprovar, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 770);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(594, 77);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // btnAprovar
            // 
            btnAprovar.BorderRadius = 10;
            btnAprovar.CustomizableEdges = customizableEdges5;
            btnAprovar.DisabledState.BorderColor = Color.DarkGray;
            btnAprovar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAprovar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAprovar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAprovar.Dock = DockStyle.Fill;
            btnAprovar.FillColor = Color.FromArgb(243, 108, 33);
            btnAprovar.Font = new Font("Roboto", 11.25F);
            btnAprovar.ForeColor = Color.White;
            btnAprovar.Location = new Point(306, 15);
            btnAprovar.Margin = new Padding(10, 15, 10, 15);
            btnAprovar.Name = "btnAprovar";
            btnAprovar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnAprovar.Size = new Size(187, 47);
            btnAprovar.TabIndex = 9;
            btnAprovar.Text = "Aprovar";
            // 
            // btnReprovar
            // 
            btnReprovar.BorderRadius = 10;
            btnReprovar.CustomizableEdges = customizableEdges7;
            btnReprovar.DisabledState.BorderColor = Color.DarkGray;
            btnReprovar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnReprovar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnReprovar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnReprovar.Dock = DockStyle.Fill;
            btnReprovar.FillColor = Color.Silver;
            btnReprovar.Font = new Font("Roboto", 11.25F);
            btnReprovar.ForeColor = Color.Black;
            btnReprovar.Location = new Point(99, 15);
            btnReprovar.Margin = new Padding(10, 15, 10, 15);
            btnReprovar.Name = "btnReprovar";
            btnReprovar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnReprovar.Size = new Size(187, 47);
            btnReprovar.TabIndex = 7;
            btnReprovar.Text = "Reprovar";
            // 
            // txtObs
            // 
            txtObs.BorderColor = Color.FromArgb(224, 224, 224);
            txtObs.BorderRadius = 15;
            txtObs.CustomizableEdges = customizableEdges9;
            txtObs.DefaultText = "";
            txtObs.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtObs.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtObs.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtObs.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtObs.Dock = DockStyle.Fill;
            txtObs.FocusedState.BorderColor = Color.FromArgb(243, 108, 33);
            txtObs.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtObs.HoverState.BorderColor = Color.FromArgb(255, 192, 128);
            txtObs.IconRightCursor = Cursors.IBeam;
            txtObs.Location = new Point(10, 636);
            txtObs.Margin = new Padding(10);
            txtObs.Multiline = true;
            txtObs.Name = "txtObs";
            txtObs.PlaceholderForeColor = Color.Silver;
            txtObs.PlaceholderText = "Observações";
            txtObs.SelectedText = "";
            txtObs.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtObs.Size = new Size(580, 121);
            txtObs.TabIndex = 5;
            // 
            // lblClose
            // 
            lblClose.AutoSize = true;
            lblClose.Location = new Point(545, 0);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(0, 33);
            lblClose.TabIndex = 1;
            // 
            // PedidosDetalhes
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Controls.Add(Curvas);
            Font = new Font("Roboto", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "PedidosDetalhes";
            Size = new Size(600, 850);
            Load += PedidosDetalhes_Load;
            Curvas.ResumeLayout(false);
            Orientacao1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDevolucoes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
            Devolucao.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            pnlPedidos.ResumeLayout(false);
            OrientacaoPedidos.ResumeLayout(false);
            OrientacaoPedidos.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel Curvas;
        private Label lblClose;
        private TableLayoutPanel Orientacao1;
        private Guna.UI2.WinForms.Guna2Panel Devolucao;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label4;
        private Guna.UI2.WinForms.Guna2Panel pnlPedidos;
        private TableLayoutPanel OrientacaoPedidos;
        private Label label3;
        private Label close;
        private TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Button btnAprovar;
        private Guna.UI2.WinForms.Guna2Button btnReprovar;
        private Guna.UI2.WinForms.Guna2TextBox txtObs;
        private PEPIDI_0._5.Models.PEPIDIDataGridView dgvDevolucoes;
        private PEPIDI_0._5.Models.PEPIDIDataGridView dgvPedidos;
    }
}
