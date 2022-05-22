using _1Pass.DBEngine;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace _1Pass.DBAPI
{
    public class Database
    {
        private readonly string _dataSource;
        public string Password { set; get; }

        public Database(string dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        private string _connectionString => new SqliteConnectionStringBuilder()
        {
            DataSource = _dataSource,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Password = Password
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

        public int DeleteDatabase()
        {
            try
            {
                File.Delete(_dataSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return 1;
        }

        public bool CheckExistence=>File.Exists(_dataSource);
    }
}
