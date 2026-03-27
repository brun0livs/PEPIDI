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

        public static void AprenderNovaRegra(string errado, string certo, string tipo, bool recarregar = true)
        {
            // 1. Validação básica para não guardar lixo
            if (string.IsNullOrWhiteSpace(errado) || string.IsNullOrWhiteSpace(certo)) return;

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                // 2. Define dinamicamente a tabela e a coluna alvo
                string tabela = (tipo == "Familia") ? "RegrasFamilia" : "RegrasFuncao";
                string colDestino = (tipo == "Familia") ? "FamiliaDestino" : "FuncaoDestino";

                // 3. UPSERT: Se a palavra-chave já existe, atualiza o destino. Se não, insere nova.
                string sql = $@"
            IF EXISTS (SELECT 1 FROM {tabela} WHERE PalavraChave = @e)
                UPDATE {tabela} SET {colDestino} = @c WHERE PalavraChave = @e
            ELSE
                INSERT INTO {tabela} (PalavraChave, {colDestino}) VALUES (@e, @c)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Limpamos e pomos em lowercase para a IA não ser sensível a Caps Lock
                    cmd.Parameters.AddWithValue("@e", errado.ToLower().Trim());
                    cmd.Parameters.AddWithValue("@c", certo.Trim());
                    cmd.ExecuteNonQuery();
                }
            }

            // 4. A MAGIA DA PERFORMANCE:
            // Só recarrega a lista da RAM se o utilizador pedir (padrão é sim)
            if (recarregar)
            {
                CarregarRegrasDaBD();
            }
        }
    }
}