using System.Collections.Generic;

public class Sale
{
    public int Id { get; set; }
    public string CashierName { get; set; }
    public string CustomerName { get; set; }
    public decimal Total { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}
