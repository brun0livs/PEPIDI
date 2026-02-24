namespace PEPIDI.UCs.UcsSecundarios
{
    partial class PedidosDetalhes
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private string state;
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PedidosDetalhes));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Curvas = new Guna.UI2.WinForms.Guna2Panel();
            Orientacao1 = new TableLayoutPanel();
            tlpDevolucoes = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            label2 = new Label();
            label5 = new Label();
            label7 = new Label();
            pnlScroll2 = new Panel();
            flpDevolucoes = new FlowLayoutPanel();
            pnlPedidos = new Guna.UI2.WinForms.Guna2Panel();
            OrientacaoPedidos = new TableLayoutPanel();
            label3 = new Label();
            close = new Label();
            Devolucao = new Guna.UI2.WinForms.Guna2Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label4 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnAprovar = new Guna.UI2.WinForms.Guna2Button();
            btnReprovar = new Guna.UI2.WinForms.Guna2Button();
            txtObs = new Guna.UI2.WinForms.Guna2TextBox();
            pnlConteudo = new Panel();
            tlpDesign = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblModelo = new Label();
            lblTamanho = new Label();
            lblQuantDisp = new Label();
            label1 = new Label();
            pnlScroll = new Panel();
            flpLinhas = new FlowLayoutPanel();
            lblClose = new Label();
            Curvas.SuspendLayout();
            Orientacao1.SuspendLayout();
            tlpDevolucoes.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            pnlScroll2.SuspendLayout();
            pnlPedidos.SuspendLayout();
            OrientacaoPedidos.SuspendLayout();
            Devolucao.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlConteudo.SuspendLayout();
            tlpDesign.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            pnlScroll.SuspendLayout();
            SuspendLayout();
            // 
            // Curvas
            // 
            Curvas.BorderColor = Color.Silver;
            Curvas.BorderRadius = 15;
            Curvas.BorderThickness = 1;
            Curvas.Controls.Add(Orientacao1);
            Curvas.CustomizableEdges = customizableEdges13;
            Curvas.Dock = DockStyle.Fill;
            Curvas.Location = new Point(0, 0);
            Curvas.Name = "Curvas";
            Curvas.ShadowDecoration.CustomizableEdges = customizableEdges14;
            Curvas.Size = new Size(600, 850);
            Curvas.TabIndex = 0;
            // 
            // Orientacao1
            // 
            Orientacao1.BackColor = Color.Transparent;
            Orientacao1.ColumnCount = 1;
            Orientacao1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Orientacao1.Controls.Add(tlpDevolucoes, 0, 3);
            Orientacao1.Controls.Add(pnlPedidos, 0, 0);
            Orientacao1.Controls.Add(Devolucao, 0, 2);
            Orientacao1.Controls.Add(tableLayoutPanel1, 0, 5);
            Orientacao1.Controls.Add(txtObs, 0, 4);
            Orientacao1.Controls.Add(pnlConteudo, 0, 1);
            Orientacao1.Dock = DockStyle.Fill;
            Orientacao1.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            Orientacao1.ForeColor = Color.White;
            Orientacao1.Location = new Point(0, 0);
            Orientacao1.Margin = new Padding(0);
            Orientacao1.Name = "Orientacao1";
            Orientacao1.RowCount = 7;
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.705883F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.64705849F));
            Orientacao1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            Orientacao1.Size = new Size(600, 850);
            Orientacao1.TabIndex = 3;
            // 
            // tlpDevolucoes
            // 
            tlpDevolucoes.ColumnCount = 1;
            tlpDevolucoes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpDevolucoes.Controls.Add(tableLayoutPanel5, 0, 0);
            tlpDevolucoes.Controls.Add(pnlScroll2, 0, 1);
            tlpDevolucoes.Dock = DockStyle.Fill;
            tlpDevolucoes.Location = new Point(0, 362);
            tlpDevolucoes.Margin = new Padding(0);
            tlpDevolucoes.Name = "tlpDevolucoes";
            tlpDevolucoes.RowCount = 2;
            tlpDevolucoes.RowStyles.Add(new RowStyle(SizeType.Percent, 19.7580643F));
            tlpDevolucoes.RowStyles.Add(new RowStyle(SizeType.Percent, 80.2419357F));
            tlpDevolucoes.Size = new Size(600, 248);
            tlpDevolucoes.TabIndex = 11;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.Transparent;
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Controls.Add(label2, 0, 0);
            tableLayoutPanel5.Controls.Add(label5, 1, 0);
            tableLayoutPanel5.Controls.Add(label7, 2, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(1, 1);
            tableLayoutPanel5.Margin = new Padding(1);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(598, 47);
            tableLayoutPanel5.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            label2.Location = new Point(3, 0);
            label2.Margin = new Padding(3, 0, 3, 5);
            label2.Name = "label2";
            label2.Size = new Size(293, 42);
            label2.TabIndex = 0;
            label2.Text = "Modelo";
            label2.TextAlign = ContentAlignment.BottomCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(64, 64, 64);
            label5.Location = new Point(302, 0);
            label5.Margin = new Padding(3, 0, 3, 5);
            label5.Name = "label5";
            label5.Size = new Size(143, 42);
            label5.TabIndex = 1;
            label5.Text = "Tamanho";
            label5.TextAlign = ContentAlignment.BottomCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(64, 64, 64);
            label7.Location = new Point(451, 0);
            label7.Margin = new Padding(3, 0, 3, 5);
            label7.Name = "label7";
            label7.Size = new Size(144, 42);
            label7.TabIndex = 3;
            label7.Text = "Quantidade";
            label7.TextAlign = ContentAlignment.BottomCenter;
            // 
            // pnlScroll2
            // 
            pnlScroll2.AutoScroll = true;
            pnlScroll2.Controls.Add(flpDevolucoes);
            pnlScroll2.Dock = DockStyle.Fill;
            pnlScroll2.Location = new Point(0, 49);
            pnlScroll2.Margin = new Padding(0);
            pnlScroll2.Name = "pnlScroll2";
            pnlScroll2.Size = new Size(600, 199);
            pnlScroll2.TabIndex = 0;
            // 
            // flpDevolucoes
            // 
            flpDevolucoes.AutoScroll = true;
            flpDevolucoes.Dock = DockStyle.Fill;
            flpDevolucoes.FlowDirection = FlowDirection.TopDown;
            flpDevolucoes.Location = new Point(0, 0);
            flpDevolucoes.Margin = new Padding(0);
            flpDevolucoes.Name = "flpDevolucoes";
            flpDevolucoes.Size = new Size(600, 199);
            flpDevolucoes.TabIndex = 0;
            flpDevolucoes.WrapContents = false;
            // 
            // pnlPedidos
            // 
            pnlPedidos.BorderRadius = 15;
            pnlPedidos.Controls.Add(OrientacaoPedidos);
            customizableEdges15.BottomLeft = false;
            customizableEdges15.BottomRight = false;
            pnlPedidos.CustomizableEdges = customizableEdges15;
            pnlPedidos.Dock = DockStyle.Fill;
            pnlPedidos.FillColor = Color.Gray;
            pnlPedidos.Location = new Point(0, 0);
            pnlPedidos.Margin = new Padding(0);
            pnlPedidos.Name = "pnlPedidos";
            pnlPedidos.ShadowDecoration.CustomizableEdges = customizableEdges16;
            pnlPedidos.Size = new Size(600, 57);
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
            OrientacaoPedidos.Size = new Size(600, 57);
            OrientacaoPedidos.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Roboto Medium", 20.25F, FontStyle.Bold);
            label3.Location = new Point(0, 0);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(510, 57);
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
            close.Image = (Image)resources.GetObject("close.Image");
            close.Location = new Point(510, 0);
            close.Margin = new Padding(0);
            close.Name = "close";
            close.Size = new Size(90, 57);
            close.TabIndex = 4;
            close.TextAlign = ContentAlignment.MiddleCenter;
            close.Click += close_Click;
            // 
            // Devolucao
            // 
            Devolucao.BorderRadius = 15;
            Devolucao.Controls.Add(tableLayoutPanel2);
            customizableEdges17.BottomLeft = false;
            customizableEdges17.BottomRight = false;
            Devolucao.CustomizableEdges = customizableEdges17;
            Devolucao.Dock = DockStyle.Fill;
            Devolucao.FillColor = Color.Gray;
            Devolucao.Location = new Point(0, 305);
            Devolucao.Margin = new Padding(0);
            Devolucao.Name = "Devolucao";
            Devolucao.ShadowDecoration.CustomizableEdges = customizableEdges18;
            Devolucao.Size = new Size(600, 57);
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
            tableLayoutPanel2.Size = new Size(600, 57);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Roboto Medium", 20.25F, FontStyle.Bold);
            label4.Location = new Point(10, 0);
            label4.Margin = new Padding(10, 0, 0, 0);
            label4.Name = "label4";
            label4.Size = new Size(590, 57);
            label4.TabIndex = 3;
            label4.Text = "DEVOLUÇÃO DE ROUPA";
            label4.TextAlign = ContentAlignment.MiddleLeft;
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
            tableLayoutPanel1.Location = new Point(3, 751);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(594, 73);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // btnAprovar
            // 
            btnAprovar.BorderRadius = 10;
            btnAprovar.CustomizableEdges = customizableEdges19;
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
            btnAprovar.ShadowDecoration.CustomizableEdges = customizableEdges20;
            btnAprovar.Size = new Size(187, 43);
            btnAprovar.TabIndex = 9;
            btnAprovar.Text = "Aprovar";
            btnAprovar.Click += btnAprovar_Click;
            // 
            // btnReprovar
            // 
            btnReprovar.BorderRadius = 10;
            btnReprovar.CustomizableEdges = customizableEdges21;
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
            btnReprovar.ShadowDecoration.CustomizableEdges = customizableEdges22;
            btnReprovar.Size = new Size(187, 43);
            btnReprovar.TabIndex = 7;
            btnReprovar.Text = "Reprovar";
            btnReprovar.Click += btnReprovar_Click;
            // 
            // txtObs
            // 
            txtObs.BorderColor = Color.FromArgb(224, 224, 224);
            txtObs.BorderRadius = 15;
            txtObs.CustomizableEdges = customizableEdges23;
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
            txtObs.Location = new Point(10, 620);
            txtObs.Margin = new Padding(10);
            txtObs.Multiline = true;
            txtObs.Name = "txtObs";
            txtObs.PlaceholderForeColor = Color.Silver;
            txtObs.PlaceholderText = "Observações";
            txtObs.SelectedText = "";
            txtObs.ShadowDecoration.CustomizableEdges = customizableEdges24;
            txtObs.Size = new Size(580, 118);
            txtObs.TabIndex = 5;
            // 
            // pnlConteudo
            // 
            pnlConteudo.Controls.Add(tlpDesign);
            pnlConteudo.Dock = DockStyle.Fill;
            pnlConteudo.Location = new Point(0, 57);
            pnlConteudo.Margin = new Padding(0);
            pnlConteudo.Name = "pnlConteudo";
            pnlConteudo.Size = new Size(600, 248);
            pnlConteudo.TabIndex = 10;
            // 
            // tlpDesign
            // 
            tlpDesign.ColumnCount = 1;
            tlpDesign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpDesign.Controls.Add(tableLayoutPanel3, 0, 0);
            tlpDesign.Controls.Add(pnlScroll, 0, 1);
            tlpDesign.Dock = DockStyle.Fill;
            tlpDesign.Location = new Point(0, 0);
            tlpDesign.Margin = new Padding(0);
            tlpDesign.Name = "tlpDesign";
            tlpDesign.RowCount = 2;
            tlpDesign.RowStyles.Add(new RowStyle(SizeType.Percent, 19.7580643F));
            tlpDesign.RowStyles.Add(new RowStyle(SizeType.Percent, 80.2419357F));
            tlpDesign.Size = new Size(600, 248);
            tlpDesign.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Transparent;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(lblModelo, 0, 0);
            tableLayoutPanel3.Controls.Add(lblTamanho, 1, 0);
            tableLayoutPanel3.Controls.Add(lblQuantDisp, 2, 0);
            tableLayoutPanel3.Controls.Add(label1, 3, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(1, 1);
            tableLayoutPanel3.Margin = new Padding(1);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
            tableLayoutPanel3.Size = new Size(598, 47);
            tableLayoutPanel3.TabIndex = 6;
            // 
            // lblModelo
            // 
            lblModelo.AutoSize = true;
            lblModelo.Dock = DockStyle.Fill;
            lblModelo.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblModelo.ForeColor = Color.FromArgb(64, 64, 64);
            lblModelo.Location = new Point(3, 0);
            lblModelo.Margin = new Padding(3, 0, 3, 5);
            lblModelo.Name = "lblModelo";
            lblModelo.Size = new Size(233, 42);
            lblModelo.TabIndex = 0;
            lblModelo.Text = "Modelo";
            lblModelo.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lblTamanho
            // 
            lblTamanho.AutoSize = true;
            lblTamanho.Dock = DockStyle.Fill;
            lblTamanho.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblTamanho.ForeColor = Color.FromArgb(64, 64, 64);
            lblTamanho.Location = new Point(242, 0);
            lblTamanho.Margin = new Padding(3, 0, 3, 5);
            lblTamanho.Name = "lblTamanho";
            lblTamanho.Size = new Size(113, 42);
            lblTamanho.TabIndex = 1;
            lblTamanho.Text = "Tamanho";
            lblTamanho.TextAlign = ContentAlignment.BottomCenter;
            // 
            // lblQuantDisp
            // 
            lblQuantDisp.AutoSize = true;
            lblQuantDisp.Dock = DockStyle.Fill;
            lblQuantDisp.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            lblQuantDisp.ForeColor = Color.FromArgb(64, 64, 64);
            lblQuantDisp.Location = new Point(361, 0);
            lblQuantDisp.Margin = new Padding(3, 0, 3, 5);
            lblQuantDisp.Name = "lblQuantDisp";
            lblQuantDisp.Size = new Size(113, 42);
            lblQuantDisp.TabIndex = 2;
            lblQuantDisp.Text = "Quant. Disp";
            lblQuantDisp.TextAlign = ContentAlignment.BottomCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto Medium", 11.25F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(480, 0);
            label1.Margin = new Padding(3, 0, 3, 5);
            label1.Name = "label1";
            label1.Size = new Size(115, 42);
            label1.TabIndex = 3;
            label1.Text = "Quantidade";
            label1.TextAlign = ContentAlignment.BottomCenter;
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.Controls.Add(flpLinhas);
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.Location = new Point(0, 49);
            pnlScroll.Margin = new Padding(0);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Size = new Size(600, 199);
            pnlScroll.TabIndex = 0;
            // 
            // flpLinhas
            // 
            flpLinhas.AutoScroll = true;
            flpLinhas.Dock = DockStyle.Fill;
            flpLinhas.FlowDirection = FlowDirection.TopDown;
            flpLinhas.Location = new Point(0, 0);
            flpLinhas.Margin = new Padding(0);
            flpLinhas.Name = "flpLinhas";
            flpLinhas.Size = new Size(600, 199);
            flpLinhas.TabIndex = 0;
            flpLinhas.WrapContents = false;
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
            tlpDevolucoes.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            pnlScroll2.ResumeLayout(false);
            pnlPedidos.ResumeLayout(false);
            OrientacaoPedidos.ResumeLayout(false);
            OrientacaoPedidos.PerformLayout();
            Devolucao.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            pnlConteudo.ResumeLayout(false);
            tlpDesign.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            pnlScroll.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel Curvas;
        private Label lblClose;
        private TableLayoutPanel Orientacao1;
        private Guna.UI2.WinForms.Guna2Panel pnlPedidos;
        private TableLayoutPanel OrientacaoPedidos;
        private Label label3;
        private Label close;
        private Guna.UI2.WinForms.Guna2Panel Devolucao;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label4;
        private TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Button btnAprovar;
        private Guna.UI2.WinForms.Guna2Button btnReprovar;
        private Guna.UI2.WinForms.Guna2TextBox txtObs;
        private Panel pnlConteudo;
        private TableLayoutPanel tlpDesign;
        private TableLayoutPanel tableLayoutPanel3;
        private Label lblModelo;
        private Label lblTamanho;
        private Label lblQuantDisp;
        private Label label1;
        private Panel pnlScroll;
        private FlowLayoutPanel flpLinhas;
        private TableLayoutPanel tlpDevolucoes;
        private TableLayoutPanel tableLayoutPanel5;
        private Label label2;
        private Label label5;
        private Label label7;
        private Panel pnlScroll2;
        private FlowLayoutPanel flpDevolucoes;
    }
}