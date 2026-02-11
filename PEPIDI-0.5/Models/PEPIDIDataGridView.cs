using System;

using System.ComponentModel;

using System.Drawing;

using System.Drawing.Drawing2D;

using System.Linq;

using System.Windows.Forms;



namespace PEPIDI.Models

{

    [ToolboxItem(true)]

    [DefaultProperty("BadgeColumnName")]

    [Description("DataGridView com estilo cartão + badge (PEPIDI).")]

    public class PEPIDIDataGridView : DataGridView

    {

        // ==== Propriedades (Designer) ====



        [Category("PEPIDI")]

        [DefaultValue("")]

        [Description("Nome da coluna (invisível) que contém o código HEX da cor (ex: #FF0000).")]

        public string BadgeColorColumnName { get; set; } = "";



        [Category("PEPIDI")]

        [DefaultValue(10f)]

        [Description("Tamanho da fonte dos títulos das colunas.")]

        public float HeaderFontSize { get; set; } = 15f;



        [Category("PEPIDI")]

        [DefaultValue(16)]

        public int CardCornerRadius { get; set; } = 16;



        [Category("PEPIDI")]

        [DefaultValue(54)]

        public int CardRowHeight { get; set; } = 54;



        [Category("PEPIDI")]

        [DefaultValue(typeof(Padding), "18, 10, 18, 10")]

        public Padding CellPadding { get; set; } = new Padding(18, 10, 18, 10);



        [Category("PEPIDI")]

        [DefaultValue(typeof(Color), "White")]

        public Color HeaderBackColor { get; set; } = Color.White;



        [Category("PEPIDI")]

        [DefaultValue(typeof(Color), "Black")]

        public Color HeaderForeColor { get; set; } = Color.Black;



        [Category("PEPIDI")]

        [DefaultValue("Departamento")]

        public string BadgeColumnName { get; set; } = "Departamento";



        // Cores personalizadas (Lógica ShouldSerialize para o Designer não reclamar)

        private Color _cardBackColor = Color.FromArgb(0xF3, 0xF3, 0xF3);

        [Category("PEPIDI")]

        public Color CardBackColor

        {

            get { return _cardBackColor; }

            set { _cardBackColor = value; }

        }

        private bool ShouldSerializeCardBackColor() => _cardBackColor != Color.FromArgb(0xF3, 0xF3, 0xF3);

        private void ResetCardBackColor() => _cardBackColor = Color.FromArgb(0xF3, 0xF3, 0xF3);





        private Color _badgeBackColor = Color.FromArgb(0xFE, 0xE3, 0xD1);

        [Category("PEPIDI")]

        public Color BadgeBackColor

        {

            get { return _badgeBackColor; }

            set { _badgeBackColor = value; }

        }

        private bool ShouldSerializeBadgeBackColor() => _badgeBackColor != Color.FromArgb(0xFE, 0xE3, 0xD1);

        private void ResetBadgeBackColor() => _badgeBackColor = Color.FromArgb(0xFE, 0xE3, 0xD1);





        private Color _badgeForeColor = Color.FromArgb(0xFE, 0x6B, 0x00);

        [Category("PEPIDI")]

        public Color BadgeForeColor

        {

            get { return _badgeForeColor; }

            set { _badgeForeColor = value; }

        }

        private bool ShouldSerializeBadgeForeColor() => _badgeForeColor != Color.FromArgb(0xFE, 0x6B, 0x00);

        private void ResetBadgeForeColor() => _badgeForeColor = Color.FromArgb(0xFE, 0x6B, 0x00);



        // === AÇÃO "⋮" EMBUTIDA ===

        [Category("PEPIDI")]

        [DefaultValue(false)]

        public bool ShowActionDots { get; set; } = false;



        [Category("PEPIDI")]

        [DefaultValue("Acao")]

        public string ActionDotsColumnName { get; set; } = "Acao";



        [Category("PEPIDI")]

        [DefaultValue("Estab")]

        public string ActionDotsAfterColumn { get; set; } = "Estab";



        [Category("PEPIDI")]

        [DefaultValue(36)]

        public int ActionDotsWidth { get; set; } = 36;



        [Category("PEPIDI")]

        [DefaultValue(typeof(Color), "Black")]

        public Color ActionDotsColor { get; set; } = Color.Black;



