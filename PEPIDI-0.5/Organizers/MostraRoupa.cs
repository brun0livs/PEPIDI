using ClosedXML.Graphics;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.Data.SqlClient;
using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.Data;

namespace PEPIDI
{
    internal class MostraRoupa
    {
        // ====================================================================================
        // 1. OBTER DADOS
        // ====================================================================================

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
                        var familia = reader["Familia"].ToString();

                        resultado.Add(new LinhaPedidoInfo
                        {
                            CodigoEpi = reader["Codigo"].ToString(), 
                            Cor = reader["Cor"].ToString(),          
                            Modelo = modelo,
                            Familia = familia,
                            TamanhoAtual = reader["Tamanho"].ToString(),
                            TamanhosDisponiveis = ObterTamanhosDisponiveis(modelo, reader["Cor"].ToString()),
                            ModelosDisponiveis = ObterModelosPorFamilia(familia)
                        });
                    }
                }
            }
            return resultado;
        }

        // Método auxiliar novo
        private List<string> ObterModelosPorFamilia(string familia)
        {
            var modelos = new List<string>();
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Modelo FROM EPI WHERE Familia = @fam", conn))
            {
                cmd.Parameters.AddWithValue("@fam", familia);
                conn.Open();
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) modelos.Add(r.GetString(0));
            }
            return modelos;
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
                        // No while (reader.Read()) do teu ObterRoupaUsadaPorFuncionarioNovo:
                        var modelo = reader["Modelo"].ToString();
                        var tamanho = reader["Tamanho"].ToString();
                        var familia = reader["Familia"].ToString();
                        var cor = reader["Cor"].ToString(); // Lemos a cor

                        if (!resultado.Any(x => x.Modelo == modelo))
                        {
                            resultado.Add(new LinhaPedidoInfo
                            {
                                CodigoEpi = reader["Codigo"].ToString(), // Era IDEPI
                                Cor = cor,                               // Guardamos a cor na mochila
                                Modelo = modelo,
                                Familia = familia,
                                TamanhoAtual = tamanho,
                                TamanhosDisponiveis = ObterTamanhosUsadosPorFuncionario(modelo, nrFuncionario)
                            });
                        }
                    }
                }
            }
            return resultado;
        }

        public List<string> ObterTamanhosUsadosPorFuncionario(string modelo, int nrFunc)
        {
            List<string> tamanhos = new List<string>();

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT DISTINCT E.Tamanho FROM PedidoPacote PP
        INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
        INNER JOIN EPI E ON PP.CodigoEPI = E.Codigo -- <--- AQUI TAMBÉM MUDOU
        WHERE PR.NrFunc = @NrFunc AND E.Modelo = @Modelo", conn))
            {
                cmd.Parameters.AddWithValue("@NrFunc", nrFunc);
                cmd.Parameters.AddWithValue("@Modelo", modelo);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tamanhos.Add(reader["Tamanho"].ToString().Trim());
                    }
                }
            }
            return tamanhos;
        }

        // 1. Atualizar o método que chama a tua SP alterada
        private List<string> ObterTamanhosDisponiveis(string modelo, string cor) // <-- Adicionado string cor
        {
            List<string> tamanhos = new List<string>();
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetTamanhosPorModelo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Modelo", modelo);
                cmd.Parameters.AddWithValue("@Cor", cor); // <-- Passar o parâmetro novo!

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tamanhos.Add(reader.GetString(0).Trim());
                    }
                }
            }
            return tamanhos;
        }

        // 2. Atualizar o Resolutor de Código para ser 100% à prova de bala
        public string ResolveCodigoEpi(string modelo, string tamanho, string cor)
        {
            string codigo = null;

            // 1. Limpa os espaços. Se a cor vier nula/vazia, assumimos que é "00"
            modelo = modelo?.Trim() ?? "";
            tamanho = tamanho?.Trim() ?? "";
            cor = string.IsNullOrWhiteSpace(cor) ? "00" : cor.Trim();

            // 2. O LTRIM e RTRIM no SQL limpam os espaços invisíveis nas colunas
            // O ISNULL(Cor, '00') garante que se o SQL tiver a cor a NULL, ele lê como '00'
            string sql = @"SELECT TOP 1 Codigo 
                            FROM EPI 
                            WHERE LTRIM(RTRIM(Modelo)) = @m 
                              AND LTRIM(RTRIM(Tamanho)) = @t 
                              AND ISNULL(Cor, '00') = @c 
                              AND Ativo = 1";

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@m", modelo);
                cmd.Parameters.AddWithValue("@t", tamanho);
                cmd.Parameters.AddWithValue("@c", cor);

                try
                {
                    conn.Open();
                    object res = cmd.ExecuteScalar();
                    if (res != null && res != DBNull.Value)
                    {
                        codigo = res.ToString();
                    }
                }
                catch { /* Opcional: Console.WriteLine(ex.Message) para debugar */ }
            }

            return codigo;
        }

        // ATENÇÃO: Mudou de int idEpi para string codigoEpi
        public int GetConsumidoDisponivel(int nrFunc, string codigoEpi)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"SELECT ISNULL(SUM(PP.Quantidade), 0) FROM PedidoPacote PP 
                                                    INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
                                                    WHERE PR.NrFunc = @nr AND PP.CodigoEPI = @codigo;", conn)) 
            {
                cmd.Parameters.AddWithValue("@nr", nrFunc);
                cmd.Parameters.AddWithValue("@codigo", codigoEpi);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ====================================================================================
        // 2. RESOLVEDORES DE IDs
        // ====================================================================================



        // ====================================================================================
        // 3. SUBMISSÃO DE PEDIDOS E ENTREGAS (MÉTODOS NOVOS LIMPOS)
        // ====================================================================================



        // 2. O INSERTER DE PEDIDOS (CEGOS)
        public void GravarPedidosCegos(int idPedReg, List<(string codigoEpi, int qtd)> pedidos)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                foreach (var p in pedidos)
                {
                    // O IDStock vai a NULL propositadamente!
                    string sql = "INSERT INTO PedidoPacote (IDPedReg, CodigoEPI, Quantidade, IDStock) VALUES (@idReg, @codigo, @qtd, NULL)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@idReg", idPedReg);
                        cmd.Parameters.AddWithValue("@codigo", p.codigoEpi);
                        cmd.Parameters.AddWithValue("@qtd", p.qtd);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // 3. O INSERTER DE DEVOLUÇÕES (CEGAS)
        public void GravarDevolucoesCegas(int idPedReg, List<(string codigoEpi, int qtd)> devolucoes)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                foreach (var d in devolucoes)
                {
                    // IDStock a NULL. O RH decide para onde vai esta devolução mais tarde.
                    string sql = "INSERT INTO RoupaPacote (IDPedReg, CodigoEPI, Quantidade, IDStock) VALUES (@idReg, @codigo, @qtd, NULL)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@idReg", idPedReg);
                        cmd.Parameters.AddWithValue("@codigo", d.codigoEpi);
                        cmd.Parameters.AddWithValue("@qtd", d.qtd);
                        cmd.ExecuteNonQuery();
                    }
                }
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
                        using (var check = new SqlCommand(@"
                            SELECT TOP 1 ID FROM PedidoRegistos
                            WHERE NrFunc = @NrFunc AND Estado = 'Pendente'
                            ORDER BY Data DESC;", conn, tran))
                        {
                            check.Parameters.AddWithValue("@NrFunc", nrFunc);
                            object res = check.ExecuteScalar();
                            if (res != null && res != DBNull.Value)
                            {
                                tran.Commit();
                                return Convert.ToInt32(res);
                            }
                        }

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

        // ATENÇÃO: Mudou de int idEpi para string codigoEpi
        public void AtualizarTamanhoPadraoFuncionario(int nrFunc, string codigoEpi, string novoTamanho)
        {
            try
            {
                using (var conn = GetConn.GetConnection())
                {
                    conn.Open();

                    string familia = "";
                    // Procura por Codigo em vez de ID
                    using (var cmdBusca = new SqlCommand("SELECT Familia FROM EPI WHERE Codigo = @Codigo", conn))
                    {
                        cmdBusca.Parameters.AddWithValue("@Codigo", codigoEpi);
                        var result = cmdBusca.ExecuteScalar();

                        if (result != null)
                            familia = result.ToString().ToLower().Trim();
                    }

                    if (string.IsNullOrEmpty(familia)) return;

                    string colunaTabela = "";
                    if (familia.Contains("t-shirt") || familia.Contains("tshirt")) colunaTabela = "TShirt";
                    else if (familia.Contains("casaco")) colunaTabela = "Casaco";
                    else if (familia.Contains("polo manga curta") || familia.Contains("polo m. curta")) colunaTabela = "PoloMCurta";
                    else if (familia.Contains("polo manga comprida") || familia.Contains("polo m. comprida")) colunaTabela = "PoloMCompr";
                    else if (familia.Contains("calça") || familia.Contains("calca")) colunaTabela = "Calca";
                    else if (familia.Contains("sapato") || familia.Contains("calçado") || familia.Contains("bota")) colunaTabela = "Sapato";
                    else if (familia.Contains("bata")) colunaTabela = "Bata";
                    else return;

                    string query = $"UPDATE Funcionarios SET {colunaTabela} = @Tamanho WHERE Nr = @NrFunc";
                    using (var cmdUpdate = new SqlCommand(query, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Tamanho", novoTamanho);
                        cmdUpdate.Parameters.AddWithValue("@NrFunc", nrFunc);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch { /* Ignorado silenciosamente */ }
        }

        public string ObterUltimoModeloConsumidoPorFamilia(int nrFunc, string familia)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            {
                // AQUI: Trocámos PP.IDEPI = E.ID por PP.CodigoEPI = E.Codigo
                string sql = @"
        SELECT TOP 1 E.Modelo 
        FROM PedidoPacote PP
        INNER JOIN PedidoRegistos PR ON PP.IDPedReg = PR.ID
        INNER JOIN EPI E ON PP.CodigoEPI = E.Codigo 
        WHERE PR.NrFunc = @nr AND E.Familia = @fam
        ORDER BY PR.Data DESC, PR.ID DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nr", nrFunc);
                    cmd.Parameters.AddWithValue("@fam", familia);
                    conn.Open();
                    object res = cmd.ExecuteScalar();
                    return res?.ToString() ?? string.Empty;
                }
            }
        }
    }
}