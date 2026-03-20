using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.Charts.WinForms;
// Não te esqueças de ter o ClosedXML instalado via NuGet para a exportação de Excel!
// using ClosedXML.Excel; 

namespace PEPIDI.UCs
{
    public partial class Graficos : UserControl
    {
        public Graficos()
        {
            InitializeComponent();
        }

        // ==========================================
        // 1. CARREGAMENTO INICIAL (LOAD)
        // ==========================================
        private async void Graficos_Load(object sender, EventArgs e)
        {
            GestorTema.AplicarEstilos(this);

            // Carregar combos e tags
            await CarregarFuncionariosAsync();
            await CarregarFuncoesAsync();
            await CarregarFiltrosTextoAsync("Familia", flpFamilia);
            await CarregarFiltrosTextoAsync("Modelo", flpModelos);
            await CarregarFiltrosTextoAsync("Tamanho", flpTamanhos);

            
            FiltrosWorking(true);
        }

        // ==========================================
        // 2. METÓDOS DE CONSTRUÇÃO VISUAL DAS TAGS
        // ==========================================
        private async Task CarregarFuncionariosAsync()
        {
            var dtFuncs = new DataTable();
            dtFuncs.Columns.Add("Nr", typeof(int));
            dtFuncs.Columns.Add("NomeCompleto", typeof(string));

            string query = "SELECT Nr, Nome FROM Funcionarios ORDER BY Nr";

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                int nr = Convert.ToInt32(r["Nr"]);
                                string nome = r["Nome"].ToString();
                                dtFuncs.Rows.Add(nr, $"{nr} - {nome}");
                            }
                        }
                    }
                    catch { }
                }
            });

            // Remove temporariamente o evento para não disparar durante o preenchimento
            guna2ComboBox1.SelectedIndexChanged -= Filtros_Changed;
            guna2ComboBox1.DataSource = dtFuncs;
            guna2ComboBox1.DisplayMember = "NomeCompleto";
            guna2ComboBox1.ValueMember = "Nr";
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox1.SelectedIndexChanged += Filtros_Changed;
        }

        private async Task CarregarFuncoesAsync()
        {
            flpFuncoes.Visible = false;
            flpFuncoes.SuspendLayout();
            flpFuncoes.Controls.Clear();

            var lista = new List<KeyValuePair<int, string>>();
            string query = "SELECT ID, Nome FROM Funcoes ORDER BY Nome";

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                int id = Convert.ToInt32(r["ID"]);
                                string nome = r["Nome"].ToString().Trim();
                                lista.Add(new KeyValuePair<int, string>(id, nome));
                            }
                        }
                    }
                    catch { }
                }
            });

            foreach (var item in lista) CriarTag(item.Key.ToString(), item.Value, flpFuncoes);

            flpFuncoes.ResumeLayout(true);
            flpFuncoes.Visible = true;
        }

        private async Task CarregarFiltrosTextoAsync(string coluna, FlowLayoutPanel painel)
        {
            painel.Visible = false;
            painel.SuspendLayout();
            painel.Controls.Clear();

            var lista = new List<string>();
            string query = $"SELECT DISTINCT {coluna} FROM EPI WHERE {coluna} IS NOT NULL AND {coluna} <> '' ORDER BY {coluna}";

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read()) lista.Add(r[coluna].ToString().Trim());
                        }
                    }
                    catch { }
                }
            });

            foreach (var item in lista) CriarTag(item, item, painel);

            painel.ResumeLayout(true);
            painel.Visible = true;
        }

        private void CriarTag(string idOuTexto, string labelVisivel, FlowLayoutPanel painel)
        {
            Guna2Button tag = new Guna2Button();
            tag.Name = idOuTexto; // ID (Função) ou Texto (Modelos, Tamanhos) para a query
            tag.Text = labelVisivel;
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);

            int larguraTexto = TextRenderer.MeasureText(labelVisivel, tag.Font).Width;
            tag.Size = new Size(larguraTexto + 30, 35);
            tag.BorderRadius = 15;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;
            tag.FillColor = Color.FromArgb(230, 232, 235);
            tag.ForeColor = Color.FromArgb(64, 64, 64);
            tag.Margin = new Padding(0, 0, 10, 10);
            tag.Tag = false; // Estado Desligado

            tag.Click += (s, e) =>
            {
                bool isLigado = (bool)tag.Tag;
                tag.Tag = !isLigado;
                tag.FillColor = (bool)tag.Tag ? Color.FromArgb(242, 103, 34) : Color.FromArgb(230, 232, 235);
                tag.ForeColor = (bool)tag.Tag ? Color.White : Color.FromArgb(64, 64, 64);

                FiltrosWorking(true); // Atualiza os dados quando o utilizador clica!
            };

            painel.Controls.Add(tag);
        }

        // ==========================================
        // 3. MOTOR DE FILTROS E CONSTRUÇÃO DO GRÁFICO
        // ==========================================

        private void Filtros_Changed(object sender, EventArgs e)
        {
            FiltrosWorking(true);
        }

        // Método auxiliar para ler as tags que o utilizador selecionou num painel
        private string GetSelectedTags(FlowLayoutPanel flp)
        {
            var ativos = flp.Controls.OfType<Guna2Button>()
                                     .Where(b => b.Tag != null && (bool)b.Tag == true)
                                     .Select(b => b.Name);
            return string.Join(",", ativos);
        }

        private void FiltrosWorking(bool agruparPorFuncionario)
        {
            int nrFunc = guna2ComboBox1.SelectedValue != null && int.TryParse(guna2ComboBox1.SelectedValue.ToString(), out int val) ? val : 0;

            // Vai ler aos teus novos painéis FlowLayout quais os chips/tags que estão laranjas (ativos)
            string funcoesStr = GetSelectedTags(flpFuncoes);
            string familiasStr = GetSelectedTags(flpFamilia);
            string modelosStr = GetSelectedTags(flpModelos);
            string tamanhosStr = GetSelectedTags(flpTamanhos);

            DateTime? inicio = dtpInicio.Value.Date;
            DateTime? fim = dtpFim.Value.Date;

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ConsumosFiltrados", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NrFunc", nrFunc > 0 ? (object)nrFunc : DBNull.Value);
                cmd.Parameters.AddWithValue("@Funcoes", string.IsNullOrEmpty(funcoesStr) ? (object)DBNull.Value : funcoesStr);
                cmd.Parameters.AddWithValue("@Familias", string.IsNullOrEmpty(familiasStr) ? (object)DBNull.Value : familiasStr);
                cmd.Parameters.AddWithValue("@Modelos", string.IsNullOrEmpty(modelosStr) ? (object)DBNull.Value : modelosStr);
                cmd.Parameters.AddWithValue("@Tamanhos", string.IsNullOrEmpty(tamanhosStr) ? (object)DBNull.Value : tamanhosStr);
                cmd.Parameters.AddWithValue("@DataInicio", inicio.HasValue ? (object)inicio.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@DataFim", fim.HasValue ? (object)fim.Value : DBNull.Value);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dgvTabela.DataSource = dt;

                // Construção do GunaChart
                Grafico.Datasets.Clear();

                if (dt.Rows.Count > 0)
                {
                    // Usa GunaBarDataset (barras) em vez do Series
                    var dataset = new GunaBarDataset();
                    dataset.Label = "Quantidade Consumida";
                    dataset.FillColors.Add(Color.FromArgb(242, 103, 34)); // O teu laranja corporativo

                    if (agruparPorFuncionario)
                    {
                        var agrupamentoFunc = dt.AsEnumerable()
                            .GroupBy(row => row["NomeFuncionario"].ToString())
                            .Select(g => new
                            {
                                Nome = g.Key,
                                Total = g.Sum(r => Convert.ToInt32(r["Quantidade"]))
                            });

                        foreach (var item in agrupamentoFunc) dataset.DataPoints.Add(item.Nome, item.Total);
                        Grafico.Title.Text = "Consumo por Funcionário";
                    }
                    else
                    {
                        var agrupamentoModelo = dt.AsEnumerable()
                            .GroupBy(row => row["Modelo"].ToString())
                            .Select(g => new
                            {
                                Item = g.Key,
                                Total = g.Sum(r => Convert.ToInt32(r["Quantidade"]))
                            });

                        foreach (var item in agrupamentoModelo) dataset.DataPoints.Add(item.Item, item.Total);
                        Grafico.Title.Text = "Consumo por Produto";
                    }

                    Grafico.Datasets.Add(dataset);
                }

                Grafico.Update(); // Força o gráfico a desenhar as novas barras
            }
        }

        // ==========================================
        // 4. AÇÕES DE BOTÕES
        // ==========================================

        private void btnClear_Click(object sender, EventArgs e)
        {
            // 1. Limpa Combo
            guna2ComboBox1.SelectedIndexChanged -= Filtros_Changed;
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox1.SelectedIndexChanged += Filtros_Changed;

            // 2. Limpa todas as tags colocando a False e reponto a cor original
            foreach (var painel in new[] { flpFuncoes, flpFamilia, flpModelos, flpTamanhos })
            {
                foreach (Guna2Button btn in painel.Controls.OfType<Guna2Button>())
                {
                    btn.Tag = false;
                    btn.FillColor = Color.FromArgb(230, 232, 235);
                    btn.ForeColor = Color.FromArgb(64, 64, 64);
                }
            }

            // Atualiza para mostrar tudo outra vez
            FiltrosWorking(true);
        }

        private void ExpTab_Click(object sender, EventArgs e)
        {
            if (dgvTabela.DataSource is DataTable dt && dt.Rows.Count > 0)
            {
                ExportarParaExcel(dt, "ConsumoDetalhado");
            }
            else
            {
                MessageBox.Show("Não existem dados para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExpGraf_Click(object sender, EventArgs e)
        {
            // 1. Verifica se a lista de datasets está vazia
            if (Grafico.Datasets.Count == 0)
            {
                MessageBox.Show("O gráfico está vazio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Converte (Cast) o dataset genérico para GunaBarDataset para aceder aos DataPoints
            var dataset = Grafico.Datasets[0] as Guna.Charts.WinForms.GunaBarDataset;

            // 3. Agora já consegue verificar se os DataPoints estão vazios
            if (dataset == null || dataset.DataPoints.Count == 0)
            {
                MessageBox.Show("O gráfico está vazio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "Imagem PNG (*.png)|*.png";
                saveFile.FileName = $"GraficoConsumos_{DateTime.Now:yyyyMMdd}.png";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // O GunaChart renderiza gerando um Bitmap do tamanho do controlo
                    Bitmap bmp = new Bitmap(Grafico.Width, Grafico.Height);
                    Grafico.DrawToBitmap(bmp, new Rectangle(0, 0, Grafico.Width, Grafico.Height));
                    bmp.Save(saveFile.FileName, System.Drawing.Imaging.ImageFormat.Png);

                    MessageBox.Show("Gráfico guardado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportarParaExcel(DataTable dt, string titulo)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files|*.xlsx";
                sfd.FileName = $"{titulo}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // IMPORTANTE: Para o ClosedXML funcionar, tens de garantir que tens 
                        // using ClosedXML.Excel; no topo e o package NuGet instalado

                        /* DESCOMENTA ESTE BLOCO SE ESTIVERES A USAR O CLOSEDXML
                        using (var workbook = new ClosedXML.Excel.XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Dados");
                            worksheet.Cell(1, 1).InsertTable(dt, titulo, true);
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Exportação concluída com sucesso!", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        */

                        // Caso contrário, usa um método alternativo CSV ou Interop.
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao exportar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }
    }
}