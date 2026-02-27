using Guna.UI2.WinForms;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class CriarStock : UserControl
    {
        public CriarStock()
        {
            // Para o motor de layout e o desenho
            this.SuspendLayout();
            InitializeComponent();

            // PREPARAÇÃO DO PAINEL DE TAGS
            flpFuncoes.AutoScroll = true;
            flpFuncoes.Padding = new Padding(0);
            flpFuncoes.Margin = new Padding(0);
            // Isto garante que o painel tenta não mostrar scrollbars se não for preciso
            flpFuncoes.WrapContents = true;
            // Ativa o DoubleBuffered em cascata (como fizemos antes)
            // Isso evita que o utilizador veja os controlos a serem "desenhados" um a um
            this.DoubleBuffered = true;
            HelperPerformance.AtivarDoubleBufferRecursivo(this);

            // Só agora é que o Windows calcula onde tudo fica
            this.ResumeLayout(true);
        }

        private async void CriarStock_Load(object sender, EventArgs e)
        {
            CarregarCombo(cmbFamilia);
            await CarregarFuncoesAsync(); // <-- AGORA É ASSÍNCRONO!
        }

        // ==========================================
        // 1. CARREGAMENTO DE DADOS UI
        // ==========================================
        private void CarregarCombo(Guna2ComboBox combo)
        {
            DataTable dtFamilias = new DataTable();
            dtFamilias.Columns.Add("ID", typeof(int));
            dtFamilias.Columns.Add("NomeFamilia", typeof(string));
            dtFamilias.Columns.Add("Apresentacao", typeof(string));

            dtFamilias.Rows.Add(0, "Null", "Familía...");
            dtFamilias.Rows.Add(1, "TShirt", "T-Shirt");
            dtFamilias.Rows.Add(2, "Casaco", "Casaco");
            dtFamilias.Rows.Add(3, "PoloMCurta", "Polo Manga Curta");
            dtFamilias.Rows.Add(4, "PoloMCompr", "Polo Manga Comprida");
            dtFamilias.Rows.Add(5, "Calca", "Calça");
            dtFamilias.Rows.Add(6, "Sapato", "Sapato");

            combo.DataSource = dtFamilias;
            combo.DisplayMember = "Apresentacao";
            combo.ValueMember = "NomeFamilia";

            combo.SelectedIndex = 0;
        }

        private async void CarregarModelos(string familia)
        {
            DataTable dtModelos = new DataTable();
            dtModelos.Columns.Add("Modelo", typeof(string));

            dtModelos.Rows.Add("Modelo...");
            dtModelos.Rows.Add("+ Escrever Novo Modelo...");

            string query = "SELECT DISTINCT Modelo FROM EPI WHERE Familia = @Familia AND Modelo IS NOT NULL AND Modelo <> ''";

            // Executa a query pesada em background para não bugar a ComboBox principal
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Familia", familia);
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    dtModelos.Rows.Add(reader["Modelo"].ToString().Trim());
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Erro silencioso na thread background para não rebentar o programa
                        }
                    }
                }
            });

            cmbModelo.DataSource = dtModelos;
            cmbModelo.DisplayMember = "Modelo";
            cmbModelo.ValueMember = "Modelo";

            cmbModelo.DropDownStyle = ComboBoxStyle.DropDownList;

            if (cmbModelo.Items.Count > 0)
                cmbModelo.SelectedIndex = 0;
        }

        private void CarregarTamanhos(string familia)
        {
            DataTable dtTamanhos = new DataTable();
            dtTamanhos.Columns.Add("Valor", typeof(string));
            dtTamanhos.Columns.Add("Apresentacao", typeof(string));

            dtTamanhos.Rows.Add("Null", "Tamanho...");

            if (familia == "Sapato")
            {
                for (int i = 35; i <= 48; i++)
                    dtTamanhos.Rows.Add(i.ToString(), i.ToString());
            }
            else if (familia == "Calca")
            {
                for (int i = 36; i <= 58; i += 2)
                    dtTamanhos.Rows.Add(i.ToString(), i.ToString());
            }
            else if (familia != "Null")
            {
                string[] tamanhosLetras = { "XS", "S", "M", "L", "XL", "XXL", "3XL", "4XL" };
                foreach (string tamanho in tamanhosLetras)
                    dtTamanhos.Rows.Add(tamanho, tamanho);
            }

            cmbTamanho.DataSource = dtTamanhos;
            cmbTamanho.DisplayMember = "Apresentacao";
            cmbTamanho.ValueMember = "Valor";

            cmbTamanho.SelectedIndex = 0;
        }

        // ==========================================
        // 2. FUNÇÕES AUTORIZADAS (TAGS) - O MOTOR V8! 🚀
        // ==========================================
        private async Task CarregarFuncoesAsync()
        {
            // Esconde e congela o painel enquanto desenha
            flpFuncoes.Visible = false;
            flpFuncoes.SuspendLayout();
            flpFuncoes.Controls.Clear();

            List<string> listaFuncoes = new List<string>();
            string query = "SELECT Nome FROM Funcoes ORDER BY Nome";

            // 1. Busca os nomes todos num abrir e piscar de olhos noutra thread
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    listaFuncoes.Add(reader["Nome"].ToString().Trim());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar as funções: " + ex.Message, "Erro SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            });

            // 2. Com todos os nomes já em RAM, cospe os botões para o ecrã à bruta!
            foreach (string nomeFuncao in listaFuncoes)
            {
                CriarTagFuncao(nomeFuncao);
            }

            // 3. Liberta a besta!
            flpFuncoes.ResumeLayout(true);
            flpFuncoes.Visible = true;
        }

        private void CriarTagFuncao(string nomeFuncao)
        {
            Guna2Button tag = new Guna2Button();
            tag.Text = nomeFuncao;

            // 1. Definir a fonte e medir o tamanho
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
            int larguraTexto = TextRenderer.MeasureText(nomeFuncao, tag.Font).Width;

            // Tamanho com folga (+30) para o texto respirar dentro do botão redondo
            tag.Size = new Size(larguraTexto + 30, 35);

            // 2. A MAGIA REDONDINHA DO GUNA!
            tag.BorderRadius = 15;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;

            // 3. Design Inicial (Desligado)
            tag.FillColor = Color.FromArgb(230, 232, 235); // Fundo Cinza
            tag.ForeColor = Color.FromArgb(64, 64, 64);    // Texto Escuro
            tag.Margin = new Padding(0, 0, 10, 10);

            // Usamos o "Tag" para saber se está ligado ou desligado
            tag.Tag = false;

            // 4. Evento para ligar/desligar ao clicar
            tag.Click += (s, e) =>
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

            // Criar o controlo em memória RAM antes de o atirar para o painel!
            tag.CreateControl();
            flpFuncoes.Controls.Add(tag);
        }

        // ==========================================
        // 3. EVENTOS DA INTERFACE E VALIDAÇÕES
        // ==========================================
        private void cmbFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFamilia.SelectedValue == null) return;

            string familiaSelecionada = cmbFamilia.SelectedValue.ToString();
            CarregarTamanhos(familiaSelecionada);

            cmbModelo.Enabled = true;
            CarregarModelos(familiaSelecionada);

            ValidarTamanho();
        }

        private void ValidarTamanho()
        {
            bool ativarTamanho = false;

            if (cmbModelo.Text == "+ Escrever Novo Modelo...")
            {
                ativarTamanho = !string.IsNullOrWhiteSpace(txtNovoModelo.Text);
            }
            else
            {
                string modeloEscolhido = cmbModelo.Text.Trim();
                ativarTamanho = !string.IsNullOrEmpty(modeloEscolhido) && modeloEscolhido != "Selecionar...";
            }

            cmbTamanho.Enabled = ativarTamanho;
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modeloEscolhido = cmbModelo.Text.Trim();

            if (modeloEscolhido == "+ Escrever Novo Modelo...")
            {
                tlpModelo.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 0F);
                tlpModelo.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100F);

                txtNovoModelo.Enabled = true;
                txtNovoModelo.Visible = true;
                txtNovoModelo.Focus();
            }
            else
            {
                tlpModelo.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0F);
                tlpModelo.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 100F);
            }

            ValidarTamanho();
        }

        private void txtNovoModelo_TextChanged(object sender, EventArgs e)
        {
            ValidarTamanho();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtNovoModelo.Clear();
            txtQuantidade.Clear();
            if (cmbModelo.Items.Count > 0)
                cmbModelo.SelectedIndex = 0;

            tlpModelo.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0F);
            tlpModelo.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 100F);

            ValidarTamanho();
        }

        // ==========================================
        // 4. GRAVAÇÃO NA BASE DE DADOS
        // ==========================================
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbFamilia.SelectedIndex <= 0)
            {
                MessageBox.Show("Por favor, seleciona uma Família.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string familia = cmbFamilia.SelectedValue.ToString();

            string modelo = txtNovoModelo.Visible ? txtNovoModelo.Text.Trim() : cmbModelo.Text.Trim();
            if (string.IsNullOrEmpty(modelo) || modelo == "Selecionar..." || modelo == "+ Escrever Novo Modelo...")
            {
                MessageBox.Show("Por favor, escolhe ou escreve um Modelo válido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbTamanho.SelectedIndex <= 0)
            {
                MessageBox.Show("Por favor, seleciona um Tamanho.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tamanho = cmbTamanho.SelectedValue.ToString();

            List<string> funcoesSelecionadas = new List<string>();
            foreach (Control ctrl in flpFuncoes.Controls)
            {
                if (ctrl is Guna2Button btn && btn.Tag is bool isLigado && isLigado)
                {
                    funcoesSelecionadas.Add(btn.Text);
                }
            }

            if (funcoesSelecionadas.Count == 0)
            {
                MessageBox.Show("Tens de selecionar pelo menos uma Função Autorizada!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantidade = 0;
            if (!string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                if (!int.TryParse(txtQuantidade.Text, out quantidade))
                {
                    MessageBox.Show("A quantidade tem de ser um número inteiro válido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                int acessoID = ObterOuCriarAcessoID(funcoesSelecionadas);

                string query = @"INSERT INTO EPI (Familia, Modelo, Tamanho, Acesso, Quantidade) 
                                 VALUES (@Familia, @Modelo, @Tamanho, @Acesso, @Quantidade)";

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Familia", familia);
                        cmd.Parameters.AddWithValue("@Modelo", modelo);
                        cmd.Parameters.AddWithValue("@Tamanho", tamanho);
                        cmd.Parameters.AddWithValue("@Acesso", acessoID);
                        cmd.Parameters.AddWithValue("@Quantidade", quantidade);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Stock criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar Ecrã
                cmbFamilia.SelectedIndex = 0;
                txtQuantidade.Clear();
                foreach (Control ctrl in flpFuncoes.Controls)
                {
                    if (ctrl is Guna2Button btn)
                    {
                        btn.Tag = false;
                        btn.FillColor = Color.FromArgb(230, 232, 235);
                        btn.ForeColor = Color.FromArgb(64, 64, 64);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar na base de dados: " + ex.Message, "Erro SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ObterOuCriarAcessoID(List<string> funcoesSelecionadas)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
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
    }
}