namespace Doctorify.Infrastructure.Data.Repositories.Identity
{
    public interface IAuthenticationService
    {
        Task<bool> PasswordSignInAsync(string userName, string password);
    }
}
