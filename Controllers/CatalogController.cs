using Microsoft.AspNetCore.Mvc;
using ShopApi.Filters;
using ShopApi.Models;
using ShopApi.Selectors;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/catalog")]
[ServiceFilter(typeof(LogActionFilter))]
public class CatalogController : ControllerBase
{
    public static readonly List<Product> _products = new()
    {
        new() { Id = 1, Name = "iPhone 15", Slug = "iphone-15", Price = 999, CategoryId = 1 },
        new() { Id = 2, Name = "Galaxy S24", Slug = "galaxy-s24", Price = 899, CategoryId = 1 },
        new() { Id = 3, Name = "MacBook Pro", Slug = "macbook-pro", Price = 1999, CategoryId = 2 },
    };

    // GET /api/catalog
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_products);
    }

    // GET /api/catalog/1 — с рейтингом
    [HttpGet("{id:int:min(1)}", Name = "GetProductById")]
    [ActionName("GetProductById")]
    public IActionResult GetByIdInternal(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null) return NotFound();

        var withRating = EnrichWithRating(product);
        return Ok(withRating);
    }

    // Вспомогательный метод — НЕ эндпоинт
    [NonAction]
    public object EnrichWithRating(Product p)
    {
        return new
        {
            p.Id, p.Name, p.Slug, p.Price,
            Rating = 4.5,
            InStock = true
        };
    }

    // GET /api/catalog/page/1 (или просто /page)
    [HttpGet("page/{page:int=1}")]
    public IActionResult GetPage(int page)
    {
        const int size = 2;
        var items = _products
            .Skip((page - 1) * size)
            .Take(size);

        return Ok(new
        {
            page,
            total = _products.Count,
            items
        });
    }

    // GET /api/catalog/iphone-15
    [HttpGet("{slug:regex(^[[a-z0-9-]]*[[a-z]][[a-z0-9-]]*$)}")]
    public IActionResult GetBySlug(string slug)
    {
        var product = _products.FirstOrDefault(p => p.Slug == slug);
        return product is null ? NotFound() : Ok(product);
    }

    // GET /api/catalog/images/iphone-15/main.jpg
    [HttpGet("images/{*path}")]
    public IActionResult GetImage(string path)
    {
        return Ok(new { path, exists = true });
    }

    // GET /api/catalog/sale — только в выходные
    [HttpGet("sale")]
    [WeekendOnly]
    public IActionResult GetSale()
    {
        var onSale = _products.Where(p => p.IsOnSale);
        return Ok(onSale);
    }

    // GET /api/catalog/admin/stats — только для админа
    [HttpGet("admin/stats")]
    [AdminsOnly]
    public IActionResult AdminStats()
    {
        return Ok(new
        {
            totalProducts = _products.Count,
            avgPrice = _products.Average(p => p.Price)
        });
    }

    // POST /api/catalog
    [HttpPost]
    [ValidationFilter]
    public IActionResult Create([FromBody] ProductCreateDto dto)
    {
        var product = new Product
        {
            Id = _products.Max(p => p.Id) + 1,
            Name = dto.Name,
            Slug = dto.Slug,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };
        _products.Add(product);
        return CreatedAtAction("GetProductById", new { id = product.Id }, product);
    }

    // GET /api/catalog/crash — проверка GlobalExceptionFilter
    [HttpGet("crash")]
    public IActionResult Crash()
    {
        throw new Exception("Тестовая ошибка");
    }
}