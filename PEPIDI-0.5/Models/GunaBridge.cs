using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace PEPIDI_0._5.Models
{
    // 1. A COLUNA
    public class GunaColumn : DataGridViewComboBoxColumn
    {
        public GunaColumn()
        {
            CellTemplate = new GunaCell();
        }
    }

    // 2. A CÉLULA
    public class GunaCell : DataGridViewComboBoxCell
    {
        public override Type EditType => typeof(GunaEditingControl);

        protected override void Paint(Graphics g, Rectangle clip, Rectangle b, int row, DataGridViewElementStates s, object val, object fVal, string err, DataGridViewCellStyle st, DataGridViewAdvancedBorderStyle ad, DataGridViewPaintParts p)
        {
            var grid = DataGridView as PEPIDIDataGridView;
            Color corFundo = grid != null ? grid.CardBackColor : Color.White;

            if ((s & DataGridViewElementStates.Selected) != 0)
                corFundo = Color.FromArgb(220, 220, 220);

            using (var brush = new SolidBrush(corFundo))
                g.FillRectangle(brush, b);

            using (var f = new Font("Segoe UI", 9f))
            {
                TextRenderer.DrawText(g, fVal?.ToString(), f, b, Color.Black,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            }

            g.DrawString("∨", new Font("Segoe UI", 8), Brushes.Gray, b.Right - 20, b.Y + b.Height / 2 - 7);
        }
    }

    // 3. O EDITOR (CORRIGIDO PARA O ERRO WFO10 DA IMAGEM 13A25B)
    public class GunaEditingControl : Guna2ComboBox, IDataGridViewEditingControl
    {
        // --- ESTES ATRIBUTOS [Browsable(false)] E [DesignerSerializationVisibility] SÃO OBRIGATÓRIOS PARA CORRIGIR O ERRO WFO10 ---

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView EditingControlDataGridView { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object EditingControlFormattedValue
        {
            get => Text;
            set => Text = value?.ToString();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int EditingControlRowIndex { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EditingControlValueChanged { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Cursor EditingPanelCursor => base.Cursor;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RepositionEditingControlOnValueChange => false;

        public GunaEditingControl()
        {
            BorderRadius = 10;
            BorderThickness = 0;
            FillColor = Color.FromArgb(243, 243, 243);
            ForeColor = Color.Black;
            ItemHeight = 25;
            DropDownHeight = 200;
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.Transparent;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle style)
        {
            Font = new Font("Segoe UI", 9f);
            ForeColor = Color.Black;
            if (EditingControlDataGridView is PEPIDIDataGridView grid)
                FillColor = grid.CardBackColor;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Enter:
                case Keys.Escape: return true;
                default: return !dataGridViewWantsInputKey;
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => Text;
        public void PrepareEditingControlForEdit(bool selectAll) { }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            EditingControlValueChanged = true;
            if (EditingControlDataGridView != null)
                EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }
    }
}