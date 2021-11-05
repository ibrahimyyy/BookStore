using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="Enter your frist name")]
        [Display(Name ="First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your last name")]
        [Display(Name = "last name")]
        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

    }
}
