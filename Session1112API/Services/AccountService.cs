
using Microsoft.AspNetCore.Identity;
using Session1112API.Models;

namespace Session1112API.Services
{
    public class AccountService:IAccountService
    {
        UserManager<ApplicationUsers> usermanager;
        SignInManager<ApplicationUsers> signInManager;
        RoleManager<IdentityRole> roleManager;

        public AccountService(UserManager<ApplicationUsers> _usermanager,SignInManager<ApplicationUsers> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            usermanager = _usermanager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        public async Task<IdentityResult> CreateAccount(SignUp signUp)
        {
            ApplicationUsers user = new ApplicationUsers()
            {
                UserName = signUp.Email,
                Email=signUp.Email,
                //PasswordHash=signUp.Password,
                Name=signUp.Name,
                Gender=signUp.Gender
            };

            var result= await usermanager.CreateAsync(user,signUp.Password);
            return result;
        }

        public async Task<SignInResult> SignIn(SignIn signIn)
        {
           var result=await signInManager.PasswordSignInAsync(signIn.Username, signIn.Password, signIn.rememberMe, false);
            return result;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddRole(Role role)
        {
            IdentityRole Irole = new IdentityRole()
            {
                Name = role.Name
            };

            var result = await roleManager.CreateAsync(Irole);
            return result;

        }

        public List<UsersDTO> getUsers()
        {
            List<UsersDTO> users = new List<UsersDTO>();
            List<ApplicationUsers> li = usermanager.Users.ToList();
            foreach (var item in li)
            {
                users.Add(new UsersDTO()
                {
                    Name = item.Name,
                    Email = item.Email,
                    Id=item.Id
                });

            }

            return users;
        }

        public async Task<List<UserRoles>> getRoles(string UserId)
        {
            List<Role> roles = new List<Role>();
            List<IdentityRole> li= roleManager.Roles.ToList();

            List<UserRoles> userRoles = new List<UserRoles>();


            foreach (var item in li)
            {
                
                userRoles.Add(new UserRoles()
                {
                    RoleId = item.Id,
                    RoleName = item.Name,
                    IsSelected = false,
                    UserId = UserId
                }) ;
		
			}

            foreach (var item in userRoles)
            {
				var user = await usermanager.FindByIdAsync(UserId);
				var uroles = await usermanager.GetRolesAsync(user);
                foreach (var roleName in uroles)
                {
                    if(roleName== item.RoleName)
                    {
                        item.IsSelected = true;
                    }
                }

			}

            return userRoles;
        }

        public async Task UpdateUserRoles(List<UserRoles> liUserRoles)
        {
            
            foreach (var item in liUserRoles)
            {
				var user = await usermanager.FindByIdAsync(item.UserId);
                if (item.IsSelected)
                {
                    if (await usermanager.IsInRoleAsync(user, item.RoleName) == false)
                        await usermanager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    if (await usermanager.IsInRoleAsync(user, item.RoleName))
                    {
                        await usermanager.RemoveFromRoleAsync(user, item.RoleName);
                    }
                }
			}
        }

        public async Task<ApplicationUsers> getUSerInfo(string username)
        {
            var result = await usermanager.FindByNameAsync(username);
            return result;

		}

        public List<string> getUserRoles(ApplicationUsers obj)
        {
            var result = usermanager.GetRolesAsync(obj).Result.ToList();
            return result;

		}

	}
}
