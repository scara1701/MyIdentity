using Dapper;
using Microsoft.Extensions.Configuration;
using MyIdentity.API.Services;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MyIdentity.API.Internal.DataAccess
{
    //prevents direct access from outside the class library, only other classes within the library can use this class.
    internal class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConnectionStringService _connectionStringService;

        public SqlDataAccess(IConnectionStringService connectionStringService)
        {
            _connectionStringService = connectionStringService;
        }

        public string GetConnectionString(string name)
        {
            return _connectionStringService.GetConnectionString();
        }

        //playing with generics
        //Get data with Dapper
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        //Save data with Dapper
        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
