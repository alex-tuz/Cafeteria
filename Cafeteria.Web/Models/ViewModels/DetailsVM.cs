namespace Cafeteria.Web.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Dish = new Dish();
        }

        public Dish Dish { get; set; }
        public bool ExistInCart { get; set; }

    }
}
