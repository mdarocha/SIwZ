using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Discount
    {
        public enum DiscountValueTypes
        {
            Percentage,
            Flat
        }
        
        [Key] public int Id { set; get; }

        [Required]
        public string Type { set; get; }

        [Required] public int Value { set; get; }

        [Required] public int ValueType { set; get; }
    }
}