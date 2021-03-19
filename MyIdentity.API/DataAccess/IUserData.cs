using MyIdentity.API.Authentication.DTOModels;
using System.Collections.Generic;

namespace MyIdentity.API.DataAccess
{
    public interface IUserData
    {
        bool CreateUser(DTOUser dTOUser);
        List<DTOUser> GetUserById(string Id);
    }
}