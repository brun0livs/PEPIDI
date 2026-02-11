using System;
using System.Windows.Forms;

public static class TouchScrollHelper
{
    public static void AtivarScrollPorArrasto(DataGridView dgv)
    {
        if (dgv == null) return;

        bool isDragging = false;
        int startY = 0;
        int startFirstRow = 0;

        dgv.MouseDown += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                // Verifica se clicou no header - se sim, não inicia o scroll
                if (e.Y < dgv.ColumnHeadersHeight) return;

                isDragging = true;
                startY = e.Y;

                // Salva a posição inicial do scroll com segurança
                startFirstRow = dgv.FirstDisplayedScrollingRowIndex >= 0
                                ? dgv.FirstDisplayedScrollingRowIndex
                                : 0;

                dgv.Cursor = Cursors.Hand;
            }
        };

        dgv.MouseMove += (s, e) =>
        {
            if (!isDragging) return;

            // Proteção contra objetos nulos ou controle sendo destruído
            if (dgv.IsDisposed || dgv.RowTemplate == null || dgv.Rows.Count == 0)
            {
                isDragging = false;
                return;
            }

            int diffY = e.Y - startY;
            int rowHeight = dgv.RowTemplate.Height;

            // Evita divisão por zero se o RowHeight for inválido
            if (rowHeight <= 0) rowHeight = 20;

            // Cálculo do deslocamento de linhas
            int rowDelta = -diffY / rowHeight;
            int newIndex = startFirstRow + rowDelta;

            // Limita o index dentro do range real da grid
            if (newIndex < 0) newIndex = 0;
            if (newIndex >= dgv.Rows.Count) newIndex = dgv.Rows.Count - 1;

            try
            {
                if (newIndex != dgv.FirstDisplayedScrollingRowIndex)
                {
                    dgv.FirstDisplayedScrollingRowIndex = newIndex;
                }
            }
            catch
            {
                // Ignora erros de pintura ou mudanças rápidas na coleção de dados
            }
        };

        dgv.MouseUp += (s, e) =>
        {
            isDragging = false;
            if (!dgv.IsDisposed) dgv.Cursor = Cursors.Default;
        };

        dgv.MouseLeave += (s, e) =>
        {
            if (isDragging)
            {
                isDragging = false;
                if (!dgv.IsDisposed) dgv.Cursor = Cursors.Default;
            }
        };
    }
}