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
    public class UserTestsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public UserTestsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/UserTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTest>>> GetuserTests()
        {
          if (_context.userTests == null)
          {
              return NotFound();
          }
            return await _context.userTests.ToListAsync();
        }

        // GET: api/UserTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTest>> GetUserTest(int id)
        {
          if (_context.userTests == null)
          {
              return NotFound();
          }
            var userTest = await _context.userTests.FindAsync(id);

            if (userTest == null)
            {
                return NotFound();
            }

            return userTest;
        }

        // PUT: api/UserTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTest(int id, UserTest userTest)
        {
            if (id != userTest.UserTestId)
            {
                return BadRequest();
            }

            _context.Entry(userTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTestExists(id))
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

        // POST: api/UserTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTest>> PostUserTest(UserTest userTest)
        {
          if (_context.userTests == null)
          {
              return Problem("Entity set 'BackEndDbContext.userTests'  is null.");
          }
            _context.userTests.Add(userTest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserTest", new { id = userTest.UserTestId }, userTest);
        }

        // DELETE: api/UserTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTest(int id)
        {
            if (_context.userTests == null)
            {
                return NotFound();
            }
            var userTest = await _context.userTests.FindAsync(id);
            if (userTest == null)
            {
                return NotFound();
            }

            _context.userTests.Remove(userTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTestExists(int id)
        {
            return (_context.userTests?.Any(e => e.UserTestId == id)).GetValueOrDefault();
        }
    }
}
