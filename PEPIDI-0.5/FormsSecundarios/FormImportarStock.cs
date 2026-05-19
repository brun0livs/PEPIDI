using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.FormsSecundarios
{
    public partial class FormImportarStock : Form
    {
        // Cache local para a IA
        private Dictionary<string, string> regrasFamilia = new Dictionary<string, string>();

        public FormImportarStock()
        {
            InitializeComponent();
            ConfigurarGrelha();

            // Otimização de performance para evitar flickering
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvImport, new object[] { true });
        }

        private async void FormImportarStock_Load(object sender, EventArgs e)
        {
            await CarregarFuncoesAsync();
            await Task.Run(() => MotorIA.CarregarRegrasDaBD());
            GestorTema.AplicarEstilos(this);
        }

        // ==========================================
        // 1. CONFIGURAR A GRELHA
        // ==========================================
        private void ConfigurarGrelha()
        {
            dgvImport.Columns.Clear();
            dgvImport.Columns.Add("Codigo", "Código/Ref");
            dgvImport.Columns.Add("Modelo", "Modelo / Artigo");
            dgvImport.Columns.Add("Tamanho", "Tamanho");
            dgvImport.Columns.Add("Quantidade", "Quant.");
            dgvImport.Columns.Add("Familia", "Família (IA)");

            // --- Inserção da Coluna de Cores (ComboBox) ---
            DataGridViewComboBoxColumn colCor = new DataGridViewComboBoxColumn();
            colCor.Name = "Cor";
            colCor.HeaderText = "Cor";
            colCor.DataSource = ObterCoresBD();
            colCor.DisplayMember = "Nome"; // O que o utilizador vê
            colCor.ValueMember = "ID";     // O ID que gravamos na BD
            colCor.FlatStyle = FlatStyle.Flat;
            dgvImport.Columns.Add(colCor);

            // Estética da Família IA
            dgvImport.Columns["Familia"].DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvImport.Columns["Familia"].DefaultCellStyle.ForeColor = Color.FromArgb(242, 103, 34);
            dgvImport.Columns["Familia"].DefaultCellStyle.Font = new Font("Roboto", 11F, FontStyle.Bold);

            dgvImport.Columns["Modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // ==========================================
        // 2. COLAR DO EXCEL E INTELIGÊNCIA ARTIFICIAL
        // ==========================================
        private void DgvImport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                try
                {
                    string[] linhas = Clipboard.GetText().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int linhaAtual = dgvImport.CurrentRow?.Index ?? 0;
                    dgvImport.SuspendLayout();

                    foreach (string linha in linhas)
                    {
                        string[] celulas = linha.Split('\t');
                        if (linhaAtual >= dgvImport.Rows.Count - 1) dgvImport.Rows.Add();

                        // DENTRO DO FOREACH DAS LINHAS NO FormImportarStock.cs

                        // Assumimos que no Excel copias APENAS: Modelo | Tamanho | Quantidade
                        string modeloColado = celulas.Length > 0 ? celulas[0].Trim() : "";
                        string tamanhoColado = celulas.Length > 1 ? celulas[1].Trim() : "";
                        string quantColada = celulas.Length > 2 ? celulas[2].Trim() : "1";

                        // 1. O Código fica vazio (será gerado automaticamente depois na gravação)
                        dgvImport["Codigo", linhaAtual].Value = "";

                        // 2. Colocamos os dados nas colunas certas!
                        dgvImport["Modelo", linhaAtual].Value = modeloColado;
                        dgvImport["Tamanho", linhaAtual].Value = tamanhoColado;
                        dgvImport["Quantidade", linhaAtual].Value = quantColada;

                        // --- IA: Detetar Família ---
                        string familiaIA = MotorIA.CorrigirFamilia(modeloColado);
                        if (familiaIA == "Null")
                        {
                            string primeiraPalavra = modeloColado.Split(' ')[0];
                            familiaIA = ProcurarSugestaoNasLinhasAcima(primeiraPalavra, linhaAtual);
                        }

                        dgvImport["Familia", linhaAtual].Value = familiaIA;
                        if (familiaIA == "Null")
                            dgvImport["Familia", linhaAtual].Style.BackColor = Color.LightGoldenrodYellow;

                        // --- IA: Detetar Cor ---
                        string sugestaoCor = MotorIA.ExtrairCorDoModelo(modeloColado);
                        if (!string.IsNullOrEmpty(sugestaoCor))
                        {
                            DataGridViewComboBoxColumn comboCol = (DataGridViewComboBoxColumn)dgvImport.Columns["Cor"];
                            var dtCores = (DataTable)comboCol.DataSource;

                            DataRow[] rows = dtCores.Select($"Nome = '{sugestaoCor}'");
                            if (rows.Length > 0)
                            {
                                dgvImport["Cor", linhaAtual].Value = rows[0]["ID"];
                            }
                        }

                        linhaAtual++;
                    }
                    dgvImport.ResumeLayout();
                }
                catch (Exception ex) { MessageBox.Show("Erro ao colar dados: " + ex.Message); }
            }
        }

        private string ProcurarSugestaoNasLinhasAcima(string keyword, int linhaLimite)
        {
            for (int i = 0; i < linhaLimite; i++)
            {
                string modAnterior = dgvImport["Modelo", i].Value?.ToString() ?? "";
                string famAnterior = dgvImport["Familia", i].Value?.ToString() ?? "";

                if (modAnterior.ToLower().Contains(keyword.ToLower()) && famAnterior != "Null")
                {
                    return famAnterior;
                }
            }
            return "Null";
        }

        // ==========================================
        // 3. GRAVAÇÃO EM MASSA (BULK INSERT EPI + STOCK)
        // ==========================================
        private async void btnImportar_Click(object sender, EventArgs e)
        {
            List<string> funcoesSelecionadas = flpFuncoes.Controls.OfType<Guna2Button>()
                .Where(btn => btn.Tag is bool isLigado && isLigado)
                .Select(btn => btn.Text).ToList();

            if (funcoesSelecionadas.Count == 0)
            {
                MessageBox.Show("Tens de selecionar pelo menos uma Função!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resposta = MessageBox.Show(
                "Desejas importar estes artigos para o Stock?",
                "Confirmação de Importação",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
                await ProcessarImportacao(funcoesSelecionadas);
        }

        private async Task ProcessarImportacao(List<string> funcoes)
        {
            try
            {
                int acessoID = ObterOuCriarAcessoID(funcoes);
                int estadoPadrao = 1;
                int countSucesso = 0;

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    await conn.OpenAsync();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataGridViewRow row in dgvImport.Rows)
                            {
                                if (row.IsNewRow) continue;

                                string modelo = row.Cells["Modelo"].Value?.ToString();
                                string familia = row.Cells["Familia"].Value?.ToString();
                                string tamanho = row.Cells["Tamanho"].Value?.ToString();
                                int quant = Convert.ToInt32(row.Cells["Quantidade"].Value ?? 0);
                                string idCor = row.Cells["Cor"].Value?.ToString() ?? "00";

                                // Linhas inválidas: sem modelo ou família por resolver
                                if (string.IsNullOrEmpty(modelo) || familia == "Null" || familia == "") continue;

                                // ─── GERAÇÃO AUTOMÁTICA DO CÓDIGO ───────────────────────────
                                string codigo = row.Cells["Codigo"].Value?.ToString();
                                if (string.IsNullOrEmpty(codigo))
                                    codigo = await GerarCodigoEPIAsync(conn, trans, familia, modelo, tamanho, idCor);
                                // ─────────────────────────────────────────────────────────────

                                // A. Garantir que o EPI existe na tabela EPI
                                string sqlEpi = @"
                            IF NOT EXISTS (SELECT 1 FROM EPI WHERE Codigo = @cod)
                                INSERT INTO EPI (Codigo, Familia, Modelo, Tamanho, Cor, Acesso, Ativo)
                                VALUES (@cod, @fam, @mod, @tam, @cor, @acc, 1)";

                                using (SqlCommand cmd = new SqlCommand(sqlEpi, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@cod", codigo);
                                    cmd.Parameters.AddWithValue("@fam", familia);
                                    cmd.Parameters.AddWithValue("@mod", modelo);
                                    cmd.Parameters.AddWithValue("@tam", (object)tamanho ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@cor", (idCor == "00" || string.IsNullOrEmpty(idCor))
                                                                            ? (object)DBNull.Value : idCor);
                                    cmd.Parameters.AddWithValue("@acc", acessoID);
                                    await cmd.ExecuteNonQueryAsync();
                                }

                                // B. Atualizar/Inserir Stock
                                string sqlStock = @"
                            IF EXISTS (SELECT 1 FROM Stock WHERE Codigo = @cod AND Estado = @est)
                                UPDATE Stock SET Quant = Quant + @q WHERE Codigo = @cod AND Estado = @est
                            ELSE
                                INSERT INTO Stock (Codigo, Estado, Quant) VALUES (@cod, @est, @q)";

                                using (SqlCommand cmd = new SqlCommand(sqlStock, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@cod", codigo);
                                    cmd.Parameters.AddWithValue("@est", estadoPadrao);
                                    cmd.Parameters.AddWithValue("@q", quant);
                                    await cmd.ExecuteNonQueryAsync();
                                }

                                // C. Ensinar a IA com o modelo desta linha
                                MotorIA.AprenderNovaRegra(modelo.Split(' ')[0].ToLower(), familia, "Familia");

                                countSucesso++;
                            }

                            trans.Commit();
                            MotorIA.CarregarRegrasDaBD();
                            MessageBox.Show($"{countSucesso} artigos importados com sucesso!", "Sucesso",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro Crítico: " + ex.Message); }
        }

        // ==========================================
        // GERAÇÃO DE CÓDIGO (portado do UC Artigo)
        // ==========================================

        private async Task<string> GerarCodigoEPIAsync(SqlConnection conn, SqlTransaction tran,
            string familia, string modelo, string tamanho, string idCor)
        {
            string idFam = familia switch
            {
                "TShirt" => "1",
                "PoloMCurta" => "2",
                "PoloMCompr" => "3",
                "Casaco" => "4",
                "Bata" => "5",
                "Calca" => "6",
                "Sapato" => "7",
                _ => "9"
            };

            string idMod = await ObterProximoIdModeloAsync(conn, tran, familia, modelo);
            string idC = (string.IsNullOrEmpty(idCor) || idCor == "00") ? "00" : idCor.PadLeft(2, '0');
            string idTam = CalcularIdTamanho(tamanho);

            return $"{idFam}{idMod}{idC}{idTam}";
        }

        private async Task<string> ObterProximoIdModeloAsync(SqlConnection conn, SqlTransaction tran,
            string familia, string modelo)
        {
            // 1. Se o modelo já existe, reutiliza o mesmo ID de modelo
            string sqlCheck = @"SELECT TOP 1 SUBSTRING(CAST(Codigo AS VARCHAR(20)), 2, 2) 
                        FROM EPI 
                        WHERE Modelo = @m AND LEN(CAST(Codigo AS VARCHAR(20))) >= 7";

            using (SqlCommand cmd = new SqlCommand(sqlCheck, conn, tran))
            {
                cmd.Parameters.AddWithValue("@m", modelo);
                object res = await cmd.ExecuteScalarAsync();
                if (res != null && res != DBNull.Value) return res.ToString();
            }

            // 2. Novo modelo → próximo ID disponível dentro da família
            string sqlMax = @"SELECT MAX(TRY_CAST(SUBSTRING(CAST(Codigo AS VARCHAR(20)), 2, 2) AS INT)) 
                      FROM EPI 
                      WHERE Familia = @f AND LEN(CAST(Codigo AS VARCHAR(20))) >= 7";

            using (SqlCommand cmd = new SqlCommand(sqlMax, conn, tran))
            {
                cmd.Parameters.AddWithValue("@f", familia);
                object resMax = await cmd.ExecuteScalarAsync();
                if (resMax != null && resMax != DBNull.Value)
                    return (Convert.ToInt32(resMax) + 1).ToString("D2");
            }

            return "01"; // Primeira entrada da família
        }

        private string CalcularIdTamanho(string tamanho)
        {
            var letras = new Dictionary<string, string>
    {
        {"XXS","01"}, {"XS","02"}, {"S","03"}, {"M","04"},
        {"L","05"}, {"XL","06"}, {"XXL","07"}, {"XXXL","08"}, {"3XL","08"}
    };

            if (letras.TryGetValue(tamanho ?? "", out string id)) return id;
            if (int.TryParse(tamanho, out int num)) return (num - 27).ToString("D2");
            return "00";
        }

        // ==========================================
        // 4. MÉTODOS AUXILIARES E EVENTOS
        // ==========================================
        private DataTable ObterCoresBD()
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                string query = "SELECT ID, Nome FROM Cor ORDER BY Nome";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void dgvImport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string nomeColuna = dgvImport.Columns[e.ColumnIndex].Name;

            if (nomeColuna == "Familia" || nomeColuna == "Cor")
            {
                string colunaChave = "Modelo";

                var valorChaveOriginal = dgvImport.Rows[e.RowIndex].Cells[colunaChave].Value?.ToString();
                var novoValorAtribuido = dgvImport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                if (string.IsNullOrEmpty(valorChaveOriginal)) return;

                dgvImport.CellValueChanged -= dgvImport_CellValueChanged;

                foreach (DataGridViewRow row in dgvImport.Rows)
                {
                    if (row.Index != e.RowIndex && row.Cells[colunaChave].Value?.ToString() == valorChaveOriginal)
                    {
                        row.Cells[e.ColumnIndex].Value = novoValorAtribuido;
                        row.Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    }
                }

                // Voltar a ligar o evento (estava em falta no teu bloco original)
                dgvImport.CellValueChanged += dgvImport_CellValueChanged;
            }
        }

        private async Task CarregarFuncoesAsync()
        {
            flpFuncoes.SuspendLayout();
            flpFuncoes.Controls.Clear();

            List<string> listaFuncoes = new List<string>();

            await Task.Run(() =>
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Nome FROM Funcoes ORDER BY Nome", conn))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                                while (reader.Read()) listaFuncoes.Add(reader["Nome"].ToString().Trim());
                        }
                        catch { }
                    }
                }
            });

            foreach (string nomeFuncao in listaFuncoes)
            {
                Guna2Button tag = new Guna2Button();
                tag.Text = nomeFuncao;
                tag.Font = new Font("Roboto", 9F, FontStyle.Regular);
                tag.Height = 35;
                tag.Width = TextRenderer.MeasureText(nomeFuncao, tag.Font).Width + 30;
                tag.BorderRadius = 15;
                tag.Cursor = Cursors.Hand;
                tag.Animated = true;
                tag.FillColor = Color.FromArgb(230, 232, 235);
                tag.ForeColor = Color.FromArgb(64, 64, 64);
                tag.Margin = new Padding(3);
                tag.Tag = false;

                tag.Click += (s, ev) =>
                {
                    bool isLigado = (bool)tag.Tag;
                    if (!isLigado)
                    {
                        tag.FillColor = Color.FromArgb(242, 103, 34);
                        tag.ForeColor = Color.White;
                        tag.Tag = true;
                    }
                    else
                    {
                        tag.FillColor = Color.FromArgb(230, 232, 235);
                        tag.ForeColor = Color.FromArgb(64, 64, 64);
                        tag.Tag = false;
                    }
                };

                tag.CreateControl();
                flpFuncoes.Controls.Add(tag);
            }

            flpFuncoes.ResumeLayout(true);
        }

        private int ObterOuCriarAcessoID(List<string> funcoesSelecionadas)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                SqlCommand cmdIDs = new SqlCommand("SELECT DISTINCT AcessoID FROM AcessoFuncoes", conn);
                List<int> ids = new List<int>();
                using (SqlDataReader rdr = cmdIDs.ExecuteReader())
                {
                    while (rdr.Read()) ids.Add(rdr.GetInt32(0));
                }

                foreach (int id in ids)
                {
                    SqlCommand cmdFuncoes = new SqlCommand("SELECT Nome FROM AcessoFuncoes INNER JOIN Funcoes ON Funcoes.ID = AcessoFuncoes.FuncaoID WHERE AcessoID = @id", conn);
                    cmdFuncoes.Parameters.AddWithValue("@id", id);

                    List<string> funcoesDoGrupo = new List<string>();
                    using (SqlDataReader rdr = cmdFuncoes.ExecuteReader())
                    {
                        while (rdr.Read()) funcoesDoGrupo.Add(rdr.GetString(0));
                    }

                    if (funcoesDoGrupo.Count == funcoesSelecionadas.Count && !funcoesDoGrupo.Except(funcoesSelecionadas).Any())
                    {
                        return id;
                    }
                }

                SqlCommand cmdNovoID = new SqlCommand("INSERT INTO Acessos DEFAULT VALUES; SELECT SCOPE_IDENTITY();", conn);
                int novoID = Convert.ToInt32(cmdNovoID.ExecuteScalar());

                foreach (string funcao in funcoesSelecionadas)
                {
                    SqlCommand cmdGetID = new SqlCommand("SELECT ID FROM Funcoes WHERE Nome = @nome", conn);
                    cmdGetID.Parameters.AddWithValue("@nome", funcao);
                    int idFunc = (int)cmdGetID.ExecuteScalar();

                    SqlCommand cmdInsert = new SqlCommand("INSERT INTO AcessoFuncoes (AcessoID, FuncaoID) VALUES (@acesso, @funcao)", conn);
                    cmdInsert.Parameters.AddWithValue("@acesso", novoID);
                    cmdInsert.Parameters.AddWithValue("@funcao", idFunc);
                    cmdInsert.ExecuteNonQuery();
                }

                return novoID;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}