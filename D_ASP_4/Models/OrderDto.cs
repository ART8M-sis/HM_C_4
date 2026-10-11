using System.ComponentModel.DataAnnotations;

namespace D_ASP_4.Models
{
    public class OrderDto
    {
        [Required(ErrorMessage = "Number is required")]
        public string? Number { get; set; }
        [Required(ErrorMessage = "Total is required")]
        public decimal? Total { get; set; }
    }
}
