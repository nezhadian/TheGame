using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class RegisterData
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(4, ErrorMessage = "Username must be at least 4 charcters")]
        public string Username { get; set; }

        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage ="Password is not match ")]
        public string ConfirmPassword { get; set; }


    }
}
