using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingSystem.API.Data;
using VendingSystem.API.Models;

namespace VendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Требует JWT токен
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound(new { message = $"Товар с ID {id} не найден" });
            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Валидация согласно ТЗ (1.v, 1.w, 1.x)
            if (product.Price <= 0)
                return BadRequest(new { message = "Цена должна быть положительной" });

            if (product.InStock < 0 || product.MinStock < 0)
                return BadRequest(new { message = "Количество товара не может быть отрицательным" });

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }
    }
}