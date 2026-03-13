using Guna.UI2.WinForms;
using PEPIDI.Models; // Assume que a GetConn está aqui
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

namespace PEPIDI.FormsSecundarios
{
    public partial class FormGestaoDeFiltros : Form
    {
        public FormGestaoDeFiltros()
        {
            InitializeComponent();

            // Prevenção de Flashes no ecrã (Double Buffering)
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
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

            // Desenhar os botões
            foreach (var f in funcoes) CriarTagFiltro(f, flpFuncoes);
            foreach (var fa in familias) CriarTagFiltro(fa, flpFamilia);
            foreach (var m in modelos) CriarTagFiltro(m, flpModelo);
            foreach (var t in tamanhos) CriarTagFiltro(t, flpTamanho);

            flpFuncoes.ResumeLayout(true);
            flpFamilia.ResumeLayout(true);
            flpModelo.ResumeLayout(true);
            flpTamanho.ResumeLayout(true);
        }

        // ==========================================
        // 2. DESIGN E LÓGICA DAS TAGS
        // ==========================================
        private void CriarTagFiltro(string texto, FlowLayoutPanel painel)
        {
            Guna2Button tag = new Guna2Button();
            tag.Text = texto;
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
            tag.Height = 35;

            // Força a Tag a esticar quase até à borda do painel
            int larguraPainel = painel.ClientSize.Width > 0 ? painel.ClientSize.Width : 200;
            tag.Width = larguraPainel - 25;

            tag.BorderRadius = 10;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;
            tag.TextAlign = HorizontalAlignment.Left;
            tag.TextOffset = new Point(10, 0);

            // Cores iniciais (Desligado)
            tag.FillColor = Color.White;
            tag.BorderColor = Color.FromArgb(200, 200, 200);
            tag.BorderThickness = 1;
            tag.ForeColor = Color.FromArgb(64, 64, 64);
            tag.Margin = new Padding(3, 3, 3, 3);
            tag.Tag = false; // "false" = não selecionado

            // Evento ao clicar
            tag.Click += (s, e) =>
            {
                bool isLigado = (bool)tag.Tag;
                if (!isLigado)
                {
                    // Ligado (Estilo Laranja)
                    tag.FillColor = Color.FromArgb(254, 235, 226);
                    tag.BorderColor = Color.FromArgb(254, 107, 0);
                    tag.ForeColor = Color.FromArgb(254, 107, 0);
                    tag.Font = new Font("Roboto", 10F, FontStyle.Bold);
                    tag.Tag = true;
                }
                else
                {
                    // Desligado
                    tag.FillColor = Color.White;
                    tag.BorderColor = Color.FromArgb(200, 200, 200);
                    tag.ForeColor = Color.FromArgb(64, 64, 64);
                    tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
                    tag.Tag = false;
                }
            };

            tag.CreateControl();
            painel.Controls.Add(tag);
        }

        private List<string> ObterSelecionados(FlowLayoutPanel painel)
        {
            return painel.Controls.OfType<Guna2Button>()
                         .Where(b => b.Tag is bool estado && estado)
                         .Select(b => b.Text).ToList();
        }

        // ==========================================
        // 3. MOTOR DE QUERY E BASE DE DADOS
        // ==========================================
        private string GerarQuery()
        {
            List<string> filtros = new List<string>();

            var funcoes = ObterSelecionados(flpFuncoes);
            if (funcoes.Any())
            {
                List<int> acessos = ObterAcessoIDs(funcoes);
                if (acessos.Any()) filtros.Add($"Acesso IN ({string.Join(",", acessos)})");
                else filtros.Add("Acesso IN (-1)");
            }

            filtros.AddRange(FiltroTexto("Familia", ObterSelecionados(flpFamilia)));
            filtros.AddRange(FiltroTexto("Modelo", ObterSelecionados(flpModelo)));
            filtros.AddRange(FiltroTexto("Tamanho", ObterSelecionados(flpTamanho)));

            return filtros.Count > 0
                ? "SELECT Modelo, Tamanho, Quantidade FROM EPI WHERE " + string.Join(" AND ", filtros)
                : "SELECT Modelo, Tamanho, Quantidade FROM EPI";
        }

        private List<string> FiltroTexto(string coluna, List<string> valores)
        {
            if (valores.Any())
            {
                string filtro = $"{coluna} IN ({string.Join(",", valores.Select(x => $"'{x}'"))})";
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

        // ==========================================
        // 4. BOTÕES E AÇÕES
        // ==========================================
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao testar filtro:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nome = cmbVisaoNome.Text.Trim();
            string query = GerarQuery();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Por favor, escreve ou seleciona um Nome para a Visão.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(query)) return;

            try
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Query WHERE Nome = @nome", conn);
                    checkCmd.Parameters.AddWithValue("@nome", nome);
                    int count = (int)checkCmd.ExecuteScalar();

                    SqlCommand cmd;
                    if (count > 0)
                        cmd = new SqlCommand("UPDATE Query SET Query = @query WHERE Nome = @nome", conn);
                    else
                        cmd = new SqlCommand("INSERT INTO Query (Nome, Query) VALUES (@nome, @query)", conn);

                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@query", query);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Visão guardada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarVisoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string nome = cmbVisaoNome.Text.Trim();
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Selecione a visão que pretende eliminar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Tem a certeza que pretende eliminar a visão '{nome}'?", "Confirmar Eliminação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Query WHERE Nome = @nome", conn);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Visão eliminada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarVisoes();
                cmbVisaoNome.Text = "";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("FK_DGVs_Query"))
                    MessageBox.Show("Não podes eliminar esta visão porque está a ser utilizada numa tabela principal.", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Erro ao eliminar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarVisoes()
        {
            cmbVisaoNome.Items.Clear();
            using (SqlConnection conn = GetConn.GetConnection())
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Nome FROM Query ORDER BY Nome", conn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) cmbVisaoNome.Items.Add(reader.GetString(0));
                    }
                }
                catch { /* Ignora se a base de dados falhar no Load */ }
            }
        }
    }
}