using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Models
{
    public class ManagerModel
    {
        [Required(ErrorMessage = "You Name is required {0}")]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "Too long or too short")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You Email is required {0}")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "You Password is required {0}")]
        [MinLength(6, ErrorMessage = "Too Short"), MaxLength(20, ErrorMessage = "Too long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Are you Kidding Who?")]
        public string ConfirmPassword { get; set; }
    }
}