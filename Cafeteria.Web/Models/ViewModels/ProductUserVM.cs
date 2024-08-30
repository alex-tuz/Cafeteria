namespace Cafeteria.Web.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            DishList = new List<Dish>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<Dish> DishList { get; set; }

    }
}
