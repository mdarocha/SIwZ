using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Discount
    {
        [Key] public int Id { set; get; }

        [StringLength(1)]
        public string Type { set; get; }

        [Required] public int Value { set; get; }

        [Required] public int ValueType { set; get; }
    }
}