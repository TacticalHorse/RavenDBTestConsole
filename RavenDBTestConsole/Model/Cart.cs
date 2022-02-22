namespace RavenDBTestConsole.Model
{
    internal class Cart
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public List<CartLine> Lines { get; set; } = new();
    }
    public class CartLine
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
