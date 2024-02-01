using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADO.NET.WebApp.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter a valid email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@*#])[A-Za-z\d@*#]{8,}$", ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }
                        

    }
}