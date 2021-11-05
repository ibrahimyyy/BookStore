using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShop.Service
{
    //this calss can give me some important services about the user like the User's id and if it loggedin or not, and i can use this services every where i want.
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserService(IHttpContextAccessor httpContex)
        {
            _httpContext = httpContex;
        }
        public string GetUserId()//this method will return the id of the user so i can use it for searching for user.
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public bool IsAuthenticated()//this method will back a boolean if the user is now login or not.
        {
            return _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
