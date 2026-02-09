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
            tlpOP = new TableLayoutPanel();
            modernComboBox2 = new Guna.UI2.WinForms.Guna2ComboBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tlpQuant = new TableLayoutPanel();
            btnMais = new Guna.UI2.WinForms.Guna2Button();
            lblQuantidade = new Label();
            btnMenos = new Guna.UI2.WinForms.Guna2Button();
            tlpOP.SuspendLayout();
            tlpQuant.SuspendLayout();
            SuspendLayout();
            // 
            // tlpOP
            // 
            tlpOP.BackColor = Color.White;
            tlpOP.ColumnCount = 5;
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.46667F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.13333F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.86667F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.33333F));
            tlpOP.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.13333F));
            tlpOP.Controls.Add(modernComboBox2, 2, 0);
            tlpOP.Controls.Add(label3, 3, 0);
            tlpOP.Controls.Add(label2, 1, 0);
            tlpOP.Controls.Add(label1, 0, 0);
            tlpOP.Controls.Add(tlpQuant, 4, 0);
            tlpOP.Dock = DockStyle.Fill;
            tlpOP.Location = new Point(0, 0);
            tlpOP.Margin = new Padding(0);
            tlpOP.Name = "tlpOP";
            tlpOP.RowCount = 1;
            tlpOP.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpOP.Size = new Size(1450, 100);
            tlpOP.TabIndex = 0;
            // 
            // modernComboBox2
            // 
            modernComboBox2.BackColor = Color.Transparent;
            modernComboBox2.BorderColor = Color.FromArgb(254, 107, 0);
            modernComboBox2.BorderRadius = 18;
            modernComboBox2.CustomizableEdges = customizableEdges1;
            modernComboBox2.DisabledState.BorderColor = Color.FromArgb(254, 107, 0);
            modernComboBox2.Dock = DockStyle.Fill;
            modernComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            modernComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            modernComboBox2.FocusedColor = Color.FromArgb(255, 128, 0);
            modernComboBox2.FocusedState.BorderColor = Color.FromArgb(255, 128, 0);
            modernComboBox2.FocusedState.ForeColor = Color.Black;
            modernComboBox2.Font = new Font("Roboto", 15.75F);
            modernComboBox2.ForeColor = Color.Black;
            modernComboBox2.HoverState.BorderColor = Color.FromArgb(254, 107, 0);
            modernComboBox2.HoverState.FillColor = Color.White;
            modernComboBox2.HoverState.ForeColor = Color.Black;
            modernComboBox2.ItemHeight = 30;
            modernComboBox2.Location = new Point(927, 32);
            modernComboBox2.Margin = new Padding(5, 32, 5, 32);
            modernComboBox2.Name = "modernComboBox2";
            modernComboBox2.ShadowDecoration.CustomizableEdges = customizableEdges2;
            modernComboBox2.Size = new Size(162, 36);
            modernComboBox2.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Roboto", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(1097, 0);
            label3.Name = "label3";
            label3.Size = new Size(158, 100);
            label3.TabIndex = 4;
            label3.Text = "Quantidade:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Roboto", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(764, 0);
            label2.Name = "label2";
            label2.Size = new Size(155, 100);
            label2.TabIndex = 2;
            label2.Text = "Tamanho:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(755, 100);
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
            tlpQuant.Location = new Point(1268, 25);
            tlpQuant.Margin = new Padding(10, 25, 10, 25);
            tlpQuant.Name = "tlpQuant";
            tlpQuant.RowCount = 1;
            tlpQuant.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpQuant.Size = new Size(172, 50);
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
            btnMais.Location = new Point(114, 0);
            btnMais.Margin = new Padding(0);
            btnMais.Name = "btnMais";
            btnMais.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnMais.Size = new Size(58, 50);
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
            lblQuantidade.Location = new Point(57, 0);
            lblQuantidade.Margin = new Padding(0);
            lblQuantidade.Name = "lblQuantidade";
            lblQuantidade.Size = new Size(57, 50);
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
            btnMenos.Size = new Size(57, 50);
            btnMenos.TabIndex = 1;
            btnMenos.Text = "-";
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
        private Guna.UI2.WinForms.Guna2ComboBox modernComboBox2;
        private Label lblQuantidade;
        private Guna.UI2.WinForms.Guna2Button btnMais;
        private Guna.UI2.WinForms.Guna2Button btnMenos;
    }
}
