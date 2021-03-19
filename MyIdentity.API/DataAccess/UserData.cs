using Microsoft.Extensions.Configuration;
using MyIdentity.API.Authentication.DTOModels;
using MyIdentity.API.Internal.DataAccess;
using System;
using System.Collections.Generic;

namespace MyIdentity.API.DataAccess
{
    public class UserData
    {
        private readonly IConfiguration _configuration;

        public UserData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<DTOUser> GetUserById(string Id)
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            var p = new { Id = Id };

            var output = sql.LoadData<DTOUser, dynamic>("dbo.spUserLookup", p, "DefaultConnection");

            return output;
        }

        public bool CreateUser(DTOUser dTOUser)
        {
            bool success = false;

            SqlDataAccess sql = new SqlDataAccess(_configuration);
            try
            {
                sql.SaveData<DTOUser>("dbo.spCreateNewUser", dTOUser, "DefaultConnection");
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return success;
        }


    }
}
