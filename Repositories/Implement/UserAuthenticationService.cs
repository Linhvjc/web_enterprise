using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebEnterprise.Infrastructure.Persistance;
using WebEnterprise.Models.Entities;
using WebEnterprise.Repositories.Abstraction;
using WebEnterprise.ViewModels;

namespace WebEnterprise.Repositories.Implement
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAuthenticationService(SignInManager<User> signInManager, UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid Email";
                return status;
            }
            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid password";
                return status;
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email)
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));
                }
                status.StatusCode = 1;
                status.Message = "Login successfully";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked out temporarily";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "An error occured while logging in";
                return status;
            }
        }

        public async Task<Status> RegistrationAsync(RegisterModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "This email already exists";
                return status;
            }
            User user = new User
            {
                FullName = model.FullName,
                UserName = model.FullName.Replace(" ", ""),
                Email = model.Email,
                ProfilePicture = "avt.jpg",
                PhoneNumber = model.PhoneNumber,
                FacultyId = model.FacultyId,

            };
            try
            {

                var result = await userManager.CreateAsync(user, model.Password);
                //await _DbContext.SaveChangesAsync();
                if (!result.Succeeded)
                {
                    status.StatusCode = 0;


                    foreach (var item in result.Errors)
                    {
                        status.Message += item.Description + " ";
                    }

                    return status;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            if (!await roleManager.RoleExistsAsync(model.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            }

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "Your account was registered successfully";
            return status;
        }

        public async Task TaskLogoutAsync()
        {
            await signInManager.SignOutAsync();
            httpContextAccessor.HttpContext.Session.Remove("Email");
            httpContextAccessor.HttpContext.Session.Remove("UserId");
            httpContextAccessor.HttpContext.Session.Remove("UserName");
            httpContextAccessor.HttpContext.Session.Remove("FacultyId");
        }
    }

}

