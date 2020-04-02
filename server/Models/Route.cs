
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { set; get; }
    }
}