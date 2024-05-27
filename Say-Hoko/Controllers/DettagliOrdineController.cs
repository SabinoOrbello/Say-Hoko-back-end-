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
    public class DettagliOrdineController : ControllerBase
    {
        private readonly SushiContext _context;

        public DettagliOrdineController(SushiContext context)
        {
            _context = context;
        }

        // GET: api/DettagliOrdine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DettagliOrdine>>> GetDettagliOrdines()
        {
            return await _context.DettagliOrdines.ToListAsync();
        }

        // GET: api/DettagliOrdine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DettagliOrdine>> GetDettagliOrdine(int id)
        {
            var dettagliOrdine = await _context.DettagliOrdines.FindAsync(id);

            if (dettagliOrdine == null)
            {
                return NotFound();
            }

            return dettagliOrdine;
        }

        // PUT: api/DettagliOrdine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDettagliOrdine(int id, DettagliOrdine dettagliOrdine)
        {
            if (id != dettagliOrdine.DettagliOrdineId)
            {
                return BadRequest();
            }

            _context.Entry(dettagliOrdine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DettagliOrdineExists(id))
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

        // POST: api/DettagliOrdine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DettagliOrdine>> PostDettagliOrdine(DettagliOrdine dettagliOrdine)
        {
            _context.DettagliOrdines.Add(dettagliOrdine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDettagliOrdine", new { id = dettagliOrdine.DettagliOrdineId }, dettagliOrdine);
        }

        // DELETE: api/DettagliOrdine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDettagliOrdine(int id)
        {
            var dettagliOrdine = await _context.DettagliOrdines.FindAsync(id);
            if (dettagliOrdine == null)
            {
                return NotFound();
            }

            _context.DettagliOrdines.Remove(dettagliOrdine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DettagliOrdineExists(int id)
        {
            return _context.DettagliOrdines.Any(e => e.DettagliOrdineId == id);
        }

        // Aggiungi questo metodo al tuo controller DettagliOrdineController
        [HttpGet("product-sales")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductSales()
        {
            var productSales = await _context.DettagliOrdines
                .GroupBy(d => d.ProdottoId)
                .Select(g => new
                {
                    ProdottoId = g.Key,
                    QuantitàTotale = g.Sum(d => d.Quantità),
                    ProdottoNome = g.First().Prodotto.Nome // Supponendo che tu abbia un campo Nome nella tua entità Prodotto
                })
                .ToListAsync();

            if (productSales == null)
            {
                return NotFound();
            }

            return Ok(productSales);
        }


        public class ProductSales
        {
            public int ProdottoId { get; set; }
            public string? Nome { get; set; }
            public int? QuantitaVenduta { get; set; }
        }

    }
}
