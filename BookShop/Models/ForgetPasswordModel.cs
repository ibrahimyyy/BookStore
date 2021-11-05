using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class ForgetPasswordModel
    {
        [Required , EmailAddress , Display(Name = "Enter the email")]
        public string Email { get; set; }

        public bool EmailSent { get; set; }
    }
}
