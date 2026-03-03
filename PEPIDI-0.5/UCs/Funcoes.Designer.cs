namespace PEPIDI.UCs
{
    partial class Funcoes
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnAddFuncao = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            dgvFuncoes = new PEPIDI.Models.PEPIDIDataGridView();
            ID = new DataGridViewTextBoxColumn();
            Nome = new DataGridViewTextBoxColumn();
            PodeVerStock = new DataGridViewCheckBoxColumn();
            PodeCriarStock = new DataGridViewCheckBoxColumn();
            PodeInserirStock = new DataGridViewCheckBoxColumn();
            PodeEditarFunc = new DataGridViewCheckBoxColumn();
            PodeSubmeter = new DataGridViewCheckBoxColumn();
            PodeAprovar = new DataGridViewCheckBoxColumn();
            PodeEntregar = new DataGridViewCheckBoxColumn();
            PodeCriarFuncoes = new DataGridViewCheckBoxColumn();
            PodeAlterarDefinicoes = new DataGridViewCheckBoxColumn();
            CorHex = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFuncoes).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(dgvFuncoes, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.Size = new Size(1837, 858);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(btnAddFuncao, 2, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1837, 60);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btnAddFuncao
            // 
            btnAddFuncao.BorderRadius = 10;
            btnAddFuncao.CustomizableEdges = customizableEdges1;
            btnAddFuncao.DisabledState.BorderColor = Color.DarkGray;
            btnAddFuncao.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAddFuncao.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAddFuncao.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAddFuncao.Dock = DockStyle.Fill;
            btnAddFuncao.FillColor = Color.FromArgb(243, 108, 33);
            btnAddFuncao.Font = new Font("Roboto", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddFuncao.ForeColor = Color.White;
            btnAddFuncao.Location = new Point(1256, 13);
            btnAddFuncao.Margin = new Padding(32, 13, 32, 13);
            btnAddFuncao.Name = "btnAddFuncao";
            btnAddFuncao.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAddFuncao.Size = new Size(549, 34);
            btnAddFuncao.TabIndex = 4;
            btnAddFuncao.Text = "Criar Função";
            btnAddFuncao.Click += btnAddFuncao_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto Medium", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(19, 0);
            label1.Margin = new Padding(19, 0, 0, 0);
            label1.Name = "label1";
            label1.Size = new Size(593, 60);
            label1.TabIndex = 0;
            label1.Text = "FUNÇÕES";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvFuncoes
            // 
            dgvFuncoes.AllowUserToAddRows = false;
            dgvFuncoes.AllowUserToDeleteRows = false;
            dgvFuncoes.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.Transparent;
            dgvFuncoes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvFuncoes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFuncoes.BackgroundColor = Color.White;
            dgvFuncoes.BorderStyle = BorderStyle.None;
            dgvFuncoes.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvFuncoes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.Padding = new Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvFuncoes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvFuncoes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFuncoes.Columns.AddRange(new DataGridViewColumn[] { ID, Nome, PodeVerStock, PodeCriarStock, PodeInserirStock, PodeEditarFunc, PodeSubmeter, PodeAprovar, PodeEntregar, PodeCriarFuncoes, PodeAlterarDefinicoes, CorHex });
            dgvFuncoes.Cursor = Cursors.Hand;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Transparent;
            dataGridViewCellStyle3.Font = new Font("Roboto Medium", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.Padding = new Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvFuncoes.DefaultCellStyle = dataGridViewCellStyle3;
            dgvFuncoes.Dock = DockStyle.Fill;
            dgvFuncoes.EnableHeadersVisualStyles = false;
            dgvFuncoes.GridColor = Color.White;
            dgvFuncoes.HeaderFontSize = 15F;
            dgvFuncoes.Location = new Point(19, 79);
            dgvFuncoes.Margin = new Padding(19, 19, 19, 0);
            dgvFuncoes.MultiSelect = false;
            dgvFuncoes.Name = "dgvFuncoes";
            dgvFuncoes.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.Transparent;
            dataGridViewCellStyle4.Font = new Font("Roboto", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvFuncoes.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvFuncoes.RowTemplate.Height = 54;
            dgvFuncoes.ScrollBars = ScrollBars.None;
            dgvFuncoes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFuncoes.Size = new Size(1799, 779);
            dgvFuncoes.TabIndex = 3;
            dgvFuncoes.CellDoubleClick += dgvFuncoes_CellDoubleClick;
            dgvFuncoes.CellValueChanged += dgvFuncoes_CellValueChanged;
            dgvFuncoes.CurrentCellDirtyStateChanged += dgvFuncoes_CurrentCellDirtyStateChanged;
            // 
            // ID
            // 
            ID.DataPropertyName = "ID";
            ID.HeaderText = "ID";
            ID.Name = "ID";
            ID.Visible = false;
            // 
            // Nome
            // 
            Nome.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Nome.DataPropertyName = "Nome";
            Nome.HeaderText = "Nome";
            Nome.Name = "Nome";
            Nome.ReadOnly = true;
            // 
            // PodeVerStock
            // 
            PodeVerStock.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeVerStock.DataPropertyName = "PodeVerStock";
            PodeVerStock.HeaderText = "Ver Stock";
            PodeVerStock.Name = "PodeVerStock";
            PodeVerStock.Resizable = DataGridViewTriState.True;
            PodeVerStock.SortMode = DataGridViewColumnSortMode.Automatic;
            PodeVerStock.Width = 89;
            // 
            // PodeCriarStock
            // 
            PodeCriarStock.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeCriarStock.DataPropertyName = "PodeCriarStock";
            PodeCriarStock.HeaderText = "Criar Stock";
            PodeCriarStock.Name = "PodeCriarStock";
            PodeCriarStock.Width = 78;
            // 
            // PodeInserirStock
            // 
            PodeInserirStock.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeInserirStock.DataPropertyName = "PodeInserirStock";
            PodeInserirStock.HeaderText = "Inserir Stock";
            PodeInserirStock.Name = "PodeInserirStock";
            PodeInserirStock.Width = 87;
            // 
            // PodeEditarFunc
            // 
            PodeEditarFunc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeEditarFunc.DataPropertyName = "PodeEditarFunc";
            PodeEditarFunc.HeaderText = "Editar Func";
            PodeEditarFunc.Name = "PodeEditarFunc";
            PodeEditarFunc.Width = 78;
            // 
            // PodeSubmeter
            // 
            PodeSubmeter.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeSubmeter.DataPropertyName = "PodeSubmeter";
            PodeSubmeter.HeaderText = "Submeter";
            PodeSubmeter.Name = "PodeSubmeter";
            PodeSubmeter.Width = 76;
            // 
            // PodeAprovar
            // 
            PodeAprovar.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeAprovar.DataPropertyName = "PodeAprovar";
            PodeAprovar.HeaderText = "Aprovar";
            PodeAprovar.Name = "PodeAprovar";
            PodeAprovar.Width = 64;
            // 
            // PodeEntregar
            // 
            PodeEntregar.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeEntregar.DataPropertyName = "PodeEntregar";
            PodeEntregar.HeaderText = "Entregar";
            PodeEntregar.Name = "PodeEntregar";
            PodeEntregar.Width = 68;
            // 
            // PodeCriarFuncoes
            // 
            PodeCriarFuncoes.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeCriarFuncoes.DataPropertyName = "PodeCriarFuncoes";
            PodeCriarFuncoes.HeaderText = "Criar Funcoes";
            PodeCriarFuncoes.Name = "PodeCriarFuncoes";
            PodeCriarFuncoes.Width = 94;
            // 
            // PodeAlterarDefinicoes
            // 
            PodeAlterarDefinicoes.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            PodeAlterarDefinicoes.DataPropertyName = "PodeAlterarDefinicoes";
            PodeAlterarDefinicoes.HeaderText = "Alterar Definicoes";
            PodeAlterarDefinicoes.Name = "PodeAlterarDefinicoes";
            PodeAlterarDefinicoes.Width = 119;
            // 
            // CorHex
            // 
            CorHex.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            CorHex.DataPropertyName = "CorHex";
            CorHex.HeaderText = "Cor";
            CorHex.Name = "CorHex";
            CorHex.Width = 5;
            // 
            // Funcoes
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "Funcoes";
            Size = new Size(1837, 858);
            Load += Funcoes_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFuncoes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Button btnAddFuncao;
        private PEPIDI.Models.PEPIDIDataGridView dgvFuncoes;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Nome;
        private DataGridViewCheckBoxColumn PodeVerStock;
        private DataGridViewCheckBoxColumn PodeCriarStock;
        private DataGridViewCheckBoxColumn PodeInserirStock;
        private DataGridViewCheckBoxColumn PodeEditarFunc;
        private DataGridViewCheckBoxColumn PodeSubmeter;
        private DataGridViewCheckBoxColumn PodeAprovar;
        private DataGridViewCheckBoxColumn PodeEntregar;
        private DataGridViewCheckBoxColumn PodeCriarFuncoes;
        private DataGridViewCheckBoxColumn PodeAlterarDefinicoes;
        private DataGridViewTextBoxColumn CorHex;
    }
}
