namespace TheMovieDB.Models
{
    public class ShoppingCartItem
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}