        [Category("PEPIDI")]

        [DefaultValue(13f)]

        public float ActionDotsFontSize { get; set; } = 13f;



        // Dispara quando clicas na célula "⋮"

        public event EventHandler<DataGridViewCellEventArgs> ActionDotsCellClick;



        public PEPIDIDataGridView()

        {

            // DoubleBuffered (via reflexão)

            var pi = typeof(DataGridView).GetProperty("DoubleBuffered",

                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (pi != null) pi.SetValue(this, true, null);



            BorderStyle = BorderStyle.None;

            BackgroundColor = Color.White;

            GridColor = BackColor;

            EnableHeadersVisualStyles = false;

            RowHeadersVisible = false;

            SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            MultiSelect = false;

            AllowUserToResizeRows = false;

            AllowUserToAddRows = false;

            CellBorderStyle = DataGridViewCellBorderStyle.None;

            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;



            AlternatingRowsDefaultCellStyle.BackColor = Color.Transparent;

            DefaultCellStyle.BackColor = Color.Transparent;

            DefaultCellStyle.ForeColor = Color.Black;

            DefaultCellStyle.SelectionBackColor = Color.Transparent;

            DefaultCellStyle.SelectionForeColor = Color.Black;

            DefaultCellStyle.Padding = CellPadding;





            ColumnHeadersDefaultCellStyle.BackColor = HeaderBackColor;

            ColumnHeadersDefaultCellStyle.ForeColor = HeaderForeColor;

            ColumnHeadersDefaultCellStyle.Font = new Font(Font, FontStyle.Bold);

            ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 8, 0, 8);

            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;



            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            RowTemplate.Height = CardRowHeight;

            RowsDefaultCellStyle.BackColor = Color.Transparent;

            RowTemplate.DividerHeight = 0;



            ScrollBars = ScrollBars.None;



            RowPrePaint += OnRowPrePaintCards;

            CellPainting += OnCellPaintingModern;

            Paint += OnPaintHeaderUnderline;

            ColumnAdded += OnColumnAddedAlignments;

        }



        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)

