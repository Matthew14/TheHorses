using System.Collections.Generic;
using System.Data.Common;

namespace TheHorses.Database
{
    public interface IDatabase
    {
        DbDataReader Query(DbCommand command);
        void NonQuery(DbCommand command);
        void Open();
        bool IsOpen { get; }
        void Close();
        DbCommand GetCommand(string sql);
        string ConnectionString { get; }
        DbParameter GetParam(string name, object value);
    }
}