using _1Pass.Entities;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace _1Pass.DBAPI
{
    public class ServiceRepo
    {
        private Database _db { get; set; }

        public ServiceRepo (Database db)
        {
            _db = db;
        }

        public async Task<int> CreateServiceAsync(Service service)
        {
            var command = "INSERT INTO \"Service\" (\"Name\") VALUES (@name); SELECT \"Id\" FROM \"Services\" WHERE \"Name\"=@name";
            var parameters = new DynamicParameters();
            parameters.Add("@name", service.Name);
            using var connection = _db.GetConnection();
            try
            {
                connection.Open(); 
                using var transaction = connection.BeginTransaction();
                var res = await connection.ExecuteScalarAsync<int?>(command, parameters,transaction, commandType:CommandType.Text).ConfigureAwait(false);
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
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<Service> UpdateServiceAsync(Service service)
        {
            var command = "UPDATE \"Service\" SET \"Name\" = @name \"LastUpdate\" = date() WHERE \"Id\"=@id; SELECT * FROM \"Services\" WHERE \"Id\"=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@name", service.Name);
            parameters.Add("@id",service.Id);
            using var connection = _db.GetConnection();
            try
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var res = await connection.ExecuteScalarAsync<Service>(command, parameters, transaction, commandType: CommandType.Text).ConfigureAwait(false);
                if (res!=null)
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
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> DeleteServicesAsync(int id)
        {
            var command = "DELETE FROM \"Service\" WHERE \"Id\"=@id; SELECT * FROM \"Services\" WHERE \"Id\"=@id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            using var connection = _db.GetConnection();
            try
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
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
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            var command = "SELECT * FROM \"Services\"";
            using var connection = _db.GetConnection();
            try
            {
                var entities = new List<Service>();
                connection.Open();
                using var reader = await connection.ExecuteReaderAsync(command, commandType: CommandType.Text).ConfigureAwait(false);
                var rowParser = reader.GetRowParser<Service>();
                while (reader.Read())
                {
                    entities.Add(rowParser(reader));
                }
                return entities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Service>> FindServices(string searchStr)
        {
            var command = "SELECT * FROM \"Services\" WHERE \"Name\" LIKE @searchparam";
            var param = new DynamicParameters();
            param.Add("@searchparam", '%' + searchStr + '%');
            using var connection = _db.GetConnection();
            try
            {
                var entities = new List<Service>();
                connection.Open();
                using var reader = await connection.ExecuteReaderAsync(command,param, commandType:CommandType.Text).ConfigureAwait(false);
                var rowParser = reader.GetRowParser<Service>();
                while (reader.Read())
                {
                    entities.Add(rowParser(reader));
                }
                return entities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ServiceWithAccounts> GetServiceWithAccounts(int id)
        {
            var res = new ServiceWithAccounts() { Id = id };
            var command = "SELECT * FROM \"Accounts\" WHERE \"ServiceId\"=@id";
            var param = new DynamicParameters();
            param.Add("@id", id);
            using var connection = _db.GetConnection();
            try
            {
                var entities = new List<Account>();
                connection.Open();
                using var reader = await connection.ExecuteReaderAsync(command, param, commandType: CommandType.Text).ConfigureAwait(false);
                var rowParser = reader.GetRowParser<Account>();
                while (reader.Read())
                {
                    entities.Add(rowParser(reader));
                }
                res.Accounts = entities;
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
