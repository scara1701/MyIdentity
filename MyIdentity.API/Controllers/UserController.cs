using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyIdentity.API.Authentication.DTOModels;
using MyIdentity.API.Authentication.Models;
using MyIdentity.API.DataAccess;
using System.Linq;

namespace MyIdentity.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet]
        public DTOUser GetById()
        {
            //zoek Id van ingelogde gebruiker
            string userId = _userManager.GetUserId(User);
            UserData data = new UserData(_configuration);
            return data.GetUserById(userId).First();
        }
    }
}
