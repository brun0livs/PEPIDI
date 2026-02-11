using PEPIDI.Organizers;
using PEPIDI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PEPIDI
{
    internal class MostraRoupa
    {
        List<int> ids = new List<int>();
        List<string> modelos = new List<string>();
        List<string> tamanhos = new List<string>();
        List<string> vals = new List<string>();

        public List<LinhaPedidoInfo> ObterRoupaPorFuncionarioNovo(int nrFuncionario)
        {
            var resultado = new List<LinhaPedidoInfo>();

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_RoupaPorFuncionario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NrFunc", nrFuncionario);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var modelo = reader["Modelo"].ToString();
                        var tamanho = reader["Tamanho"].ToString();

                        var linha = new LinhaPedidoInfo
                        {
                            IdEpi = Convert.ToInt32(reader["ID"]),
                            Modelo = modelo,
                            TamanhoAtual = tamanho,
                            TamanhosDisponiveis = ObterTamanhosDisponiveis(modelo)
                        };

                        resultado.Add(linha);
                    }
                }
            }

            if (resultado.Count == 0)
            {
                MessageBox.Show("Nenhum dado encontrado para o funcionário.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return resultado; // nunca devolve null
        }

        public List<LinhaPedidoInfo> ObterRoupaUsadaPorFuncionarioNovo(int nrFuncionario)
        {
            var resultado = new List<LinhaPedidoInfo>();

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ProdutosConsumidosPorFuncionario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NrFunc", nrFuncionario);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var modelo = reader["Modelo"].ToString();
                        var tamanho = reader["Tamanho"].ToString();

                        var linha = new LinhaPedidoInfo
                        {
                            IdEpi = Convert.ToInt32(reader["IDEPI"]),
                            Modelo = modelo,
                            TamanhoAtual = tamanho,
                            // devolve só tamanhos que o funcionário já usou
                            TamanhosDisponiveis = ObterTamanhosUsadosPorFuncionario(modelo, nrFuncionario)
                        };

                        resultado.Add(linha);
                    }
                }
            }

            if (resultado.Count == 0)
            {
                MessageBox.Show(
                    "Nenhum produto usado encontrado para o funcionário.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return resultado; // nunca null
        }

        public DataTable ObterRoupaUsadaPorFuncionario(int nrFuncionario, Form form)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ProdutosConsumidosPorFuncionario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NrFunc", nrFuncionario);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            ids.Clear();
            modelos.Clear();
            tamanhos.Clear();
            vals.Clear();

            foreach (DataRow row in dt.Rows)
            {
                ids.Add(Convert.ToInt32(row["IDEPI"]));
                modelos.Add(row["Modelo"].ToString());
                tamanhos.Add(row["Tamanho"].ToString());
                vals.Add("0");
            }

            if (dt.Rows.Count == 0)
            {
                var resultado = MessageBox.Show("Nenhum produto usado encontrado para o funcionário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (resultado == DialogResult.OK)
                    form.Close();
            }

            else
            {
                HashSet<string> modelosUsados = new HashSet<string>();

                for (int i = 0, n = 1; i < ids.Count && n <= 6; i++)
                {
                    string modeloAtual = modelos[i];
                    string tamanhoAtual = tamanhos[i];

                    if (modelosUsados.Contains(modeloAtual))
                        continue;

                    modelosUsados.Add(modeloAtual);

                    Label labelAtual = form.Controls.Find("lbl" + n, true).FirstOrDefault() as Label;
                    ComboBox combo = form.Controls.Find("combo" + n, true).FirstOrDefault() as ComboBox;
                    Label labelVal = form.Controls.Find("lblVal" + n, true).FirstOrDefault() as Label;
                    Button btnMenos = form.Controls.Find("btnMenos" + n, true).FirstOrDefault() as Button;
                    Button btnMais = form.Controls.Find("btnMais" + n, true).FirstOrDefault() as Button;

                    if (labelAtual == null || combo == null || labelVal == null || btnMenos == null || btnMais == null)
                        continue;

                    labelAtual.Text = modeloAtual;

                    List<string> tamanhosUsados = ObterTamanhosUsadosPorFuncionario(modeloAtual, nrFuncionario);

                    combo.Items.Clear();
                    foreach (string t in tamanhosUsados)
                    {
                        combo.Items.Add(t);
                    }

                    int index = combo.Items.IndexOf(tamanhoAtual);
                    if (index >= 0)
                    {
                        combo.SelectedIndex = index;
                    }
                    else if (combo.Items.Count > 0)
                    {
                        combo.SelectedIndex = 0;
                    }
                    else
                    {
                        combo.SelectedItem = null;
                    }


                    combo.SelectedIndexChanged -= ComboBoxTamanho_SelectedIndexChanged;
                    combo.SelectedIndexChanged += ComboBoxTamanho_SelectedIndexChanged;

                    labelVal.Text = "0";

                    labelAtual.Visible = combo.Visible = labelVal.Visible = btnMenos.Visible = btnMais.Visible = true;
                    labelAtual.Enabled = combo.Enabled = labelVal.Enabled = btnMenos.Enabled = btnMais.Enabled = true;

                    labelAtual.Font = new Font("Calibri", 27.75F);
                    combo.Font = new Font("Calibri", 27.75F);
                    labelVal.Font = new Font("Calibri", 27.75F, FontStyle.Underline);
                    btnMenos.Font = btnMais.Font = new Font("Calibri", 27.75F);

                    if (btnMais.Tag is EventHandler antigoMais)
                        btnMais.Click -= antigoMais;
                    if (btnMenos.Tag is EventHandler antigoMenos)
                        btnMenos.Click -= antigoMenos;

                    EventHandler maisHandler = (s, e) => BtnMais_Click(s, e, form);
                    EventHandler menosHandler = (s, e) => BtnMenos_Click(s, e, form);

                    btnMais.Click += maisHandler;
                    btnMenos.Click += menosHandler;

                    btnMais.Tag = maisHandler;
                    btnMenos.Tag = menosHandler;

                    n++;
                }
            }

            return dt;
        }

        public List<string> ObterTamanhosUsadosPorFuncionario(string modelo, int nrFunc)
        {
            List<string> tamanhos = new List<string>();

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT E.Tamanho FROM PedidoPacote PP
                                                    INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
                                                    INNER JOIN EPI E ON PP.IDEPI = E.ID
                                                    WHERE PR.NrFunc = @NrFunc AND E.Modelo = @Modelo", conn))
            {
                cmd.Parameters.AddWithValue("@NrFunc", nrFunc);
                cmd.Parameters.AddWithValue("@Modelo", modelo);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tamanhos.Add(reader["Tamanho"].ToString());
                    }
                }
            }

            return tamanhos;
        }

        private List<string> ObterTamanhosDisponiveis(string modelo)
        {
            List<string> tamanhos = new List<string>();
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetTamanhosPorModelo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Modelo", modelo);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tamanhos.Add(reader.GetString(0));
                    }
                }
            }
            return tamanhos;
        }

        public void BtnMais_Click(object sender, EventArgs e, Form chamador)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string labelName = "lblVal" + btn.Name.Substring("btnMais".Length);
                Label label = chamador.Controls.Find(labelName, true).FirstOrDefault() as Label;

                int index = int.Parse(btn.Name.Substring("btnMais".Length)) - 1;

                if (label != null && index >= 0 && index < vals.Count)
                {
                    int currentValue = int.Parse(label.Text);
                    if (currentValue < 5)
                    {
                        currentValue++;
                        label.Text = currentValue.ToString();
                        vals[index] = currentValue.ToString();
                    }
                }
            }
        }

        public void BtnMenos_Click(object sender, EventArgs e, Form chamador)
        {
            Button btn = sender as Button;

            if (btn != null)
            {
                // Nome do label associado (ex: lblVal1, lblVal2...)
                string labelName = "lblVal" + btn.Name.Substring("btnMenos".Length);
                Label label = chamador.Controls.Find(labelName, true).FirstOrDefault() as Label;

                // Índice do item na lista vals
                int index = int.Parse(btn.Name.Substring("btnMenos".Length)) - 1;

                if (label != null && index >= 0 && index < vals.Count)
                {
                    int currentValue = int.Parse(label.Text);

                    if (currentValue > 0) // Limite mínimo
                    {
                        currentValue--;
                        label.Text = currentValue.ToString();
                        vals[index] = currentValue.ToString(); // Atualiza a lista
                    }
                }
            }
        }

        public void SubmeterEntrega(int nrFunc, Form form)
        {
            bool temQuantidade = vals.Any(v => int.TryParse(v, out int q) && q > 0);
            if (!temQuantidade)
            {
                MessageBox.Show("Não há peças para entregar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // 👉 1. Criar novo PedidoRegistos
                    SqlCommand createPedido = new SqlCommand(@"
                INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
                OUTPUT INSERTED.ID
                VALUES (@Data, @NrFunc, 'Pendente')", conn, tran);
                    createPedido.Parameters.AddWithValue("@Data", DateTime.Now);
                    createPedido.Parameters.AddWithValue("@NrFunc", nrFunc);
                    int idPedidoReg = Convert.ToInt32(createPedido.ExecuteScalar());

                    //// 👉 2. Guardar o ID no formulário (caso seja popup)
                    //if (form is FrmPopUp popup)
                    //    popup.IDPedidoCriado = idPedidoReg;

                    // 👉 3. Processar cada peça de roupa
                    for (int i = 0; i < modelos.Count; i++)
                    {
                        int quantidade = int.Parse(vals[i]);
                        string modelo = modelos[i];
                        string tamanho = tamanhos[i];

                        if (quantidade > 0)
                        {
                            // Procurar ID da roupa (modelo + tamanho)
                            SqlCommand cmdFindRoupa = new SqlCommand(@"
                        SELECT TOP 1 ID FROM Roupa 
                        WHERE Modelo = @Modelo AND Tamanho = @Tamanho", conn, tran);
                            cmdFindRoupa.Parameters.AddWithValue("@Modelo", modelo);
                            cmdFindRoupa.Parameters.AddWithValue("@Tamanho", tamanho);
                            object res = cmdFindRoupa.ExecuteScalar();

                            int idRoupa;

                            if (res != null)
                            {
                                idRoupa = Convert.ToInt32(res);
                            }
                            else
                            {
                                // Criar novo registo na tabela Roupa
                                SqlCommand insertRoupa = new SqlCommand(@"
                            INSERT INTO Roupa (Familia, Modelo, Tamanho, Quantidade)
                            OUTPUT INSERTED.ID
                            VALUES ('Extra', @Modelo, @Tamanho, 0)", conn, tran);
                                insertRoupa.Parameters.AddWithValue("@Modelo", modelo);
                                insertRoupa.Parameters.AddWithValue("@Tamanho", tamanho);
                                idRoupa = Convert.ToInt32(insertRoupa.ExecuteScalar());
                            }

                            // 👉 4. Inserir na RoupaPacote
                            SqlCommand insertPacote = new SqlCommand(@"
                        INSERT INTO RoupaPacote (IDPedReg, IDRoupa, Quantidade)
                        VALUES (@IDPedReg, @IDRoupa, @Quantidade)", conn, tran);
                            insertPacote.Parameters.AddWithValue("@IDPedReg", idPedidoReg);
                            insertPacote.Parameters.AddWithValue("@IDRoupa", idRoupa);
                            insertPacote.Parameters.AddWithValue("@Quantidade", quantidade);
                            insertPacote.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();

                    // 👉 5. Sucesso
                    MessageBox.Show("Entrega registada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Erro ao registar entrega: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 👉 6. Reset visual
            for (int i = 0; i < modelos.Count; i++)
            {
                Label lblVal = form.Controls.Find("lblVal" + (i + 1), true).FirstOrDefault() as Label;
                if (lblVal != null)
                    lblVal.Text = "0";
                vals[i] = "0";
            }
        }

        public void SubmeterPedido(int nrFunc, Form form)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            {
                bool temQuantidade = vals.Any(v => int.TryParse(v, out int q) && q > 0);
                if (!temQuantidade)
                {
                    MessageBox.Show("Não há quantidades válidas para submeter.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    int idPedidoRegisto;

                    // 👉 1. Verificar se já existe PedidoRegistos pendente
                    SqlCommand checkCmd = new SqlCommand(@"
                SELECT TOP 1 ID FROM PedidoRegistos 
                WHERE NrFunc = @NrFunc AND Estado = 'Pendente' 
                ORDER BY Data DESC", conn, tran);
                    checkCmd.Parameters.AddWithValue("@NrFunc", nrFunc);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        idPedidoRegisto = Convert.ToInt32(result);
                    }
                    else
                    {
                        // 👉 2. Criar novo PedidoRegistos
                        SqlCommand insertRegisto = new SqlCommand(@"
                    INSERT INTO PedidoRegistos (Data, NrFunc, Aprovacao, Entrega, PDF)
                    OUTPUT INSERTED.ID
                    VALUES (@Data, @NrFunc, @Aprovacao, @Entrega, 0)", conn, tran);
                        insertRegisto.Parameters.AddWithValue("@Data", DateTime.Now);
                        insertRegisto.Parameters.AddWithValue("@NrFunc", nrFunc);
                        insertRegisto.Parameters.AddWithValue("@Aprovacao", DBNull.Value);
                        insertRegisto.Parameters.AddWithValue("@Entrega", DBNull.Value);
                        idPedidoRegisto = Convert.ToInt32(insertRegisto.ExecuteScalar());
                    }

                    // 👉 3. Inserir os itens no PedidoPacote
                    for (int i = 0; i < modelos.Count; i++)
                    {
                        int quantidade = int.Parse(vals[i]);
                        string modelo = modelos[i];
                        string tamanho = tamanhos[i];

                        if (quantidade > 0)
                        {
                            SqlCommand findIDEPI = new SqlCommand(@"
                        SELECT TOP 1 ID FROM EPI 
                        WHERE Modelo = @Modelo AND Tamanho = @Tamanho", conn, tran);
                            findIDEPI.Parameters.AddWithValue("@Modelo", modelo);
                            findIDEPI.Parameters.AddWithValue("@Tamanho", tamanho);
                            object res = findIDEPI.ExecuteScalar();

                            if (res != null)
                            {
                                int idepi = Convert.ToInt32(res);

                                SqlCommand insertItem = new SqlCommand(@"
                            INSERT INTO PedidoPacote (IDPedReg, IDEPI, Quantidade)
                            VALUES (@IDPedReg, @IDEPI, @Quantidade)", conn, tran);
                                insertItem.Parameters.AddWithValue("@IDPedReg", idPedidoRegisto);
                                insertItem.Parameters.AddWithValue("@IDEPI", idepi);
                                insertItem.Parameters.AddWithValue("@Quantidade", quantidade);
                                insertItem.ExecuteNonQuery();
                            }
                            else
                            {
                                throw new Exception($"Produto não encontrado: {modelo} - {tamanho}");
                            }
                        }
                    }

                    tran.Commit();
                    MessageBox.Show("Pedido submetido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Erro ao submeter pedido: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 👉 4. Reset visual
            for (int i = 0; i < modelos.Count; i++)
            {
                Label lblVal = form.Controls.Find("lblVal" + (i + 1), true).FirstOrDefault() as Label;
                if (lblVal != null)
                    lblVal.Text = "0";
                vals[i] = "0";
            }
        }


        private void ComboBoxTamanho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox combo)
            {
                string nomeCombo = combo.Name; // ex: combo1, combo2...
                int index = int.Parse(nomeCombo.Replace("combo", "")) - 1;

                if (index >= 0 && index < tamanhos.Count)
                {
                    tamanhos[index] = combo.SelectedItem.ToString(); // atualiza o valor escolhido!
                }
            }
        }

        private static int CriarPedidoRegisto(SqlConnection conn, SqlTransaction tran, int nrFunc)
        {
            using (var cmd = new SqlCommand(@"
            INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
            OUTPUT INSERTED.ID
            VALUES (@Data, @NrFunc, 'Pendente');", conn, tran))
            {
                cmd.Parameters.AddWithValue("@Data", DateTime.Now);
                cmd.Parameters.AddWithValue("@NrFunc", nrFunc);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static void ValidaItens(IEnumerable<(int idEpi, string tamanho, int quantidade)> itens)
        {
            if (itens == null) throw new ArgumentNullException(nameof(itens));
            if (!itens.Any() || itens.All(i => i.quantidade <= 0))
                throw new InvalidOperationException("Não há linhas com quantidade > 0.");
        }
        // ------------------------------------------------------------------------------------
        // PEDIDO (vai para PedidoPacote.IDPedReg)
        // ------------------------------------------------------------------------------------
        public int SubmeterPedidoNovo(int nrFunc, List<(int idEpi, string tamanho, int qtd)> itens)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) cria registo do pedido
                        var cmdReg = new SqlCommand(@"
                    INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
                    OUTPUT INSERTED.ID
                    VALUES (@Data, @NrFunc, 'Pendente');", conn, tran);
                        cmdReg.Parameters.AddWithValue("@Data", DateTime.Now);
                        cmdReg.Parameters.AddWithValue("@NrFunc", nrFunc);

                        int idPedReg = Convert.ToInt32(cmdReg.ExecuteScalar());

                        // 2) insere itens no PedidoPacote (IDPedReg, IDEPI, Quantidade)
                        var cmdItem = new SqlCommand(@"
                    INSERT INTO PedidoPacote (IDPedReg, IDEPI, Quantidade)
                    VALUES (@IDPedReg, @IDEPI, @Quantidade);", conn, tran);
                        cmdItem.Parameters.Add("@IDPedReg", SqlDbType.Int);
                        cmdItem.Parameters.Add("@IDEPI", SqlDbType.Int);
                        cmdItem.Parameters.Add("@Quantidade", SqlDbType.Int);

                        foreach (var (idEpi, _, qtd) in itens)
                        {
                            if (qtd <= 0) continue;

                            cmdItem.Parameters["@IDPedReg"].Value = idPedReg;
                            cmdItem.Parameters["@IDEPI"].Value = idEpi;
                            cmdItem.Parameters["@Quantidade"].Value = qtd;
                            cmdItem.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return idPedReg;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }
        // ------------------------------------------------------------------------------------
        // DEVOLUÇÃO (vai para RoupaPacote.IDPedReq)
        //  -> se a tua coluna se chamar diferente (ex: IDPedReg), muda no INSERT.
        //  -> se a coluna do EPI nas devoluções for IDRoupa em vez de IDEPI, muda também.
        // ------------------------------------------------------------------------------------
        public int SubmeterEntregaNovo(int nrFunc, List<(int idRoupa, string tamanho, int qtd)> itens)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) cria registo do pedido (usa o mesmo PedidoRegistos)
                        var cmdReg = new SqlCommand(@"
                    INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
                    OUTPUT INSERTED.ID
                    VALUES (@Data, @NrFunc, 'Pendente');", conn, tran);
                        cmdReg.Parameters.AddWithValue("@Data", DateTime.Now);
                        cmdReg.Parameters.AddWithValue("@NrFunc", nrFunc);

                        int idPedReq = Convert.ToInt32(cmdReg.ExecuteScalar());

                        // 2) insere itens na RoupaPacote (IDPedReq, IDRoupa, Quantidade)
                        var cmdItem = new SqlCommand(@"
                    INSERT INTO RoupaPacote (IDPedReq, IDRoupa, Quantidade)
                    VALUES (@IDPedReq, @IDRoupa, @Quantidade);", conn, tran);
                        cmdItem.Parameters.Add("@IDPedReq", SqlDbType.Int);
                        cmdItem.Parameters.Add("@IDRoupa", SqlDbType.Int);
                        cmdItem.Parameters.Add("@Quantidade", SqlDbType.Int);

                        foreach (var (idRoupa, _, qtd) in itens)
                        {
                            if (qtd <= 0) continue;

                            cmdItem.Parameters["@IDPedReq"].Value = idPedReq;
                            cmdItem.Parameters["@IDRoupa"].Value = idRoupa;   // NOTA: aqui é IDRoupa, não IDEPI
                            cmdItem.Parameters["@Quantidade"].Value = qtd;
                            cmdItem.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return idPedReq;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public int ResolveEpiIdPorModeloTamanho(string modelo, string tamanho)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 ID FROM EPI WHERE Modelo=@m AND Tamanho=@t", conn))
            {
                cmd.Parameters.AddWithValue("@m", modelo);
                cmd.Parameters.AddWithValue("@t", tamanho);
                conn.Open();
                object r = cmd.ExecuteScalar();
                return (r == null || r == DBNull.Value) ? 0 : Convert.ToInt32(r);
            }
        }

        public int ResolveRoupaIdPorModeloTamanho(string modelo, string tamanho)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                // 1) Tentar encontrar na tabela Roupa
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 1 ID FROM Roupa WHERE Modelo = @m AND Tamanho = @t", conn))
                {
                    cmd.Parameters.AddWithValue("@m", modelo);
                    cmd.Parameters.AddWithValue("@t", tamanho);

                    object r = cmd.ExecuteScalar();
                    if (r != null && r != DBNull.Value)
                        return Convert.ToInt32(r);
                }

                // 2) Não existe em Roupa → tentar descobrir família a partir do EPI
                string familia = "Extra"; // fallback
                using (SqlCommand cmdFam = new SqlCommand(
                    "SELECT TOP 1 Familia FROM EPI WHERE Modelo = @m AND Tamanho = @t", conn))
                {
                    cmdFam.Parameters.AddWithValue("@m", modelo);
                    cmdFam.Parameters.AddWithValue("@t", tamanho);

                    object rf = cmdFam.ExecuteScalar();
                    if (rf != null && rf != DBNull.Value)
                        familia = rf.ToString();
                }

                // 3) Criar novo registo em Roupa
                using (SqlCommand insert = new SqlCommand(@"
            INSERT INTO Roupa (Familia, Modelo, Tamanho, Quantidade)
            OUTPUT INSERTED.ID
            VALUES (@Familia, @Modelo, @Tamanho, 0);", conn))
                {
                    insert.Parameters.AddWithValue("@Familia", familia);
                    insert.Parameters.AddWithValue("@Modelo", modelo);
                    insert.Parameters.AddWithValue("@Tamanho", tamanho);

                    int novoId = Convert.ToInt32(insert.ExecuteScalar());
                    return novoId;
                }
            }
        }

        public int GetConsumidoDisponivel(int nrFunc, int idEpi)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(SUM(PP.Quantidade), 0)
                FROM PedidoPacote PP
                INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
                WHERE PR.NrFunc = @nr AND PP.IDEPI = @id;", conn))
            {
                cmd.Parameters.AddWithValue("@nr", nrFunc);
                cmd.Parameters.AddWithValue("@id", idEpi);

                conn.Open();
                object r = cmd.ExecuteScalar();
                return Convert.ToInt32(r);
            }
        }

        public int GetOrCreatePedidoPendente(int nrFunc)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Tentar reaproveitar pedido pendente
                        using (var check = new SqlCommand(@"
                    SELECT TOP 1 ID 
                    FROM PedidoRegistos
                    WHERE NrFunc = @NrFunc AND Estado = 'Pendente'
                    ORDER BY Data DESC;", conn, tran))
                        {
                            check.Parameters.AddWithValue("@NrFunc", nrFunc);
                            object res = check.ExecuteScalar();
                            if (res != null && res != DBNull.Value)
                            {
                                int idExistente = Convert.ToInt32(res);
                                tran.Commit();
                                return idExistente;
                            }
                        }

                        // 2) Não existe → cria novo
                        using (var insert = new SqlCommand(@"
                    INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
                    OUTPUT INSERTED.ID
                    VALUES (@Data, @NrFunc, 'Pendente');", conn, tran))
                        {
                            insert.Parameters.AddWithValue("@Data", DateTime.Now);
                            insert.Parameters.AddWithValue("@NrFunc", nrFunc);
                            int idNovo = Convert.ToInt32(insert.ExecuteScalar());
                            tran.Commit();
                            return idNovo;
                        }
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SubmeterPedidoParaPedidoReg(int idPedReg, List<(int idEpi, string tamanho, int qtd)> itens)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmdItem = new SqlCommand(@"
                    INSERT INTO PedidoPacote (IDPedReg, IDEPI, Quantidade)
                    VALUES (@IDPedReg, @IDEPI, @Quantidade);", conn, tran))
                        {
                            cmdItem.Parameters.Add("@IDPedReg", SqlDbType.Int);
                            cmdItem.Parameters.Add("@IDEPI", SqlDbType.Int);
                            cmdItem.Parameters.Add("@Quantidade", SqlDbType.Int);

                            foreach (var (idEpi, _, qtd) in itens)
                            {
                                if (qtd <= 0) continue;

                                cmdItem.Parameters["@IDPedReg"].Value = idPedReg;
                                cmdItem.Parameters["@IDEPI"].Value = idEpi;
                                cmdItem.Parameters["@Quantidade"].Value = qtd;
                                cmdItem.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SubmeterEntregaParaPedidoReg(int idPedReg, List<(int idRoupa, string tamanho, int qtd)> itens)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmdItem = new SqlCommand(@"INSERT INTO RoupaPacote (IDPedReg, IDRoupa, Quantidade) VALUES (@IDPedReg, @IDRoupa, @Quantidade);", conn, tran))
                        {
                            cmdItem.Parameters.Add("@IDPedReg", SqlDbType.Int);
                            cmdItem.Parameters.Add("@IDRoupa", SqlDbType.Int);
                            cmdItem.Parameters.Add("@Quantidade", SqlDbType.Int);

                            foreach (var (idRoupa, _, qtd) in itens)
                            {
                                if (qtd <= 0) continue;

                                cmdItem.Parameters["@IDPedReg"].Value = idPedReg;
                                cmdItem.Parameters["@IDRoupa"].Value = idRoupa;
                                cmdItem.Parameters["@Quantidade"].Value = qtd;
                                cmdItem.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }


    }
}
