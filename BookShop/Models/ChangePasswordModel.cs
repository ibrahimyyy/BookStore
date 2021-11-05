using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Current password")]
        public string CurrentPass { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "New password")]
        public string NewPass { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Confirm new password")]
        public string ConfirmNewPass { get; set; }
    }
}
