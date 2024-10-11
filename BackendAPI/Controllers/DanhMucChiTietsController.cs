using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPI.DataEnity;
using Core.Model;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucChiTietsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public DanhMucChiTietsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/DanhMucChiTiets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMucChiTiet>>> GetdanhMucChiTiets()
        {
          if (_context.danhMucChiTiets == null)
          {
              return NotFound();
          }
            return await _context.danhMucChiTiets.ToListAsync();
        }

        // GET: api/DanhMucChiTiets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DanhMucChiTiet>> GetDanhMucChiTiet(int id)
        {
          if (_context.danhMucChiTiets == null)
          {
              return NotFound();
          }
            var danhMucChiTiet = await _context.danhMucChiTiets.FindAsync(id);

            if (danhMucChiTiet == null)
            {
                return NotFound();
            }

            return danhMucChiTiet;
        }

        // PUT: api/DanhMucChiTiets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDanhMucChiTiet(int id, DanhMucChiTiet danhMucChiTiet)
        {
            if (id != danhMucChiTiet.DanhMucChiTietId)
            {
                return BadRequest();
            }

            _context.Entry(danhMucChiTiet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhMucChiTietExists(id))
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

        // POST: api/DanhMucChiTiets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DanhMucChiTiet>> PostDanhMucChiTiet(DanhMucChiTiet danhMucChiTiet)
        {
          if (_context.danhMucChiTiets == null)
          {
              return Problem("Entity set 'BackEndDbContext.danhMucChiTiets'  is null.");
          }
            _context.danhMucChiTiets.Add(danhMucChiTiet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDanhMucChiTiet", new { id = danhMucChiTiet.DanhMucChiTietId }, danhMucChiTiet);
        }

        // DELETE: api/DanhMucChiTiets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhMucChiTiet(int id)
        {
            if (_context.danhMucChiTiets == null)
            {
                return NotFound();
            }
            var danhMucChiTiet = await _context.danhMucChiTiets.FindAsync(id);
            if (danhMucChiTiet == null)
            {
                return NotFound();
            }

            _context.danhMucChiTiets.Remove(danhMucChiTiet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DanhMucChiTietExists(int id)
        {
            return (_context.danhMucChiTiets?.Any(e => e.DanhMucChiTietId == id)).GetValueOrDefault();
        }
    }
}
