using WebEnterprise.ViewModels;

namespace WebEnterprise.Repositories.Abstraction
{
    public interface IUserAuthenticationService
    {
        public Task<Status> LoginAsync(LoginModel model);
        public Task<Status> RegistrationAsync(RegisterModel model);
        public Task TaskLogoutAsync();
    }
}
