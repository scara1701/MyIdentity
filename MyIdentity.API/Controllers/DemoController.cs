using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [Authorize(Roles ="Demo")]
        [HttpGet]
        public string GetById()
        {
            //zoek Id van ingelogde gebruiker
            return "Welcome.";
        }
    }
}
