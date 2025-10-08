using System.ComponentModel.DataAnnotations;

namespace e_project.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Please select at least one photo.")]
        public List<PhotoPrintSelection> Photos { get; set; } = new();

        [Required(ErrorMessage = "Please select a payment method.")]
        
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "Shipping address must be at least 10 characters.")]
        public string ShippingAddress { get; set; }

        // Credit Card Fields (Only required if payment method is CreditCard)
        [StringLength(100, ErrorMessage = "Cardholder name must be less than 100 characters.")]
        [RegularExpression(@"^[A-Za-z\s]{2,50}$", ErrorMessage = "Cardholder name must contain only letters and spaces.")]
        public string? CardholderName { get; set; }

        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be exactly 16 digits.")]
        public string? CreditCardNumber { get; set; }

        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
        public string? CVV { get; set; }

        [Range(1, 12, ErrorMessage = "Expiry month must be between 1 and 12.")]
        public int? ExpiryMonth { get; set; }

        [Range(2024, 2100, ErrorMessage = "Expiry year must be between 2024 and 2100.")]
        public int? ExpiryYear { get; set; }
    }
}
