using Microsoft.Data.SqlClient;

namespace PEPIDI.Organizers
{
    public static class GetConn
    {
        public static string ConnectionString { get; set; }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
