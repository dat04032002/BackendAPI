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
    public class UserTestDetailsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public UserTestDetailsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/UserTestDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTestDetail>>> GetuserTestDetails()
        {
          if (_context.userTestDetails == null)
          {
              return NotFound();
          }
            return await _context.userTestDetails.ToListAsync();
        }

        // GET: api/UserTestDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTestDetail>> GetUserTestDetail(int id)
        {
          if (_context.userTestDetails == null)
          {
              return NotFound();
          }
            var userTestDetail = await _context.userTestDetails.FindAsync(id);

            if (userTestDetail == null)
            {
                return NotFound();
            }

            return userTestDetail;
        }

        // PUT: api/UserTestDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTestDetail(int id, UserTestDetail userTestDetail)
        {
            if (id != userTestDetail.UserTestDetailId)
            {
                return BadRequest();
            }

            _context.Entry(userTestDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTestDetailExists(id))
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

        // POST: api/UserTestDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTestDetail>> PostUserTestDetail(UserTestDetail userTestDetail)
        {
          if (_context.userTestDetails == null)
          {
              return Problem("Entity set 'BackEndDbContext.userTestDetails'  is null.");
          }
            _context.userTestDetails.Add(userTestDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserTestDetail", new { id = userTestDetail.UserTestDetailId }, userTestDetail);
        }

        // DELETE: api/UserTestDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTestDetail(int id)
        {
            if (_context.userTestDetails == null)
            {
                return NotFound();
            }
            var userTestDetail = await _context.userTestDetails.FindAsync(id);
            if (userTestDetail == null)
            {
                return NotFound();
            }

            _context.userTestDetails.Remove(userTestDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTestDetailExists(int id)
        {
            return (_context.userTestDetails?.Any(e => e.UserTestDetailId == id)).GetValueOrDefault();
        }
    }
}
