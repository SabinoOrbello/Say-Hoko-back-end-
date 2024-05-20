using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Say_Hoko.Models;

namespace Say_Hoko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdiniController : ControllerBase
    {
        private readonly SushiContext _context;

        public OrdiniController(SushiContext context)
        {
            _context = context;
        }

        // GET: api/Ordini
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ordini>>> GetOrdini()
        {
            return await _context.Ordinis
                .Include(o => o.DettagliOrdines)
                .Include(o => o.Pagamentis)
                .ToListAsync();
        }


        // GET: api/Ordini/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ordini>> GetOrdine(int id)
        {
            var ordine = await _context.Ordinis
                .Include(o => o.DettagliOrdines)
                .Include(o => o.Pagamentis)
                .FirstOrDefaultAsync(o => o.OrdiniId == id);

            if (ordine == null)
            {
                return NotFound();
            }

            return ordine;
        }

        // PUT: api/Ordini/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdini(int id, Ordini ordini)
        {
            if (id != ordini.OrdiniId)
            {
                return BadRequest();
            }

            _context.Entry(ordini).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdiniExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ordini
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ordini>> PostOrdini(Ordini ordini)
        {
            _context.Ordinis.Add(ordini);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrdini", new { id = ordini.OrdiniId }, ordini);
        }

        // DELETE: api/Ordini/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdini(int id)
        {
            var ordini = await _context.Ordinis.FindAsync(id);
            if (ordini == null)
            {
                return NotFound();
            }

            _context.Ordinis.Remove(ordini);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdiniExists(int id)
        {
            return _context.Ordinis.Any(e => e.OrdiniId == id);
        }
    }
}
