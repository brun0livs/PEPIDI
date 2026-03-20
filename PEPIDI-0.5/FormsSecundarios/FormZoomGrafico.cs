using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.Charts.WinForms; // Certifica-te que tens este using

namespace PEPIDI.FormsSecundarios
{
    public partial class FormZoomGrafico : Form
    {
        DataTable DT;
        string Titulo;

        public FormZoomGrafico(DataTable dados, string titulo, int nivelDetalhe)
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.DT = dados;
            this.Titulo = titulo;

            // Define o título na Label do topo e no gráfico
            lblTitulo.Text = "PEPIDI - " + titulo.ToUpper();

            ConfigurarGrafico(nivelDetalhe);
        }

        private void ConfigurarGrafico(int nivel)
        {
            gunaChart1.Datasets.Clear();
            gunaChart1.Title.Text = Titulo;

            var dataset = new GunaBarDataset();
            dataset.Label = "Quantidade Consumida";
            dataset.FillColors.Add(Color.FromArgb(242, 103, 34)); // Teu laranja padrão

            // Lógica de agrupamento baseada no nível da TrackBar
            string colunaChave = "";
            if (nivel == 0) colunaChave = "Funcao";
            else if (nivel == 1) colunaChave = "Familia";
            else if (nivel == 2) colunaChave = "Modelo";
            else colunaChave = "NomeFuncionario";

            try
            {
                var agrupado = DT.AsEnumerable()
                    .GroupBy(r => r[colunaChave]?.ToString() ?? "N/D")
                    .Select(g => new {
                        Chave = g.Key,
                        Total = g.Sum(r => Convert.ToInt32(r["Quantidade"]))
                    })
                    .OrderByDescending(x => x.Total);

                foreach (var item in agrupado)
                {
                    dataset.DataPoints.Add(item.Chave, item.Total);
                }

                gunaChart1.Datasets.Add(dataset);
                gunaChart1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar gráfico: " + ex.Message);
            }
        }

        private void lblDownload_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png|JPeg Image|*.jpg";
                sfd.Title = "Guardar Gráfico como Imagem";
                sfd.FileName = "Grafico_" + Titulo.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Criamos um Bitmap do tamanho do gráfico
                    Bitmap bmp = new Bitmap(gunaChart1.Width, gunaChart1.Height);
                    // "Desenha" o gráfico para dentro do bitmap
                    gunaChart1.DrawToBitmap(bmp, new Rectangle(0, 0, gunaChart1.Width, gunaChart1.Height));

                    bmp.Save(sfd.FileName);
                    MessageBox.Show("Imagem guardada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lblFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormZoomGrafico_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}