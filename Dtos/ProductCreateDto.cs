using System.ComponentModel.DataAnnotations;

public class ProductCreateDto
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = null!;

    [Required, StringLength(200)]
    public string Slug { get; set; } = null!;

    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}