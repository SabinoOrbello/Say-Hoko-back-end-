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
    public class ProdottiController : ControllerBase
    {
        private readonly SushiContext _context;

        public ProdottiController(SushiContext context)
        {
            _context = context;
        }

        // GET: api/Prodotti
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prodotti>>> GetProdottis()
        {
            return await _context.Prodottis.ToListAsync();
        }

        // GET: api/Prodotti/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prodotti>> GetProdotti(int id)
        {
            var prodotti = await _context.Prodottis.FindAsync(id);

            if (prodotti == null)
            {
                return NotFound();
            }

            return Ok(prodotti);
        }

        // GET: api/Prodotti/Categoria/5
        [HttpGet("Categoria/{id}")]
        public async Task<ActionResult<IEnumerable<Prodotti>>> GetProdottiByCategoria(int id)
        {
            return await _context.Prodottis.Where(p => p.CategorieId == id).ToListAsync();
        }


        // PUT: api/Prodotti/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdotti(int id, Prodotti prodotti)
        {
            if (id != prodotti.ProdottoId)
            {
                return BadRequest();
            }

            _context.Entry(prodotti).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdottiExists(id))
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

        // POST: api/Prodotti
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prodotti>> PostProdotti(Prodotti prodotti)
        {
            _context.Prodottis.Add(prodotti);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdotti", new { id = prodotti.ProdottoId }, prodotti);
        }

        // DELETE: api/Prodotti/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdotti(int id)
        {
            var prodotti = await _context.Prodottis.FindAsync(id);
            if (prodotti == null)
            {
                return NotFound();
            }

            _context.Prodottis.Remove(prodotti);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdottiExists(int id)
        {
            return _context.Prodottis.Any(e => e.ProdottoId == id);
        }
    }
}
