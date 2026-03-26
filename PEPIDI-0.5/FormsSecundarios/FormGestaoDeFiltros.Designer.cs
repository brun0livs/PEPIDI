namespace PEPIDI.FormsSecundarios
{
    partial class FormGestaoDeFiltros
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGestaoDeFiltros));
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblFechar = new Label();
            lblFuncao = new Label();
            tlpConteudo = new TableLayoutPanel();
            tlpControlos = new TableLayoutPanel();
            txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            cmbVisaoNome = new Guna.UI2.WinForms.Guna2ComboBox();
            btnTestar = new Guna.UI2.WinForms.Guna2Button();
            btnEliminar = new Guna.UI2.WinForms.Guna2Button();
            btnGuardar = new Guna.UI2.WinForms.Guna2Button();
            tlpFiltros = new TableLayoutPanel();
            lblTituloFuncoes = new Label();
            lblTituloFamilia = new Label();
            lblTituloModelo = new Label();
            lblTituloTamanho = new Label();
            flpFuncoes = new FlowLayoutPanel();
            flpFamilia = new FlowLayoutPanel();
            flpModelo = new FlowLayoutPanel();
            flpTamanho = new FlowLayoutPanel();
            dgvPreview = new PEPIDI.Models.PEPIDIDataGridView();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tlpConteudo.SuspendLayout();
            tlpControlos.SuspendLayout();
            tlpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPreview).BeginInit();
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
            tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tlpConteudo, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 86F));
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
            tableLayoutPanel2.Size = new Size(1030, 75);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lblFechar
            // 
            lblFechar.Cursor = Cursors.Hand;
            lblFechar.Dock = DockStyle.Right;
            lblFechar.Image = (Image)resources.GetObject("lblFechar.Image");
            lblFechar.Location = new Point(963, 0);
            lblFechar.Margin = new Padding(10, 0, 0, 0);
            lblFechar.Name = "lblFechar";
            lblFechar.Size = new Size(67, 75);
            lblFechar.TabIndex = 1;
            lblFechar.Click += lblFechar_Click;
            // 
            // lblFuncao
            // 
            lblFuncao.AutoSize = true;
            lblFuncao.Dock = DockStyle.Fill;
            lblFuncao.Font = new Font("Roboto Medium", 18.75F, FontStyle.Bold);
            lblFuncao.ForeColor = Color.White;
            lblFuncao.Location = new Point(10, 0);
            lblFuncao.Margin = new Padding(10, 0, 0, 0);
            lblFuncao.Name = "lblFuncao";
            lblFuncao.Size = new Size(505, 75);
            lblFuncao.TabIndex = 0;
            lblFuncao.Text = "GESTÃO DE FILTROS";
            lblFuncao.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpConteudo
            // 
            tlpConteudo.ColumnCount = 1;
            tlpConteudo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpConteudo.Controls.Add(tlpControlos, 0, 0);
            tlpConteudo.Controls.Add(tlpFiltros, 0, 1);
            tlpConteudo.Controls.Add(dgvPreview, 0, 2);
            tlpConteudo.Dock = DockStyle.Fill;
            tlpConteudo.Location = new Point(20, 95);
            tlpConteudo.Margin = new Padding(20);
            tlpConteudo.Name = "tlpConteudo";
            tlpConteudo.RowCount = 3;
            tlpConteudo.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpConteudo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpConteudo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpConteudo.Size = new Size(990, 888);
            tlpConteudo.TabIndex = 2;
            // 
            // tlpControlos
            // 
            tlpControlos.ColumnCount = 5;
            tlpControlos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpControlos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpControlos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpControlos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpControlos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpControlos.Controls.Add(txtNome, 1, 0);
            tlpControlos.Controls.Add(cmbVisaoNome, 0, 0);
            tlpControlos.Controls.Add(btnTestar, 2, 0);
            tlpControlos.Controls.Add(btnEliminar, 3, 0);
            tlpControlos.Controls.Add(btnGuardar, 4, 0);
            tlpControlos.Dock = DockStyle.Fill;
            tlpControlos.Location = new Point(0, 0);
            tlpControlos.Margin = new Padding(0);
            tlpControlos.Name = "tlpControlos";
            tlpControlos.RowCount = 1;
            tlpControlos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpControlos.Size = new Size(990, 60);
            tlpControlos.TabIndex = 0;
            // 
            // txtNome
            // 
            txtNome.AutoRoundedCorners = true;
            txtNome.CustomizableEdges = customizableEdges1;
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
            txtNome.Location = new Point(203, 13);
            txtNome.Margin = new Padding(5, 13, 5, 10);
            txtNome.MaxLength = 20;
            txtNome.Name = "txtNome";
            txtNome.PlaceholderForeColor = Color.Silver;
            txtNome.PlaceholderText = "";
            txtNome.SelectedText = "";
            txtNome.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtNome.Size = new Size(188, 37);
            txtNome.TabIndex = 10;
            txtNome.TextOffset = new Point(10, 0);
            // 
            // cmbVisaoNome
            // 
            cmbVisaoNome.BackColor = Color.Transparent;
            cmbVisaoNome.BorderRadius = 15;
            cmbVisaoNome.CustomizableEdges = customizableEdges3;
            cmbVisaoNome.Dock = DockStyle.Fill;
            cmbVisaoNome.DrawMode = DrawMode.OwnerDrawFixed;
            cmbVisaoNome.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVisaoNome.FocusedColor = Color.Empty;
            cmbVisaoNome.Font = new Font("Roboto", 12F);
            cmbVisaoNome.ForeColor = Color.FromArgb(64, 64, 64);
            cmbVisaoNome.ItemHeight = 30;
            cmbVisaoNome.Location = new Point(3, 10);
            cmbVisaoNome.Margin = new Padding(3, 10, 15, 10);
            cmbVisaoNome.Name = "cmbVisaoNome";
            cmbVisaoNome.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cmbVisaoNome.Size = new Size(180, 36);
            cmbVisaoNome.TabIndex = 0;
            cmbVisaoNome.SelectedIndexChanged += cmbVisaoNome_SelectedIndexChanged;
            // 
            // btnTestar
            // 
            btnTestar.BorderRadius = 10;
            btnTestar.Cursor = Cursors.Hand;
            btnTestar.CustomizableEdges = customizableEdges5;
            btnTestar.Dock = DockStyle.Fill;
            btnTestar.FillColor = Color.LightGray;
            btnTestar.Font = new Font("Roboto", 11.25F, FontStyle.Bold);
            btnTestar.ForeColor = Color.Black;
            btnTestar.Location = new Point(406, 10);
            btnTestar.Margin = new Padding(10);
            btnTestar.Name = "btnTestar";
            btnTestar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnTestar.Size = new Size(178, 40);
            btnTestar.TabIndex = 1;
            btnTestar.Text = "Testar";
            btnTestar.Click += btnTestar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BorderRadius = 10;
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.CustomizableEdges = customizableEdges7;
            btnEliminar.Dock = DockStyle.Fill;
            btnEliminar.FillColor = Color.IndianRed;
            btnEliminar.Font = new Font("Roboto", 11.25F, FontStyle.Bold);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(604, 10);
            btnEliminar.Margin = new Padding(10);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnEliminar.Size = new Size(178, 40);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "Eliminar";
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BorderRadius = 10;
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.CustomizableEdges = customizableEdges9;
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.FillColor = Color.FromArgb(242, 103, 34);
            btnGuardar.Font = new Font("Roboto", 11.25F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(802, 10);
            btnGuardar.Margin = new Padding(10);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnGuardar.Size = new Size(178, 40);
            btnGuardar.TabIndex = 3;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            // 
            // tlpFiltros
            // 
            tlpFiltros.ColumnCount = 4;
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpFiltros.Controls.Add(lblTituloFuncoes, 0, 0);
            tlpFiltros.Controls.Add(lblTituloFamilia, 1, 0);
            tlpFiltros.Controls.Add(lblTituloModelo, 2, 0);
            tlpFiltros.Controls.Add(lblTituloTamanho, 3, 0);
            tlpFiltros.Controls.Add(flpFuncoes, 0, 1);
            tlpFiltros.Controls.Add(flpFamilia, 1, 1);
            tlpFiltros.Controls.Add(flpModelo, 2, 1);
            tlpFiltros.Controls.Add(flpTamanho, 3, 1);
            tlpFiltros.Dock = DockStyle.Fill;
            tlpFiltros.Location = new Point(0, 70);
            tlpFiltros.Margin = new Padding(0, 10, 0, 10);
            tlpFiltros.Name = "tlpFiltros";
            tlpFiltros.RowCount = 2;
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFiltros.Size = new Size(990, 394);
            tlpFiltros.TabIndex = 1;
            // 
            // lblTituloFuncoes
            // 
            lblTituloFuncoes.AutoSize = true;
            lblTituloFuncoes.Dock = DockStyle.Fill;
            lblTituloFuncoes.Font = new Font("Roboto", 14F, FontStyle.Bold);
            lblTituloFuncoes.ForeColor = Color.FromArgb(64, 64, 64);
            lblTituloFuncoes.Location = new Point(3, 0);
            lblTituloFuncoes.Name = "lblTituloFuncoes";
            lblTituloFuncoes.Size = new Size(241, 40);
            lblTituloFuncoes.TabIndex = 0;
            lblTituloFuncoes.Text = "Funções";
            lblTituloFuncoes.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTituloFamilia
            // 
            lblTituloFamilia.AutoSize = true;
            lblTituloFamilia.Dock = DockStyle.Fill;
            lblTituloFamilia.Font = new Font("Roboto", 14F, FontStyle.Bold);
            lblTituloFamilia.ForeColor = Color.FromArgb(64, 64, 64);
            lblTituloFamilia.Location = new Point(250, 0);
            lblTituloFamilia.Name = "lblTituloFamilia";
            lblTituloFamilia.Size = new Size(241, 40);
            lblTituloFamilia.TabIndex = 1;
            lblTituloFamilia.Text = "Famílias";
            lblTituloFamilia.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTituloModelo
            // 
            lblTituloModelo.AutoSize = true;
            lblTituloModelo.Dock = DockStyle.Fill;
            lblTituloModelo.Font = new Font("Roboto", 14F, FontStyle.Bold);
            lblTituloModelo.ForeColor = Color.FromArgb(64, 64, 64);
            lblTituloModelo.Location = new Point(497, 0);
            lblTituloModelo.Name = "lblTituloModelo";
            lblTituloModelo.Size = new Size(241, 40);
            lblTituloModelo.TabIndex = 2;
            lblTituloModelo.Text = "Modelos";
            lblTituloModelo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTituloTamanho
            // 
            lblTituloTamanho.AutoSize = true;
            lblTituloTamanho.Dock = DockStyle.Fill;
            lblTituloTamanho.Font = new Font("Roboto", 14F, FontStyle.Bold);
            lblTituloTamanho.ForeColor = Color.FromArgb(64, 64, 64);
            lblTituloTamanho.Location = new Point(744, 0);
            lblTituloTamanho.Name = "lblTituloTamanho";
            lblTituloTamanho.Size = new Size(243, 40);
            lblTituloTamanho.TabIndex = 3;
            lblTituloTamanho.Text = "Tamanhos";
            lblTituloTamanho.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpFuncoes
            // 
            flpFuncoes.AutoScroll = true;
            flpFuncoes.Dock = DockStyle.Fill;
            flpFuncoes.FlowDirection = FlowDirection.TopDown;
            flpFuncoes.Location = new Point(3, 43);
            flpFuncoes.Name = "flpFuncoes";
            flpFuncoes.Padding = new Padding(10, 0, 10, 0);
            flpFuncoes.Size = new Size(241, 348);
            flpFuncoes.TabIndex = 4;
            flpFuncoes.WrapContents = false;
            // 
            // flpFamilia
            // 
            flpFamilia.AutoScroll = true;
            flpFamilia.Dock = DockStyle.Fill;
            flpFamilia.FlowDirection = FlowDirection.TopDown;
            flpFamilia.Location = new Point(250, 43);
            flpFamilia.Name = "flpFamilia";
            flpFamilia.Padding = new Padding(10, 0, 10, 0);
            flpFamilia.Size = new Size(241, 348);
            flpFamilia.TabIndex = 5;
            flpFamilia.WrapContents = false;
            // 
            // flpModelo
            // 
            flpModelo.AutoScroll = true;
            flpModelo.Dock = DockStyle.Fill;
            flpModelo.FlowDirection = FlowDirection.TopDown;
            flpModelo.Location = new Point(497, 43);
            flpModelo.Name = "flpModelo";
            flpModelo.Padding = new Padding(10, 0, 10, 0);
            flpModelo.Size = new Size(241, 348);
            flpModelo.TabIndex = 6;
            flpModelo.WrapContents = false;
            // 
            // flpTamanho
            // 
            flpTamanho.AutoScroll = true;
            flpTamanho.Dock = DockStyle.Fill;
            flpTamanho.FlowDirection = FlowDirection.TopDown;
            flpTamanho.Location = new Point(744, 43);
            flpTamanho.Name = "flpTamanho";
            flpTamanho.Padding = new Padding(10, 0, 10, 0);
            flpTamanho.Size = new Size(243, 348);
            flpTamanho.TabIndex = 7;
            flpTamanho.WrapContents = false;
            // 
            // dgvPreview
            // 
            dgvPreview.AllowUserToAddRows = false;
            dgvPreview.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvPreview.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPreview.BackgroundColor = Color.White;
            dgvPreview.BorderStyle = BorderStyle.None;
            dgvPreview.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPreview.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 12F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 12F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPreview.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPreview.Dock = DockStyle.Fill;
            dgvPreview.EnableHeadersVisualStyles = false;
            dgvPreview.GridColor = SystemColors.Control;
            dgvPreview.HeaderFontSize = 15F;
            dgvPreview.Location = new Point(0, 474);
            dgvPreview.Margin = new Padding(0);
            dgvPreview.MultiSelect = false;
            dgvPreview.Name = "dgvPreview";
            dgvPreview.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvPreview.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPreview.RowTemplate.Height = 54;
            dgvPreview.ScrollBars = ScrollBars.None;
            dgvPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPreview.Size = new Size(990, 414);
            dgvPreview.TabIndex = 2;
            // 
            // FormGestaoDeFiltros
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1030, 1003);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 12F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormGestaoDeFiltros";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestão de Filtros";
            Load += FormGestaoDeFiltros_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tlpConteudo.ResumeLayout(false);
            tlpControlos.ResumeLayout(false);
            tlpFiltros.ResumeLayout(false);
            tlpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblFechar;
        private System.Windows.Forms.Label lblFuncao;

        private System.Windows.Forms.TableLayoutPanel tlpConteudo;
        private System.Windows.Forms.TableLayoutPanel tlpControlos;
        private Guna.UI2.WinForms.Guna2ComboBox cmbVisaoNome;
        private Guna.UI2.WinForms.Guna2Button btnTestar;
        private Guna.UI2.WinForms.Guna2Button btnEliminar;
        private Guna.UI2.WinForms.Guna2Button btnGuardar;

        private System.Windows.Forms.TableLayoutPanel tlpFiltros;
        private System.Windows.Forms.Label lblTituloFuncoes;
        private System.Windows.Forms.Label lblTituloFamilia;
        private System.Windows.Forms.Label lblTituloModelo;
        private System.Windows.Forms.Label lblTituloTamanho;
        private System.Windows.Forms.FlowLayoutPanel flpFuncoes;
        private System.Windows.Forms.FlowLayoutPanel flpFamilia;
        private System.Windows.Forms.FlowLayoutPanel flpModelo;
        private System.Windows.Forms.FlowLayoutPanel flpTamanho;
        private Models.PEPIDIDataGridView dgvPreview;
        private Guna.UI2.WinForms.Guna2TextBox txtNome;
    }
}