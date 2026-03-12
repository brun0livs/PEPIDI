using System;
using System.Data.SqlClient;

namespace PEPIDI.Organizers
{
    // Criei esta classe para "arrumar" o método
    public class PermissoesPerfil
    {
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
                    PodeVerStock, PodeInserirStock, PodeCriarStock, PodeVerHistorico, PodeEditarFunc,
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
                            // Validações para evitar erro se o campo for NULL na BD
                            PodeVerStock = !reader.IsDBNull(0) && reader.GetBoolean(0),
                            PodeCriarStock = !reader.IsDBNull(1) && reader.GetBoolean(1),
                            PodeInserirStock = !reader.IsDBNull(2) && reader.GetBoolean(2),
                            PodeVerHistorico = !reader.IsDBNull(3) && reader.GetBoolean(3),
                            PodeEditarFunc = !reader.IsDBNull(4) && reader.GetBoolean(4),
                            PodeSubmeter = !reader.IsDBNull(5) && reader.GetBoolean(5),
                            PodeAprovar = !reader.IsDBNull(6) && reader.GetBoolean(6),
                            PodeEntregar = !reader.IsDBNull(7) && reader.GetBoolean(7),
                            PodeCriarFuncoes = !reader.IsDBNull(8) && reader.GetBoolean(8),
                            PodeAlterarDefinicoes = !reader.IsDBNull(9) && reader.GetBoolean(9)
                        };
                    }
                }
            }

            // Se não encontrar o utilizador ou der erro, devolve um perfil vazio (tudo false)
            return new PermissoesPerfil();
        }
    }
}