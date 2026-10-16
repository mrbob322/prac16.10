namespace ShopApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public bool IsOnSale { get; set; }
}