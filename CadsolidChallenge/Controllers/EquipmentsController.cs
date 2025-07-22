using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadsolidChallenge.Server.Data;
using CadsolidChallenge.Shared;

namespace CadsolidChallenge.Server.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentsController : Controller
    {
        private readonly DataContext _context;

        public EquipmentsController(DataContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<ActionResult<List<Equipment>>> Get() =>
         await _context.Equipment.Include(e => e.Availability).ToListAsync();

        // GET: Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipmentById(int? id)
        {

            var equipment = await _context.Equipment.FindAsync(id);

            if (equipment == null)
                return NotFound();

            return equipment;
        }

        // GET: Equipments/details/5
        [HttpGet("details/{id}")]
        public async Task<ActionResult<Equipment>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // POST: Equipments/
        [HttpPost]
        public async Task<ActionResult> Post(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = equipment.Id }, equipment);
        
        }


        //PUT: Equipments/Edit/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Put(int id, Equipment equipment)
        {
            if (id != equipment.Id)
                return BadRequest();

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Equipment.Any(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }



        // DELETE: Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
                return NotFound();

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
