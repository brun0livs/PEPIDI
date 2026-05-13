namespace PEPIDI.UCs
{
    partial class CriarStock
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

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CriarStock));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            dgvStock = new PEPIDI.Models.PEPIDIDataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlDetails = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnImportarEPI = new Guna.UI2.WinForms.Guna2Button();
            btnCriarEPI = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            cmbEstado = new Guna.UI2.WinForms.Guna2ComboBox();
            lblTituloCriarEPI = new Label();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.Transparent;
            guna2Panel1.BorderColor = Color.FromArgb(242, 103, 34);
            guna2Panel1.BorderRadius = 25;
            guna2Panel1.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            guna2Panel1.BorderThickness = 1;
            guna2Panel1.Controls.Add(dgvStock);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.FillColor = Color.WhiteSmoke;
            guna2Panel1.Location = new Point(5, 65);
            guna2Panel1.Margin = new Padding(5);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(1114, 788);
            guna2Panel1.TabIndex = 5;
            // 
            // dgvStock
            // 
            dgvStock.AllowUserToAddRows = false;
            dgvStock.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStock.BackgroundColor = Color.White;
            dgvStock.BorderStyle = BorderStyle.None;
            dgvStock.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvStock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvStock.DefaultCellStyle = dataGridViewCellStyle3;
            dgvStock.Dock = DockStyle.Fill;
            dgvStock.EnableHeadersVisualStyles = false;
            dgvStock.GridColor = SystemColors.Control;
            dgvStock.HeaderFontSize = 15F;
            dgvStock.Location = new Point(0, 0);
            dgvStock.Margin = new Padding(13, 13, 13, 0);
            dgvStock.MultiSelect = false;
            dgvStock.Name = "dgvStock";
            dgvStock.ReadOnly = true;
            dgvStock.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvStock.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvStock.RowTemplate.Height = 54;
            dgvStock.ScrollBars = ScrollBars.None;
            dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Size = new Size(1114, 788);
            dgvStock.TabIndex = 4;
            dgvStock.CellClick += dgvStock_CellClick;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 61.186718F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38.813282F));
            tableLayoutPanel1.Controls.Add(guna2Panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(pnlDetails, 1, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1837, 858);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlDetails
            // 
            pnlDetails.Dock = DockStyle.Fill;
            pnlDetails.Location = new Point(1129, 60);
            pnlDetails.Margin = new Padding(5, 0, 5, 10);
            pnlDetails.Name = "pnlDetails";
            pnlDetails.Size = new Size(703, 788);
            pnlDetails.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnImportarEPI, 1, 0);
            tableLayoutPanel2.Controls.Add(btnCriarEPI, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(1124, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(713, 60);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // btnImportarEPI
            // 
            btnImportarEPI.BorderRadius = 10;
            btnImportarEPI.CustomImages.ImageAlign = HorizontalAlignment.Left;
            btnImportarEPI.CustomizableEdges = customizableEdges3;
            btnImportarEPI.DisabledState.BorderColor = Color.DarkGray;
            btnImportarEPI.DisabledState.CustomBorderColor = Color.DarkGray;
            btnImportarEPI.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnImportarEPI.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnImportarEPI.Dock = DockStyle.Fill;
            btnImportarEPI.Enabled = false;
            btnImportarEPI.FillColor = Color.FromArgb(243, 108, 33);
            btnImportarEPI.Font = new Font("Roboto", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnImportarEPI.ForeColor = Color.White;
            btnImportarEPI.Image = (Image)resources.GetObject("btnImportarEPI.Image");
            btnImportarEPI.Location = new Point(366, 12);
            btnImportarEPI.Margin = new Padding(10, 12, 10, 12);
            btnImportarEPI.Name = "btnImportarEPI";
            btnImportarEPI.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnImportarEPI.Size = new Size(337, 36);
            btnImportarEPI.TabIndex = 12;
            btnImportarEPI.Text = "Importar";
            btnImportarEPI.Visible = false;
            btnImportarEPI.Click += btnImportarEPI_Click;
            // 
            // btnCriarEPI
            // 
            btnCriarEPI.BorderRadius = 10;
            btnCriarEPI.CustomImages.ImageAlign = HorizontalAlignment.Left;
            btnCriarEPI.CustomizableEdges = customizableEdges5;
            btnCriarEPI.DisabledState.BorderColor = Color.DarkGray;
            btnCriarEPI.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCriarEPI.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCriarEPI.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCriarEPI.Dock = DockStyle.Fill;
            btnCriarEPI.FillColor = Color.FromArgb(243, 108, 33);
            btnCriarEPI.Font = new Font("Roboto", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCriarEPI.ForeColor = Color.White;
            btnCriarEPI.Location = new Point(10, 12);
            btnCriarEPI.Margin = new Padding(10, 12, 10, 12);
            btnCriarEPI.Name = "btnCriarEPI";
            btnCriarEPI.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnCriarEPI.Size = new Size(336, 36);
            btnCriarEPI.TabIndex = 11;
            btnCriarEPI.Text = "Adicionar Novo Artigo";
            btnCriarEPI.Visible = false;
            btnCriarEPI.Click += btnCriarEPI_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(cmbEstado, 1, 0);
            tableLayoutPanel3.Controls.Add(lblTituloCriarEPI, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(1124, 60);
            tableLayoutPanel3.TabIndex = 9;
            // 
            // cmbEstado
            // 
            cmbEstado.BackColor = Color.Transparent;
            cmbEstado.BorderRadius = 18;
            cmbEstado.CustomizableEdges = customizableEdges7;
            cmbEstado.Dock = DockStyle.Fill;
            cmbEstado.DrawMode = DrawMode.OwnerDrawFixed;
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.FocusedColor = Color.FromArgb(242, 103, 34);
            cmbEstado.FocusedState.BorderColor = Color.FromArgb(242, 103, 34);
            cmbEstado.Font = new Font("Roboto", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbEstado.ForeColor = Color.FromArgb(64, 64, 64);
            cmbEstado.HoverState.BorderColor = Color.FromArgb(242, 103, 34);
            cmbEstado.ItemHeight = 31;
            cmbEstado.Location = new Point(572, 12);
            cmbEstado.Margin = new Padding(10, 12, 10, 12);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.ShadowDecoration.CustomizableEdges = customizableEdges8;
            cmbEstado.Size = new Size(542, 37);
            cmbEstado.TabIndex = 29;
            cmbEstado.TextAlign = HorizontalAlignment.Center;
            cmbEstado.SelectedIndexChanged += cmbEstado_SelectedIndexChanged;
            // 
            // lblTituloCriarEPI
            // 
            lblTituloCriarEPI.AutoSize = true;
            lblTituloCriarEPI.Dock = DockStyle.Fill;
            lblTituloCriarEPI.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloCriarEPI.Location = new Point(3, 0);
            lblTituloCriarEPI.Name = "lblTituloCriarEPI";
            lblTituloCriarEPI.Size = new Size(556, 60);
            lblTituloCriarEPI.TabIndex = 7;
            lblTituloCriarEPI.Text = "GESTÃO DE EPI";
            lblTituloCriarEPI.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CriarStock
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5, 4, 5, 4);
            Name = "CriarStock";
            Size = new Size(1837, 858);
            Load += CriarStock_Load;
            guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStock).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Models.PEPIDIDataGridView dgvStock;
        private Panel pnlDetails;
        private TableLayoutPanel tableLayoutPanel2;
        private Guna.UI2.WinForms.Guna2Button btnImportarEPI;
        private Guna.UI2.WinForms.Guna2Button btnCriarEPI;
        private TableLayoutPanel tableLayoutPanel3;
        private Label lblTituloCriarEPI;
        private Guna.UI2.WinForms.Guna2ComboBox cmbEstado;
    }
}