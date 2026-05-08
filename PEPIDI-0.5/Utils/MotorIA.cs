using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace PEPIDI.Organizers
{
    public static class MotorIA
    {
        private static List<KeyValuePair<string, string>> _regrasFamilia = new List<KeyValuePair<string, string>>();
        private static List<KeyValuePair<string, string>> _regrasFuncao = new List<KeyValuePair<string, string>>();

        // --- NOVA LISTA PARA CACHE DAS CORES ---
        private static List<string> _coresConhecidas = new List<string>();

        public static void CarregarRegrasDaBD()
        {
            _regrasFamilia.Clear();
            _regrasFuncao.Clear();
            _coresConhecidas.Clear(); // Limpar a cache de cores

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                // 1. FAMÍLIA (STOCK)
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

                // 2. FUNÇÃO (FUNCIONÁRIOS)
                string sqlFunc = "SELECT PalavraChave, FuncaoDestino FROM RegrasFuncao ORDER BY LEN(PalavraChave) DESC";
                using (SqlCommand cmd = new SqlCommand(sqlFunc, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _regrasFuncao.Add(new KeyValuePair<string, string>(
                            rdr["PalavraChave"].ToString().ToLower().Trim(),
                            rdr["FuncaoDestino"].ToString().Trim()
                        ));
                    }
                }

                // --- 3. CARREGAR AS CORES OFICIAIS ---
                // O ORDER BY LEN DESC garante que "Azul Escuro" é testado antes de "Azul"
                string sqlCor = "SELECT Nome FROM Cor ORDER BY LEN(Nome) DESC";
                using (SqlCommand cmd = new SqlCommand(sqlCor, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        _coresConhecidas.Add(rdr["Nome"].ToString().Trim());
                    }
                }
            }
        }

        // ... (CorrigirFamilia e CorrigirFuncao mantêm-se iguais)
        public static string CorrigirFamilia(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "Verificar Colagem";
            string busca = texto.ToLower().Trim();

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

        // --- NOVO MÉTODO PARA EXTRAIR A COR ---
        public static string ExtrairCorDoModelo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            string busca = texto.ToLower().Trim();

            // Varre as cores conhecidas carregadas da BD
            foreach (string cor in _coresConhecidas)
            {
                // Se a string do excel (ex: "T-Shirt Azul Escuro") contiver a cor da BD
                if (busca.Contains(cor.ToLower()))
                {
                    return cor; // Retorna o nome oficial (útil para selecionar na ComboBox)
                }
            }

            return ""; // Retorna vazio se não encontrar menção a cores
        }

        public static void AprenderNovaRegra(string errado, string certo, string tipo, bool recarregar = true)
        {
            if (string.IsNullOrWhiteSpace(errado) || string.IsNullOrWhiteSpace(certo)) return;

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();

                string tabela = (tipo == "Familia") ? "RegrasFamilia" : "RegrasFuncao";
                string colDestino = (tipo == "Familia") ? "FamiliaDestino" : "FuncaoDestino";

                string sql = $@"
            IF EXISTS (SELECT 1 FROM {tabela} WHERE PalavraChave = @e)
                UPDATE {tabela} SET {colDestino} = @c WHERE PalavraChave = @e
            ELSE
                INSERT INTO {tabela} (PalavraChave, {colDestino}) VALUES (@e, @c)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@e", errado.ToLower().Trim());
                    cmd.Parameters.AddWithValue("@c", certo.Trim());
                    cmd.ExecuteNonQuery();
                }
            }

            if (recarregar)
            {
                CarregarRegrasDaBD();
            }
        }
    }
}