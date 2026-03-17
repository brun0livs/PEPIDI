using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.FormsSecundarios;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs.UcsSecundarios
{
    public partial class Artigo : UserControl
    {
        int id;
        int gestor;
        string estado;
        EfeitoUI M = new();
        public event EventHandler ArtigoGuardado;

        public Artigo(string _estado, int _id, int _gestor)
        {
            InitializeComponent();
            id = _id;
            estado = _estado;
            gestor = _gestor;
        }

        private void Artigo_Load(object sender, EventArgs e)
        {
            GereEstado(estado);
            GestorTema.AplicarEstilos(this);
        }

        private async void GereEstado(string estado)
        {
            // Carregamento base obrigatório
            CarregarCombo(cmbFamilia);
            await CarregarFuncoesAsync();

            switch (estado)
            {
                case "Criar":
                    lblTituloCima.Text = "Criar Novo EPI";
                    cmbFamilia.SelectedIndex = 0;
                    txtQuantidadeEPI.Clear();
                    cmbFamilia.Enabled = true;
                    cmbModelo.Enabled = false;
                    cmbTamanho.Enabled = false;

                    cmbFamilia.FillColor = Color.White;
                    cmbModelo.FillColor = Color.White;
                    cmbTamanho.FillColor = Color.White;

                    btnEliminar.Visible = false;
                    btnEliminar.Enabled = false;

                    // LIMPAR AS TAGS (FUNÇÕES)
                    foreach (Control c in flpFuncoes.Controls)
                    {
                        if (c is Guna2Button btn)
                        {
                            btn.Tag = false;
                            btn.FillColor = Color.FromArgb(230, 232, 235);
                            btn.ForeColor = Color.FromArgb(64, 64, 64);
                        }
                    }
                    break;

                case "Editar":
                    await CarregarDadosArtigoAsync(id);

                    lblTituloCima.Text = "Editar EPI";

                    // BLOQUEIO DOS CAMPOS (Apenas quantidade e funções editáveis)
                    cmbFamilia.Enabled = false;
                    cmbModelo.Enabled = false;
                    cmbTamanho.Enabled = false;
                    txtNovoModeloEPI.Enabled = false;

                    // Feedback visual de bloqueio para o ecrã de 768px
                    cmbFamilia.FillColor = Color.FromArgb(230, 232, 235);
                    cmbModelo.FillColor = Color.FromArgb(230, 232, 235);
                    cmbTamanho.FillColor = Color.FromArgb(230, 232, 235);
                    break;

                default:
                    M.AbrirMensagem("Estado desconhecido: " + estado, "Erro");
                    break;
            }
        }

        // MOTOR DE DADOS: Obtém a tabela de modelos de forma síncrona para ser usada em Tasks
        private DataTable ObterTabelaModelos(string familia)
        {
            DataTable dtModelos = new DataTable();
            dtModelos.Columns.Add("Modelo", typeof(string));
            dtModelos.Rows.Add("Modelo...");
            dtModelos.Rows.Add("+ Escrever Novo Modelo...");

            string query = "SELECT DISTINCT Modelo FROM EPI WHERE Familia = @Familia AND Modelo IS NOT NULL AND Modelo <> ''";

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
                    catch { /* Erro silencioso em thread secundária */ }
                }
            }
            return dtModelos;
        }

        private async Task CarregarDadosArtigoAsync(int artigoId)
        {
            string queryArtigo = "SELECT Familia, Modelo, Tamanho, Quantidade FROM EPI WHERE ID = @id";

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryArtigo, conn);
                cmd.Parameters.AddWithValue("@id", artigoId);

                try
                {
                    await conn.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            string fam = reader["Familia"].ToString();
                            string mod = reader["Modelo"].ToString();
                            string tam = reader["Tamanho"].ToString();
                            string quant = reader["Quantidade"].ToString();

                            // 1. Bloquear eventos para evitar recarregamentos automáticos durante o preenchimento
                            cmbFamilia.SelectedIndexChanged -= cmbFamilia_SelectedIndexChanged;
                            cmbModelo.SelectedIndexChanged -= cmbModelo_SelectedIndexChanged;

                            // 2. Preencher Família
                            cmbFamilia.SelectedValue = fam;

                            // 3. Carregar e esperar pelos Modelos (Garante que o texto aparece)
                            DataTable dtModelos = await Task.Run(() => ObterTabelaModelos(fam));
                            cmbModelo.DisplayMember = "Modelo"; // O que o utilizador vê
                            cmbModelo.ValueMember = "Modelo";   // O valor por trás
                            cmbModelo.DataSource = dtModelos;

                            cmbModelo.Text = mod; // Agora o texto selecionado será o nome real

                            // 4. Preencher Tamanhos
                            CarregarTamanhos(fam);
                            cmbTamanho.SelectedValue = tam;

                            // 5. Preencher Quantidade
                            txtQuantidadeEPI.Text = quant;

                            // 6. Reativar eventos
                            cmbFamilia.SelectedIndexChanged += cmbFamilia_SelectedIndexChanged;
                            cmbModelo.SelectedIndexChanged += cmbModelo_SelectedIndexChanged;
                        }
                    }
                    await MarcarFuncoesAtivasAsync(artigoId);
                }
                catch (Exception ex) { M.AbrirMensagem("Erro: " + ex.Message, "Erro"); }
            }
        }

        private async Task MarcarFuncoesAtivasAsync(int artigoId)
        {
            string queryFuncoes = @"SELECT f.Nome FROM Funcoes f 
                                    INNER JOIN AcessoFuncoes af ON f.ID = af.FuncaoID 
                                    INNER JOIN EPI e ON e.Acesso = af.AcessoID 
                                    WHERE e.ID = @id";

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryFuncoes, conn);
                cmd.Parameters.AddWithValue("@id", artigoId);
                try
                {
                    await conn.OpenAsync();
                    List<string> funcoesAtivas = new List<string>();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            funcoesAtivas.Add(reader["Nome"].ToString().Trim());
                        }
                    }

                    foreach (Control c in flpFuncoes.Controls)
                    {
                        if (c is Guna2Button btn && funcoesAtivas.Contains(btn.Text.Trim()))
                        {
                            btn.Tag = true;
                            btn.FillColor = Color.FromArgb(242, 103, 34);
                            btn.ForeColor = Color.White;
                        }
                    }
                }
                catch { }
            }
        }

        private void CarregarCombo(Guna2ComboBox combo)
        {
            DataTable dtFamilias = new DataTable();
            dtFamilias.Columns.Add("ID", typeof(int));
            dtFamilias.Columns.Add("NomeFamilia", typeof(string));
            dtFamilias.Columns.Add("Apresentacao", typeof(string));

            dtFamilias.Rows.Add(0, "Null", "Família...");
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

        private void CarregarTamanhos(string familia)
        {
            DataTable dtTamanhos = new DataTable();
            dtTamanhos.Columns.Add("Valor", typeof(string));
            dtTamanhos.Columns.Add("Apresentacao", typeof(string));
            dtTamanhos.Rows.Add("Null", "Tamanho...");

            if (familia == "Sapato")
            {
                for (int i = 35; i <= 48; i++) dtTamanhos.Rows.Add(i.ToString(), i.ToString());
            }
            else if (familia == "Calca")
            {
                for (int i = 36; i <= 58; i += 2) dtTamanhos.Rows.Add(i.ToString(), i.ToString());
            }
            else if (familia != "Null")
            {
                string[] tamanhosLetras = { "XS", "S", "M", "L", "XL", "XXL", "3XL", "4XL" };
                foreach (string t in tamanhosLetras) dtTamanhos.Rows.Add(t, t);
            }

            cmbTamanho.DataSource = dtTamanhos;
            cmbTamanho.DisplayMember = "Apresentacao";
            cmbTamanho.ValueMember = "Valor";
            cmbTamanho.SelectedIndex = 0;
        }

        private async Task CarregarFuncoesAsync()
        {
            flpFuncoes.Visible = false;
            flpFuncoes.SuspendLayout();
            flpFuncoes.Controls.Clear();
            List<string> lista = new List<string>();
            string query = "SELECT Nome FROM Funcoes ORDER BY Nome";

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataReader r = cmd.ExecuteReader())
                                while (r.Read()) lista.Add(r["Nome"].ToString().Trim());
                        }
                        catch { }
                    }
                }
            });

            foreach (string n in lista) CriarTagFuncao(n);
            flpFuncoes.ResumeLayout(true);
            flpFuncoes.Visible = true;
        }

        private void CriarTagFuncao(string nomeFuncao)
        {
            Guna2Button tag = new Guna2Button();
            tag.Text = nomeFuncao;
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
            int larguraTexto = TextRenderer.MeasureText(nomeFuncao, tag.Font).Width;
            tag.Size = new Size(larguraTexto + 30, 35);
            tag.BorderRadius = 15;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;
            tag.FillColor = Color.FromArgb(230, 232, 235);
            tag.ForeColor = Color.FromArgb(64, 64, 64);
            tag.Margin = new Padding(0, 0, 10, 10);
            tag.Tag = false;

            tag.Click += (s, e) =>
            {
                bool isLigado = (bool)tag.Tag;
                tag.Tag = !isLigado;
                tag.FillColor = (bool)tag.Tag ? Color.FromArgb(242, 103, 34) : Color.FromArgb(230, 232, 235);
                tag.ForeColor = (bool)tag.Tag ? Color.White : Color.FromArgb(64, 64, 64);
            };
            tag.CreateControl();
            flpFuncoes.Controls.Add(tag);
        }

        // EVENTOS DE UI (Necessários para a lógica de "Criar")
        private void cmbFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFamilia.SelectedValue == null) return;

            string fam = cmbFamilia.SelectedValue.ToString();
            CarregarTamanhos(fam);
            cmbModelo.Enabled = true;

            Task.Run(async () =>
            {
                DataTable dt = ObterTabelaModelos(fam);

                this.Invoke(new MethodInvoker(() =>
                {
                    // ADICIONA ESTAS DUAS LINHAS AQUI:
                    cmbModelo.DisplayMember = "Modelo";
                    cmbModelo.ValueMember = "Modelo";

                    cmbModelo.DataSource = dt;
                }));
            });
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mod = cmbModelo.Text.Trim();
            bool isNovo = mod == "+ Escrever Novo Modelo...";
            tlpModelo.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, isNovo ? 0F : 100F);
            tlpModelo.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, isNovo ? 100F : 0F);
            if (isNovo) { txtNovoModeloEPI.Visible = true; txtNovoModeloEPI.Focus(); }
            ValidarTamanho();
        }

        private void txtNovoModelo_TextChanged(object sender, EventArgs e) => ValidarTamanho();

        private void ValidarTamanho()
        {
            string mod = cmbModelo.Text.Trim();
            bool ok = (mod == "+ Escrever Novo Modelo...") ? !string.IsNullOrWhiteSpace(txtNovoModeloEPI.Text) : (!string.IsNullOrEmpty(mod) && mod != "Modelo...");
            cmbTamanho.Enabled = ok;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.Parent != null) { this.Parent.Controls.Remove(this); this.Dispose(); }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            // --- 1. LÓGICA DO MODELO (O TEU FIX) ---
            // Se a combo está na opção de novo modelo, usamos o texto da TextBox. 
            // Caso contrário, usamos o que está selecionado na combo.
            string modeloFinal = (cmbModelo.Text == "+ Escrever Novo Modelo...")
                                 ? txtNovoModeloEPI.Text.Trim()
                                 : cmbModelo.Text.Trim();

            // Validações de segurança
            if (string.IsNullOrEmpty(modeloFinal) || modeloFinal == "Modelo...")
            {
                M.AbrirMensagem("Por favor, introduz um modelo válido.", "Aviso");
                return;
            }

            if (!int.TryParse(txtQuantidadeEPI.Text, out int novaQuantidade))
            {
                M.AbrirMensagem("Insere uma quantidade válida.", "Aviso");
                return;
            }

            List<string> funcoesMarcadas = new List<string>();
            foreach (Control c in flpFuncoes.Controls)
            {
                if (c is Guna2Button btn && btn.Tag is bool ativo && ativo)
                    funcoesMarcadas.Add(btn.Text.Trim());
            }

            if (funcoesMarcadas.Count == 0)
            {
                M.AbrirMensagem("Seleciona pelo menos uma função autorizada.", "Aviso");
                return;
            }

            try
            {
                // 2. Obter ou Criar o Grupo de Funções (AcessoID)
                int novoAcessoID = await Task.Run(() => ObterOuCriarAcessoID(funcoesMarcadas));

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    await conn.OpenAsync();

                    if (estado == "Editar")
                    {
                        // No modo editar, apenas mudamos quantidade e acesso (o modelo já está bloqueado)
                        string sqlUpdate = "UPDATE EPI SET Quantidade = @q, Acesso = @a WHERE ID = @id";
                        using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
                        {
                            cmd.Parameters.AddWithValue("@q", novaQuantidade);
                            cmd.Parameters.AddWithValue("@a", novoAcessoID);
                            cmd.Parameters.AddWithValue("@id", id);
                            await cmd.ExecuteNonQueryAsync();
                        }
                        M.AbrirMensagem("EPI atualizado com sucesso!", "Sucesso");
                    }
                    else
                    {
                        // MODO CRIAR: Verificar se já existe usando o MODELO FINAL
                        string sqlCheck = "SELECT COUNT(*) FROM EPI WHERE Familia = @f AND Modelo = @m AND Tamanho = @t";
                        using (SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@f", cmbFamilia.SelectedValue.ToString());
                            cmdCheck.Parameters.AddWithValue("@m", modeloFinal); // <--- FIX AQUI
                            cmdCheck.Parameters.AddWithValue("@t", cmbTamanho.SelectedValue.ToString());

                            int existe = (int)await cmdCheck.ExecuteScalarAsync();
                            if (existe > 0)
                            {
                                M.AbrirMensagem($"O artigo '{modeloFinal}' já existe no stock.", "Atenção");
                                return;
                            }
                        }

                        string sqlInsert = "INSERT INTO EPI (Familia, Modelo, Tamanho, Quantidade, Acesso) VALUES (@f, @m, @t, @q, @a)";
                        using (SqlCommand cmd = new SqlCommand(sqlInsert, conn))
                        {
                            cmd.Parameters.AddWithValue("@f", cmbFamilia.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@m", modeloFinal); // <--- FIX AQUI
                            cmd.Parameters.AddWithValue("@t", cmbTamanho.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@q", novaQuantidade);
                            cmd.Parameters.AddWithValue("@a", novoAcessoID);
                            await cmd.ExecuteNonQueryAsync();
                        }
                        M.AbrirMensagem("Novo EPI registado!", "Sucesso");
                    }
                }

                // Avisa o pai para atualizar a DGV
                ArtigoGuardado?.Invoke(this, EventArgs.Empty);

                // Fecha a janela
                btnCancelar_Click(null, null);
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao guardar: " + ex.Message, "Erro");
            }
        }

        private int ObterOuCriarAcessoID(List<string> funcoesSelecionadas)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();

                // 1. Buscar todos os grupos de acesso existentes
                SqlCommand cmdIDs = new SqlCommand("SELECT DISTINCT AcessoID FROM AcessoFuncoes", conn);
                List<int> gruposExistentes = new List<int>();
                using (SqlDataReader rdr = cmdIDs.ExecuteReader())
                {
                    while (rdr.Read()) gruposExistentes.Add(rdr.GetInt32(0));
                }

                // 2. Comparar as funções de cada grupo com as selecionadas
                foreach (int groupID in gruposExistentes)
                {
                    SqlCommand cmdFuncoes = new SqlCommand(@"SELECT f.Nome FROM AcessoFuncoes af 
                                                     INNER JOIN Funcoes f ON f.ID = af.FuncaoID 
                                                     WHERE af.AcessoID = @id", conn);
                    cmdFuncoes.Parameters.AddWithValue("@id", groupID);

                    List<string> funcoesDoGrupo = new List<string>();
                    using (SqlDataReader rdr = cmdFuncoes.ExecuteReader())
                    {
                        while (rdr.Read()) funcoesDoGrupo.Add(rdr.GetString(0).Trim());
                    }

                    // Se o conjunto de funções for exatamente igual (mesma contagem e sem diferenças)
                    if (funcoesDoGrupo.Count == funcoesSelecionadas.Count && !funcoesDoGrupo.Except(funcoesSelecionadas).Any())
                    {
                        return groupID; // Já existe um grupo com estas funções, usamos este ID
                    }
                }

                // 3. Se chegou aqui, o conjunto é novo. Criar novo AcessoID
                SqlCommand cmdNovoID = new SqlCommand("INSERT INTO Acessos DEFAULT VALUES; SELECT SCOPE_IDENTITY();", conn);
                int novoID = Convert.ToInt32(cmdNovoID.ExecuteScalar());

                foreach (string funcNome in funcoesSelecionadas)
                {
                    SqlCommand cmdInsert = new SqlCommand(@"INSERT INTO AcessoFuncoes (AcessoID, FuncaoID) 
                                                     VALUES (@aid, (SELECT ID FROM Funcoes WHERE Nome = @nome))", conn);
                    cmdInsert.Parameters.AddWithValue("@aid", novoID);
                    cmdInsert.Parameters.AddWithValue("@nome", funcNome);
                    cmdInsert.ExecuteNonQuery();
                }

                return novoID;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Usa a tua MSGBX personalizada que configurámos para o "Sim/Não"
            using (MSGBX m = new MSGBX("Deseja mesmo eliminar este EPI?", "Eliminar EPI"))
            {
                if (m.ShowDialog() == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                        {
                            conn.Open();

                            // USAR PARÂMETROS para ser mais seguro e profissional
                            string sql = "UPDATE EPI SET Ativo = '0' WHERE ID = @id";

                            using (SqlCommand elimina = new SqlCommand(sql, conn))
                            {
                                elimina.Parameters.AddWithValue("@id", id);

                                // O "ENTER" NO CORREIO: Executa o comando na BD
                                int linhasAfetadas = elimina.ExecuteNonQuery();

                                if (linhasAfetadas > 0)
                                {
                                    M.AbrirMensagem("Artigo eliminado com sucesso!", "Sucesso");

                                    // DISPARAR O EVENTO PARA O PAI ATUALIZAR A TABELA
                                    ArtigoGuardado?.Invoke(this, EventArgs.Empty);

                                    // FECHAR A UC DE EDIÇÃO
                                    btnCancelar_Click(null, null);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        M.AbrirMensagem("Erro ao eliminar: " + ex.Message, "Erro SQL");
                    }
                }
                else
                {
                    // FECHAR A UC DE EDIÇÃO
                    btnCancelar_Click(null, null);
                }
            }
        }
    }
}