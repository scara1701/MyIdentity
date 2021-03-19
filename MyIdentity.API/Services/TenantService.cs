using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MyIdentity.API.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public TenantService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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

        public string GetTokenIssuer()
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            string issuer = _configuration[$"{host}:Issuer"];
            return issuer;
        }

        public string GetTokenSecret()
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
            string secret = _configuration[$"{host}:Secret"];
            return secret;
        }
    }
}
