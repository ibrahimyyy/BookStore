using BookShop.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.Repository
{
    public interface IAccountRepository
    {
        Task<List<AllUsersInfo>> UsersSearch(string email);
        Task<IdentityResult> DeleteUserAccount(string userid);
        Task<ApplicationUser> GetUserByIdAsync(string userid);
        Task<List<KeyValuePair<string, bool>>> SelectedRoles(string userid);
        Task<IdentityResult> EditeTheRolesOfUser(UserRoleModel model);
        Task<List<AllUsersInfo>> GetAllUsersWithRoles();
        Task<IdentityResult> EditeRole(RoleinfoModel model);
        Task<RoleinfoModel> GetRole(string roleid);
        Task Deleterole(string roleid);
        Task<List<RoleinfoModel>> GetAllRoles();
        Task<IdentityResult> AddRoels(AddRolesModel addRolesModel);
        Task<List<string>> GetUserRolesNames(string userid);
        Task<List<string>> GetUserRolesIds(string userid);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<SignInResult> PasswordSignInAsync(LogInModel logInModel);
        Task SignoutAsync();
        Task<IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task GenerateForgetPasswordTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
    }
}