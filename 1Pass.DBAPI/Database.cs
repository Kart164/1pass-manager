using _1Pass.DBEngine;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace _1Pass.DBAPI
{
    public class Database
    {
        private readonly string _dataSource;
        private readonly string _password;

        public Database(string dataSource,string password)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        private string _connectionString => new SqliteConnectionStringBuilder()
        {
            DataSource = _dataSource,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Password = _password
        }.ToString();

        public IDbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
        
        public int CreateDatabase()
        {
            var connection = GetConnection();
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                connection.Dispose();
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                connection.Close();
            }

            var res = UpdateToActual();
            return res;
        }

        public int UpdateToActual()
        {
            int res = UpgradeEngine.UpdgradeToActualVersion(_connectionString);
            if (res > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }


    }
}
