using Microsoft.Extensions.Configuration;
using MyIdentity.API.Authentication.DTOModels;
using MyIdentity.API.Internal.DataAccess;
using System;
using System.Collections.Generic;

namespace MyIdentity.API.DataAccess
{
    public class UserData : IUserData
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlDataAccess _sqlDataAccess;

        public UserData(IConfiguration configuration, ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
            _configuration = configuration;
        }

        public List<DTOUser> GetUserById(string Id)
        {
            //SqlDataAccess sql = new SqlDataAccess(_configuration);

            var p = new { Id = Id };

            var output = _sqlDataAccess.LoadData<DTOUser, dynamic>("dbo.spUserLookup", p, "DefaultConnection");

            return output;
        }

        public bool CreateUser(DTOUser dTOUser)
        {
            bool success = false;

            //SqlDataAccess sql = new SqlDataAccess(_configuration);
            try
            {
                _sqlDataAccess.SaveData<DTOUser>("dbo.spCreateNewUser", dTOUser, "DefaultConnection");
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
