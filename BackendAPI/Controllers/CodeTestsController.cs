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
    public class CodeTestsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public CodeTestsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/CodeTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeTest>>> GetcodeTests()
        {
          if (_context.codeTests == null)
          {
              return NotFound();
          }
            return await _context.codeTests.ToListAsync();
        }

        // GET: api/CodeTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeTest>> GetCodeTest(int id)
        {
          if (_context.codeTests == null)
          {
              return NotFound();
          }
            var codeTest = await _context.codeTests.FindAsync(id);

            if (codeTest == null)
            {
                return NotFound();
            }

            return codeTest;
        }

        // PUT: api/CodeTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodeTest(int id, CodeTest codeTest)
        {
            if (id != codeTest.CodeTestId)
            {
                return BadRequest();
            }

            _context.Entry(codeTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeTestExists(id))
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

        // POST: api/CodeTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CodeTest>> PostCodeTest(CodeTest codeTest)
        {
          if (_context.codeTests == null)
          {
              return Problem("Entity set 'BackEndDbContext.codeTests'  is null.");
          }
            _context.codeTests.Add(codeTest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCodeTest", new { id = codeTest.CodeTestId }, codeTest);
        }

        // DELETE: api/CodeTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCodeTest(int id)
        {
            if (_context.codeTests == null)
            {
                return NotFound();
            }
            var codeTest = await _context.codeTests.FindAsync(id);
            if (codeTest == null)
            {
                return NotFound();
            }

            _context.codeTests.Remove(codeTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodeTestExists(int id)
        {
            return (_context.codeTests?.Any(e => e.CodeTestId == id)).GetValueOrDefault();
        }
    }
}
