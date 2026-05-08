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
        private void btnImportar_Click(object sender, EventArgs e)
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
            {
                ProcessarImportacao(funcoesSelecionadas);
            }
        }

        private void ProcessarImportacao(List<string> funcoes)
        {
            try
            {
                int acessoID = ObterOuCriarAcessoID(funcoes);
                int estadoPadrao = 1; // "Novo"
                int countSucesso = 0;

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataGridViewRow row in dgvImport.Rows)
                            {
                                if (row.IsNewRow) continue;

                                string codigo = row.Cells["Codigo"].Value?.ToString();
                                string modelo = row.Cells["Modelo"].Value?.ToString();
                                string familia = row.Cells["Familia"].Value?.ToString();
                                string tamanho = row.Cells["Tamanho"].Value?.ToString();
                                int quant = Convert.ToInt32(row.Cells["Quantidade"].Value ?? 0);
                                var idCor = row.Cells["Cor"].Value; // Pode ser null

                                if (string.IsNullOrEmpty(codigo) || familia == "Null" || familia == "") continue;

                                // A. Garantir que o EPI existe (Tabela EPI)
                                string cmdEpi = @"
                                    IF NOT EXISTS (SELECT 1 FROM EPI WHERE Codigo = @cod)
                                    INSERT INTO EPI (Codigo, Familia, Modelo, Tamanho, Cor, Acesso, Ativo) 
                                    VALUES (@cod, @fam, @mod, @tam, @cor, @acc, 1)";

                                using (SqlCommand cmd = new SqlCommand(cmdEpi, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@cod", codigo);
                                    cmd.Parameters.AddWithValue("@fam", familia);
                                    cmd.Parameters.AddWithValue("@mod", modelo);
                                    cmd.Parameters.AddWithValue("@tam", tamanho ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@cor", idCor ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("@acc", acessoID);
                                    cmd.ExecuteNonQuery();
                                }

                                // B. Atualizar Stock (Tabela Stock)
                                string cmdStock = @"
                                    IF EXISTS (SELECT 1 FROM Stock WHERE Codigo = @cod AND Estado = @est)
                                        UPDATE Stock SET Quant = Quant + @q WHERE Codigo = @cod AND Estado = @est
                                    ELSE
                                        INSERT INTO Stock (Codigo, Estado, Quant) VALUES (@cod, @est, @q)";

                                using (SqlCommand cmd = new SqlCommand(cmdStock, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@cod", codigo);
                                    cmd.Parameters.AddWithValue("@est", estadoPadrao);
                                    cmd.Parameters.AddWithValue("@q", quant);
                                    cmd.ExecuteNonQuery();
                                }

                                // C. Ensinar IA
                                string keyword = modelo.Split(' ')[0].ToLower();
                                MotorIA.AprenderNovaRegra(keyword, familia, "Familia");

                                countSucesso++;
                            }

                            trans.Commit();
                            MotorIA.CarregarRegrasDaBD(); // Recarrega cache da IA
                            MessageBox.Show($"{countSucesso} artigos importados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro Crítico: " + ex.Message); }
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