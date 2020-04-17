using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [PersonalData]
        public string Name { set; get; }
        
        [Required]
        [PersonalData]
        public string Surname { set; get; }

        [Required]
        public bool IsAdmin { set; get; }
    }

    public class Role : IdentityRole<int>
    {
        
    }
}