        {

            base.OnDataBindingComplete(e);

            EnsureActionDotsColumn();

            ClearSelection();

        }



        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)

        {

            base.OnColumnAdded(e);

            EnsureActionDotsColumn();

        }



        protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)

        {

            base.OnCellMouseMove(e);

            Cursor = IsActionDotsCell(e.ColumnIndex, e.RowIndex) ? Cursors.Hand : Cursors.Default;

        }



        protected override void OnCellClick(DataGridViewCellEventArgs e)

        {

            base.OnCellClick(e);

            if (IsActionDotsCell(e.ColumnIndex, e.RowIndex))

                ActionDotsCellClick?.Invoke(this, e);

        }



        private void EnsureActionDotsColumn()

        {

            if (!ShowActionDots) return;



            if (!Columns.Contains(ActionDotsColumnName))

            {

                var col = new DataGridViewTextBoxColumn

                {

                    Name = ActionDotsColumnName,

                    HeaderText = "",

                    ReadOnly = true,

                    SortMode = DataGridViewColumnSortMode.NotSortable,

                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,

                    Width = ActionDotsWidth

                };

                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                Columns.Add(col);

            }



            if (Columns.Contains(ActionDotsAfterColumn))

                Columns[ActionDotsColumnName].DisplayIndex = Columns[ActionDotsAfterColumn].DisplayIndex + 1;

            else

                Columns[ActionDotsColumnName].DisplayIndex = Columns.Count - 1;



            Columns[ActionDotsColumnName].Resizable = DataGridViewTriState.False;

            Columns[ActionDotsColumnName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            Columns[ActionDotsColumnName].Width = ActionDotsWidth;

        }



        private bool IsActionDotsCell(int colIndex, int rowIndex)

        {

            return ShowActionDots &&

                   rowIndex >= 0 && colIndex >= 0 &&

                   Columns[colIndex].Name == ActionDotsColumnName;

        }





        // Scroll vertical com a roda do rato (barras invisíveis)

        protected override void OnMouseWheel(MouseEventArgs e)

        {

            base.OnMouseWheel(e);



            if (Rows.Count == 0) return;



            int linesPerNotch = SystemInformation.MouseWheelScrollLines;

            if (linesPerNotch <= 0) linesPerNotch = 3;



            int steps = -(e.Delta / 120) * linesPerNotch;



            int first = FirstDisplayedScrollingRowIndex < 0 ? 0 : FirstDisplayedScrollingRowIndex;

            int target = Math.Max(0, Math.Min(Rows.Count - 1, first + steps));



            if (target != first)

            {

                try { FirstDisplayedScrollingRowIndex = target; } catch { }

            }

        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)

        {

            if (Rows.Count == 0) return base.ProcessCmdKey(ref msg, keyData);



            int first = FirstDisplayedScrollingRowIndex < 0 ? 0 : FirstDisplayedScrollingRowIndex;

            int visible = DisplayedRowCount(true);

            int target = first;



            switch (keyData)

            {

                case Keys.PageDown: target = Math.Min(Rows.Count - 1, first + Math.Max(1, visible - 1)); break;

                case Keys.PageUp: target = Math.Max(0, first - Math.Max(1, visible - 1)); break;

                case Keys.Down: target = Math.Min(Rows.Count - 1, first + 1); break;

                case Keys.Up: target = Math.Max(0, first - 1); break;

                default: return base.ProcessCmdKey(ref msg, keyData);

            }



            try { FirstDisplayedScrollingRowIndex = target; } catch { }

            return true;

        }



        protected override void OnCreateControl()

        {

            base.OnCreateControl();

            var vs = Controls.OfType<VScrollBar>().FirstOrDefault();

            var hs = Controls.OfType<HScrollBar>().FirstOrDefault();

            if (vs != null) { vs.Visible = false; vs.Enabled = false; }

            if (hs != null) { hs.Visible = false; hs.Enabled = false; }

        }



        private void OnPaintHeaderUnderline(object sender, PaintEventArgs e)

        {

            using (var p = new Pen(Color.FromArgb(225, 225, 225), 1))

            {

                int y = ColumnHeadersHeight - 1;

                e.Graphics.DrawLine(p, 0, y, Width, y);

            }

        }



        private void OnColumnAddedAlignments(object sender, DataGridViewColumnEventArgs e)

        {

            if (e.Column.ValueType == typeof(int) ||

                e.Column.HeaderText.IndexOf("Quantidade", StringComparison.OrdinalIgnoreCase) >= 0)

                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;



            if (e.Column.HeaderText.Equals(BadgeColumnName, StringComparison.OrdinalIgnoreCase))

                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }



        private void OnRowPrePaintCards(object sender, DataGridViewRowPrePaintEventArgs e)

        {

            if (e.RowIndex < 0) return;



            // 1. Limpa o fundo para garantir que não há rastros de pintura anterior

            var fullRow = new Rectangle(-1, e.RowBounds.Top - 1, ClientSize.Width + 2, e.RowBounds.Height + 2);

            using (var bg = new SolidBrush(Color.White))

                e.Graphics.FillRectangle(bg, fullRow);



            // 2. Define o retângulo do "Cartão"

            var rowRect = new Rectangle(

                -1,

                e.RowBounds.Top + 4,

                ClientSize.Width + 2,

                Math.Max(0, e.RowBounds.Height - 8)

            );



            // 3. Lógica de Cor de Seleção:

            // Se a linha estiver selecionada, escurecemos a cor de fundo (CardBackColor)

            Color corParaPintar = CardBackColor;



            if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)

            {

                // Escurece a cor em 20 unidades (ajustável)

                corParaPintar = Color.FromArgb(

                    Math.Max(0, CardBackColor.R - 20),

                    Math.Max(0, CardBackColor.G - 20),

                    Math.Max(0, CardBackColor.B - 20)

                );

            }



            // 4. Desenha o cartão com cantos arredondados

            using (var path = RoundedRect(rowRect, CardCornerRadius))

            using (var b = new SolidBrush(corParaPintar))

            {

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                e.Graphics.FillPath(b, path);

            }

        }



        // ==========================================

        //  MÉTODO DE PINTURA (ATUALIZADO PARA COR)

        // ==========================================

        private void OnCellPaintingModern(object sender, DataGridViewCellPaintingEventArgs e)

        {

            // Remove bordas

            if (e.RowIndex >= 0 || e.RowIndex == -1 && e.ColumnIndex >= 0)

            {

                e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;

                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;

                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            }



            // Cabeçalho

            if (e.RowIndex == -1 && e.ColumnIndex >= 0)

            {

                using (var back = new SolidBrush(HeaderBackColor))

                using (var bold = new Font(Font, FontStyle.Bold))

                {

                    e.Graphics.FillRectangle(back, e.CellBounds);

                    TextRenderer.DrawText(e.Graphics, Convert.ToString(e.FormattedValue),

                        bold, e.CellBounds, HeaderForeColor,

                        TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);

                }

                e.Handled = true;

                return;

            }



            // Ação "⋮"

            if (IsActionDotsCell(e.ColumnIndex, e.RowIndex))

            {

                e.Handled = true;

                using (var f = new Font("Segoe UI Symbol", ActionDotsFontSize, FontStyle.Bold))

                {

                    TextRenderer.DrawText(

                        e.Graphics, "⋮", f, e.CellBounds, ActionDotsColor,

                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter

                    );

                }

                return;

            }



            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;



            // Conteúdo

            e.PaintContent(e.CellBounds);

            e.Handled = true;



            // Badge na coluna configurada

            var col = Columns[e.ColumnIndex];

            if (col.HeaderText.Equals(BadgeColumnName, StringComparison.OrdinalIgnoreCase))

            {

                var text = Convert.ToString(e.FormattedValue);

                if (!string.IsNullOrWhiteSpace(text))

                {

                    var g = e.Graphics;

                    g.SmoothingMode = SmoothingMode.AntiAlias;



                    // --- LÓGICA DE COR DINÂMICA ---

                    Color backColorToUse = BadgeBackColor; // Padrão

                    Color foreColorToUse = BadgeForeColor; // Padrão



                    // Verifica se há coluna de cor definida e se existe

                    if (!string.IsNullOrEmpty(BadgeColorColumnName) && Columns.Contains(BadgeColorColumnName))

                    {

                        var colorValue = Rows[e.RowIndex].Cells[BadgeColorColumnName].Value;

                        if (colorValue != null)

                        {

                            try

                            {

                                // Tenta converter #RRGGBB

                                Color dynamicColor = ColorTranslator.FromHtml(colorValue.ToString());

                                backColorToUse = dynamicColor;

                                // Calcula contraste

                                foreColorToUse = GetContrastingTextColor(dynamicColor);

                            }

                            catch { /* Se falhar, mantém a cor padrão */ }

                        }

                    }

                    // -----------------------------



                    int padH = 10, padV = 4;

                    var size = TextRenderer.MeasureText(text, Font);

                    int w = size.Width + padH * 2;

                    int h = Math.Max(size.Height - 4, 12) + padV * 2;



                    int cx = e.CellBounds.X + (e.CellBounds.Width - w) / 2;

                    int cy = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;

                    var pill = new Rectangle(cx, cy, w, h);



                    // Desenha com as cores calculadas

                    using (var path = RoundedRect(pill, h / 2))

                    using (var back = new SolidBrush(backColorToUse))

                    {

                        g.FillPath(back, path);

                    }



                    TextRenderer.DrawText(g, text, Font, pill, foreColorToUse,

                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                }

            }

        }



        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)

        {

            int d = radius * 2;

            var path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);

            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);

            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);

            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);

            path.CloseFigure();

            return path;

        }



        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)

        {

            base.OnRowPostPaint(e);

            using (var bg = new SolidBrush(Color.White))

            {

                e.Graphics.FillRectangle(bg,

                    new Rectangle(0, e.RowBounds.Bottom - 1, ClientSize.Width, 1));

            }

        }



        protected override void WndProc(ref Message m)

        {

            const int WM_NCPAINT = 0x0085;

            const int WM_ERASEBKGND = 0x0014;

            const int WM_HSCROLL = 0x114;

            const int WM_VSCROLL = 0x115;



            if (m.Msg == WM_NCPAINT || m.Msg == WM_ERASEBKGND) return;



            base.WndProc(ref m);



            if (m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL) Invalidate();

        }



        // Função de contraste

        private Color GetContrastingTextColor(Color backColor)

        {

            double luminance = (0.299 * backColor.R + 0.587 * backColor.G + 0.114 * backColor.B) / 255;

            return luminance > 0.5 ? Color.Black : Color.White;

        }

    }

}