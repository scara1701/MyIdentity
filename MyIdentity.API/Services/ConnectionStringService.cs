using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MyIdentity.API.Services
{
    public class ConnectionStringService : IConnectionStringService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ConnectionStringService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public string GetConnectionString()
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            string constring = _configuration.GetConnectionString(host);
            return constring;
        }
    }
}
