using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace _1Pass.NetStandart.Libs.DBAPI
{
    public class Database : IDatabase
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
            using (var connection = GetConnection())
            {
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

        public async Task<int> DeleteDatabase()
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

        public async Task<bool> TryLogin(string pass)
        {
            Password = pass;
            using (var connection = GetConnection())
            {
                var command = "select count(seq) from sqlite_sequence";
                try
                {
                    connection.Open();
                    var res = await connection.ExecuteScalarAsync<int>(command, CommandType.Text).ConfigureAwait(false);
                    return res >= 0;
                }
                catch (Exception)
                {
                    
                    return false;

                }
                finally
                {
                    connection.Close();
                }
            }
            
        }

        public bool CheckExistence=>File.Exists(_dataSource);
    }
}
