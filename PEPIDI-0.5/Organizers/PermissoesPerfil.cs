using System;
using System.Data.SqlClient;

namespace PEPIDI.Organizers
{
    // Criei esta classe para "arrumar" o método
    public class PermissoesPerfil
    {
        public int NivelAcesso { get; set; }
        public bool PodeSubmeter { get; set; }
        public bool PodeVerStock { get; set; }
        public bool PodeInserirStock { get; set; }
        public bool PodeCriarStock { get; set; }
        public bool PodeVerHistorico { get; set; }
        public bool PodeEditarFunc { get; set; }
        public bool PodeAprovar { get; set; }
        public bool PodeEntregar { get; set; }
        public bool PodeCriarFuncoes { get; set; }
        public bool PodeAlterarDefinicoes { get; set; }

        // Mudei de 'void' para 'PermissoesPerfil' e pus 'static' para ser mais fácil usar
        public static PermissoesPerfil VerPermissoes(int idFuncionario)
        {
            // Confirma se o teu GetConn devolve uma string ou uma conexão já aberta.
            // Vou assumir o padrão que usámos antes:
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                    NivelAcesso, PodeVerStock, PodeInserirStock, PodeCriarStock, PodeVerHistorico, PodeEditarFunc,
                    PodeSubmeter, PodeAprovar, PodeEntregar, PodeCriarFuncoes, PodeAlterarDefinicoes
                    FROM Funcionarios FU
                    JOIN Funcoes F ON FU.Funcao = F.ID
                    WHERE FU.Nr = @NrFunc", conn);

                cmd.Parameters.AddWithValue("@NrFunc", idFuncionario);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PermissoesPerfil
                        {
                            // Ao chamar pelo nome (reader["NomeColuna"]), não interessa a ordem do SELECT!
                            NivelAcesso = reader["NivelAcesso"] != DBNull.Value ? Convert.ToInt32(reader["NivelAcesso"]) : 3, // 3 por defeito (menos poderes)

                            PodeVerStock = reader["PodeVerStock"] != DBNull.Value && Convert.ToBoolean(reader["PodeVerStock"]),
                            PodeInserirStock = reader["PodeInserirStock"] != DBNull.Value && Convert.ToBoolean(reader["PodeInserirStock"]),
                            PodeCriarStock = reader["PodeCriarStock"] != DBNull.Value && Convert.ToBoolean(reader["PodeCriarStock"]),
                            PodeVerHistorico = reader["PodeVerHistorico"] != DBNull.Value && Convert.ToBoolean(reader["PodeVerHistorico"]),
                            PodeEditarFunc = reader["PodeEditarFunc"] != DBNull.Value && Convert.ToBoolean(reader["PodeEditarFunc"]),
                            PodeSubmeter = reader["PodeSubmeter"] != DBNull.Value && Convert.ToBoolean(reader["PodeSubmeter"]),
                            PodeAprovar = reader["PodeAprovar"] != DBNull.Value && Convert.ToBoolean(reader["PodeAprovar"]),
                            PodeEntregar = reader["PodeEntregar"] != DBNull.Value && Convert.ToBoolean(reader["PodeEntregar"]),
                            PodeCriarFuncoes = reader["PodeCriarFuncoes"] != DBNull.Value && Convert.ToBoolean(reader["PodeCriarFuncoes"]),
                            PodeAlterarDefinicoes = reader["PodeAlterarDefinicoes"] != DBNull.Value && Convert.ToBoolean(reader["PodeAlterarDefinicoes"])
                        };
                    }
                }
            }

            // Se não encontrar o utilizador ou der erro, devolve um perfil vazio (tudo false)
            return new PermissoesPerfil();
        }
    }
}