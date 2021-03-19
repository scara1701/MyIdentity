namespace MyIdentity.API.Services
{
    public interface ITenantService
    {
        string GetConnectionString();

        string GetTokenSecret();
        string GetTokenIssuer();
    }
}