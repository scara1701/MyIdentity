using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyIdentity.API.Services;

namespace MyIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ITenantService _connectionStringService;

        public DemoController(ITenantService connectionStringService)
        {
            _connectionStringService = connectionStringService;
        }

        [Authorize(Roles ="Demo")]
        [HttpGet]
        public string GetById()
        {
            //zoek Id van ingelogde gebruiker
            return "Welcome.";
        }

        [HttpGet]
        [Route("constring")]
        public string GetConnectionString()
        {;
            return _connectionStringService.GetConnectionString();
        }
    }
}
