using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendingSystem.API.Data;
using VendingSystem.API.Models;

namespace VendingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Только для авторизованных пользователей с JWT
    public class VendingMachinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendingMachinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/vendingmachines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendingMachine>>> GetMachines()
        {
            return await _context.VendingMachines.ToListAsync();
        }

        // GET: api/vendingmachines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VendingMachine>> GetMachine(int id)
        {
            var machine = await _context.VendingMachines.FindAsync(id);
            if (machine == null) return NotFound(new { message = $"ТА с ID {id} не найден" });
            return machine;
        }

        // POST: api/vendingmachines
        [HttpPost]
        public async Task<ActionResult<VendingMachine>> PostMachine(VendingMachine machine)
        {
            // Валидация дублей
            if (await _context.VendingMachines.AnyAsync(m => m.SerialNumber == machine.SerialNumber))
                return BadRequest(new { message = "ТА с таким серийным номером уже существует" });

            if (await _context.VendingMachines.AnyAsync(m => m.InventoryNumber == machine.InventoryNumber))
                return BadRequest(new { message = "ТА с таким инвентарным номером уже существует" });

            // Валидация дат
            if (machine.DateOfCommissioning < machine.ManufactureDate)
                return BadRequest(new { message = "Дата ввода в эксплуатацию не может быть раньше даты изготовления" });

            // Валидация ресурса
            if (machine.ResourceHours <= 0)
                return BadRequest(new { message = "Ресурс ТА должен быть положительным числом" });

            // Валидация времени обслуживания
            if (machine.MaintenanceTimeHours < 1 || machine.MaintenanceTimeHours > 20)
                return BadRequest(new { message = "Время обслуживания должно быть от 1 до 20 часов" });

            // Расчет даты следующей поверки (если указаны интервал и дата последней поверки)
            if (machine.LastVerificationDate.HasValue && machine.VerificationInterval.HasValue)
            {
                machine.DateOfNextFixing = machine.LastVerificationDate.Value.AddMonths(machine.VerificationInterval.Value);
            }

            _context.VendingMachines.Add(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMachine), new { id = machine.MachineId }, machine);
        }

        // PUT: api/vendingmachines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine(int id, VendingMachine machine)
        {
            if (id != machine.MachineId) return BadRequest();

            _context.Entry(machine).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.VendingMachines.Any(e => e.MachineId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/vendingmachines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var machine = await _context.VendingMachines.FindAsync(id);
            if (machine == null) return NotFound();

            _context.VendingMachines.Remove(machine);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}