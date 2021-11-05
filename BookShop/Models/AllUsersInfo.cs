using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class AllUsersInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        //public string RoleName { get; set; }
        //public string RoleId { get; set; }
        public string UserId { get; set; }
        public List<string> RolesNames { get; set; }
        public List<string> RolesIds { get; set; }
        public List<KeyValuePair<string , bool>> SelectedRoles { get; set; }
    }
}
