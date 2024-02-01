using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ADO.NET.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter FirstName")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter LastName")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@*#])[A-Za-z\d@*#]{8,}$", ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
        [DataType(DataType.Password)]
        public string Confirmpwd { get; set; }

        
        [Required(ErrorMessage = "Please enter a valid email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }

        
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }


        [NotMapped]
        [Display(Name = "Resume")]
        public HttpPostedFileBase ResumeFile { get; set; }
        [Display(Name = "Resume URL")]
        public string ResumeUrl { get; set; }

        public int RoleId { get; set; }

    }
}
