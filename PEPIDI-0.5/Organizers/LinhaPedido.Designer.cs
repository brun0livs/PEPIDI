namespace PEPIDI.Organizers
{
    partial class LinhaPedido
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
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
            tlpOP = new TableLayoutPanel();
            ModerComboBoxTamanho = new Guna.UI2.WinForms.Guna2ComboBox();
            label1 = new Label();
            tlpQuant = new TableLayoutPanel();
            btnMais = new Guna.UI2.WinForms.Guna2Button();
            lblQuantidade = new Label();
            btnMenos = new Guna.UI2.WinForms.Guna2Button();
            label3 = new Label();
            label2 = new Label();
            label4 = new Label();
            moderComboBoxModelo = new Guna.UI2.WinForms.Guna2ComboBox();
            modernComboBoxTamanho = new Guna.UI2.WinForms.Guna2ComboBox();
            tlpOP.SuspendLayout();
            tlpQuant.SuspendLayout();
            SuspendLayout();
            // 
            // tlpOP
            // 
            tlpOP.BackColor = Color.White;
            tlpOP.ColumnCount = 7;
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.5F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.5F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpOP.Controls.Add(ModerComboBoxTamanho, 4, 0);
            tlpOP.Controls.Add(label1, 0, 0);
            tlpOP.Controls.Add(tlpQuant, 6, 0);
            tlpOP.Controls.Add(label3, 5, 0);
            tlpOP.Controls.Add(label2, 3, 0);
            tlpOP.Controls.Add(label4, 1, 0);
            tlpOP.Controls.Add(moderComboBoxModelo, 2, 0);
            tlpOP.Dock = DockStyle.Fill;
            tlpOP.Location = new Point(0, 0);
            tlpOP.Margin = new Padding(0);
            tlpOP.Name = "tlpOP";
            tlpOP.RowCount = 1;
            tlpOP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpOP.Size = new Size(1450, 100);
            tlpOP.TabIndex = 0;
            // 
            // ModerComboBoxTamanho
            // 
            ModerComboBoxTamanho.BackColor = Color.Transparent;
            ModerComboBoxTamanho.BorderColor = Color.FromArgb(254, 107, 0);
            ModerComboBoxTamanho.BorderRadius = 18;
            ModerComboBoxTamanho.CustomizableEdges = customizableEdges1;
            ModerComboBoxTamanho.DisabledState.BorderColor = Color.FromArgb(254, 107, 0);
            ModerComboBoxTamanho.Dock = DockStyle.Fill;
            ModerComboBoxTamanho.DrawMode = DrawMode.OwnerDrawFixed;
            ModerComboBoxTamanho.DropDownStyle = ComboBoxStyle.DropDownList;
            ModerComboBoxTamanho.FocusedColor = Color.FromArgb(255, 128, 0);
            ModerComboBoxTamanho.FocusedState.BorderColor = Color.FromArgb(255, 128, 0);
            ModerComboBoxTamanho.FocusedState.ForeColor = Color.Black;
            ModerComboBoxTamanho.Font = new Font("Roboto", 20.75F);
            ModerComboBoxTamanho.ForeColor = Color.Black;
            ModerComboBoxTamanho.HoverState.BorderColor = Color.FromArgb(254, 107, 0);
            ModerComboBoxTamanho.HoverState.FillColor = Color.White;
            ModerComboBoxTamanho.HoverState.ForeColor = Color.Black;
            ModerComboBoxTamanho.ItemHeight = 35;
            ModerComboBoxTamanho.Location = new Point(932, 31);
            ModerComboBoxTamanho.Margin = new Padding(5, 31, 5, 31);
            ModerComboBoxTamanho.Name = "ModerComboBoxTamanho";
            ModerComboBoxTamanho.ShadowDecoration.CustomizableEdges = customizableEdges2;
            ModerComboBoxTamanho.Size = new Size(120, 41);
            ModerComboBoxTamanho.TabIndex = 10;
            ModerComboBoxTamanho.SelectedIndexChanged += ComboTamanho_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 20.75F);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(385, 100);
            label1.TabIndex = 1;
            label1.Text = "Descrição EPI";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpQuant
            // 
            tlpQuant.ColumnCount = 3;
            tlpQuant.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tlpQuant.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tlpQuant.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tlpQuant.Controls.Add(btnMais, 2, 0);
            tlpQuant.Controls.Add(lblQuantidade, 1, 0);
            tlpQuant.Controls.Add(btnMenos, 0, 0);
            tlpQuant.Dock = DockStyle.Fill;
            tlpQuant.Location = new Point(1241, 25);
            tlpQuant.Margin = new Padding(10, 25, 10, 25);
            tlpQuant.Name = "tlpQuant";
            tlpQuant.RowCount = 1;
            tlpQuant.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpQuant.Size = new Size(199, 50);
            tlpQuant.TabIndex = 5;
            // 
            // btnMais
            // 
            btnMais.BorderRadius = 5;
            customizableEdges3.BottomLeft = false;
            customizableEdges3.TopLeft = false;
            btnMais.CustomizableEdges = customizableEdges3;
            btnMais.DisabledState.BorderColor = Color.DarkGray;
            btnMais.DisabledState.CustomBorderColor = Color.DarkGray;
            btnMais.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnMais.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnMais.Dock = DockStyle.Fill;
            btnMais.FillColor = Color.FromArgb(254, 107, 0);
            btnMais.Font = new Font("Segoe UI", 20.25F);
            btnMais.ForeColor = Color.White;
            btnMais.Location = new Point(132, 0);
            btnMais.Margin = new Padding(0);
            btnMais.Name = "btnMais";
            btnMais.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnMais.Size = new Size(67, 50);
            btnMais.TabIndex = 2;
            btnMais.Text = "+";
            // 
            // lblQuantidade
            // 
            lblQuantidade.AutoSize = true;
            lblQuantidade.BackColor = Color.Transparent;
            lblQuantidade.Dock = DockStyle.Fill;
            lblQuantidade.Font = new Font("Roboto", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuantidade.ForeColor = Color.Black;
            lblQuantidade.Location = new Point(66, 0);
            lblQuantidade.Margin = new Padding(0);
            lblQuantidade.Name = "lblQuantidade";
            lblQuantidade.Size = new Size(66, 50);
            lblQuantidade.TabIndex = 0;
            lblQuantidade.Text = "0";
            lblQuantidade.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnMenos
            // 
            btnMenos.BorderRadius = 5;
            customizableEdges5.BottomRight = false;
            customizableEdges5.TopRight = false;
            btnMenos.CustomizableEdges = customizableEdges5;
            btnMenos.DisabledState.BorderColor = Color.DarkGray;
            btnMenos.DisabledState.CustomBorderColor = Color.DarkGray;
            btnMenos.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnMenos.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnMenos.Dock = DockStyle.Fill;
            btnMenos.FillColor = Color.FromArgb(254, 107, 0);
            btnMenos.Font = new Font("Segoe UI", 20.25F);
            btnMenos.ForeColor = Color.White;
            btnMenos.Location = new Point(0, 0);
            btnMenos.Margin = new Padding(0);
            btnMenos.Name = "btnMenos";
            btnMenos.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnMenos.Size = new Size(66, 50);
            btnMenos.TabIndex = 1;
            btnMenos.Text = "-";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Roboto", 20.75F);
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(1060, 0);
            label3.Name = "label3";
            label3.Size = new Size(168, 100);
            label3.TabIndex = 4;
            label3.Text = "Quantidade:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Roboto", 20.75F);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(785, 0);
            label2.Name = "label2";
            label2.Size = new Size(139, 100);
            label2.TabIndex = 2;
            label2.Text = "Tamanho:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Roboto", 20.75F);
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(394, 0);
            label4.Name = "label4";
            label4.Size = new Size(117, 100);
            label4.TabIndex = 2;
            label4.Text = "Modelo:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            label4.Visible = false;
            // 
            // moderComboBoxModelo
            // 
            moderComboBoxModelo.BackColor = Color.Transparent;
            moderComboBoxModelo.BorderColor = Color.FromArgb(254, 107, 0);
            moderComboBoxModelo.BorderRadius = 18;
            moderComboBoxModelo.CustomizableEdges = customizableEdges7;
            moderComboBoxModelo.DisabledState.BorderColor = Color.FromArgb(254, 107, 0);
            moderComboBoxModelo.Dock = DockStyle.Fill;
            moderComboBoxModelo.DrawMode = DrawMode.OwnerDrawFixed;
            moderComboBoxModelo.DropDownStyle = ComboBoxStyle.DropDownList;
            moderComboBoxModelo.FocusedColor = Color.FromArgb(255, 128, 0);
            moderComboBoxModelo.FocusedState.BorderColor = Color.FromArgb(255, 128, 0);
            moderComboBoxModelo.FocusedState.ForeColor = Color.Black;
            moderComboBoxModelo.Font = new Font("Roboto", 20.75F);
            moderComboBoxModelo.ForeColor = Color.Black;
            moderComboBoxModelo.HoverState.BorderColor = Color.FromArgb(254, 107, 0);
            moderComboBoxModelo.HoverState.FillColor = Color.White;
            moderComboBoxModelo.HoverState.ForeColor = Color.Black;
            moderComboBoxModelo.ItemHeight = 35;
            moderComboBoxModelo.Location = new Point(519, 31);
            moderComboBoxModelo.Margin = new Padding(5, 31, 5, 31);
            moderComboBoxModelo.Name = "moderComboBoxModelo";
            moderComboBoxModelo.ShadowDecoration.CustomizableEdges = customizableEdges8;
            moderComboBoxModelo.Size = new Size(258, 41);
            moderComboBoxModelo.TabIndex = 9;
            moderComboBoxModelo.Visible = false;
            // 
            // modernComboBoxTamanho
            // 
            modernComboBoxTamanho.BackColor = Color.Transparent;
            modernComboBoxTamanho.BorderColor = Color.FromArgb(254, 107, 0);
            modernComboBoxTamanho.BorderRadius = 18;
            modernComboBoxTamanho.CustomizableEdges = customizableEdges9;
            modernComboBoxTamanho.DisabledState.BorderColor = Color.FromArgb(254, 107, 0);
            modernComboBoxTamanho.Dock = DockStyle.Fill;
            modernComboBoxTamanho.DrawMode = DrawMode.OwnerDrawFixed;
            modernComboBoxTamanho.DropDownStyle = ComboBoxStyle.DropDownList;
            modernComboBoxTamanho.FocusedColor = Color.FromArgb(255, 128, 0);
            modernComboBoxTamanho.FocusedState.BorderColor = Color.FromArgb(255, 128, 0);
            modernComboBoxTamanho.FocusedState.ForeColor = Color.Black;
            modernComboBoxTamanho.Font = new Font("Roboto", 20.75F);
            modernComboBoxTamanho.ForeColor = Color.Black;
            modernComboBoxTamanho.HoverState.BorderColor = Color.FromArgb(254, 107, 0);
            modernComboBoxTamanho.HoverState.FillColor = Color.White;
            modernComboBoxTamanho.HoverState.ForeColor = Color.Black;
            modernComboBoxTamanho.ItemHeight = 35;
            modernComboBoxTamanho.Location = new Point(932, 31);
            modernComboBoxTamanho.Margin = new Padding(5, 31, 5, 31);
            modernComboBoxTamanho.Name = "modernComboBoxTamanho";
            modernComboBoxTamanho.ShadowDecoration.CustomizableEdges = customizableEdges10;
            modernComboBoxTamanho.Size = new Size(120, 41);
            modernComboBoxTamanho.TabIndex = 9;
            modernComboBoxTamanho.SelectedIndexChanged += ComboTamanho_SelectedIndexChanged;
            // 
            // LinhaPedido
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.Transparent;
            Controls.Add(tlpOP);
            Margin = new Padding(0, 0, 0, 8);
            Name = "LinhaPedido";
            Size = new Size(1450, 100);
            tlpOP.ResumeLayout(false);
            tlpOP.PerformLayout();
            tlpQuant.ResumeLayout(false);
            tlpQuant.PerformLayout();
            ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel tlpOP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tlpQuant;
        private Guna.UI2.WinForms.Guna2ComboBox modernComboBoxTamanho;
        private Label lblQuantidade;
        private Guna.UI2.WinForms.Guna2Button btnMais;
        private Guna.UI2.WinForms.Guna2Button btnMenos;
        private Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox moderComboBoxModelo;
        private Guna.UI2.WinForms.Guna2ComboBox ModerComboBoxTamanho;
    }
}
