namespace Doctorify.Infrastructure.Data.Repositories.Identity
{
    public interface ITokenService
    {
        string GenerateAccessToken();
    }
}
