using System.Reflection;
using System.Windows.Forms;

namespace PEPIDI.Organizers // Ou o nome exato da tua pasta
{
    public static class HelperPerformance
    {
        public static void AtivarDoubleBufferRecursivo(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // Ativa a aceleração de desenho via memória (DoubleBuffer)
                typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(c, true, null);

                if (c.HasChildren)
                {
                    AtivarDoubleBufferRecursivo(c);
                }
            }
        }
    }
}