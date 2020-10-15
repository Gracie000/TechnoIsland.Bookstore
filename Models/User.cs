using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnoIsland_Bookstore.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^[0]\d{10}$", ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }
               


    }
}
