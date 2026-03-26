using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Models; // Confirma se o GetConn está neste namespace ou noutro
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
        // O nosso dicionário RAM para guardar as regras e não massacrar a Base de Dados
        private Dictionary<string, string> regrasFamilia = new Dictionary<string, string>();

        public FormImportarStock()
        {
            InitializeComponent();
            ConfigurarGrelha();

            // Ativar DoubleBuffered para a grelha não piscar quando colamos 100 linhas
            typeof(DataGridView).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, dgvImport, new object[] { true });
        }

        private async void FormImportarStock_Load(object sender, EventArgs e)
        {
            await CarregarFuncoesAsync();
            await Task.Run(() => MotorIA.CarregarRegrasDaBD());
            GestorTema.AplicarEstilos(this);
        }

        // ==========================================
        // 1. CONFIGURAR A GRELHA E O "COLAR DO EXCEL"
        // ==========================================
        private void ConfigurarGrelha()
        {
            dgvImport.Columns.Clear();
            dgvImport.Columns.Add("Modelo", "Modelo / Artigo");
            dgvImport.Columns.Add("Tamanho", "Tamanho");
            dgvImport.Columns.Add("Quantidade", "Quantidade");

            // Coluna Inteligente
            dgvImport.Columns.Add("Familia", "Família (Automática)");

            // Destacar a coluna automática a Laranja e Cinza claro
            dgvImport.Columns["Familia"].DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvImport.Columns["Familia"].DefaultCellStyle.ForeColor = Color.FromArgb(242, 103, 34);
            dgvImport.Columns["Familia"].DefaultCellStyle.Font = new Font("Roboto", 11F, FontStyle.Bold);

            // Ajustar larguras
            dgvImport.Columns["Modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvImport.Columns["Tamanho"].Width = 120;
            dgvImport.Columns["Quantidade"].Width = 120;
            dgvImport.Columns["Familia"].Width = 200;
        }

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

                        string modeloColado = celulas[0].Trim();
                        dgvImport["Modelo", linhaAtual].Value = modeloColado;
                        dgvImport["Tamanho", linhaAtual].Value = celulas.Length > 1 ? celulas[1].Trim() : "";
                        dgvImport["Quantidade", linhaAtual].Value = celulas.Length > 2 ? celulas[2].Trim() : "0";

                        // 1. TENTATIVA DE DETECÇÃO
                        string familiaIA = MotorIA.DetetarFamilia(modeloColado);

                        // 2. O TRUQUE: Se a IA não sabe ("Null"), mas o utilizador já preencheu 
                        // uma linha acima com o mesmo padrão, a IA aprende NA HORA.
                        if (familiaIA == "Null")
                        {
                            // Procura nas linhas acima se já existe um modelo que contenha a mesma palavra
                            string primeiraPalavra = modeloColado.Split(' ')[0];
                            familiaIA = ProcurarSugestaoNasLinhasAcima(primeiraPalavra, linhaAtual);
                        }

                        dgvImport["Familia", linhaAtual].Value = familiaIA;

                        if (familiaIA == "Null")
                            dgvImport["Familia", linhaAtual].Style.BackColor = Color.LightGoldenrodYellow;

                        linhaAtual++;
                    }
                    dgvImport.ResumeLayout();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        // Método auxiliar para a IA "olhar para cima" na grelha
        private string ProcurarSugestaoNasLinhasAcima(string keyword, int linhaLimite)
        {
            for (int i = 0; i < linhaLimite; i++)
            {
                string modAnterior = dgvImport["Modelo", i].Value?.ToString() ?? "";
                string famAnterior = dgvImport["Familia", i].Value?.ToString() ?? "";

                // Se a linha de cima tem a mesma palavra e já tem família, usamos essa!
                if (modAnterior.ToLower().Contains(keyword.ToLower()) && famAnterior != "Null")
                {
                    return famAnterior;
                }
            }
            return "Null";
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

        // ==========================================
        // 4. GRAVAÇÃO EM MASSA (BULK INSERT)
        // ==========================================
        private void btnImportar_Click(object sender, EventArgs e)
        {
            // 1. Validar as Funções
            List<string> funcoesSelecionadas = flpFuncoes.Controls.OfType<Guna2Button>()
                .Where(btn => btn.Tag is bool isLigado && isLigado)
                .Select(btn => btn.Text).ToList();

            if (funcoesSelecionadas.Count == 0)
            {
                MessageBox.Show("Tens de selecionar pelo menos uma Função!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Filtrar linhas válidas e ENSINAR A IA
            List<DataGridViewRow> linhasValidas = new List<DataGridViewRow>();

            // Lista temporária para não abrir 500 conexões à BD dentro do loop
            List<(string Key, string Fam)> novasRegras = new List<(string, string)>();

            foreach (DataGridViewRow row in dgvImport.Rows)
            {
                if (row.IsNewRow) continue;

                var cellModelo = row.Cells["Modelo"].Value;
                var cellFamilia = row.Cells["Familia"].Value;

                if (cellModelo != null && cellFamilia != null)
                {
                    string modelo = cellModelo.ToString();
                    string familiaFinal = cellFamilia.ToString();

                    if (familiaFinal != "Null" && familiaFinal != "")
                    {
                        linhasValidas.Add(row);

                        // Se a IA não conhecia (estava amarelo ou Tag era Null)
                        // Vamos preparar para ensinar
                        string keyword = modelo.Split(' ')[0].ToLower().Trim();
                        novasRegras.Add((keyword, familiaFinal));
                    }
                }
            }

            if (linhasValidas.Count == 0)
            {
                MessageBox.Show("Não existem linhas válidas com Família definida.");
                return;
            }

            // 3. Pergunta duplicados
            DialogResult resposta = MessageBox.Show(
                "Desejas somar as quantidades ao stock já existente?",
                "Tratamento de Duplicados",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (resposta == DialogResult.Cancel) return;
            bool somarAosExistentes = (resposta == DialogResult.Yes);

            // 4. Processar a Gravação
            try
            {
                int acessoID = ObterOuCriarAcessoID(funcoesSelecionadas);
                int countSucesso = 0;

                using (SqlConnection conn = GetConn.GetConnection())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // A. GUARDAR REGRAS NA BD (APRENDER)
                            foreach (var regra in novasRegras.Distinct())
                            {
                                // Chamamos o método que criámos no MotorIA
                                MotorIA.AprenderNovaRegra(regra.Key, regra.Fam, "Familia");
                            }

                            // B. GUARDAR STOCK
                            string query = somarAosExistentes
                                ? @"IF EXISTS (SELECT 1 FROM EPI WHERE Modelo = @Modelo AND Tamanho = @Tamanho AND Acesso = @Acesso)
                                UPDATE EPI SET Quantidade = Quantidade + @Quantidade 
                                WHERE Modelo = @Modelo AND Tamanho = @Tamanho AND Acesso = @Acesso
                            ELSE
                                INSERT INTO EPI (Familia, Modelo, Tamanho, Acesso, Quantidade) 
                                VALUES (@Familia, @Modelo, @Tamanho, @Acesso, @Quantidade)"
                                : @"INSERT INTO EPI (Familia, Modelo, Tamanho, Acesso, Quantidade) 
                            VALUES (@Familia, @Modelo, @Tamanho, @Acesso, @Quantidade)";

                            using (SqlCommand cmd = new SqlCommand(query, conn, trans))
                            {
                                cmd.Parameters.Add("@Familia", SqlDbType.NVarChar, 100);
                                cmd.Parameters.Add("@Modelo", SqlDbType.NVarChar, 100);
                                cmd.Parameters.Add("@Tamanho", SqlDbType.NVarChar, 50);
                                cmd.Parameters.Add("@Acesso", SqlDbType.Int);
                                cmd.Parameters.Add("@Quantidade", SqlDbType.Int);

                                foreach (DataGridViewRow row in linhasValidas)
                                {
                                    cmd.Parameters["@Familia"].Value = row.Cells["Familia"].Value;
                                    cmd.Parameters["@Modelo"].Value = row.Cells["Modelo"].Value;
                                    cmd.Parameters["@Tamanho"].Value = row.Cells["Tamanho"].Value;
                                    cmd.Parameters["@Acesso"].Value = acessoID;
                                    cmd.Parameters["@Quantidade"].Value = Convert.ToInt32(row.Cells["Quantidade"].Value ?? 0);

                                    cmd.ExecuteNonQuery();
                                    countSucesso++;
                                }
                            }

                            trans.Commit();

                            // RECARREGAR A RAM DA IA PARA A PRÓXIMA VEZ
                            MotorIA.CarregarRegrasDaBD();

                            MessageBox.Show($"{countSucesso} artigos importados e IA atualizada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("Erro na transação: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro Fatal: " + ex.Message); }
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

        private void dgvImport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Se o humano alterou a coluna "Familia"
            if (e.RowIndex >= 0 && dgvImport.Columns[e.ColumnIndex].Name == "Familia")
            {
                string novaFamilia = dgvImport[e.ColumnIndex, e.RowIndex].Value?.ToString();
                string modeloDestaLinha = dgvImport["Modelo", e.RowIndex].Value?.ToString() ?? "";

                if (string.IsNullOrEmpty(modeloDestaLinha) || novaFamilia == "Null") return;

                string keyword = modeloDestaLinha.Split(' ')[0]; // Ex: "T-Shirt"

                // PERGUNTA MÁGICA: "Queres aplicar esta família a todos os modelos parecidos?"
                // Ou faz automaticamente para as linhas que estão abaixo e estão vazias:
                for (int i = e.RowIndex + 1; i < dgvImport.Rows.Count; i++)
                {
                    string modeloAbaixo = dgvImport["Modelo", i].Value?.ToString() ?? "";
                    string familiaAbaixo = dgvImport["Familia", i].Value?.ToString() ?? "";

                    if (modeloAbaixo.ToLower().Contains(keyword.ToLower()) && (familiaAbaixo == "Null" || string.IsNullOrEmpty(familiaAbaixo)))
                    {
                        dgvImport["Familia", i].Value = novaFamilia;
                        dgvImport["Familia", i].Style.BackColor = Color.White; // Já não precisa de atenção
                    }
                }
            }
        }
    }
}