using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> OldRoles { get; set; }
        public List<RoleinfoModel> AllRoles { get; set; }        
        public List<KeyValuePair<string, bool>> SelectedRoles { get; set; }
    }
}
