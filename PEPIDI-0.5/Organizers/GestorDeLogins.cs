using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PEPIDI.Organizers
{
    internal class GestorDeLogins
    {
        public static void RegistarOuAtualizarLogin(string nrMecanografico)
        {
            try
            {
                Hash H = new();
                // 1. Usa a classe Hash (cada um com a sua responsabilidade!)
                string passwordEmHash = H.GerarHashSenha(nrMecanografico);

                // 2. Grava na Base de Dados
                using (var conn = GetConn.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_RegistarLogin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nr", nrMecanografico);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordEmHash);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Como isto não é um Form, lançamos o erro para ser apanhado por quem chamou
                throw new Exception("Erro ao gerir as credenciais de segurança: " + ex.Message);
            }
        }
    }
}
