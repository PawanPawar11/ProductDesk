using System.ComponentModel.DataAnnotations;

namespace ProductDesk.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Cost { get; set; }
    }
}
