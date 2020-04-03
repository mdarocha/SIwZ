using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class User
    {
        [Key]
        public int Id { set; get; }
        
        [Required]
        public string Login { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        public string Token { set; get; }
        
        [Required]
        public bool IsAdmin { set; get; }
    }
}