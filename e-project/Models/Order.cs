using System.ComponentModel.DataAnnotations;

namespace e_project.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }  // Foreign key to User
        public User User { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // "CreditCard" or "BranchPayment"

        // Optional for credit card
        public string? CardholderName { get; set; }
        public string? CreditCardNumber { get; set; } // Encrypted
        public int? ExpiryMonth { get; set; }
        public int? ExpiryYear { get; set; }

        public decimal TotalAmount { get; set; }

        public List<PhotoOrderItem> PhotoOrderItems { get; set; } = new();
    }
}
