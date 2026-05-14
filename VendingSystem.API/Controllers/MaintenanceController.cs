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
    public class MaintenanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaintenanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Maintenance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetMaintenanceRecords()
        {
            return await _context.MaintenanceRecords
                .Include(m => m.Machine)
                .Include(m => m.DoneByUserNavigation)
                .ToListAsync();
        }

        // POST: api/Maintenance
        [HttpPost]
        public async Task<ActionResult<MaintenanceRecord>> PostMaintenance(MaintenanceRecord record)
        {
            // Валидация даты (1.am)
            if (record.MaintenanceDate > DateTime.Now)
                return BadRequest(new { message = "Дата обслуживания не может быть в будущем" });

            // Проверка существования аппарата
            if (record.MachineId.HasValue && !await _context.VendingMachines.AnyAsync(m => m.MachineId == record.MachineId))
                return BadRequest(new { message = "Указанный вендинговый аппарат не существует" });

            // Проверка исполнителя (если передан ID)
            if (record.DoneByUserId.HasValue && !await _context.Users.AnyAsync(u => u.UserId == record.DoneByUserId))
                return BadRequest(new { message = "Указанный исполнитель не существует" });

            _context.MaintenanceRecords.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaintenanceRecords), new { id = record.NoteId }, record);
        }
    }
}