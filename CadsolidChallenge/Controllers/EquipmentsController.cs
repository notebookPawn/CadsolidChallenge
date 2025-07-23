using CadsolidChallenge.Server.Data;
using CadsolidChallenge.Shared;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
         await _context.Equipment.ToListAsync();

        // GET: Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipmentById(int? id)
        {

            var equipment = await _context.Equipment.Include(e => e.Availability).FirstOrDefaultAsync(m => m.Id == id);

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
                .Include(e => e.Availability)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        //POST: Equipments/
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Post([FromForm] string name, [FromForm] string model, [FromForm] DateTime? inicialDate, [FromForm] DateTime? endDate, [FromForm] IFormFile? image)
        { 
            if (inicialDate >= endDate)
            {
                return BadRequest("A data de início deve ser anterior à data de fim.");
            }


            string url = null;

            if (image != null && image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream);

                url = $"/uploads/{fileName}";
            }

            Equipment equipmentToCreate = new Equipment()
            {
                Name = name,
                Model = model,
                ImagemUrl = url,
                Availability = new Availability()
                {
                    inicialDate = (DateTime)inicialDate,
                    endDate = (DateTime)endDate,
                },
            };

            _context.Equipment.Add(equipmentToCreate);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = equipmentToCreate.Id }, equipmentToCreate);

        }


        [HttpPost("{id}/upload")]
        public async Task<ActionResult<string>> UploadImagem(int id, IFormFile file)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (file == null || file.Length == 0)
                return BadRequest("Invalid File");
            if (equipment == null)
                return BadRequest("User not Found");
            _context.Entry(equipment).State = EntityState.Modified;

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var url = $"/uploads/{fileName}";
            equipment.ImagemUrl = url;

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
            return Ok(url);
        }


        //PUT: Equipments/Edit/5
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Put(int id, Equipment equipment)
        {
            if (id != equipment.Id)
                return BadRequest();

            if (equipment.Availability.inicialDate >= equipment.Availability.endDate)
            {
                return BadRequest("A data de início deve ser anterior à data de fim.");
            }

            _context.Entry(equipment).State = EntityState.Modified;
            _context.Entry(equipment.Availability).State = EntityState.Modified;

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
