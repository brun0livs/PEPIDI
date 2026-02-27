using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

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
                        var tamanho = reader["Tamanho"].ToString();

                        resultado.Add(new LinhaPedidoInfo
                        {
                            IdEpi = Convert.ToInt32(reader["ID"]),
                            Modelo = modelo,
                            TamanhoAtual = tamanho,
                            TamanhosDisponiveis = ObterTamanhosDisponiveis(modelo)
                        });
                    }
                }
            }
            return resultado;
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

                        resultado.Add(new LinhaPedidoInfo
                        {
                            IdEpi = Convert.ToInt32(reader["IDEPI"]),
                            Modelo = modelo,
                            TamanhoAtual = tamanho,
                            TamanhosDisponiveis = ObterTamanhosUsadosPorFuncionario(modelo, nrFuncionario)
                        });
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
                        tamanhos.Add(reader["Tamanho"].ToString().Trim());
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
                        tamanhos.Add(reader.GetString(0).Trim());
                    }
                }
            }
            return tamanhos;
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
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ====================================================================================
        // 2. RESOLVEDORES DE IDs
        // ====================================================================================

        public int ResolveEpiIdPorModeloTamanho(string modelo, string tamanho)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ID FROM EPI WHERE Modelo=@m AND Tamanho=@t", conn))
            {
                cmd.Parameters.AddWithValue("@m", modelo.Trim());
                cmd.Parameters.AddWithValue("@t", tamanho.Trim());
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

                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ID FROM Roupa WHERE Modelo = @m AND Tamanho = @t", conn))
                {
                    cmd.Parameters.AddWithValue("@m", modelo.Trim());
                    cmd.Parameters.AddWithValue("@t", tamanho.Trim());

                    object r = cmd.ExecuteScalar();
                    if (r != null && r != DBNull.Value)
                        return Convert.ToInt32(r);
                }

                string familia = "Extra";
                using (SqlCommand cmdFam = new SqlCommand("SELECT TOP 1 Familia FROM EPI WHERE Modelo = @m AND Tamanho = @t", conn))
                {
                    cmdFam.Parameters.AddWithValue("@m", modelo.Trim());
                    cmdFam.Parameters.AddWithValue("@t", tamanho.Trim());
                    object rf = cmdFam.ExecuteScalar();
                    if (rf != null && rf != DBNull.Value) familia = rf.ToString();
                }

                using (SqlCommand insert = new SqlCommand(@"
                    INSERT INTO Roupa (Familia, Modelo, Tamanho, Quantidade)
                    OUTPUT INSERTED.ID
                    VALUES (@Familia, @Modelo, @Tamanho, 0);", conn))
                {
                    insert.Parameters.AddWithValue("@Familia", familia);
                    insert.Parameters.AddWithValue("@Modelo", modelo.Trim());
                    insert.Parameters.AddWithValue("@Tamanho", tamanho.Trim());
                    return Convert.ToInt32(insert.ExecuteScalar());
                }
            }
        }

        // ====================================================================================
        // 3. SUBMISSÃO DE PEDIDOS E ENTREGAS (MÉTODOS NOVOS LIMPOS)
        // ====================================================================================

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

        public int SubmeterPedidoNovo(int nrFunc, List<(int idEpi, string tamanho, int qtd)> itens)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmdReg = new SqlCommand(@"
                            INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
                            OUTPUT INSERTED.ID
                            VALUES (@Data, @NrFunc, 'Pendente');", conn, tran);
                        cmdReg.Parameters.AddWithValue("@Data", DateTime.Now);
                        cmdReg.Parameters.AddWithValue("@NrFunc", nrFunc);

                        int idPedReg = Convert.ToInt32(cmdReg.ExecuteScalar());

                        var cmdItem = new SqlCommand(@"
                            IF EXISTS (SELECT 1 FROM PedidoPacote WHERE IDPedReg = @IDPedReg AND IDEPI = @IDEPI)
                            BEGIN
                            -- Se já existe, SOMA à quantidade que lá está
                            UPDATE PedidoPacote 
                            SET Quantidade = Quantidade + @Quantidade 
                            WHERE IDPedReg = @IDPedReg AND IDEPI = @IDEPI;
                            END
                            ELSE
                            BEGIN
                            -- Se não existe, INSERE uma nova linha
                            INSERT INTO PedidoPacote (IDPedReg, IDEPI, Quantidade)
                            VALUES (@IDPedReg, @IDEPI, @Quantidade);
                            END", conn, tran);
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

        public int SubmeterEntregaNovo(int nrFunc, List<(int idRoupa, string tamanho, int qtd)> itens)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var cmdReg = new SqlCommand(@"
                            INSERT INTO PedidoRegistos (Data, NrFunc, Estado)
                            OUTPUT INSERTED.ID
                            VALUES (@Data, @NrFunc, 'Pendente');", conn, tran);
                        cmdReg.Parameters.AddWithValue("@Data", DateTime.Now);
                        cmdReg.Parameters.AddWithValue("@NrFunc", nrFunc);

                        int idPedReg = Convert.ToInt32(cmdReg.ExecuteScalar());

                        var cmdItem = new SqlCommand(@"
                            IF EXISTS (SELECT 1 FROM RoupaPacote WHERE IDPedReg = @IDPedReg AND IDRoupa = @IDRoupa)
                            BEGIN
                            -- Se já existe, SOMA à quantidade
                            UPDATE RoupaPacote 
                            SET Quantidade = Quantidade + @Quantidade 
                            WHERE IDPedReg = @IDPedReg AND IDRoupa = @IDRoupa;
                            END
                            ELSE
                            BEGIN
                            -- Se não existe, INSERE uma nova linha
                            INSERT INTO RoupaPacote (IDPedReg, IDRoupa, Quantidade)
                            VALUES (@IDPedReg, @IDRoupa, @Quantidade);
                            END", conn, tran);
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
                            IF EXISTS (SELECT 1 FROM PedidoPacote WHERE IDPedReg = @IDPedReg AND IDEPI = @IDEPI)
                            BEGIN
                            -- Se já existe, SOMA à quantidade que lá está
                            UPDATE PedidoPacote 
                            SET Quantidade = Quantidade + @Quantidade 
                            WHERE IDPedReg = @IDPedReg AND IDEPI = @IDEPI;
                            END
                            ELSE
                            BEGIN
                            -- Se não existe, INSERE uma nova linha
                            INSERT INTO PedidoPacote (IDPedReg, IDEPI, Quantidade)
                            VALUES (@IDPedReg, @IDEPI, @Quantidade);
                            END", conn, tran))
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
                        using (var cmdItem = new SqlCommand(@"
                            IF EXISTS (SELECT 1 FROM RoupaPacote WHERE IDPedReg = @IDPedReg AND IDRoupa = @IDRoupa)
                            BEGIN
                            -- Se já existe, SOMA à quantidade
                            UPDATE RoupaPacote 
                            SET Quantidade = Quantidade + @Quantidade 
                            WHERE IDPedReg = @IDPedReg AND IDRoupa = @IDRoupa;
                            END
                            ELSE
                            BEGIN
                            -- Se não existe, INSERE uma nova linha
                            INSERT INTO RoupaPacote (IDPedReg, IDRoupa, Quantidade)
                            VALUES (@IDPedReg, @IDRoupa, @Quantidade);
                            END", conn, tran))
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