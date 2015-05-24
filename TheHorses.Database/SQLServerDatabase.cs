using System.Data.Common;
using System.Data.SqlClient;

namespace TheHorses.Database
{
    public class SQLServerDatabase : IDatabase
    {
        private SqlConnection _conn;

        public bool IsOpen { get; private set; }

        public string ConnectionString { get; private set; } 


        public DbCommand GetCommand(string sql) => new SqlCommand(sql, _conn);
        public DbParameter GetParam(string name, object value) => new SqlParameter(name, value);
        public DbDataReader Query(DbCommand command) => command.ExecuteReader();
        public void NonQuery(DbCommand command) => command.ExecuteNonQuery();


        public SQLServerDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            _conn = new SqlConnection(ConnectionString);
        }

        public SQLServerDatabase(DatabaseCredentials credentials) : this($"user id={credentials.User};" +
                                                                         $"password={credentials.Password};" +
                                                                         $"server={credentials.Host};" +
                                                                         $"database={credentials.Database}"){}

        public void Open()
        {
            try
            {
                _conn.Open();
                IsOpen = true;
            }
            catch
            {
                throw;//TODO
            }
        }

        public void Close()
        {
            try
            {
                _conn.Close();
                IsOpen = false;
            }
            catch (SqlException)
            {
                throw; //TODO
            }
        }
    }
}