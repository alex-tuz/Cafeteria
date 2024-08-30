using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Web.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [Range(1, 999)]
        public int Code { get; set; }
        [Required]
        public string? Symbol { get; set; }
    }
}
