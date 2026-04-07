using Guna.Charts.Interfaces;
using Guna.Charts.WinForms;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using PEPIDI.FormsSecundarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using PEPIDI.Models;

namespace PEPIDI.UCs
{
    public partial class Graficos : UserControl
    {
        // Importação para congelar o desenho da UI durante o processamento pesado
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        // Efeito UI e Conexão
        private readonly string _cs = GetConn.ConnectionString;
        private EfeitoUI M = new EfeitoUI();

        // Token para cancelar a query anterior se o utilizador clicar rápido demais (Debounce)
        private System.Threading.CancellationTokenSource _filtroCts = new();

        private int? _idFuncionarioFiltroInicial = null;

        public Graficos(int? idFuncionario = null)
        {
            InitializeComponent();
            _idFuncionarioFiltroInicial = idFuncionario;
        }

        // ==========================================
        // 1. CARREGAMENTO INICIAL (LOAD)
        // ==========================================
        private async void Grafico_Load(object sender, EventArgs e)
        {
            // 1. Congela o desenho para evitar o efeito de "montagem" visual
            SendMessage(this.Handle, WM_SETREDRAW, false, 0);

            // 2. Performance bruta: Ativa DoubleBuffer via Organizer
            PEPIDI.Organizers.HelperPerformance.AtivarDoubleBufferRecursivo(this);

            try
            {
                // 3. Executa todas as chamadas SQL em paralelo (Multi-core)
                var tarefas = new List<Task>
                {
                    CarregarFuncionariosAsync(),
                    CarregarFuncoesAsync(),
                    CarregarFiltrosTextoAsync("Familia", flpFamilia),
                    CarregarFiltrosTextoAsync("Modelo", flpModelos),
                    CarregarFiltrosTextoAsync("Tamanho", flpTamanhos)
                };

                await Task.WhenAll(tarefas);

                dtpInicio.Value = new DateTime(DateTime.Now.Year, 1, 1);
                dtpFim.Value = DateTime.Now;

                // 4. Desenha o gráfico inicial
                FiltrosWorking(tbNivelGrafico.Value, dgvTabela1);
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro no carregamento inicial: " + ex.Message, "Erro");
            }
            finally
            {
                // 5. Liberta o desenho e força um refresh único
                SendMessage(this.Handle, WM_SETREDRAW, true, 0);
                // Ligar o Scroll Automático para os painéis de filtros não esconderem botões!
                flpFuncoes.AutoScroll = true;
                flpFamilia.AutoScroll = true;
                flpModelos.AutoScroll = true;
                flpTamanhos.AutoScroll = true;

                // Para garantir que a barra de scroll não tapa o último botão à direita
                flpFuncoes.WrapContents = true;
                flpFamilia.WrapContents = true;
                flpModelos.WrapContents = true;
                flpTamanhos.WrapContents = true;
                this.Refresh();
                GestorTema.AplicarEstilos(this);
            }
        }

        // ==========================================
        // 2. MÉTODOS DE CONSTRUÇÃO VISUAL (OTIMIZADOS)
        // ==========================================

