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
        string codigoArtigo;
        int gestor;
        string estado;
        EfeitoUI M = new();
        public event EventHandler ArtigoGuardado;

        public Artigo(string _estado, string _codigo, int _gestor) // Era int _id
        {
            InitializeComponent();
            codigoArtigo = _codigo;
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
            CarregarComboCores(cmbCor);
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
                    cmbCor.Enabled = true;

                    cmbFamilia.FillColor = Color.White;
                    cmbModelo.FillColor = Color.White;
                    cmbTamanho.FillColor = Color.White;
                    cmbCor.FillColor = Color.White;

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
                    await CarregarDadosArtigoAsync(codigoArtigo);

                    lblTituloCima.Text = "Editar EPI";

                    // BLOQUEIO DOS CAMPOS (Apenas quantidade e funções editáveis)
                    cmbFamilia.Enabled = false;
                    cmbModelo.Enabled = false;
                    cmbTamanho.Enabled = false;
                    cmbCor.Enabled = false; // A cor define o artigo, não pode ser editada
                    txtNovoModeloEPI.Enabled = false;

                    // Feedback visual de bloqueio
                    cmbFamilia.FillColor = Color.FromArgb(230, 232, 235);
                    cmbModelo.FillColor = Color.FromArgb(230, 232, 235);
                    cmbTamanho.FillColor = Color.FromArgb(230, 232, 235);
                    cmbCor.FillColor = Color.FromArgb(230, 232, 235);
                    break;

                default:
                    M.AbrirMensagem("Estado desconhecido: " + estado, "Erro");
                    break;
            }
        }

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

        private async Task CarregarDadosArtigoAsync(string artigoCodigo) // <-- Mudou de int para string
        {
            // A query agora procura por e.Codigo e o estado do Stock é 1 (sem plicas)
            string queryArtigo = @"
        SELECT e.Familia, e.Modelo, e.Tamanho, e.Cor, ISNULL(s.Quant, 0) AS Quantidade 
        FROM EPI e
        LEFT JOIN Stock s ON e.Codigo = s.Codigo AND s.Estado = 1
        WHERE e.Codigo = @cod"; // <-- Mudou de ID para Codigo

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryArtigo, conn);
                cmd.Parameters.AddWithValue("@cod", artigoCodigo); // <-- Passamos a string

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
                            string cor = reader["Cor"]?.ToString() ?? "00";
                            string quant = reader["Quantidade"].ToString();

                            cmbFamilia.SelectedIndexChanged -= cmbFamilia_SelectedIndexChanged;
                            cmbModelo.SelectedIndexChanged -= cmbModelo_SelectedIndexChanged;

                            cmbFamilia.SelectedValue = fam;
                            cmbCor.SelectedValue = cor;

                            DataTable dtModelos = await Task.Run(() => ObterTabelaModelos(fam));
                            cmbModelo.DisplayMember = "Modelo";
                            cmbModelo.ValueMember = "Modelo";
                            cmbModelo.DataSource = dtModelos;
                            cmbModelo.Text = mod;

                            CarregarTamanhos(fam);
                            cmbTamanho.SelectedValue = tam;

                            txtQuantidadeEPI.Text = quant;

                            cmbFamilia.SelectedIndexChanged += cmbFamilia_SelectedIndexChanged;
                            cmbModelo.SelectedIndexChanged += cmbModelo_SelectedIndexChanged;
                        }
                    }
                    // Chama a função das tags passando o CÓDIGO
                    await MarcarFuncoesAtivasAsync(artigoCodigo);
                }
                catch (Exception ex) { M.AbrirMensagem("Erro: " + ex.Message, "Erro"); }
            }
        }

        private async Task MarcarFuncoesAtivasAsync(string artigoCodigo) // <-- Passa a string
        {
            string queryFuncoes = @"SELECT f.Nome FROM Funcoes f 
                            INNER JOIN AcessoFuncoes af ON f.ID = af.FuncaoID 
                            INNER JOIN EPI e ON e.Acesso = af.AcessoID 
                            WHERE e.Codigo = @cod"; // <-- Mudou de e.ID para e.Codigo

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryFuncoes, conn);
                cmd.Parameters.AddWithValue("@cod", artigoCodigo);
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

        private void CarregarComboCores(Guna2ComboBox combo)
        {
            DataTable dtCores = new DataTable();
            dtCores.Columns.Add("ID", typeof(string));
            dtCores.Columns.Add("Nome", typeof(string));

            dtCores.Rows.Add("00", "Cor...");
            dtCores.Rows.Add("01", "Branco");
            dtCores.Rows.Add("02", "Beje");
            dtCores.Rows.Add("03", "Azul Marinho");
            dtCores.Rows.Add("04", "Azul Bebé");
            dtCores.Rows.Add("05", "Cinzento");
            dtCores.Rows.Add("06", "Verde");

            combo.DataSource = dtCores;
            combo.DisplayMember = "Nome";
            combo.ValueMember = "ID";
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
                string[] tamanhosLetras = { "XXS", "XS", "S", "M", "L", "XL", "XXL", "XXXL", "3XL" };
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
            string modeloFinal = (cmbModelo.Text == "+ Escrever Novo Modelo...")
                                 ? txtNovoModeloEPI.Text.Trim()
                                 : cmbModelo.Text.Trim();

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

            // Validação de Cor no MODO CRIAR - Agora validamos pelo TEXTO da cor e não pelo Value
            string nomeCorSelecionada = cmbCor.Text.Trim();
            if (estado == "Criar" && (string.IsNullOrEmpty(nomeCorSelecionada) || nomeCorSelecionada == "Cor..."))
            {
                M.AbrirMensagem("Por favor, seleciona uma cor válida.", "Aviso");
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
                int novoAcessoID = await Task.Run(() => ObterOuCriarAcessoID(funcoesMarcadas));

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            if (estado == "Editar")
                            {
                                // 1. Obter o Código do EPI
                                string sqlGetCod = "SELECT Codigo FROM EPI WHERE Codigo = @codigo";
                                string codigoEpi = "";
                                using (SqlCommand cmdCod = new SqlCommand(sqlGetCod, conn, tran))
                                {
                                    cmdCod.Parameters.AddWithValue("@codigo", codigoArtigo);
                                    object resCod = await cmdCod.ExecuteScalarAsync();
                                    if (resCod != null) codigoEpi = resCod.ToString();
                                }

                                // 2. Atualizar a tabela EPI (apenas Acesso)
                                string sqlUpdateEpi = "UPDATE EPI SET Acesso = @a WHERE Codigo = @codigo";
                                using (SqlCommand cmdEpi = new SqlCommand(sqlUpdateEpi, conn, tran))
                                {
                                    cmdEpi.Parameters.AddWithValue("@a", novoAcessoID);
                                    cmdEpi.Parameters.AddWithValue("@codigo", codigoArtigo);
                                    await cmdEpi.ExecuteNonQueryAsync();
                                }

                                // 3. Atualizar/Inserir no Stock (Estado = 1 sem plicas)
                                string sqlUpsertStock = @"
                            IF EXISTS (SELECT 1 FROM Stock WHERE Codigo = @cod AND Estado = 1)
                                UPDATE Stock SET Quant = @q WHERE Codigo = @cod AND Estado = 1
                            ELSE
                                INSERT INTO Stock (Codigo, Estado, Quant) VALUES (@cod, 1, @q)";

                                using (SqlCommand cmdStk = new SqlCommand(sqlUpsertStock, conn, tran))
                                {
                                    cmdStk.Parameters.AddWithValue("@cod", codigoEpi);
                                    cmdStk.Parameters.AddWithValue("@q", novaQuantidade);
                                    await cmdStk.ExecuteNonQueryAsync();
                                }

                                M.AbrirMensagem("Artigo e Stock atualizados com sucesso!", "Sucesso");
                            }
                            else // MODO CRIAR
                            {
                                // AQUI ESTÁ A MAGIA: Obtém o ID da Cor (se não existir, cria logo na BD)
                                string idCorFinal = await ObterOuCriarCorAsync(conn, tran, nomeCorSelecionada);

                                // 1. Verificar se o EPI já existe no catálogo (inclui Cor com o ID real)
                                string sqlCheckEpi = "SELECT Codigo FROM EPI WHERE Familia = @f AND Modelo = @m AND Tamanho = @t AND Cor = @c AND Acesso = @a";
                                string codigoEpi = null;

                                using (SqlCommand cmdCheck = new SqlCommand(sqlCheckEpi, conn, tran))
                                {
                                    cmdCheck.Parameters.AddWithValue("@f", cmbFamilia.SelectedValue.ToString());
                                    cmdCheck.Parameters.AddWithValue("@m", modeloFinal);
                                    cmdCheck.Parameters.AddWithValue("@t", cmbTamanho.SelectedValue.ToString());
                                    cmdCheck.Parameters.AddWithValue("@c", idCorFinal); // <-- Usa o ID validado/criado
                                    cmdCheck.Parameters.AddWithValue("@a", novoAcessoID);

                                    object result = await cmdCheck.ExecuteScalarAsync();
                                    if (result != null) codigoEpi = result.ToString();
                                }

                                // 2. Se o EPI não existir, GERAMOS O CÓDIGO e criamos o registo
                                if (codigoEpi == null)
                                {
                                    codigoEpi = await GerarCodigoEPIAsync(conn, tran, cmbFamilia.SelectedValue.ToString(), modeloFinal, cmbTamanho.SelectedValue.ToString(), idCorFinal);

                                    string sqlInsertEpi = @"
                                INSERT INTO EPI (Codigo, Familia, Modelo, Tamanho, Cor, Acesso, Ativo) 
                                VALUES (@cod, @f, @m, @t, @c, @a, 1)";

                                    using (SqlCommand cmdIns = new SqlCommand(sqlInsertEpi, conn, tran))
                                    {
                                        cmdIns.Parameters.AddWithValue("@cod", codigoEpi);
                                        cmdIns.Parameters.AddWithValue("@f", cmbFamilia.SelectedValue.ToString());
                                        cmdIns.Parameters.AddWithValue("@m", modeloFinal);
                                        cmdIns.Parameters.AddWithValue("@t", cmbTamanho.SelectedValue.ToString());
                                        cmdIns.Parameters.AddWithValue("@c", idCorFinal); // <-- Usa o ID validado/criado
                                        cmdIns.Parameters.AddWithValue("@a", novoAcessoID);

                                        await cmdIns.ExecuteNonQueryAsync();
                                    }
                                }

                                // 3. SOMAMOS ao Stock "Novo" (Estado = 1 sem plicas)
                                string sqlUpsertStock = @"
                            IF EXISTS (SELECT 1 FROM Stock WHERE Codigo = @cod AND Estado = 1)
                                UPDATE Stock SET Quant = Quant + @q WHERE Codigo = @cod AND Estado = 1
                            ELSE
                                INSERT INTO Stock (Codigo, Estado, Quant) VALUES (@cod, 1, @q)";

                                using (SqlCommand cmdStk = new SqlCommand(sqlUpsertStock, conn, tran))
                                {
                                    cmdStk.Parameters.AddWithValue("@cod", codigoEpi);
                                    cmdStk.Parameters.AddWithValue("@q", novaQuantidade);
                                    await cmdStk.ExecuteNonQueryAsync();
                                }

                                M.AbrirMensagem("Stock registado com sucesso!", "Sucesso");
                            }

                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }

                ArtigoGuardado?.Invoke(this, EventArgs.Empty);
                btnCancelar_Click(null, null);
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao guardar: " + ex.Message, "Erro SQL");
            }
        }

        private async Task<string> ObterOuCriarCorAsync(SqlConnection conn, SqlTransaction tran, string nomeCor)
        {
            if (string.IsNullOrEmpty(nomeCor) || nomeCor == "Cor...") return "00";

            // 1. Tenta achar a cor pelo NOME
            string sqlCheck = "SELECT ID FROM Cor WHERE Nome = @nome";
            using (SqlCommand cmd = new SqlCommand(sqlCheck, conn, tran))
            {
                cmd.Parameters.AddWithValue("@nome", nomeCor);
                object res = await cmd.ExecuteScalarAsync();
                if (res != null && res != DBNull.Value)
                {
                    if (int.TryParse(res.ToString(), out int idInt))
                        return idInt.ToString("D2");
                    return res.ToString().PadLeft(2, '0');
                }
            }

            // 2. Se a cor não existe, vamos criar!
            // Descobrimos qual é o maior ID atual para somar 1.
            string sqlMax = "SELECT ISNULL(MAX(CAST(ID AS INT)), 0) FROM Cor";
            int nextId = 1;
            using (SqlCommand cmdMax = new SqlCommand(sqlMax, conn, tran))
            {
                object maxRes = await cmdMax.ExecuteScalarAsync();
                if (maxRes != null && maxRes != DBNull.Value)
                {
                    nextId = Convert.ToInt32(maxRes) + 1;
                }
            }

            string novoIdFormatado = nextId.ToString("D2");

            // Tentamos inserir com o ID explícito (Caso a tua coluna ID seja manual)
            try
            {
                string sqlInsertExplicit = "INSERT INTO Cor (ID, Nome) VALUES (@id, @nome)";
                using (SqlCommand cmdIns = new SqlCommand(sqlInsertExplicit, conn, tran))
                {
                    cmdIns.Parameters.AddWithValue("@id", nextId);
                    cmdIns.Parameters.AddWithValue("@nome", nomeCor);
                    await cmdIns.ExecuteNonQueryAsync();
                }
                return novoIdFormatado;
            }
            catch (SqlException ex) when (ex.Number == 544)
            {
                // Erro 544: Identity Insert. Significa que a tua coluna ID é AUTO-INCREMENTO.
                // Inserimos só o nome e o SQL gera o ID sozinho.
                string sqlInsertIdentity = "INSERT INTO Cor (Nome) OUTPUT INSERTED.ID VALUES (@nome)";
                using (SqlCommand cmdInsIden = new SqlCommand(sqlInsertIdentity, conn, tran))
                {
                    cmdInsIden.Parameters.AddWithValue("@nome", nomeCor);
                    object insertedId = await cmdInsIden.ExecuteScalarAsync();
                    return Convert.ToInt32(insertedId).ToString("D2");
                }
            }
        }


        // =================================================================================
        // MÉTODOS DE GERAÇÃO DE CÓDIGOS E IDS
        // =================================================================================

        private async Task<string> GerarCodigoEPIAsync(SqlConnection conn, SqlTransaction tran, string familia, string modelo, string tamanho, string idCor)
        {
            // 1. ID Família (Baseado no Excel)
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

            // 2. ID Modelo (Sempre a somar)
            string idMod = await ObterProximoIdModeloAsync(conn, tran, familia, modelo);

            // 3. ID Cor (Garantia de 2 dígitos)
            string idC = (idCor == "00" || string.IsNullOrEmpty(idCor)) ? "00" : idCor.PadLeft(2, '0');

            // 4. ID Tamanho (Calculado)
            string idTam = CalcularIdTamanho(tamanho);

            // Código final de 7 dígitos: Família(1) + Modelo(2) + Cor(2) + Tamanho(2)
            return $"{idFam}{idMod}{idC}{idTam}";
        }

        private async Task<string> ObterProximoIdModeloAsync(SqlConnection conn, SqlTransaction tran, string familia, string modelo)
        {
            // Verifica se o modelo já tem um ID atribuído noutra cor/tamanho
            // SUBSTRING 1-indexed: Posição 2, tamanho 2.
            string sqlCheck = "SELECT TOP 1 SUBSTRING(Codigo, 2, 2) FROM EPI WHERE Modelo = @m AND LEN(Codigo) >= 7";
            using (SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn, tran))
            {
                cmdCheck.Parameters.AddWithValue("@m", modelo);
                object res = await cmdCheck.ExecuteScalarAsync();
                if (res != null && res != DBNull.Value) return res.ToString();
            }

            // Se é um modelo novo, pega no maior ID de modelo desta família
            string sqlMax = "SELECT MAX(CAST(SUBSTRING(Codigo, 2, 2) AS INT)) FROM EPI WHERE Familia = @f AND LEN(Codigo) >= 7";
            using (SqlCommand cmdMax = new SqlCommand(sqlMax, conn, tran))
            {
                cmdMax.Parameters.AddWithValue("@f", familia);
                object resMax = await cmdMax.ExecuteScalarAsync();
                if (resMax != null && resMax != DBNull.Value)
                {
                    int next = Convert.ToInt32(resMax) + 1;
                    return next.ToString("D2");
                }
            }
            return "01"; // Primeiro modelo desta família
        }

        private string CalcularIdTamanho(string tamanho)
        {
            var tamanhosLetras = new Dictionary<string, string> {
                {"XXS","01"}, {"XS","02"}, {"S","03"}, {"M","04"},
                {"L","05"}, {"XL","06"}, {"XXL","07"}, {"XXXL","08"}, {"3XL","08"}
            };

            if (tamanhosLetras.ContainsKey(tamanho)) return tamanhosLetras[tamanho];

            // Para tamanhos numéricos (36 a 48) no excel o 36 corresponde ao ID 09. Matemática: Num - 27
            if (int.TryParse(tamanho, out int tamNum)) return (tamNum - 27).ToString("D2");

            return "00"; // fallback
        }

        private int ObterOuCriarAcessoID(List<string> funcoesSelecionadas)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmdIDs = new SqlCommand("SELECT DISTINCT AcessoID FROM AcessoFuncoes", conn);
                List<int> gruposExistentes = new List<int>();
                using (SqlDataReader rdr = cmdIDs.ExecuteReader())
                {
                    while (rdr.Read()) gruposExistentes.Add(rdr.GetInt32(0));
                }

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

                    if (funcoesDoGrupo.Count == funcoesSelecionadas.Count && !funcoesDoGrupo.Except(funcoesSelecionadas).Any())
                    {
                        return groupID;
                    }
                }

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
            using (MSGBX m = new MSGBX("Deseja mesmo eliminar este EPI?", "Eliminar EPI"))
            {
                if (m.ShowDialog() == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                        {
                            conn.Open();
                            string sql = "UPDATE EPI SET Ativo = '0' WHERE Codigo = @codigo";
                            using (SqlCommand elimina = new SqlCommand(sql, conn))
                            {
                                elimina.Parameters.AddWithValue("@codigo", codigoArtigo);
                                int linhasAfetadas = elimina.ExecuteNonQuery();
                                if (linhasAfetadas > 0)
                                {
                                    M.AbrirMensagem("Artigo eliminado com sucesso!", "Sucesso");
                                    ArtigoGuardado?.Invoke(this, EventArgs.Empty);
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
                    btnCancelar_Click(null, null);
                }
            }
        }
    }
}