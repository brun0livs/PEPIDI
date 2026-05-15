using Microsoft.Data.SqlClient;

namespace AgentePEPIDI
{
    public static class CONN
    {
        public static string ConnectionString { get; set; }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
