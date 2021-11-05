using BookShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Helpers
{
    //this class i made it to use it to get the User's info from DataBase (it's a pro method😁) and i call this method from view not controller by using this (@inject Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> _signinManager) in the view.
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager,
             IOptions<IdentityOptions> options)
            :base(userManager, roleManager, options)
        {            
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            return identity;
        }
    }
}
