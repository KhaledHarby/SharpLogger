
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.Configurations
{
    internal static class DbFactory
    {

        static DbFactory()
        {

        }

        internal static DbConnection CreateConnection(DatabaseType DbType, string connectionString)
        {

            switch (DbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlConnection(connectionString);
                case DatabaseType.Oracle:
                    return new OracleConnection(connectionString);
                case DatabaseType.MySql:
                    return new MySqlConnection(connectionString);

                default:
                    break;
            }
            return null;
        }

        internal static DbCommand CreateCommand(DatabaseType DbType, string commandText, DbConnection connection)
        {
            switch (DbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlCommand(commandText, (SqlConnection)connection);
                case DatabaseType.Oracle:
                    return new OracleCommand(commandText, (OracleConnection)connection);
                case DatabaseType.MySql:
                    return new MySqlCommand(commandText, (MySqlConnection)connection);

                default:
                    break;
            }
            return null;
        }

        internal static string CreateTableQueryStatement(DatabaseType DbType)
        {

            switch (DbType)
            {
                case DatabaseType.SqlServer:
                    return @"create table SharpLogger
                            (
	                            [Id] int not null primary key identity, 
                                [Message] nvarchar(4000) null, 
                                [DateTime] datetime null, 
                                [Log_Level] nvarchar(10) null, 
                                [CallingClass] nvarchar(500) NULL, 
                                [CallingMethod] nvarchar(500) NULL,
                                [LineNumber]    nvarchar(100) null ,
                                [DomainName]    nvarchar(100) null ,
                                [UserName]    nvarchar(100) null
                            );";

                case DatabaseType.Oracle:
                    return @"create table SharpLogger
                                (
                                  Id int not null primary key, 
                                   Message varchar2(4000) null, 
                                   DateTime date null, 
                                   Log_Level varchar2(10) null, 
                                   CallingClass varchar2(500) NULL, 
                                   CallingMethod varchar2(500) NULL ,
                                   LineNumber    nvarchar2(100) null ,
                                  DomainName    varchar2(100) null ,
                                  UserName    varchar2(100) null
                                );
                                create sequence seq_log nocache;";


                case DatabaseType.MySql:
                    return @"create table SharpLogger
                            (
	                            Id int not null auto_increment,
                                Message varchar(4000) null, 
                                DateTime datetime null, 
                                Log_Level varchar(10) null, 
                                CallingClass varchar(500) NULL, 
                                CallingMethod varchar(500) NULL,
                                LineNumber    varchar(100) NULL ,
                                DomainName    varchar(100) null ,
                                UserName    varchar(100) null
                                PRIMARY KEY (Id)
                            );";

                default:
                    break;
            }

            return string.Empty;
        }

        internal static string CreateTableExistsQueryStatement(DatabaseType DbType)
        {
            switch (DbType)
            {
                case DatabaseType.SqlServer:
                    return @"SELECT object_name(object_id) as table_name 
                               FROM sys.objects
                              WHERE type_desc LIKE '%USER_TABLE' 
                                AND lower(object_name(object_id)) like 'SharpLogger';";

                case DatabaseType.Oracle:
                    return @"SELECT TABLE_NAME 
                               FROM ALL_TABLES 
                              WHERE LOWER(TABLE_NAME) LIKE :'SharpLogger'";

                case DatabaseType.MySql:
                    return @"SELECT table_name
                               FROM information_schema.tables
                              WHERE LOWER(table_name) = 'SharpLogger';";

                default:
                    break;
            }

            return string.Empty;
        }

        internal static string CreateInsertQueryStatement(DatabaseType DbType, string tableName, Logging.LogMessage logMessage)
        {
            switch (DbType)
            {
                case DatabaseType.SqlServer:
                    return string.Format(@"insert into SharpLogger ([Message], [DateTime], [Log_Level], [CallingClass], [CallingMethod] , [LineNumber] , [DomainName] , [UserName]) 
                                           values ('{0}', '{1}', '{2}','{3}', '{4}','{5}','{6}','{7}');", logMessage.Message, logMessage.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), logMessage.Level, logMessage.CallingClass, logMessage.CallingMethod, logMessage.LineNumber, Environment.UserDomainName, Environment.UserName);

                case DatabaseType.Oracle:
                    return string.Format(@"insert into SharpLogger (Id, Message, DateTime, Log_Level, CallingClass, CallingMethod) 
                                           values (seq_log.nextval, :text, :dateTime, :log_level, :callingClass, :callingMethod)", tableName);

                case DatabaseType.MySql:
                    return string.Format(@"insert into SharpLogger (Message, DateTime, Log_Level, CallingClass, CallingMethod , LineNumber , DomainName , UserName) 
                                            values ('{0}', '{1}', '{2}','{3}', '{4}' , '{5}' , '{6}' , '{7}');", logMessage.Message, logMessage.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), logMessage.Level, logMessage.CallingClass, logMessage.CallingMethod, logMessage.LineNumber, Environment.UserDomainName, Environment.UserName);

                default:
                    break;
            }

            return string.Empty;
        }


        

    }
}
