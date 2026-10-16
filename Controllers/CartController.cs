using Microsoft.AspNetCore.Mvc;
using ShopApi.Filters;
using ShopApi.Models;
using ShopApi.Selectors;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/cart")]
[ServiceFilter(typeof(LogActionFilter))]
public class CartController : ControllerBase
{
    private static readonly List<CartItem> _cart = new();

    // GET /api/cart — посмотреть корзину
    [HttpGet]
    public IActionResult GetCart()
    {
        var total = _cart.Sum(i => i.Product.Price * i.Quantity);
        return Ok(new { items = _cart, total });
    }

    // POST /api/cart/add
    [HttpPost("add")]
    [ValidationFilter]
    public IActionResult Add([FromBody] CartItemDto dto)
    {
        var product = CatalogController._products.FirstOrDefault(p => p.Id == dto.ProductId);
        if (product is null) return NotFound();

        var existing = _cart.FirstOrDefault(i => i.Product.Id == dto.ProductId);
        if (existing is null)
            _cart.Add(new CartItem { Product = product, Quantity = dto.Quantity });
        else
            existing.Quantity += dto.Quantity;

        return Ok(_cart);
    }

    // DELETE /api/cart/1
    [HttpDelete("{productId:int}")]
    public IActionResult Remove(int productId)
    {
        var item = _cart.FirstOrDefault(i => i.Product.Id == productId);
        if (item is null) return NotFound();

        _cart.Remove(item);
        return NoContent();
    }

    // POST /api/cart/checkout — только в рабочие дни
    [HttpPost("checkout")]
    [WeekdaysOnly]
    public IActionResult Checkout()
    {
        var total = _cart.Sum(i => i.Product.Price * i.Quantity);
        return Ok(new { message = "Заказ оформлен", items = _cart.Count, total });
    }
}