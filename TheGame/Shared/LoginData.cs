using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.Shared
{
    public class LoginData
    {
        [Required]
        public string Email { get; set; }
        [Required,MinLength(8,ErrorMessage ="Password must be at least 8 characters")]
        public string Password { get; set; }
    }
}
