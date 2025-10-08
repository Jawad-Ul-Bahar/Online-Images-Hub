    using System.ComponentModel.DataAnnotations;

    namespace e_project.Models
    {
        public class PrintSizePrice
        {
            public int Id { get; set; }

            [Required]
            [StringLength(10)]
            public string Size { get; set; }  // Example: "4x6", "5x7"

            [Required]
            [Range(0.01, 9999)]
            public decimal Price { get; set; }
        }
    }
