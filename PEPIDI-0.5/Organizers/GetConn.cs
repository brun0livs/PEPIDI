using Microsoft.Data.SqlClient;

namespace PEPIDI.Organizers
{
    internal static class GetConn
    {
        public static string ConnectionString { get; set; }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
