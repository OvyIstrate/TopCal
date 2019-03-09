using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TopCal.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public float CaloriesTarget { get; set; } = 2500;

        [Column(TypeName = "varchar(30)")]
        [Required(ErrorMessage = "Please enter your First Name")]
        [StringLength(30, ErrorMessage = "First Name should not exceed 30 characters")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(30")]
        [Required(ErrorMessage = "Please enter your Last Name")]
        [StringLength(30, ErrorMessage = "Last Name should not exceed 30 characters")]
        public string LastName { get; set; }

        
    }
}
