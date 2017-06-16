using Sharp.Configurations;
using Sharp.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Loggers
{
    public class SharpDbLogger : ISharpLoggerHandler
    {

        private readonly string _connectionString;
        private readonly DatabaseType _dbType;
        private readonly object locker = new object();


        public SharpDbLogger(DatabaseType DbType, string ConnectionString)
        {
            _connectionString = _connectionString = ConnectionString;
            _dbType = DbType;
            CreateTable();
        }


        public void Publish(LogMessage logMessage)
        {
            lock (locker)
            {
                using (var connection = DbFactory.CreateConnection(_dbType, _connectionString))
                {
                    connection.Open();
                    var commandText = DbFactory.CreateInsertQueryStatement(_dbType, "", logMessage);
                    var sqlCommand = DbFactory.CreateCommand(_dbType, commandText, connection);

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }


        private void CreateTable()
        {
            lock (locker)
            {
                using (var connection = DbFactory.CreateConnection(_dbType, _connectionString))
                {
                    connection.Open();
                    var commandText = DbFactory.CreateTableExistsQueryStatement(_dbType);
                    var sqlCommand = DbFactory.CreateCommand(_dbType, commandText, connection);

                    DataTable Data = new DataTable();
                    Data.Load(sqlCommand.ExecuteReader());
                    if (Data.Rows.Count == 0)
                    {
                        commandText = DbFactory.CreateTableQueryStatement(_dbType);
                        sqlCommand.CommandText = commandText;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
