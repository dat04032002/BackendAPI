using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPI.DataEnity;
using Core.Model;
using Core.ModelView;
using System.Xml.Linq;

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
        public async Task<ActionResult<List<DanhMucViewModel>>> GetdanhMucChiTiets()
        {
          if (_context.danhMucChiTiets == null)
          {
              return NotFound();
          }
            var datadanhMucChiTiets = _context.danhMucChiTiets.ToListAsync().Result;
            var datadanhmuc=_context.danhMucs.ToListAsync().Result;
           
            if (datadanhMucChiTiets != null)
            {
                var data =( from c in datadanhmuc
                           join d in datadanhMucChiTiets
                           on c.DanhMucId equals d.DanhMucId
                           select new DanhMucViewModel
                           {
                               DanhMucId=c.DanhMucId,
                               LoaiName=c.Name,
                               DanhMucChiTietId=d.DanhMucChiTietId,
                               Name=d.Name,
                               type= d.type==0?"Danh mục dev":"Danh mục nghiệp vụ"
                           }).ToList();
                return data;
            }
           
            return new List<DanhMucViewModel>();
        }

        // GET: api/DanhMucChiTiets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DanhMucViewModel>> GetDanhMucChiTiet(int id)
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
          
            var datadanhmuc = await _context.danhMucs.FindAsync(danhMucChiTiet.DanhMucChiTietId);
            if (datadanhmuc == null)
            {
                return NotFound();
            }

            return new DanhMucViewModel
            {
                DanhMucId = danhMucChiTiet.DanhMucId,
                LoaiName = datadanhmuc.Name,
                DanhMucChiTietId = danhMucChiTiet.DanhMucChiTietId,
                Name = danhMucChiTiet.Name,
                type = danhMucChiTiet.type == 0 ? "Danh mục dev" : "Danh mục nghiệp vụ"
            };
              
          
         
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
