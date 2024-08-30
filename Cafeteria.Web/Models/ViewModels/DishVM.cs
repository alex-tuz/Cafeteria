using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cafeteria.Web.Models.ViewModels
{
    public class DishVM
    {
        public DishVM()
        {
            Dish = new Dish();
        }
        public Dish Dish { get; set; }
        public IEnumerable<SelectListItem>? CategorySelectList { get; set;}
        public IEnumerable<SelectListItem>? CurrencySelectList { get; set; }
        public string? ImagePath { get; set; }
    }
}
