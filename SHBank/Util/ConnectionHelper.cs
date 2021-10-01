using System.Data;
using MySql.Data.MySqlClient;

namespace MySqlHandle.Util
{
    public class ConnectionHelper
    {
        private static MySqlConnection _connection;
        private static readonly string Server = "127.0.0.1";
        private static  readonly string Uid = "root";
        private static readonly string Pwd = "";
        private static readonly string Database = "t2012e_springherobank";
        private static  string _connectionString = "server={0};uid={1};pwd={2};database={3};SslMode=None;";

        public static MySqlConnection GetInstance()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed){}

            {
                _connection = new MySqlConnection(string.Format(_connectionString, Server, Uid, Pwd, Database));
            }
            return _connection;
        }
    }
}