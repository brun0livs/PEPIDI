namespace PEPIDI.UCs
{
    partial class Pedidos
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlDetails = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblPedidos = new Label();
            dgvPedidos = new PEPIDI.Models.PEPIDIDataGridView();
            ID = new DataGridViewTextBoxColumn();
            Data = new DataGridViewTextBoxColumn();
            NrFunc = new DataGridViewTextBoxColumn();
            NomeFunc = new DataGridViewTextBoxColumn();
            Funcao = new DataGridViewTextBoxColumn();
            CorHex = new DataGridViewTextBoxColumn();
            PedidoEstado = new DataGridViewTextBoxColumn();
            NomeAprovador = new DataGridViewTextBoxColumn();
            NomeEntrega = new DataGridViewTextBoxColumn();
            PDF = new DataGridViewTextBoxColumn();
            Check = new DataGridViewCheckBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64.04958F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35.9504128F));
            tableLayoutPanel1.Controls.Add(pnlDetails, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1936, 1048);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlDetails
            // 
            pnlDetails.Dock = DockStyle.Fill;
            pnlDetails.Location = new Point(1244, 0);
            pnlDetails.Margin = new Padding(5, 0, 5, 10);
            pnlDetails.Name = "pnlDetails";
            pnlDetails.Size = new Size(687, 1038);
            pnlDetails.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(lblPedidos, 0, 0);
            tableLayoutPanel2.Controls.Add(dgvPedidos, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel2.Size = new Size(1239, 1048);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lblPedidos
            // 
            lblPedidos.AutoSize = true;
            lblPedidos.Dock = DockStyle.Fill;
            lblPedidos.Font = new Font("Roboto Medium", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPedidos.Location = new Point(3, 0);
            lblPedidos.Name = "lblPedidos";
            lblPedidos.Size = new Size(1233, 73);
            lblPedidos.TabIndex = 0;
            lblPedidos.Text = "PEDIDOS PENDENTES";
            lblPedidos.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvPedidos
            // 
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvPedidos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvPedidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPedidos.BackgroundColor = Color.White;
            dgvPedidos.BorderStyle = BorderStyle.None;
            dgvPedidos.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPedidos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPedidos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPedidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPedidos.Columns.AddRange(new DataGridViewColumn[] { ID, Data, NrFunc, NomeFunc, Funcao, CorHex, PedidoEstado, NomeAprovador, NomeEntrega, PDF, Check });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPedidos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPedidos.Dock = DockStyle.Fill;
            dgvPedidos.EnableHeadersVisualStyles = false;
            dgvPedidos.GridColor = SystemColors.Control;
            dgvPedidos.HeaderFontSize = 15F;
            dgvPedidos.Location = new Point(10, 83);
            dgvPedidos.Margin = new Padding(10);
            dgvPedidos.MultiSelect = false;
            dgvPedidos.Name = "dgvPedidos";
            dgvPedidos.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dgvPedidos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPedidos.RowTemplate.Height = 54;
            dgvPedidos.ScrollBars = ScrollBars.None;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.Size = new Size(1219, 955);
            dgvPedidos.TabIndex = 1;
            dgvPedidos.CellClick += dgvPedidos_CellClick;
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Visible = false;
            // 
            // Data
            // 
            Data.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Data.HeaderText = "Data";
            Data.Name = "Data";
            Data.ReadOnly = true;
            Data.Resizable = DataGridViewTriState.False;
            Data.Width = 62;
            // 
            // NrFunc
            // 
            NrFunc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            NrFunc.HeaderText = "NMEC";
            NrFunc.Name = "NrFunc";
            NrFunc.ReadOnly = true;
            NrFunc.Resizable = DataGridViewTriState.False;
            NrFunc.Width = 74;
            // 
            // NomeFunc
            // 
            NomeFunc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            NomeFunc.HeaderText = "Nome";
            NomeFunc.Name = "NomeFunc";
            NomeFunc.ReadOnly = true;
            // 
            // Funcao
            // 
            Funcao.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Funcao.HeaderText = "Função";
            Funcao.Name = "Funcao";
            Funcao.ReadOnly = true;
            Funcao.Width = 80;
            // 
            // CorHex
            // 
            CorHex.HeaderText = "CorHex";
            CorHex.Name = "CorHex";
            CorHex.ReadOnly = true;
            CorHex.Visible = false;
            // 
            // PedidoEstado
            // 
            PedidoEstado.HeaderText = "Estado";
            PedidoEstado.Name = "PedidoEstado";
            PedidoEstado.ReadOnly = true;
            PedidoEstado.Visible = false;
            // 
            // NomeAprovador
            // 
            NomeAprovador.HeaderText = "Aprovador";
            NomeAprovador.Name = "NomeAprovador";
            NomeAprovador.ReadOnly = true;
            NomeAprovador.Visible = false;
            // 
            // NomeEntrega
            // 
            NomeEntrega.HeaderText = "Entrga";
            NomeEntrega.Name = "NomeEntrega";
            NomeEntrega.ReadOnly = true;
            NomeEntrega.Visible = false;
            // 
            // PDF
            // 
            PDF.HeaderText = "PDF";
            PDF.Name = "PDF";
            PDF.ReadOnly = true;
            PDF.Visible = false;
            // 
            // Check
            // 
            Check.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Check.HeaderText = "Selecionar";
            Check.Name = "Check";
            Check.Visible = false;
            // 
            // Pedidos
            // 
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "Pedidos";
            Size = new Size(1936, 1048);
            Load += Pedidos_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPedidos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel pnlDetails;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblPedidos;
        private PEPIDI.Models.PEPIDIDataGridView dgvPedidos;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Data;
        private DataGridViewTextBoxColumn NrFunc;
        private DataGridViewTextBoxColumn NomeFunc;
        private DataGridViewTextBoxColumn Funcao;
        private DataGridViewTextBoxColumn CorHex;
        private DataGridViewTextBoxColumn PedidoEstado;
        private DataGridViewTextBoxColumn NomeAprovador;
        private DataGridViewTextBoxColumn NomeEntrega;
        private DataGridViewTextBoxColumn PDF;
        private DataGridViewCheckBoxColumn Check;
    }
}
