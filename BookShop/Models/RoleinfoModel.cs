using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class RoleinfoModel
    {
        [Required , Display(Name ="Role name:")]
        public string RoleName { get; set; }
        public string OldRoleName { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}
