using System.Data.Common;
using System.Data.SqlClient;

namespace TheHorses.Database
{
    public class SQLServerDatabase : IDatabase
    {
        private readonly DatabaseCredentials _credentials;
        private SqlConnection _conn;

        public bool IsOpen { get; private set; }

        public string ConnectionString => $"user id={_credentials.User};" +
                                          $"password={_credentials.Password};" +
                                          $"server={_credentials.Host};" +
                                          $"database={_credentials.Database}";


        public DbCommand GetCommand(string sql) => new SqlCommand(sql, _conn);
        public DbParameter GetParam(string name, object value) => new SqlParameter(name, value);
        public DbDataReader Query(DbCommand command) => command.ExecuteReader();
        public void NonQuery(DbCommand command) => command.ExecuteNonQuery();


        public SQLServerDatabase(DatabaseCredentials credentials)
        {
            _credentials = credentials;
            _conn = new SqlConnection(ConnectionString);
        }

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
            catch (SqlException sqlException)
            {
                throw; //TODO
            }
        }
    }
}