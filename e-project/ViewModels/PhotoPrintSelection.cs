using System.ComponentModel.DataAnnotations;

namespace e_project.ViewModels
{
    public class PhotoPrintSelection
    {
        [Required]
        public IFormFile PhotoFile { get; set; }

        [Required]
        public string PrintSize { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }

}