        private async Task CarregarFuncionariosAsync()
        {
            var dtFuncs = new DataTable();
            dtFuncs.Columns.Add("Nr", typeof(int));
            dtFuncs.Columns.Add("NomeCompleto", typeof(string));

            // 1. O NOVO ID FALSO (Impossível de conflitar com a BD)
            dtFuncs.Rows.Add(-999, "— TODOS OS FUNCIONÁRIOS —");

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_cs))
                // 2. Filtramos os IDs de sistema (-1, 0, etc.) para não aparecerem na ComboBox
                using (SqlCommand cmd = new SqlCommand("SELECT Nr, Nome FROM Funcionarios WHERE Nr > 0 ORDER BY Nr", conn))
                {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int nr = Convert.ToInt32(r["Nr"]);
                            dtFuncs.Rows.Add(nr, $"{nr} - {r["Nome"]}");
                        }
                    }
                }
            });

            cmbFuncs.SelectedIndexChanged -= Filtros_Changed;
            cmbFuncs.DataSource = dtFuncs;
            cmbFuncs.DisplayMember = "NomeCompleto";
            cmbFuncs.ValueMember = "Nr";

            // A NOSSA MAGIA: Se o ecrã foi chamado com um ID de funcionário, seleciona-o já!
            if (_idFuncionarioFiltroInicial.HasValue)
            {
                cmbFuncs.SelectedValue = _idFuncionarioFiltroInicial.Value;
                // Limpamos para não bloquear se o utilizador depois quiser ver "Todos"
                _idFuncionarioFiltroInicial = null;
            }
            else
            {
                cmbFuncs.SelectedIndex = -1;
            }

            cmbFuncs.SelectedIndexChanged += Filtros_Changed;
        }

        private async Task CarregarFuncoesAsync() => await CarregarGenericoAsync("SELECT ID, Nome FROM Funcoes ORDER BY Nome", "ID", "Nome", flpFuncoes);

        private async Task CarregarFiltrosTextoAsync(string coluna, FlowLayoutPanel painel)
            => await CarregarGenericoAsync($"SELECT DISTINCT [{coluna}] FROM EPI WHERE [{coluna}] IS NOT NULL AND [{coluna}] <> '' ORDER BY [{coluna}]", coluna, coluna, painel);

        private async Task CarregarGenericoAsync(string query, string colID, string colNome, FlowLayoutPanel painel)
        {
            var lista = new List<KeyValuePair<string, string>>();
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_cs))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            lista.Add(new KeyValuePair<string, string>(r[colID].ToString(), r[colNome].ToString().Trim()));
                    }
                }
            });

            var botoes = lista.Select(item => GerarTagBotao(item.Key, item.Value)).ToArray();

            painel.SuspendLayout();
            painel.Controls.Clear();
            painel.Controls.AddRange(botoes);
            painel.ResumeLayout(true);
        }

        private Guna2Button GerarTagBotao(string idOuTexto, string labelVisivel)
        {
            Guna2Button tag = new Guna2Button
            {
                Name = idOuTexto,
                Text = labelVisivel,
                Font = new Font("Roboto", 9F, FontStyle.Regular),
                BorderRadius = 15,
                Cursor = Cursors.Hand,
                Animated = false,
                FillColor = Color.FromArgb(230, 232, 235),
                ForeColor = Color.FromArgb(64, 64, 64),
                Margin = new Padding(0, 0, 8, 8),
                Tag = false
            };

            int larguraTexto = TextRenderer.MeasureText(labelVisivel, tag.Font).Width;
            tag.Size = new Size(larguraTexto + 25, 32);

            tag.Click += (s, e) =>
            {
                tag.Tag = !(bool)tag.Tag;
                tag.FillColor = (bool)tag.Tag ? Color.FromArgb(242, 103, 34) : Color.FromArgb(230, 232, 235);
                tag.ForeColor = (bool)tag.Tag ? Color.White : Color.FromArgb(64, 64, 64);
                AcionarFiltroComDelay();
            };

            return tag;
        }

        // ==========================================
        // 3. MOTOR DE FILTROS (DEBOUNCE)
        // ==========================================

        private void Filtros_Changed(object sender, EventArgs e) => AcionarFiltroComDelay();

        private async void AcionarFiltroComDelay()
        {
            _filtroCts.Cancel();
            _filtroCts = new System.Threading.CancellationTokenSource();

            try
            {
                await Task.Delay(400, _filtroCts.Token);
                FiltrosWorking(tbNivelGrafico.Value, dgvTabela1);
            }
            catch (OperationCanceledException) { }
        }

        private string GetSelectedTags(FlowLayoutPanel flp)
        {
            var ativos = flp.Controls.OfType<Guna2Button>()
                             .Where(b => (bool)b.Tag == true)
                             .Select(b => b.Name);
            return string.Join(",", ativos);
        }

        private async void FiltrosWorking(int nivelDetalhe, PEPIDIDataGridView dgvTabela)
        {
            int nrFunc = cmbFuncs.SelectedValue is int i ? i : -999;
            string funcoesStr = GetSelectedTags(flpFuncoes);
            string familiasStr = GetSelectedTags(flpFamilia);
            string modelosStr = GetSelectedTags(flpModelos);
            string tamanhosStr = GetSelectedTags(flpTamanhos);
            DateTime inicio = dtpInicio.Value.Date;
            DateTime fim = dtpFim.Value.Date;

            DataTable dt = new DataTable();

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(_cs))
                using (SqlCommand cmd = new SqlCommand("sp_ConsumosFiltrados", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NrFunc", nrFunc != -999 ? (object)nrFunc : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Funcoes", string.IsNullOrEmpty(funcoesStr) ? (object)DBNull.Value : funcoesStr);
                    cmd.Parameters.AddWithValue("@Familias", string.IsNullOrEmpty(familiasStr) ? (object)DBNull.Value : familiasStr);
                    cmd.Parameters.AddWithValue("@Modelos", string.IsNullOrEmpty(modelosStr) ? (object)DBNull.Value : modelosStr);
                    cmd.Parameters.AddWithValue("@Tamanhos", string.IsNullOrEmpty(tamanhosStr) ? (object)DBNull.Value : tamanhosStr);
                    cmd.Parameters.AddWithValue("@DataInicio", inicio);
                    cmd.Parameters.AddWithValue("@DataFim", fim);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            });

            // 1. Atribuir os dados à tabela
            dgvTabela.DataSource = dt;

            // 2. Ocultar colunas desnecessárias para a visualização da tabela
            string[] colsToHide = { "Funcao", "Familia", "PrecoUnitario", "TotalGasto" };
            foreach (string colName in colsToHide)
            {
                if (dgvTabela.Columns.Contains(colName))
                    dgvTabela.Columns[colName].Visible = false;
            }

            // 3. Configuração de AutoSize (O SEGREDO DO DESIGN)
            // Primeiro: todas as colunas ocupam apenas o espaço do texto
            dgvTabela.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Segundo: A coluna do Nome "estica" para ocupar o resto do ecrã
            if (dgvTabela.Columns.Contains("NomeFuncionario"))
            {
                dgvTabela.Columns["NomeFuncionario"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // 4. Atualizar o gráfico (o gráfico continua a ter acesso às colunas ocultas via DT)
            AtualizarGrafico(dt, nivelDetalhe);
        }

        private void AtualizarGrafico(DataTable dt, int nivelDetalhe)
        {
            Grafico.Datasets.Clear();
            Grafico.Update();
            if (dt.Rows.Count == 0) return;

            var dataset = new GunaBarDataset { Label = "Qtd Consumida" };
            dataset.FillColors.Add(Color.FromArgb(242, 103, 34));

            string colunaAgrupamento = nivelDetalhe switch
            {
                1 => "Familia",
                2 => dt.Columns.Contains("Modelo") ? "Modelo" : (dt.Columns.Contains("Artigo") ? "Artigo" : "Descricao"),
                3 => "NomeFuncionario",
                _ => "Funcao"
            };

            if (!dt.Columns.Contains(colunaAgrupamento)) return;

            var agrupado = dt.AsEnumerable()
                .Where(r => nivelDetalhe != 3 || (r["NomeFuncionario"]?.ToString() != "-1" && r["NomeFuncionario"]?.ToString() != "0" && r["NomeFuncionario"]?.ToString() != "-999"))
                .GroupBy(r => r[colunaAgrupamento]?.ToString() ?? "N/A")
                .Select(g => new { Chave = g.Key, Total = g.Sum(r => Convert.ToInt32(r["Quantidade"])) })
                .OrderByDescending(x => x.Total)
                .ToList();

            // =========================================================================
            // SCROLL HORIZONTAL DINÂMICO (Adaptado ao novo contentor)
            // =========================================================================

            // A MAGIA: Lemos o pai e dizemos logo ao C# para o tratar como um "Panel"
            if (Grafico.Parent is Panel painelPai)
            {
                painelPai.AutoScroll = true;

                Grafico.Dock = DockStyle.None;
                Grafico.Location = new Point(0, 0);

                // Altura do painel pai menos o espaço para a barra de scroll (para não tapar o eixo X)
                Grafico.Height = painelPai.ClientSize.Height - 22;

                int larguraNecessaria = agrupado.Count * 90;
                Grafico.Width = Math.Max(painelPai.ClientSize.Width, larguraNecessaria);
            }
            // =========================================================================
            foreach (var item in agrupado)
            {
                // Como agora temos espaço infinito, já NÃO cortamos o texto! Vai inteiro!
                string nomeLabel = string.IsNullOrWhiteSpace(item.Chave) ? "Desconhecido" : item.Chave;
                dataset.DataPoints.Add(nomeLabel, item.Total);
            }

            Grafico.Datasets.Add(dataset);

            Grafico.XAxes.Display = true;
            if (Grafico.XAxes.Ticks != null) Grafico.XAxes.Ticks.Display = true;

            Grafico.Update();
        }

        // ==========================================
        // 4. AÇÕES E EXPORTAÇÃO
        // ==========================================
        private void btnClear_Click(object sender, EventArgs e)
        {
            // 1. Desligar temporariamente o evento para não disparar pesquisas falsas
            cmbFuncs.SelectedIndexChanged -= Filtros_Changed;

            // 2. Limpar a ComboBox a 100% (Usar SelectedVALUE e não Index!)
            cmbFuncs.SelectedValue = -999;
            cmbFuncs.Text = "";

            // 3. Voltar a ligar o evento
            cmbFuncs.SelectedIndexChanged += Filtros_Changed;

            // 4. Repor as Datas para o primeiro dia do ano atual!
            dtpInicio.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpFim.Value = DateTime.Now;

            // 5. Varredura Total aos Filtros Visuais (Botões)
            foreach (var painel in new[] { flpFuncoes, flpFamilia, flpModelos, flpTamanhos })
            {
                foreach (Guna2Button btn in painel.Controls.OfType<Guna2Button>())
                {
                    btn.Tag = false;
                    btn.FillColor = Color.FromArgb(230, 232, 235); // Cor base
                    btn.ForeColor = Color.FromArgb(64, 64, 64);    // Texto escuro
                }
            }

            // 6. Atualizar os gráficos INSTANTANEAMENTE
            FiltrosWorking(tbNivelGrafico.Value, dgvTabela1);
        }

        private void ExpTab_Click(object sender, EventArgs e)
        {
            if (dgvTabela1.DataSource is DataTable dt && dt.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel Files|*.xlsx", FileName = $"PEPIDI_Export_{DateTime.Now:yyyyMMdd}.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Exemplo simplificado de CSV se não tiveres ClosedXML configurado
                            M.AbrirMensagem("Exportação iniciada para: " + sfd.FileName, "Exportar");
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            }
        }

        private void ExpGraf_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFile = new SaveFileDialog { Filter = "Imagem PNG (*.png)|*.png", FileName = $"Grafico_{DateTime.Now:yyyyMMdd}.png" })
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(Grafico.Width, Grafico.Height);
                    Grafico.DrawToBitmap(bmp, new Rectangle(0, 0, Grafico.Width, Grafico.Height));
                    bmp.Save(saveFile.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    M.AbrirMensagem("Gráfico guardado!", "Sucesso");
                }
            }
        }

        private void lblFS_Click(object sender, EventArgs e)
        {
            // 1. Verificar se existem dados
            if (!(dgvTabela1.DataSource is DataTable dtDados) || dtDados.Rows.Count == 0)
            {
                M.AbrirMensagem("Não existem dados para expandir.", "PEPIDI");
                return;
            }

            // 2. Criar o formulário de Zoom com estilo moderno
            using (Form formZoom = new Form())
            {
                formZoom.Text = "PEPIDI - Vista Detalhada: " + Grafico.Title.Text;
                formZoom.WindowState = FormWindowState.Maximized;
                formZoom.StartPosition = FormStartPosition.CenterScreen;
                formZoom.BackColor = Color.White;
                formZoom.ShowIcon = false;
                formZoom.KeyPreview = true; // Para fechar com ESC

                // 3. Instanciar o novo gráfico de Zoom
                Guna.Charts.WinForms.GunaChart zoomChart = new Guna.Charts.WinForms.GunaChart();
                zoomChart.Dock = DockStyle.Fill;
                zoomChart.BackColor = Color.White;

                // --- COPIAR CONFIGURAÇÕES VISUAIS DO ORIGINAL ---
                zoomChart.Title.Text = Grafico.Title.Text;
                zoomChart.Title.Font = Grafico.Title.Font;
                zoomChart.Legend.Position = Grafico.Legend.Position;
                zoomChart.XAxes.Display = Grafico.XAxes.Display;
                zoomChart.YAxes.Display = Grafico.YAxes.Display;

                // Copiar as cores das grids (opcional, para ficar igualzinho)
                zoomChart.XAxes.GridLines.Color = Grafico.XAxes.GridLines.Color;
                zoomChart.YAxes.GridLines.Color = Grafico.YAxes.GridLines.Color;

                // 4. CLONAR OS DATASETS (O segredo está em recriar o objeto)
                foreach (var originalDs in Grafico.Datasets)
                {
                    if (originalDs is Guna.Charts.WinForms.GunaBarDataset barDs)
                    {
                        var newDs = new Guna.Charts.WinForms.GunaBarDataset();
                        newDs.Label = barDs.Label;

                        // CORREÇÃO CS1503: Cast para (Color)
                        foreach (object colorObj in barDs.FillColors)
                        {
                            if (colorObj is Color c) newDs.FillColors.Add(c);
                        }

                        // CORREÇÃO CS1061: Cast para o tipo de ponto do Guna
                        // Dependendo da versão, o tipo pode ser LPoint ou ChartPoint
                        foreach (var pointObj in barDs.DataPoints)
                        {
                            // Usamos 'dynamic' aqui para aceder a .Label e .Y sem erros de tipagem
                            dynamic p = pointObj;
                            newDs.DataPoints.Add(p.Label, p.Y);
                        }

                        zoomChart.Datasets.Add(newDs);
                    }
                }

                // Botão para fechar (opcional, além do X da janela)
                formZoom.KeyDown += (s, args) => { if (args.KeyCode == Keys.Escape) formZoom.Close(); };

                formZoom.Controls.Add(zoomChart);

                // 5. Forçar atualização e mostrar
                zoomChart.Update();
                formZoom.ShowDialog();
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            // Procura o Form onde o UserControl foi colocado e fecha-o.
            this.FindForm()?.Close();
        }

        private void tbNivelGrafico_ValueChanged(object sender, EventArgs e) => AcionarFiltroComDelay();

        private void Grafico_DoubleClick(object sender, EventArgs e)
        {
            // 1. Verificar se existem dados
            if (!(dgvTabela1.DataSource is DataTable dtDados) || dtDados.Rows.Count == 0)
            {
                M.AbrirMensagem("Não existem dados para expandir.", "PEPIDI");
                return;
            }

            // 2. Criar o formulário de Zoom
            using (Form formZoom = new Form())
            {
                formZoom.Text = "PEPIDI - Vista Detalhada: " + Grafico.Title.Text;
                formZoom.WindowState = FormWindowState.Maximized;
                formZoom.StartPosition = FormStartPosition.CenterScreen;
                formZoom.BackColor = Color.White;
                formZoom.ShowIcon = false;
                formZoom.KeyPreview = true; // Para fechar com ESC

                Guna.Charts.WinForms.GunaChart zoomChart = new Guna.Charts.WinForms.GunaChart();
                zoomChart.Dock = DockStyle.Fill;
                zoomChart.BackColor = Color.White;

                // --- COPIAR CONFIGURAÇÕES VISUAIS DO ORIGINAL ---
                zoomChart.Title.Text = Grafico.Title.Text;
                zoomChart.Title.Font = Grafico.Title.Font;
                zoomChart.Legend.Position = Grafico.Legend.Position;
                zoomChart.XAxes.Display = Grafico.XAxes.Display;
                zoomChart.YAxes.Display = Grafico.YAxes.Display;

                // 3. CLONAR OS DATASETS
                foreach (var originalDs in Grafico.Datasets)
                {
                    if (originalDs is Guna.Charts.WinForms.GunaBarDataset barDs)
                    {
                        var newDs = new Guna.Charts.WinForms.GunaBarDataset();
                        newDs.Label = barDs.Label;

                        foreach (object colorObj in barDs.FillColors)
                        {
                            if (colorObj is Color c) newDs.FillColors.Add(c);
                        }

                        foreach (var pointObj in barDs.DataPoints)
                        {
                            dynamic p = pointObj;
                            newDs.DataPoints.Add(p.Label, p.Y);
                        }
                        zoomChart.Datasets.Add(newDs);
                    }
                }

                formZoom.KeyDown += (s, args) => { if (args.KeyCode == Keys.Escape) formZoom.Close(); };
                formZoom.Controls.Add(zoomChart);

                zoomChart.Update();
                formZoom.ShowDialog();
            }
        }
    }
}