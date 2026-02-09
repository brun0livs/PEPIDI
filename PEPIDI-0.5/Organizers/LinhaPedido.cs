using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace PEPIDI.Organizers
{
    public partial class LinhaPedido : UserControl
    {
        private int _quantidade = 0;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Quantidade
        {
            get => _quantidade;
            set { _quantidade = Math.Max(0, value); lblQuantidade.Text = _quantidade.ToString(); }
        }

        // Texto do item
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        // Texto do item
        public string DescricaoEPI
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        // Acede ao combo real de tamanhos
        public Guna.UI2.WinForms.Guna2ComboBox ComboTamanho => modernComboBox2;

        // Tamanho selecionado *no momento*
        public string TamanhoSelecionado => ComboTamanho.SelectedItem?.ToString() ?? string.Empty;

        public LinhaPedido()
        {
            InitializeComponent();
            Quantidade = 0;
            btnMais.Click += (s, e) => { if (Quantidade < 5) Quantidade++; };
            btnMenos.Click += (s, e) => { if (Quantidade > 0) Quantidade--; };

            // Redondos
            this.Load += (s, e) =>
            {
                ArredondarCantos(this, 20);
                ArredondarCantos(tlpQuant, 20);
            };
        }

        private void ArredondarCantos(Control ctrl, int raio)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;

            int d = raio * 2;
            var rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                // evita leak da Region anterior
                ctrl.Region?.Dispose();
                ctrl.Region = new Region(path);
            }

            ctrl.Resize += (s, ev) => ArredondarCantos(ctrl, raio);
        }

    }

}
