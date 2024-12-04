using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPI.DataEnity;
using Core.Model;
using System.Globalization;
using ExcelDataReader;
using System.Data;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public UsersController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusers()
        {
          if (_context.users == null)
          {
              return NotFound();
          }
            return await _context.users.ToListAsync();
        }
       
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.users == null)
          {
              return NotFound();
          }
            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.users == null)
          {
              return Problem("Entity set 'BackEndDbContext.users'  is null.");
          }
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ.");
            }

            var allowedContentTypes = new[] { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
            var allowedExtensions = new[] { ".xls", ".xlsx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedContentTypes.Contains(file.ContentType) || !allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("File không đúng định dạng Excel.");
            }

            try
            {
                var userList = new List<User>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0; // Đặt lại con trỏ stream về đầu

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // Hỗ trợ mã hóa
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Đọc dòng đầu tiên làm tiêu đề
                            }
                        });

                        var dataTable = dataSet.Tables[0]; // Lấy sheet đầu tiên

                        foreach (DataRow row in dataTable.Rows)
                        {
                            var user = new User
                            {
                              
                                UserName = row["UserName"].ToString() ?? string.Empty,
                                Email = row["Email"].ToString() ?? string.Empty,
                                Password = row["Password"].ToString() ?? string.Empty,
                                Brithday = ParseDate(row["Brithday"].ToString() ?? DateTime.MinValue.ToString(), "dd/MM/yyyy"),
                                Position = row["Position"].ToString() ?? string.Empty,
                                Role = row["Role"].ToString() ?? string.Empty
                            };

                            userList.Add(user);
                        }
                    }
                

                // Thêm vào cơ sở dữ liệu
                await _context.users.AddRangeAsync(userList);
                    await _context.SaveChangesAsync();

                    return Ok($"Đã thêm {userList.Count} user vào cơ sở dữ liệu.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi xử lý file: {ex.Message}");
            }
        }
        private DateTime ParseDate(string dateString, string format)
        {
            if (DateTime.TryParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate;
            }
            return DateTime.MinValue; // Hoặc giá trị mặc định nếu không hợp lệ
        }
        private bool UserExists(int id)
        {
            return (_context.users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
