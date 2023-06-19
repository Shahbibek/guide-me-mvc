using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoNotAgain.Models
{
    public class User
    {
        [Required]
        [DisplayName("ID")]
        public int? Id { get; set; }
        
        [Required]
        [DisplayName("First Name")]
        public String? fname { get; set; } 
        
        [Required]
        [DisplayName("Last Name")]
        public String? lname { get; set; }
        
        [Required]
        [DisplayName("Email")]
        public String? email { get; set; }
        
        [Required]
        [DisplayName("Salary")]
        public String? salary { get; set; }
        
        [Required]
        [DisplayName("Password")]
        public String? password { get; set; }
    }
}
