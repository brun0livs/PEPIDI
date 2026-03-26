using Guna.UI2.WinForms;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PEPIDI.FormsSecundarios;

namespace PEPIDI.FormsSecundarios
{
    public partial class FormGestaoDeFiltros : Form
    {
        EfeitoUI M = new();
        public FormGestaoDeFiltros()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            // Ligar o evento manualmente caso não esteja no Designer
            cmbVisaoNome.SelectedIndexChanged += cmbVisaoNome_SelectedIndexChanged;
        }

        private async void FormGestaoDeFiltros_Load(object sender, EventArgs e)
        {
            CarregarVisoes();
            await CarregarFiltrosAsync();
            GestorTema.AplicarEstilos(this);
        }

        private void lblFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ==========================================
        // 1. CARREGAMENTO DOS DADOS (ASSÍNCRONO)
        // ==========================================
        private async Task CarregarFiltrosAsync()
        {
            List<string> funcoes = new List<string>();
            List<string> familias = new List<string>();
            List<string> modelos = new List<string>();
            List<string> tamanhos = new List<string>();

            flpFuncoes.SuspendLayout();
            flpFamilia.SuspendLayout();
            flpModelo.SuspendLayout();
            flpTamanho.SuspendLayout();

            await Task.Run(() =>
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT Nome FROM Funcoes ORDER BY Nome", conn))
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read()) funcoes.Add(rdr.GetString(0));

                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Familia FROM EPI WHERE Familia IS NOT NULL ORDER BY Familia", conn))
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read()) familias.Add(rdr.GetString(0));

                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Modelo FROM EPI WHERE Modelo IS NOT NULL ORDER BY Modelo", conn))
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read()) modelos.Add(rdr.GetString(0));

                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Tamanho FROM EPI WHERE Tamanho IS NOT NULL ORDER BY Tamanho", conn))
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read()) tamanhos.Add(rdr.GetString(0));
                }
            });

            foreach (var f in funcoes) CriarTagFiltro(f, flpFuncoes);
            foreach (var fa in familias) CriarTagFiltro(fa, flpFamilia);
            foreach (var m in modelos) CriarTagFiltro(m, flpModelo);
            foreach (var t in tamanhos) CriarTagFiltro(t, flpTamanho);

            flpFuncoes.ResumeLayout(true);
            flpFamilia.ResumeLayout(true);
            flpModelo.ResumeLayout(true);
            flpTamanho.ResumeLayout(true);
        }

        private void CriarTagFiltro(string texto, FlowLayoutPanel painel)
        {
            Guna2Button tag = new Guna2Button();
            tag.Text = texto;
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
            tag.Height = 35;
            int larguraPainel = painel.ClientSize.Width > 0 ? painel.ClientSize.Width : 200;
            tag.Width = larguraPainel - 25;
            tag.BorderRadius = 10;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;
            tag.TextAlign = HorizontalAlignment.Left;
            tag.TextOffset = new Point(10, 0);
            tag.FillColor = Color.White;
            tag.BorderColor = Color.FromArgb(200, 200, 200);
            tag.BorderThickness = 1;
            tag.ForeColor = Color.FromArgb(64, 64, 64);
            tag.Margin = new Padding(3, 3, 3, 3);
            tag.Tag = false;

            tag.Click += (s, e) =>
            {
                bool isLigado = (bool)tag.Tag;
                if (!isLigado)
                {
                    tag.FillColor = Color.FromArgb(254, 235, 226);
                    tag.BorderColor = Color.FromArgb(254, 107, 0);
                    tag.ForeColor = Color.FromArgb(254, 107, 0);
                    tag.Font = new Font("Roboto", 10F, FontStyle.Bold);
                    tag.Tag = true;
                }
                else
                {
                    tag.FillColor = Color.White;
                    tag.BorderColor = Color.FromArgb(200, 200, 200);
                    tag.ForeColor = Color.FromArgb(64, 64, 64);
                    tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
                    tag.Tag = false;
                }
            };
            painel.Controls.Add(tag);
        }

        private List<string> ObterSelecionados(FlowLayoutPanel painel)
        {
            return painel.Controls.OfType<Guna2Button>()
                         .Where(b => b.Tag is bool estado && estado)
                         .Select(b => b.Text).ToList();
        }

        private string GerarQuery()
        {
            List<string> filtros = new List<string>();

            // 1. Lógica para Funções (AcessoIDs)
            var funcoesSelecionadas = ObterSelecionados(flpFuncoes);
            if (funcoesSelecionadas.Any())
            {
                List<int> acessos = ObterAcessoIDs(funcoesSelecionadas);
                string filtroAcesso = acessos.Any()
                    ? $"E.Acesso IN ({string.Join(",", acessos)})"
                    : "E.Acesso IN (-1)";
                filtros.Add(filtroAcesso);
            }

            // 2. Filtros de Texto
            filtros.AddRange(FiltroTexto("E.Familia", ObterSelecionados(flpFamilia)));
            filtros.AddRange(FiltroTexto("E.Modelo", ObterSelecionados(flpModelo)));
            filtros.AddRange(FiltroTexto("E.Tamanho", ObterSelecionados(flpTamanho)));

            // 3. MONTAGEM COM JOIN (Para trazer Nome e Cor da Função)
            // Usamos STRING_AGG para que, se um EPI tiver várias funções, ele não duplique a linha
            string sqlBase = @"
        SELECT 
            E.Modelo, 
            E.Tamanho, 
            E.Quantidade,
            ISNULL(STRING_AGG(F.Nome, ' | '), 'Sem Função') AS NomeFuncao,
            ISNULL(STRING_AGG(F.CorHex, ','), '#808080') AS CorFuncao
        FROM EPI E
        LEFT JOIN AcessoFuncoes AF ON E.Acesso = AF.AcessoID
        LEFT JOIN Funcoes F ON AF.FuncaoID = F.ID";

            string agrupamento = " GROUP BY E.Modelo, E.Tamanho, E.Quantidade, E.Acesso";

            if (filtros.Count > 0)
            {
                return $"{sqlBase} WHERE {string.Join(" AND ", filtros)} {agrupamento}";
            }

            return sqlBase + agrupamento;
        }

        private List<string> FiltroTexto(string coluna, List<string> valores)
        {
            if (valores != null && valores.Any())
            {
                // O Replace("'", "''") evita erros de SQL Injection e nomes com aspas
                var valoresFormatados = valores.Select(x => $"'{x.Replace("'", "''")}'");
                string filtro = $"{coluna} IN ({string.Join(",", valoresFormatados)})";
                return new List<string> { filtro };
            }
            return new List<string>();
        }

        private List<int> ObterAcessoIDs(List<string> funcoes)
        {
            List<int> ids = new List<int>();
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                foreach (string nome in funcoes)
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT af.AcessoID FROM Funcoes f JOIN AcessoFuncoes af ON f.ID = af.FuncaoID WHERE f.Nome = @nome", conn);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read()) ids.Add(rdr.GetInt32(0));
                }
            }
            return ids.Distinct().ToList();
        }

        private void btnTestar_Click(object sender, EventArgs e)
        {
            string query = GerarQuery();
            if (string.IsNullOrWhiteSpace(query)) return;
            try
            {
                DataTable resultado = new DataTable();
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(resultado);
                }

                dgvPreview.DataSource = resultado;

                // ESCONDER A COLUNA DA COR (Obrigatório para o visual)
                if (dgvPreview.Columns.Contains("CorFuncao"))
                    dgvPreview.Columns["CorFuncao"].Visible = false;

                // Ligar o evento de pintura caso não tenhas feito no Designer
                dgvPreview.CellFormatting -= dgvPreview_CellFormatting;
                dgvPreview.CellFormatting += dgvPreview_CellFormatting;
            }
            catch (Exception ex) { MessageBox.Show("Erro ao testar filtro:\n" + ex.Message); }
        }

        // ==========================================
        // LÓGICA DAS VISÕES E LAYOUT (CORRIGIDA)
        // ==========================================
        private void CarregarVisoes()
        {
            cmbVisaoNome.Items.Clear();
            cmbVisaoNome.Items.Add("+ Nova Visão");

            int visoesReais = 0;
            using (SqlConnection conn = GetConn.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Nome FROM Query ORDER BY Nome", conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbVisaoNome.Items.Add(reader.GetString(0));
                            visoesReais++;
                        }
                    }
                }
                catch { }
            }

            if (visoesReais == 0)
                MostrarModoEscrita();
            else
            {
                MostrarModoSelecao();
                cmbVisaoNome.SelectedIndex = 1;
            }
        }

        private void MostrarModoEscrita()
        {
            tlpControlos.SuspendLayout();

            // Forçar a coluna 0 (Combo) a desaparecer
            tlpControlos.ColumnStyles[0].SizeType = SizeType.Absolute;
            tlpControlos.ColumnStyles[0].Width = 0;

            // Forçar a coluna 1 (TextBox) a crescer para 40% (20% original + 20% da combo)
            tlpControlos.ColumnStyles[1].SizeType = SizeType.Percent;
            tlpControlos.ColumnStyles[1].Width = 40;

            txtNome.Visible = true;
            cmbVisaoNome.Visible = false;

            txtNome.Text = "";
            txtNome.PlaceholderText = "Nome da nova visão...";
            txtNome.Focus();

            tlpControlos.ResumeLayout();
        }

        private void MostrarModoSelecao()
        {
            tlpControlos.SuspendLayout();

            // Restaurar Coluna 0 (Combo)
            tlpControlos.ColumnStyles[0].SizeType = SizeType.Percent;
            tlpControlos.ColumnStyles[0].Width = 20;

            // Esconder Coluna 1 (TextBox)
            tlpControlos.ColumnStyles[1].SizeType = SizeType.Absolute;
            tlpControlos.ColumnStyles[1].Width = 0;

            txtNome.Visible = false;
            cmbVisaoNome.Visible = true;

            tlpControlos.ResumeLayout();
        }

        private void cmbVisaoNome_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVisaoNome.Text == "+ Nova Visão")
            {
                MostrarModoEscrita();
            }
            else
            {
                txtNome.Text = cmbVisaoNome.Text;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // O txtNome é o mestre, quer venha da Combo ou escrito à mão
            string nome = txtNome.Text.Trim();
            string query = GerarQuery();

            if (string.IsNullOrWhiteSpace(nome) || nome == "+ Nova Visão")
            {
                MessageBox.Show("Por favor, introduza um nome válido para a visão.", "PEPIDI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Se não selecionou nada, a query será o SELECT geral, convém avisar
            if (!ObterSelecionados(flpFuncoes).Any() && !ObterSelecionados(flpFamilia).Any() &&
                !ObterSelecionados(flpModelo).Any() && !ObterSelecionados(flpTamanho).Any())
            {
                if (MessageBox.Show("Não selecionou nenhum filtro. Deseja guardar uma visão geral?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            try
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();
                    // 1. Verificar se já existe para decidir entre UPDATE ou INSERT
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Query WHERE Nome = @nome", conn);
                    checkCmd.Parameters.AddWithValue("@nome", nome);
                    int count = (int)checkCmd.ExecuteScalar();

                    string sql = count > 0
                        ? "UPDATE Query SET Query = @query WHERE Nome = @nome"
                        : "INSERT INTO Query (Nome, Query) VALUES (@nome, @query)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@query", query);
                        cmd.ExecuteNonQuery();
                    }
                }

                M.AbrirMensagem("Visão '" + nome + "' guardada com sucesso!", "");

                // 2. Refresh total
                CarregarVisoes();

                // 3. Voltar ao modo de seleção para o utilizador ver o que acabou de fazer
                MostrarModoSelecao();
                cmbVisaoNome.Text = nome;
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao guardar na base de dados: " + ex.Message, "Erro Crítico");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            if (string.IsNullOrEmpty(nome)) return;

            if (MessageBox.Show($"Eliminar '{nome}'?", "Aviso", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Query WHERE Nome = @nome", conn);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.ExecuteNonQuery();
                }
                CarregarVisoes();
            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }

        private void dgvPreview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Se a coluna for a da Função e tiver dados
            if (dgvPreview.Columns[e.ColumnIndex].Name == "NomeFuncao" && e.Value != null)
            {
                // Vamos buscar o código Hex que a Query injetou na coluna CorFuncao
                // (Certifica-te que a coluna CorFuncao existe no DataTable)
                string hex = dgvPreview.Rows[e.RowIndex].Cells["CorFuncao"].Value?.ToString();

                if (!string.IsNullOrEmpty(hex))
                {
                    try
                    {
                        // Se houver várias cores, pegamos na primeira
                        string primeiraCor = hex.Split(',')[0].Trim();
                        Color corBack = ColorTranslator.FromHtml(primeiraCor);

                        e.CellStyle.BackColor = corBack;
                        e.CellStyle.SelectionBackColor = corBack;

                        // Contraste automático (Preto ou Branco)
                        double luma = (0.299 * corBack.R + 0.587 * corBack.G + 0.114 * corBack.B) / 255;
                        e.CellStyle.ForeColor = luma > 0.5 ? Color.Black : Color.White;
                    }
                    catch { /* Cor mal formatada na BD */ }
                }
            }
        }
    }
}