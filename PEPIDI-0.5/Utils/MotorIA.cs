using Microsoft.Data.SqlClient;
using PEPIDI.Models;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PEPIDI.Utils
{
    public static class MotorIA
    {
        private static Dictionary<string, string> _regrasFamilia = new Dictionary<string, string>();
        private static List<string> _funcoesOficiais = new List<string>();
        private static Dictionary<string, string> _regrasFuncao = new Dictionary<string, string>();

        public static void Inicializar(Dictionary<string, string> regrasFam, List<string> funcoes, Dictionary<string, string> regrasFunc)
        {
            _regrasFamilia = regrasFam;
            _funcoesOficiais = funcoes;
            _regrasFuncao = regrasFunc;
        }

        public static void CarregarRegrasDaBD()
        {
            _regrasFamilia.Clear();
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                // Lemos as regras de Família
                using (SqlCommand cmd = new SqlCommand("SELECT PalavraChave, FamiliaDestino FROM RegrasFamilia", conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string chave = rdr.GetString(0).ToLower().Trim();
                        string destino = rdr.GetString(1).Trim();
                        if (!_regrasFamilia.ContainsKey(chave)) _regrasFamilia.Add(chave, destino);
                    }
                }
            }
        }

        public static string DetetarFamilia(string modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo)) return "Null";

            // Passamos o modelo para minúsculas para bater com a PalavraChave da BD
            string m = modelo.ToLower().Trim();

            foreach (var regra in _regrasFamilia)
            {
                // Se o modelo contiver a palavra-chave (ex: "t-shirt")
                if (m.Contains(regra.Key))
                {
                    return regra.Value;
                }
            }
            return "Null";
        }

        public static string CorrigirFuncao(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "Verificar Colagem";
            string busca = input.Trim();

            // 1. Verificação exata
            var exata = _funcoesOficiais.FirstOrDefault(f => f.Equals(busca, StringComparison.OrdinalIgnoreCase));
            if (exata != null) return exata;

            // 2. Dicionário de Sinónimos aprendidos pela IA
            if (_regrasFuncao.ContainsKey(busca.ToLower())) return _regrasFuncao[busca.ToLower()];

            // 3. Levenshtein (Erros ortográficos)
            string melhorMatch = null;
            int menorDistancia = int.MaxValue;
            foreach (var oficial in _funcoesOficiais)
            {
                int dist = CalcularDistancia(busca.ToLower(), oficial.ToLower());
                if (dist < menorDistancia && dist <= 2) { menorDistancia = dist; melhorMatch = oficial; }
            }

            return melhorMatch ?? "Verificar Colagem";
        }

        public static void AprenderNovaRegra(string termoErrado, string termoOficial, string tipo)
        {
            if (string.IsNullOrWhiteSpace(termoErrado) || termoErrado == termoOficial || termoErrado == "Verificar Colagem") return;

            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                string tabela = (tipo == "Funcao") ? "RegrasFuncao" : "RegrasFamilia";
                string colDestino = (tipo == "Funcao") ? "FuncaoDestino" : "FamiliaDestino";

                string query = $@"IF EXISTS (SELECT 1 FROM {tabela} WHERE PalavraChave = @chave)
                                UPDATE {tabela} SET {colDestino} = @destino WHERE PalavraChave = @chave
                                ELSE INSERT INTO {tabela} (PalavraChave, {colDestino}) VALUES (@chave, @destino)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@chave", termoErrado.ToLower().Trim());
                    cmd.Parameters.AddWithValue("@destino", termoOficial.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static int CalcularDistancia(string s, string t)
        {
            int n = s.Length, m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0) return m; if (m == 0) return n;
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + ((t[j - 1] == s[i - 1]) ? 0 : 1));
            return d[n, m];
        }
    }
}