using Microsoft.AspNetCore.Mvc;
using ShopApi.Models;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private static readonly List<Category> _categories = new()
    {
        new() { Id = 1, Name = "Смартфоны", Slug = "smartphones" },
        new() { Id = 2, Name = "Ноутбуки", Slug = "laptops" },
    };

    // GET /api/categories
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_categories);
    }

    // GET /api/categories/1
    [HttpGet("{id:int:min(1)}")]
    public IActionResult GetById(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        return category is null ? NotFound() : Ok(category);
    }

    // GET /api/categories/smartphones
    [HttpGet("{slug:alpha}")]
    public IActionResult GetBySlug(string slug)
    {
        var category = _categories.FirstOrDefault(c => c.Slug == slug);
        return category is null ? NotFound() : Ok(category);
    }
}