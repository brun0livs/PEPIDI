using System;
using System.Windows.Forms;

public static class TouchScrollHelper
{
    public static void AtivarScrollPorArrasto(DataGridView dgv)
    {
        bool isDragging = false;
        int startY = 0;
        int startFirstRow = 0;

        dgv.MouseDown += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                // Ignora clique no header, se quiseres
                if (e.Y < dgv.ColumnHeadersHeight)
                    return;

                isDragging = true;
                startY = e.Y;

                if (dgv.FirstDisplayedScrollingRowIndex >= 0)
                    startFirstRow = dgv.FirstDisplayedScrollingRowIndex;
                else
                    startFirstRow = 0;

                dgv.Cursor = Cursors.Hand;
            }
        };

        dgv.MouseMove += (s, e) =>
        {
            if (!isDragging)
                return;

            if (dgv.RowTemplate.Height <= 0 || dgv.Rows.Count == 0)
                return;

            int diffY = e.Y - startY;

            // negativo porque arrastar para baixo deve scrollar para baixo
            int rowDelta = -diffY / dgv.RowTemplate.Height;

            int newIndex = startFirstRow + rowDelta;
            if (newIndex < 0) newIndex = 0;
            if (newIndex > dgv.Rows.Count - 1) newIndex = dgv.Rows.Count - 1;

            try
            {
                dgv.FirstDisplayedScrollingRowIndex = newIndex;
            }
            catch
            {
                // se der troll por causa de remoção de linhas, ignoramos
            }
        };

        dgv.MouseUp += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                dgv.Cursor = Cursors.Default;
            }
        };

        dgv.MouseLeave += (s, e) =>
        {
            // se ele sair do controlo ainda a arrastar, limpa o estado
            if (isDragging)
            {
                isDragging = false;
                dgv.Cursor = Cursors.Default;
            }
        };
    }
}
