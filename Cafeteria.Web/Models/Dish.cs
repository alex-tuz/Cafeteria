using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cafeteria.Web.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        [StringLength(250)]
        [Display(Name = "Short description")]
        public string? ShortDescription { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency? Currency { get; set; } 

        [Display(Name = "Vegetarian dish")]
        public bool IsVegetarian { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
