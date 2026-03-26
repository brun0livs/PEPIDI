using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace PEPIDI.Organizers
{
    public static class MotorIA
    {
        // Mudámos para LISTA para garantir que o ORDER BY LEN do SQL seja respeitado no loop
        private static List<KeyValuePair<string, string>> _regrasFamilia = new List<KeyValuePair<string, string>>();
        private static List<KeyValuePair<string, string>> _regrasFuncao = new List<KeyValuePair<string, string>>();

        public static void CarregarRegrasDaBD()
        {
            _regrasFamilia.Clear();
            _regrasFuncao.Clear();

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                // FAMÍLIA (STOCK) - Mais compridas primeiro!
                string sqlFam = "SELECT PalavraChave, FamiliaDestino FROM RegrasFamilia ORDER BY LEN(PalavraChave) DESC";
                using (SqlCommand cmd = new SqlCommand(sqlFam, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _regrasFamilia.Add(new KeyValuePair<string, string>(
                            rdr["PalavraChave"].ToString().ToLower().Trim(),
                            rdr["FamiliaDestino"].ToString().Trim()));
                    }
                }

                // FUNÇÃO (FUNCIONÁRIOS)
                string sqlFunc = "SELECT PalavraChave, FuncaoDestino FROM RegrasFuncao ORDER BY LEN(PalavraChave) DESC";
                using (SqlCommand cmd = new SqlCommand(sqlFunc, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _regrasFuncao.Add(new KeyValuePair<string, string>(
                            rdr["PalavraChave"].ToString().ToLower().Trim(),
                            rdr["FuncaoDestino"].ToString().Trim()));
                    }
                }
            }
        }

        public static string CorrigirFamilia(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "Verificar Colagem";
            string busca = texto.ToLower().Trim();

            // O Loop agora respeita a ordem: testa "Polo Manga Comprida" ANTES de "Polo"
            foreach (var regra in _regrasFamilia)
            {
                if (busca.Contains(regra.Key)) return regra.Value;
            }
            return "Verificar Colagem";
        }

        public static string CorrigirFuncao(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "Verificar Colagem";
            string busca = texto.ToLower().Trim();

            foreach (var regra in _regrasFuncao)
            {
                if (busca.Contains(regra.Key)) return regra.Value;
            }
            return "Verificar Colagem";
        }

        public static void AprenderNovaRegra(string errado, string certo, string tipo)
        {
            if (string.IsNullOrWhiteSpace(errado) || string.IsNullOrWhiteSpace(certo)) return;
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                string tabela = (tipo == "Familia") ? "RegrasFamilia" : "RegrasFuncao";
                string col = (tipo == "Familia") ? "FamiliaDestino" : "FuncaoDestino";
                string sql = $@"IF EXISTS (SELECT 1 FROM {tabela} WHERE PalavraChave = @e) 
                                UPDATE {tabela} SET {col} = @c WHERE PalavraChave = @e 
                                ELSE INSERT INTO {tabela} (PalavraChave, {col}) VALUES (@e, @c)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@e", errado.ToLower().Trim());
                    cmd.Parameters.AddWithValue("@c", certo.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarRegrasDaBD();
        }
    }
}