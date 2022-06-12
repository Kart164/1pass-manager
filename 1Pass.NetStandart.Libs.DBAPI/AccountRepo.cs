using _1Pass.NetStandart.Libs.Entities;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace _1Pass.NetStandart.Libs.DBAPI
{
    public class AccountRepo
    {
        private IDatabase _db { get; set; }

        public AccountRepo(IDatabase db)
        {
            _db = db;
        }

        public async Task<int> CreateAccountAsync(Account account)
        {
            var command = "INSERT INTO \"Accounts\" (\"Username\", \"Password\", \"ServiceId\") " +
                "VALUES (@username, @password, @seviceid); SELECT MAX(\"Id\") FROM \"Accounts\" " +
                "WHERE \"Username\"=@username AND \"ServiceId\"=@serviceid;";
            var parameters = new DynamicParameters();
            parameters.Add("@username", account.Username);
            parameters.Add("@password", account.Password);
            parameters.Add("@serviceid", account.ServiceId);

            using (var connection = _db.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var res = await connection.ExecuteScalarAsync<int?>(command, parameters, transaction, commandType: CommandType.Text).ConfigureAwait(false);
                        if (res.HasValue)
                        {
                            transaction.Commit();
                            return res.Value;
                        }
                        else
                        {
                            transaction.Rollback();
                            return -1;
                        }
                    }
                       
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
            
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            var command = "UPDATE \"Accounts\" SET \"Username\" = @username" +
                " \"Password\" = @password " +
                "\"LastUpdate\" = date()" +
                " WHERE \"Id\"=@id; SELECT * FROM \"Accounts\" WHERE \"Id\"=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@username", account.Username);
            parameters.Add("@password", account.Password);
            parameters.Add("@id", account.Id);

            using (var connection = _db.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var res = await connection.ExecuteScalarAsync<Account>(command, parameters, transaction, commandType: CommandType.Text).ConfigureAwait(false);
                        if (res != null)
                        {
                            transaction.Commit();
                            return res;
                        }
                        else
                        {
                            transaction.Rollback();
                            return null;
                        }
                    }                        
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            
        }

        public async Task<int> DeleteAccountAsync(int id)
        {
            var command = "DELETE FROM \"Accounts\" WHERE \"Id\"=@id; SELECT \"Id\" FROM \"Accounts\" WHERE \"Id\"=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using (var connection = _db.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var res = await connection.ExecuteScalarAsync<int?>(command, parameters, transaction, commandType: CommandType.Text).ConfigureAwait(false);
                        if (!res.HasValue)
                        {
                            transaction.Commit();
                            return id;
                        }
                        else
                        {
                            transaction.Rollback();
                            return -1;
                        }
                    }                       
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
           
        }
    }
}
