using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required, DataType(DataType.Password) ,Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm password")]
        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public bool IsSuccess { get; set; }

    }
}
