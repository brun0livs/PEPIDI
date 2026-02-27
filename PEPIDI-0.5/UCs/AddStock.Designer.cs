using Guna.UI2.WinForms;
using PEPIDI.Models;

namespace PEPIDI.UCs
{
    partial class AddStock
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            guna2CustomGradientPanel2 = new Guna2CustomGradientPanel();
            guna2CustomGradientPanel1 = new Guna2CustomGradientPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(guna2CustomGradientPanel2, 1, 1);
            tableLayoutPanel1.Controls.Add(guna2CustomGradientPanel1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.Size = new Size(1837, 858);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Roboto", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(912, 60);
            label1.TabIndex = 2;
            label1.Text = "ADICIONAR STOCK DE EPI";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // guna2CustomGradientPanel2
            // 
            guna2CustomGradientPanel2.BorderColor = Color.FromArgb(242, 103, 34);
            guna2CustomGradientPanel2.BorderRadius = 25;
            guna2CustomGradientPanel2.BorderThickness = 1;
            guna2CustomGradientPanel2.CustomizableEdges = customizableEdges1;
            guna2CustomGradientPanel2.Dock = DockStyle.Fill;
            guna2CustomGradientPanel2.Location = new Point(933, 75);
            guna2CustomGradientPanel2.Margin = new Padding(15);
            guna2CustomGradientPanel2.Name = "guna2CustomGradientPanel2";
            guna2CustomGradientPanel2.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2CustomGradientPanel2.Size = new Size(889, 768);
            guna2CustomGradientPanel2.TabIndex = 1;
            // 
            // guna2CustomGradientPanel1
            // 
            guna2CustomGradientPanel1.BorderColor = Color.FromArgb(242, 103, 34);
            guna2CustomGradientPanel1.BorderRadius = 25;
            guna2CustomGradientPanel1.BorderThickness = 1;
            guna2CustomGradientPanel1.CustomizableEdges = customizableEdges3;
            guna2CustomGradientPanel1.Dock = DockStyle.Fill;
            guna2CustomGradientPanel1.Location = new Point(15, 75);
            guna2CustomGradientPanel1.Margin = new Padding(15);
            guna2CustomGradientPanel1.Name = "guna2CustomGradientPanel1";
            guna2CustomGradientPanel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2CustomGradientPanel1.Size = new Size(888, 768);
            guna2CustomGradientPanel1.TabIndex = 0;
            // 
            // AddStock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "AddStock";
            Size = new Size(1837, 858);
            Load += UCAddStock_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private PEPIDIDataGridView dgvStock;
        private TableLayoutPanel tableLayoutPanel1;
        private Guna2CustomGradientPanel guna2CustomGradientPanel1;
        private Guna2CustomGradientPanel guna2CustomGradientPanel2;
        private Label label1;
    }
}
