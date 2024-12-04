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
using ExcelDataReader;
using System.Data;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public TestsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/Tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestViewModel>>> Gettests()
        {
          if (_context.tests == null)
          {
              return NotFound();
          }
          var test= _context.tests.ToListAsync().Result;
          var danhmuc=_context.danhMucChiTiets.ToListAsync().Result;
            if (test==null||test.Count == 0)
            {
                return NotFound();
            }
            
            var data = from t in test
                       join d in danhmuc
                       on t.DanhMucChiTietId equals d.DanhMucChiTietId
                       select new TestViewModel
                       {
                           TestId = t.TestId,
                           Name = t.Name,
                           Description = t.Description,
                           Time = t.Time,
                           EndDate = t.EndDate,
                           StartDate = t.StartDate,
                           Danhmuc = d.Name,
                           DanhMucChiTietId=d.DanhMucChiTietId
                       };
            return data.ToList();
        }

        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestViewModel>> GetTest(int id)
        {
          if (_context.tests == null)
          {
              return NotFound();
          }
            var t = await _context.tests.FindAsync(id);
          
            if (t == null)
            {
                return NotFound();
            }

            var d = await _context.danhMucChiTiets.FindAsync(t.DanhMucChiTietId);
            if (d == null)
            {
                return NotFound();
            }
            var code =  _context.codeTests.Where(e => e.TestId == t.TestId);
            var mup= _context.multipleChoiceTests.Where(e => e.TestId == t.TestId);
            TestViewModel test = new TestViewModel
            {
                TestId = t.TestId,
                Name = t.Name,
                Description = t.Description,
                Time = t.Time,
                EndDate = t.EndDate,
                StartDate = t.StartDate,
                Danhmuc = d.Name,
                DanhMucChiTietId = d.DanhMucChiTietId
            };
            if (code!=null&&code.Count()!=0)
            {
                test.CodeTest = true;
            }
            if (mup != null && mup.Count() != 0)
            {
                test.MultipleChoiceTest = true;
            }

            return test;
        }

        // PUT: api/Tests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest(int id, Test test)
        {
            if (id != test.TestId)
            {
                return BadRequest();
            }

            _context.Entry(test).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
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

        // POST: api/Tests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test test)
        {
          if (_context.tests == null)
          {
              return Problem("Entity set 'BackEndDbContext.tests'  is null.");
          }
            _context.tests.Add(test);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTest", new { id = test.TestId }, test);
        }

        // DELETE: api/Tests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            if (_context.tests == null)
            {
                return NotFound();
            }
            var test = await _context.tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            _context.tests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file, [FromQuery] int id)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is not selected or empty.");

            var testList = new List<Test>();

            try
            {
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
                            var test = new Test
                            {
                               
                                Name = row["Name"].ToString() ?? string.Empty,
                                Description = row["Description"].ToString() ?? string.Empty,
                                Time = int.Parse(row["Time"].ToString() ?? "0"),
                                StartDate = ParseDate(row["StartDate"].ToString() ?? DateTime.MinValue.ToString(), "dd/MM/yyyy"),
                                EndDate = ParseDate(row["EndDate"].ToString() ?? DateTime.MinValue.ToString(), "dd/MM/yyyy"),
                                DanhMucChiTietId=id
                            };

                            testList.Add(test);
                        }
                    }
                }
                await _context.tests.AddRangeAsync(testList);
                await _context.SaveChangesAsync();

                // TODO: Lưu `testList` vào database tại đây
                return Ok($"Đã thêm {testList.Count} bài test vào cơ sở dữ liệu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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


        private bool TestExists(int id)
        {
            return (_context.tests?.Any(e => e.TestId == id)).GetValueOrDefault();
        }
    }
}
