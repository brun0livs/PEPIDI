using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace PEPIDI.Organizers
{
    internal class GestorDePedidos
    {
        public const int MODO_NORMAL = 0;
        public const int MODO_QUANTIDADE = 1;
        public const int MODO_TAMANHO = 2;
        public const int MODO_AMBOS = 3;
        public Dictionary<int, int> quantidadesSelecionadas = new Dictionary<int, int>();
        EfeitoUI M = new EfeitoUI();

        public DataTable CarregarPedidosPorEstado(string estado)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_CarregarPedidosPorEstado", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Estado", estado);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt; 
            }
        }

        public void Aprovar(int idPedido, int idAprovador, DataGridView dgvPacote, string txtObservacoes, Dictionary<int, Tuple<int, int>> quantidadesAlteradas)
        {

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    List<string> faltas = new List<string>();
                    List<Tuple<int, int>> aprovados = new List<Tuple<int, int>>();

                    foreach (DataGridViewRow row in dgvPacote.Rows)
                    {
                        if (row.IsNewRow) continue;
                        if (!int.TryParse(row.Cells["ID"]?.Value?.ToString(), out int idEPI)) continue;

                        int original = 0;
                        if (row.Cells["QuantidadePedida"]?.Value != null)
                            int.TryParse(row.Cells["QuantidadePedida"].Value.ToString(), out original);

                        int atual = original;

                        if (quantidadesAlteradas != null && quantidadesAlteradas.ContainsKey(idEPI))
                        {
                            atual = quantidadesAlteradas[idEPI].Item2;
                        }
                        else if (row.Cells["Quantidade"]?.Value != null)
                        {
                            int.TryParse(row.Cells["Quantidade"].Value.ToString(), out atual);
                        }

                        SqlCommand cmd = new SqlCommand("SELECT Quantidade FROM EPI WHERE ID = @ID", conn, tran);
                        cmd.Parameters.AddWithValue("@ID", idEPI);
                        int stock = Convert.ToInt32(cmd.ExecuteScalar());

                        Debug.WriteLine($"[Aprovar] EPI {idEPI} → Pedido: {atual}, Stock: {stock}");

                        if (atual > stock)
                        {
                            string modelo = row.Cells["Modelo"]?.Value?.ToString() ?? "Desconhecido";
                            faltas.Add($"{modelo}: Pedido: {atual}, Stock: {stock}");
                        }
                        else
                        {
                            SqlCommand atualiza = new SqlCommand("sp_AtualizarQuantidadePedidoPacote", conn, tran)
                            {
                                CommandType = CommandType.StoredProcedure
                            };
                            atualiza.Parameters.AddWithValue("@IDPedido", idPedido);
                            atualiza.Parameters.AddWithValue("@IDEPI", idEPI);
                            atualiza.Parameters.AddWithValue("@NovaQuantidade", atual);
                            atualiza.ExecuteNonQuery();

                            Debug.WriteLine($"[Aprovar] SP executada → Pedido: {idPedido}, EPI: {idEPI}, Quantidade: {atual}");
                        }
                    }

                    if (faltas.Count > 0)
                    {
                        tran.Rollback();
                        M.AbrirMensagem("Falta de stock:\n\n" + string.Join("\n", faltas), "Erro de Stock");
                        return;
                    }

                    // Subtrair stock
                    foreach (var item in aprovados)
                    {
                        SqlCommand upd = new SqlCommand("UPDATE EPI SET Quantidade = Quantidade - @Q WHERE ID = @ID", conn, tran);
                        upd.Parameters.AddWithValue("@Q", item.Item2);
                        upd.Parameters.AddWithValue("@ID", item.Item1);
                        upd.ExecuteNonQuery();

                        Debug.WriteLine($"[Aprovar] Stock subtraído: {item.Item2} → EPI {item.Item1}");
                    }

                    // Atualizar PedidoRegistos com observações
                    SqlCommand aprova = new SqlCommand(@"
                UPDATE PedidoRegistos 
                SET Estado = 'Aprovado',
                    Aprovacao = @Aprovador,
                    Notas = @Notas
                WHERE ID = @ID", conn, tran);

                    aprova.Parameters.AddWithValue("@ID", idPedido);
                    aprova.Parameters.AddWithValue("@Aprovador", idAprovador);
                    aprova.Parameters.AddWithValue("@Notas", txtObservacoes);
                    aprova.ExecuteNonQuery();

                    tran.Commit();
                    M.AbrirMensagem("Pedido aprovado com sucesso!", "Aprovado");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    M.AbrirMensagem("Erro ao aprovar pedido: " + ex.Message, "Erro");
                }
            }
        }

        public void LimparVisualDGVPPacote(DataGridView dgv, int modoVisualizacao)
        {
            Debug.WriteLine($"[LimparVisualDGVPPacote] Modo: {modoVisualizacao}");

            if (dgv.Columns.Count == 0)
            {
                Debug.WriteLine("[LimparVisualDGVPPacote] DGV sem colunas.");
                return;
            }

            string[] ocultarFixas = { "ID", "IDResp", "PDF", "Entrega", "QuantidadePedida" };
            foreach (string col in ocultarFixas)
                if (dgv.Columns.Contains(col)) dgv.Columns[col].Visible = false;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.Visible = false;

            if (dgv.Columns.Contains("Modelo"))
            {
                var col = dgv.Columns["Modelo"];
                col.Visible = true;
                col.DisplayIndex = 0;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Debug.WriteLine("[LimparVisualDGVPPacote] Modelo visível.");
            }

            if (modoVisualizacao == MODO_QUANTIDADE)
            {
                if (dgv.Columns.Contains("Tamanho"))
                {
                    var col = dgv.Columns["Tamanho"];
                    col.Visible = true;
                    col.DisplayIndex = 1;
                }

                if (dgv.Columns.Contains("QuantidadeDisponivel"))
                {
                    var col = dgv.Columns["QuantidadeDisponivel"];
                    col.Visible = true;
                    col.DisplayIndex = 2;
                }

                if (dgv.Columns.Contains("QuantidadeCombo"))
                {
                    var col = dgv.Columns["QuantidadeCombo"];
                    col.Visible = true;
                    col.DisplayIndex = 3;
                }

                Debug.WriteLine("[LimparVisualDGVPPacote] Modo Quantidade configurado.");
            }
            else if (modoVisualizacao == MODO_TAMANHO)
            {
                if (dgv.Columns.Contains("TamanhoCombo"))
                {
                    var col = dgv.Columns["TamanhoCombo"];
                    col.Visible = true;
                    col.DisplayIndex = 1;
                }

                if (dgv.Columns.Contains("Quantidade"))
                {
                    var col = dgv.Columns["Quantidade"];
                    col.Visible = true;
                    col.DisplayIndex = 2;
                }

                Debug.WriteLine("[LimparVisualDGVPPacote] Modo Tamanho configurado.");
            }
            else if (modoVisualizacao == MODO_AMBOS)
            {
                if (dgv.Columns.Contains("Tamanho"))
                {
                    var col = dgv.Columns["Tamanho"];
                    col.Visible = true;
                    col.DisplayIndex = 1;
                }

                if (dgv.Columns.Contains("QuantidadeCombo"))
                {
                    var col = dgv.Columns["QuantidadeCombo"];
                    col.Visible = true;
                    col.DisplayIndex = 2;
                }

                Debug.WriteLine("[LimparVisualDGVPPacote] Modo Ambos configurado.");
            }
        }

        public List<string> ObterTamanhosPorModelo(string modelo)
        {
            List<string> tamanhos = new List<string>();
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetTamanhosPorModelo", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Modelo", modelo);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        tamanhos.Add(reader["Tamanho"].ToString());
                }
            }
            return tamanhos;
        }

        public void Reprovar(int idPedido, int idReprovador, RichTextBox txtObservacoes)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(@"
            UPDATE PedidoRegistos
            SET Estado = 'Rejeitado',
                Aprovacao = @Reprovador,
                Entrega = '-',
                PDF = '-',
                Notas = @Notas
            WHERE ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", idPedido);
                    cmd.Parameters.AddWithValue("@Reprovador", idReprovador);
                    cmd.Parameters.AddWithValue("@Notas", txtObservacoes.Text);

                    cmd.ExecuteNonQuery();
                }

                M.AbrirMensagem("Pedido reprovado com sucesso!", "Reprovado");
            }
        }
    }
}
