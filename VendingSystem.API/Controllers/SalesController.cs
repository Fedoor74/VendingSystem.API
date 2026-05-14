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
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            return await _context.Sales
                .Include(s => s.Machine)
                .Include(s => s.Product)
                .ToListAsync();
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            // Валидация согласно ТЗ (1.ac, 1.ad, 1.af)
            if (sale.Quantity <= 0)
                return BadRequest(new { message = "Количество должно быть больше нуля" });

            if (sale.SaleSum < 0)
                return BadRequest(new { message = "Сумма продажи не может быть отрицательной" });

            // Проверка существования связанных сущностей
            if (sale.MachineId.HasValue && !await _context.VendingMachines.AnyAsync(m => m.MachineId == sale.MachineId))
                return BadRequest(new { message = "Указанный вендинговый аппарат не существует" });

            if (sale.ProductId.HasValue && !await _context.Products.AnyAsync(p => p.ProductId == sale.ProductId))
                return BadRequest(new { message = "Указанный товар не существует" });

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSales), new { id = sale.SaleId }, sale);
        }
    }
}