namespace VideoGames.MVC.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public string VideoGameName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public List<string> CDKeys { get; set; }
    }
}
