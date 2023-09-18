using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class ChangePasswordData
    {
        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string CurrentPassword { get; set; }

        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password is not match ")]
        public string ConfirmNewPassword { get; set; }
    }
}
