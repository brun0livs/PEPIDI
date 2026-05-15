namespace AgentePEPIDI
{
    partial class FormDetalhesStock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDetalhesStock));
            this.dgvStockBaixo = new AgentePEPIDI.PEPIDIDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBaixo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStockBaixo
            // 
            this.dgvStockBaixo.AllowUserToAddRows = false;
            this.dgvStockBaixo.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Transparent;
            this.dgvStockBaixo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStockBaixo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStockBaixo.BackgroundColor = System.Drawing.Color.White;
            this.dgvStockBaixo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStockBaixo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvStockBaixo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStockBaixo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStockBaixo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(18, 10, 18, 10);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStockBaixo.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStockBaixo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockBaixo.EnableHeadersVisualStyles = false;
            this.dgvStockBaixo.GridColor = System.Drawing.SystemColors.Control;
            this.dgvStockBaixo.HeaderFontSize = 15F;
            this.dgvStockBaixo.Location = new System.Drawing.Point(0, 0);
            this.dgvStockBaixo.MultiSelect = false;
            this.dgvStockBaixo.Name = "dgvStockBaixo";
            this.dgvStockBaixo.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Transparent;
            this.dgvStockBaixo.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvStockBaixo.RowTemplate.Height = 54;
            this.dgvStockBaixo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvStockBaixo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockBaixo.Size = new System.Drawing.Size(800, 450);
            this.dgvStockBaixo.TabIndex = 0;
            // 
            // FormDetalhesStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvStockBaixo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDetalhesStock";
            this.Text = "PEPIDI | Falta de Stock - Agente";
            this.Load += new System.EventHandler(this.FormDetalhesStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockBaixo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PEPIDIDataGridView dgvStockBaixo;
    }
}