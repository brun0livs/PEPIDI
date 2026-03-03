using Guna.UI2.WinForms;
using PEPIDI.Models; // Confirma se o GetConn está neste namespace ou noutro
using PEPIDI.Organizers;
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
            await CarregarRegrasFamiliaAsync(); // Carrega o "cérebro" das famílias
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

            // Ativar evento de teclas para apanhar o Ctrl+V
            dgvImport.KeyDown += DgvImport_KeyDown;
        }

        private void DgvImport_KeyDown(object sender, KeyEventArgs e)
        {
            // O SEGREDINHO: Colar do Excel!
            if (e.Control && e.KeyCode == Keys.V)
            {
                try
                {
                    string textoClipboard = Clipboard.GetText();
                    string[] linhas = textoClipboard.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    int linhaAtual = dgvImport.CurrentRow != null ? dgvImport.CurrentRow.Index : 0;

                    // Suspendemos o desenho da grelha para colar instantaneamente (Super rápido!)
                    dgvImport.SuspendLayout();

                    foreach (string linha in linhas)
                    {
                        string[] celulas = linha.Split('\t');

                        if (linhaAtual >= dgvImport.Rows.Count - 1)
                            dgvImport.Rows.Add();

                        // 1. Colar os dados brutos (Modelo, Tamanho, Quantidade)
                        for (int i = 0; i < celulas.Length && i < 3; i++) // Limitamos a 3 para não pisar a Família sem querer
                        {
                            dgvImport[i, linhaAtual].Value = celulas[i].Trim();
                        }

                        // 2. MAGIA DA INTELIGÊNCIA ARTIFICIAL
                        string modeloColado = dgvImport["Modelo", linhaAtual].Value?.ToString() ?? "";
                        dgvImport["Familia", linhaAtual].Value = DetetarFamilia(modeloColado);

                        linhaAtual++;
                    }

                    dgvImport.ResumeLayout();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao colar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================
        // 2. O MOTOR INTELIGENTE 🧠
        // ==========================================
        private async Task CarregarRegrasFamiliaAsync()
        {
            regrasFamilia.Clear();
            await Task.Run(() =>
            {
                using (SqlConnection conn = GetConn.GetConnection())
                {
                    // Lemos a tabela que criaste. ORDER BY ID é crucial para regras prioritárias!
                    using (SqlCommand cmd = new SqlCommand("SELECT PalavraChave, FamiliaDestino FROM RegrasFamilia ORDER BY ID", conn))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string palavra = reader.GetString(0).ToLower().Trim();
                                    string destino = reader.GetString(1).Trim();

                                    if (!regrasFamilia.ContainsKey(palavra))
                                    {
                                        regrasFamilia.Add(palavra, destino);
                                    }
                                }
                            }
                        }
                        catch { /* Ignora se a tabela ainda não existir para não crashar a app */ }
                    }
                }
            });
        }

        private string DetetarFamilia(string modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo)) return "Null";

            string m = modelo.ToLower();

            foreach (var regra in regrasFamilia)
            {
                if (m.Contains(regra.Key))
                {
                    return regra.Value;
                }
            }

            return "Null";
        }

        // ==========================================
        // 3. CARREGAR AS FUNÇÕES AUTORIZADAS (AS TAGS)
        // ==========================================
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
                MessageBox.Show("Tens de selecionar pelo menos uma Função Autorizada que se aplique a este lote!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Limpar linhas vazias da Grelha
            List<DataGridViewRow> linhasValidas = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvImport.Rows)
            {
                if (!row.IsNewRow && row.Cells["Familia"].Value != null && row.Cells["Modelo"].Value != null)
                {
                    linhasValidas.Add(row);
                }
            }

            if (linhasValidas.Count == 0)
            {
                MessageBox.Show("A tabela está vazia. Adiciona ou cola artigos do Excel antes de importar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. PERGUNTA AO UTILIZADOR COMO TRATAR OS DUPLICADOS
            DialogResult resposta = MessageBox.Show(
                "Se algum destes artigos já existir no armazém, o que pretendes fazer?\n\n" +
                "[ SIM ] - Somar as quantidades ao stock já existente (Recomendado)\n" +
                "[ NÃO ] - Criar linhas repetidas na base de dados separadamente\n" +
                "[ CANCELAR ] - Abortar a importação",
                "Tratamento de Duplicados",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (resposta == DialogResult.Cancel) return; // Aborta logo aqui

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
                            // A QUERY MUDA CONFORME A RESPOSTA DO UTILIZADOR
                            string query = "";
                            if (somarAosExistentes)
                            {
                                query = @"IF EXISTS (SELECT 1 FROM EPI WHERE Modelo = @Modelo AND Tamanho = @Tamanho AND Acesso = @Acesso)
                                        UPDATE EPI SET Quantidade = Quantidade + @Quantidade WHERE Modelo = @Modelo AND Tamanho = @Tamanho AND Acesso = @Acesso
                                        ELSE
                                        INSERT INTO EPI (Familia, Modelo, Tamanho, Acesso, Quantidade) VALUES (@Familia, @Modelo, @Tamanho, @Acesso, @Quantidade)";
                            }
                            else
                            {
                                query = @"INSERT INTO EPI (Familia, Modelo, Tamanho, Acesso, Quantidade) VALUES (@Familia, @Modelo, @Tamanho, @Acesso, @Quantidade)";
                            }

                            using (SqlCommand cmd = new SqlCommand(query, conn, trans))
                            {
                                cmd.Parameters.Add("@Familia", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@Modelo", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@Tamanho", SqlDbType.NVarChar);
                                cmd.Parameters.Add("@Acesso", SqlDbType.Int);
                                cmd.Parameters.Add("@Quantidade", SqlDbType.Int);

                                foreach (DataGridViewRow row in linhasValidas)
                                {
                                    string familia = row.Cells["Familia"].Value?.ToString() ?? "";
                                    string modelo = row.Cells["Modelo"].Value?.ToString() ?? "";
                                    string tamanho = row.Cells["Tamanho"].Value?.ToString() ?? "";

                                    int quantidade = 0;
                                    int.TryParse(row.Cells["Quantidade"].Value?.ToString(), out quantidade);

                                    cmd.Parameters["@Familia"].Value = familia;
                                    cmd.Parameters["@Modelo"].Value = modelo;
                                    cmd.Parameters["@Tamanho"].Value = tamanho;
                                    cmd.Parameters["@Acesso"].Value = acessoID;
                                    cmd.Parameters["@Quantidade"].Value = quantidade;

                                    cmd.ExecuteNonQuery();
                                    countSucesso++;
                                }
                            }

                            trans.Commit();
                            MessageBox.Show($"{countSucesso} artigos importados com sucesso para a Base de Dados!", "Importação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception exTrans)
                        {
                            trans.Rollback();
                            MessageBox.Show("Erro durante a importação. Nenhuma alteração foi guardada.\n" + exTrans.Message, "Erro SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro fatal: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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