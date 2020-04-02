using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class TrainStop
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Name { get; set; }
       
    }
}