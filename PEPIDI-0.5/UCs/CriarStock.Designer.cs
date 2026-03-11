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
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            dgvStock = new PEPIDI.Models.PEPIDIDataGridView();
            lblTituloCriarEPI = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlDetails = new Panel();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStock).BeginInit();
            tableLayoutPanel1.SuspendLayout();
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
            guna2Panel1.Size = new Size(908, 788);
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
            dgvStock.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvStock.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvStock.RowTemplate.Height = 54;
            dgvStock.ScrollBars = ScrollBars.None;
            dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStock.Size = new Size(908, 788);
            dgvStock.TabIndex = 4;
            dgvStock.CellClick += dgvStock_CellClick;
            // 
            // lblTituloCriarEPI
            // 
            lblTituloCriarEPI.AutoSize = true;
            lblTituloCriarEPI.Dock = DockStyle.Fill;
            lblTituloCriarEPI.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloCriarEPI.Location = new Point(3, 0);
            lblTituloCriarEPI.Name = "lblTituloCriarEPI";
            lblTituloCriarEPI.Size = new Size(912, 60);
            lblTituloCriarEPI.TabIndex = 6;
            lblTituloCriarEPI.Text = "GESTÃO DE EPI";
            lblTituloCriarEPI.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblTituloCriarEPI, 0, 0);
            tableLayoutPanel1.Controls.Add(guna2Panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(pnlDetails, 1, 1);
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
            pnlDetails.Location = new Point(923, 60);
            pnlDetails.Margin = new Padding(5, 0, 5, 10);
            pnlDetails.Name = "pnlDetails";
            pnlDetails.Size = new Size(909, 788);
            pnlDetails.TabIndex = 7;
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
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Label lblTituloCriarEPI;
        private TableLayoutPanel tableLayoutPanel1;
        private Models.PEPIDIDataGridView dgvStock;
        private Panel pnlDetails;
    }
}