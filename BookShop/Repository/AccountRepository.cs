using BookShop.Data;
using BookShop.Models;
using BookShop.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Repository
{
    public class AccountRepository : IAccountRepository
    {               
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly BookShopContext _context;

        public AccountRepository(UserManager<ApplicationUser> userManeger , SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            IUserService userService , IEmailService emailService , IConfiguration configuration , BookShopContext context)
        {
            _userManager = userManeger;
            _signInManager = signInManager;
            _roleManager = roleManager; 
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
            _context = context;
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);            
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userid)
        {
            return await _userManager.FindByIdAsync(userid);
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            ApplicationUser user = new ApplicationUser()
            {
                FirstName=userModel.FirstName,
                LastName=userModel.LastName,
                Email = userModel.Email,
                UserName = userModel.Email,
                Birthday=userModel.Birthday
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            await _userManager.AddToRoleAsync(user, "User");
            //await _context.UserRoles.AddAsync(new IdentityUserRole<string>
            //{
            //    UserId = user.Id,
            //    RoleId = "3",
            //});
            //await _context.SaveChangesAsync();
            if (result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(user);
            }
            return result;
        }
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmail(user, token);
            }
        }
        public async Task GenerateForgetPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgetPasswordEmail(user, token);
            }
        }
        public async Task<SignInResult> PasswordSignInAsync(LogInModel logInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(logInModel.Email, logInModel.Password, logInModel.RememberMe, true); //PasswordSignInAsync this func in this row it's not the same func i write it.
            return result;
        }
        public async Task SignoutAsync()
        {
           await _signInManager.SignOutAsync();            
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var userID = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userID);
            return await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPass, changePasswordModel.NewPass);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid , string token)
        {            
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        private async Task SendEmailConfirmationEmail(ApplicationUser user, string token)
        {
            string AppDomain = _configuration.GetSection("Application:AppDomain").Value;
            string EmailConfirmation = _configuration.GetSection("Application:EmailConfirmation").Value;
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string> { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}" , user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                    string.Format(AppDomain + EmailConfirmation , user.Id , token))
                }
            };
            await _emailService.SendEmailsConfirmation(options);
        }
        private async Task SendForgetPasswordEmail(ApplicationUser user, string token)
        {
            //to reset your password you need 4 things : 1-user id .  2-token.   3-the password.   4-confirm password(i can add it or not).
            string AppDomain = _configuration.GetSection("Application:AppDomain").Value;
            string EmailConfirmation = _configuration.GetSection("Application:ForgetPassword").Value;
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string> { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}" , user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                    string.Format(AppDomain + EmailConfirmation , user.Id , token))
                }
            };
            await _emailService.SendEmailsForgetPassword(options);
        }
        public async Task<List<string>> GetUserRolesNames(string userid)
        {
            List<string> RolesNames = new List<string>();
            List<IdentityRole> Roles = new List<IdentityRole>();
            //var user = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userid));            
            var UserRole = await _context.UserRoles.Where(x => x.UserId == userid).ToListAsync();
            foreach(var userrole in UserRole)
            {
                Roles.Add(await _context.Roles.Where(x => x.Id == userrole.RoleId).FirstOrDefaultAsync());
            }
            foreach (var role in Roles)
            {
                RolesNames.Add(role.Name);
            }
            return RolesNames;
        }
        public async Task<List<string>> GetUserRolesIds(string userid)
        {
            List<string> RolesIds = new List<string>();                       
            var UserRole = await _context.UserRoles.Where(x => x.UserId == userid).ToListAsync();
            foreach (var userrole in UserRole)
            {
                RolesIds.Add(userrole.RoleId);                
            }
            
            return RolesIds;
        }

        public async Task<IdentityResult> AddRoels(AddRolesModel addRolesModel)
        {
            IdentityRole role = new IdentityRole()
                { 
                    Name = addRolesModel.RoleName,
                    NormalizedName = addRolesModel.RoleName.ToUpper(),
                    //ConcurrencyStamp = addRolesModel.RoleName.ToUpper() //not needed it will be create automatically.
            };
            var result = await _roleManager.CreateAsync(role);
            return result;
        }

        public async Task<List<RoleinfoModel>> GetAllRoles()
        {
            List<RoleinfoModel> AllRoles = new List<RoleinfoModel>();
            var data = await _context.Roles.ToListAsync();
            if(data?.Any() != null)
            {
                foreach(var r in data)
                {
                    AllRoles.Add(new RoleinfoModel
                    {
                        RoleId = r.Id,
                        RoleName = r.Name
                    });
                }
            }            
            return AllRoles;
        }
        public async Task<RoleinfoModel> GetRole(string roleid)
        {
            RoleinfoModel role = new RoleinfoModel();
            if (!string.IsNullOrEmpty(roleid))
            {
                var data = await _roleManager.FindByIdAsync(roleid);
                role.RoleId = roleid;
                role.RoleId = data.Name;
            }
            return role;
        }
        public async Task Deleterole(string roleid)
        {
           var role = await _roleManager.FindByIdAsync(roleid);
            await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> EditeRole(RoleinfoModel model)
        {            
            IdentityResult result = new IdentityResult();
            if (model != null)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                role.Name = model.RoleName;                
                result =  await _roleManager.UpdateAsync(role);
                return result;
            }            
            return result;
        }

        public async Task<List<KeyValuePair<string, bool>>> SelectedRoles(string userid)
        {
            List<KeyValuePair<string, bool>> keyValues = new List<KeyValuePair<string, bool>>();            
            var allroles = await GetAllRoles();
            var userroleids = await GetUserRolesIds(userid);
            for(int i = 0 ; i < allroles.Count() ; i++)
            {
                bool b = false;
                for(int j = 0 ; j < userroleids.Count() ; j++)
                {
                    if(allroles.ElementAt(i).RoleId == userroleids.ElementAt(j))
                    {
                        b = true;
                        break;
                    }                    
                }
                if(b == true)
                    keyValues.Add(new KeyValuePair<string, bool>(allroles.ElementAt(i).RoleId, true));
                if(b == false)
                    keyValues.Add(new KeyValuePair<string, bool>(allroles.ElementAt(i).RoleId, false));
            }            
                return keyValues;
        }

        public async Task<List<AllUsersInfo>> GetAllUsersWithRoles()
        {
            List<AllUsersInfo> allUser = new List<AllUsersInfo>();
            var Userdata = await _context.Users.Take(10).ToListAsync();
            if (Userdata?.Any() != null)
            {
                foreach(var user in Userdata)
                {
                    allUser.Add(new AllUsersInfo
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserId = user.Id,
                        SelectedRoles =  await SelectedRoles(user.Id)
                    });

                }
            }
            return allUser;
        }

        public async Task<IdentityResult> EditeTheRolesOfUser(UserRoleModel model)
        {
            List<string> NewUserRoles = new List<string>();

            foreach (var item in model.SelectedRoles)
            {
                if(item.Value == true)
                {
                    NewUserRoles.Add((await _roleManager.FindByIdAsync(item.Key)).Name);
                }
            }
            await _userManager.RemoveFromRolesAsync(await _userManager.FindByIdAsync(model.UserId) , model.OldRoles);
            var result =  await _userManager.AddToRolesAsync(await _userManager.FindByIdAsync(model.UserId), NewUserRoles);            
            return result;
        }

        public async Task<IdentityResult> DeleteUserAccount(string userid)
        {
            var result = await _userManager.DeleteAsync(await GetUserByIdAsync(userid));
            return result;
        }

        public async Task<List<AllUsersInfo>> UsersSearch(string email)
        {
            List<AllUsersInfo> UsersSearch = new List<AllUsersInfo>();
            var Userdata = await _context.Users.Where(x => x.Email.Contains(email)).ToListAsync();
            if (Userdata?.Any() != null)
            {
                foreach (var user in Userdata)
                {
                    UsersSearch.Add(new AllUsersInfo
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserId = user.Id,
                        SelectedRoles = await SelectedRoles(user.Id)
                    });

                }
            }
            return UsersSearch;
        }
    }
}