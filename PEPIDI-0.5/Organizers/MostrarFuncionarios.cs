using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEPIDI.Organizers
{
    public class MostrarFuncionarios
    {
        public DataTable CarregarFuncionarios(string pesquisa = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ProcurarFuncionarios", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Passa o texto da textbox para o SQL
                cmd.Parameters.AddWithValue("@Termo", pesquisa);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    conn.Open();
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao pesquisar: " + ex.Message);
                }
            }
            return dt;
        }

        public void BuscarNomeEFuncao(int nr, out string nome, out string funcao)
        {
            nome = string.Empty;
            funcao = string.Empty;

            using (SqlConnection conn = GetConn.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_BuscaInfoFuncAtivo", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nr", nr);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nome = reader["Nome"].ToString();
                        funcao = reader["Funcao"].ToString();
                    }
                }
            }
        }
    }
}
