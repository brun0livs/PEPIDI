using PEPIDI.Organizers;
using System;
using System.Data.SqlClient;

namespace PEPIDI_0._5.Models
{
    internal static class Details
    {
        public static (string Nome, string Funcao) GetInfoGestor(int nr)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_BuscaInfoFuncAtivo", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nr", nr);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nome = reader["Nome"].ToString();
                            string funcao = reader["Funcao"].ToString();
                            return (nome, funcao);
                        }
                    }
                }
            }

            return ("Desconhecido", "Desconhecido");
        }
    }
}
