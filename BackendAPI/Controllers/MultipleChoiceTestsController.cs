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
    public class MultipleChoiceTestsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public MultipleChoiceTestsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/MultipleChoiceTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MultipleChoiceTest>>> GetmultipleChoiceTests()
        {
          if (_context.multipleChoiceTests == null)
          {
              return NotFound();
          }
            return await _context.multipleChoiceTests.ToListAsync();
        }

        // GET: api/MultipleChoiceTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MultipleChoiceTest>> GetMultipleChoiceTest(int id)
        {
          if (_context.multipleChoiceTests == null)
          {
              return NotFound();
          }
            var multipleChoiceTest = await _context.multipleChoiceTests.FindAsync(id);

            if (multipleChoiceTest == null)
            {
                return NotFound();
            }

            return multipleChoiceTest;
        }

        // PUT: api/MultipleChoiceTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMultipleChoiceTest(int id, MultipleChoiceTest multipleChoiceTest)
        {
            if (id != multipleChoiceTest.MultipleChoiceTestId)
            {
                return BadRequest();
            }

            _context.Entry(multipleChoiceTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MultipleChoiceTestExists(id))
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

        // POST: api/MultipleChoiceTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MultipleChoiceTest>> PostMultipleChoiceTest(MultipleChoiceTest multipleChoiceTest)
        {
          if (_context.multipleChoiceTests == null)
          {
              return Problem("Entity set 'BackEndDbContext.multipleChoiceTests'  is null.");
          }
            _context.multipleChoiceTests.Add(multipleChoiceTest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMultipleChoiceTest", new { id = multipleChoiceTest.MultipleChoiceTestId }, multipleChoiceTest);
        }

        // DELETE: api/MultipleChoiceTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMultipleChoiceTest(int id)
        {
            if (_context.multipleChoiceTests == null)
            {
                return NotFound();
            }
            var multipleChoiceTest = await _context.multipleChoiceTests.FindAsync(id);
            if (multipleChoiceTest == null)
            {
                return NotFound();
            }

            _context.multipleChoiceTests.Remove(multipleChoiceTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MultipleChoiceTestExists(int id)
        {
            return (_context.multipleChoiceTests?.Any(e => e.MultipleChoiceTestId == id)).GetValueOrDefault();
        }
    }
}
