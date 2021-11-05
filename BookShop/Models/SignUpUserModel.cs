using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Enter your frist name")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage ="Please enter your email")]
        [Display(Name ="Email address")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string  Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password")]
        [Compare("Confirm_Password",ErrorMessage ="does't match")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your confirm password")]
        [Display(Name = "Confirm password")]
        public string Confirm_Password { get; set; }
    }
}
