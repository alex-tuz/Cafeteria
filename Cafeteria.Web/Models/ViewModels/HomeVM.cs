namespace Cafeteria.Web.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Dish> Dishes { get; set; } = new List<Dish>();
        public IEnumerable<Category> Categories { get; set;} = new List<Category>();
    }
}
