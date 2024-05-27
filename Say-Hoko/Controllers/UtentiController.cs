using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Say_Hoko.Models;

namespace Say_Hoko.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UtentiController : ControllerBase
    {
        private readonly SushiContext _context;
        private readonly IConfiguration _configuration;

        public UtentiController(SushiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Utenti
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utenti>>> GetUtentis()
        {
            return await _context.Utentis.ToListAsync();
        }

        // GET: api/Utenti/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utenti>> GetUtenti(int id)
        {
            var utenti = await _context.Utentis.FindAsync(id);

            if (utenti == null)
            {
                return NotFound();
            }

            return utenti;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(Utenti model)
        {
            try
            {
                var user = await _context.Utentis.SingleOrDefaultAsync(u => u.Nome == model.Nome);

                if (user == null)
                {
                    // Utente non trovato
                    return NotFound("Utente non trovato.");
                }


                // Converti l'hash della password salvata da base64 a byte array
                if (string.IsNullOrEmpty(user.Password))
                {
                    // Password non impostata
                    return Unauthorized("Password non impostata.");
                }

                byte[] hashBytes;
                try
                {
                    hashBytes = Convert.FromBase64String(user.Password);
                }
                catch (FormatException)
                {
                    // Formato della password non valido
                    return Unauthorized("Formato della password non valido.");
                }


                // Estrai il sale dall'hash
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                if (string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Password non fornita.");
                }

                // Hash della password fornita nel modello
                var pbkdf2 = new Rfc2898DeriveBytes(model.Password, salt, 10000, HashAlgorithmName.SHA256);

                byte[] hash = pbkdf2.GetBytes(20);

                Console.WriteLine("Password hash: " + Convert.ToBase64String(hash));

                // Controlla se gli hash corrispondono
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        // Password non corrispondente
                        return Unauthorized("Password non corretta.");
                    }
                }

                // Verifica che il nome dell'utente non sia null
                if (string.IsNullOrEmpty(user.Nome))
                {
                    return StatusCode(500, "Nome utente non configurato.");
                }

                // Creazione del token JWT
                var tokenHandler = new JwtSecurityTokenHandler();

                // Verifica che la chiave JWT non sia null
                string? jwtKey = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    return StatusCode(500, "Chiave JWT non configurata.");
                }

                // Genera una chiave di almeno 256 bit utilizzando un algoritmo di hash
                using var sha = SHA256.Create();
                var key = sha.ComputeHash(Encoding.UTF8.GetBytes(jwtKey));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
        new(ClaimTypes.Name, user.UserId.ToString()),
        new(ClaimTypes.GivenName, user.Nome),
        new Claim(ClaimTypes.Role, user.Role)

                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["Jwt:Issuer"]
                };


                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });



            }
            catch (Exception ex)
            {
                // Gestione degli errori generici
                return StatusCode(500, $"Errore durante il login: {ex.Message}");
            }
        }


        // PUT: api/Utenti/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtenti(int id, Utenti utenti)
        {
            if (id != utenti.UserId)
            {
                return BadRequest();
            }

            _context.Entry(utenti).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtentiExists(id))
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

        // POST: api/Utenti
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utenti>> PostUtenti(Utenti utenti)
        {
            // Verifica che l'utente non esista già
            if (_context.Utentis.Any(u => u.Nome == utenti.Nome))
            {
                return BadRequest(new { message = "Nome utente già in uso." });
            }

            utenti.Role = "utente";

            // Genera un sale
            byte[] salt = new byte[16];
            RandomNumberGenerator.Fill(salt);



            // Genera l'hash
            if (string.IsNullOrEmpty(utenti.Password))
            {
                return BadRequest("Password non fornita.");
            }

            // Genera l'hash
            var pbkdf2 = new Rfc2898DeriveBytes(utenti.Password, salt, 10000, HashAlgorithmName.SHA256);


            byte[] hash = pbkdf2.GetBytes(20);

            // Combina il sale e l'hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Converti in base64
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            // Salva l'hash (non la password!) nell'oggetto utente
            utenti.Password = savedPasswordHash;

            _context.Utentis.Add(utenti);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtenti", new { id = utenti.UserId }, utenti);
        }

        // DELETE: api/Utenti/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtenti(int id)
        {
            var utenti = await _context.Utentis.FindAsync(id);
            if (utenti == null)
            {
                return NotFound();
            }

            _context.Utentis.Remove(utenti);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Add this method to check if a string is a valid base64 string

        private bool UtentiExists(int id)
        {
            return _context.Utentis.Any(e => e.UserId == id);
        }
    }
}
