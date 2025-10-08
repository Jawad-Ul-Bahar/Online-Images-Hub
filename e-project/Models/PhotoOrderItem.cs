using System.ComponentModel.DataAnnotations;

namespace e_project.Models
{
    public class PhotoOrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; } // Foreign key to Order
        public Order Order { get; set; }

        [Required]
        public string PhotoFilePath { get; set; }

        [Required]
        public string PrintSize { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
