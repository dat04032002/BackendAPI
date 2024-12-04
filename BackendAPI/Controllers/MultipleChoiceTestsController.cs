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
using static System.Net.Mime.MediaTypeNames;
using ExcelDataReader;
using System.Data;

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
        public async Task<ActionResult<IEnumerable<MultipleChoiceTestViewModel>>> GetmultipleChoiceTests()
        {
          if (_context.multipleChoiceTests == null)
          {
              return NotFound();
          }
            var data = _context.multipleChoiceTests.ToListAsync().Result;
            List<MultipleChoiceTestViewModel> rel = new List<MultipleChoiceTestViewModel>();
             data.ForEach(item =>
             {
                MultipleChoiceTestViewModel item1 = new MultipleChoiceTestViewModel();
               var test= _context.tests.Find(item.TestId);
                item1.MultipleChoiceTestId = item.MultipleChoiceTestId;
                item1.Question = item.Question;
                item1.PlanA = item.PlanA;
                item1.PlanB = item.PlanB;
                item1.PlanC = item.PlanC;
                item1.PlanD = item.PlanD;
                item1.CorrectAnswer = item.CorrectAnswer;
                item1.TestId = item.TestId;
                item1.TestName = test?.Name;
                 rel.Add(item1);
             });
            return rel;
        }

        // GET: api/MultipleChoiceTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MultipleChoiceTestViewModel>> GetMultipleChoiceTest(int id)
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
            var datatest= await _context.tests.FindAsync(multipleChoiceTest.TestId);

            var data=new MultipleChoiceTestViewModel();
            data.MultipleChoiceTestId = multipleChoiceTest.MultipleChoiceTestId;
            data.Question = multipleChoiceTest.Question;
            data.PlanA = multipleChoiceTest.PlanA;
            data.PlanB = multipleChoiceTest.PlanB;
            data.PlanC = multipleChoiceTest.PlanC;
            data.PlanD = multipleChoiceTest.PlanD;
            data.CorrectAnswer = multipleChoiceTest.CorrectAnswer;
            data.TestId = multipleChoiceTest.TestId;
            data.TestName = datatest?.Name;
            return data;
        }
        // GET: api/MultipleChoiceTests/Test?id=5
        [HttpGet("Test")]
        public async Task<ActionResult<IEnumerable<MultipleChoiceTestViewModel>>> GetMultipleChoiceTestByTestID([FromQuery] int id)
        {
            if (_context.multipleChoiceTests == null)
            {
                return NotFound();
            }
            var data = _context.multipleChoiceTests.Where(e=>e.TestId==id).ToListAsync().Result;
            List<MultipleChoiceTestViewModel> rel = new List<MultipleChoiceTestViewModel>();
            data.ForEach(item =>
            {
                MultipleChoiceTestViewModel item1 = new MultipleChoiceTestViewModel();
                var test = _context.tests.Find(item.TestId);
                item1.MultipleChoiceTestId = item.MultipleChoiceTestId;
                item1.Question = item.Question;
                item1.PlanA = item.PlanA;
                item1.PlanB = item.PlanB;
                item1.PlanC = item.PlanC;
                item1.PlanD = item.PlanD;
                item1.CorrectAnswer = item.CorrectAnswer;
                item1.TestId = item.TestId;
                item1.TestName = test?.Name;
                rel.Add(item1);
            });
            return rel;
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
        [HttpPost("upload-excel")]
        public IActionResult UploadExcel(IFormFile file, [FromQuery] int id)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var dataTable = result.Tables[0]; // Chọn sheet đầu tiên

                        // Duyệt qua các dòng trong bảng
                        var multipleChoiceTests = new List<MultipleChoiceTest>();
                        var errors = new List<string>(); // Danh sách chứa lỗi

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            DataRow row = dataTable.Rows[i];

                            // Kiểm tra các cột null hoặc trống
                            if (string.IsNullOrEmpty(row["Question"].ToString()))
                                errors.Add($"Dòng {i + 1}: Question không được để trống.");
                            if (string.IsNullOrEmpty(row["PlanA"].ToString()))
                                errors.Add($"Dòng {i + 1}: PlanA không được để trống.");
                            if (string.IsNullOrEmpty(row["PlanB"].ToString()))
                                errors.Add($"Dòng {i + 1}: PlanB không được để trống.");
                            if (string.IsNullOrEmpty(row["PlanC"].ToString()))
                                errors.Add($"Dòng {i + 1}: PlanC không được để trống.");
                            if (string.IsNullOrEmpty(row["PlanD"].ToString()))
                                errors.Add($"Dòng {i + 1}: PlanD không được để trống.");
                            if (string.IsNullOrEmpty(row["CorrectAnswer"].ToString()))
                                errors.Add($"Dòng {i + 1}: CorrectAnswer không được để trống.");
                            

                            // Nếu có lỗi, dừng và trả về BadRequest ngay lập tức
                            if (errors.Count > 0)
                            {
                                return BadRequest(new { message = "Dữ liệu không hợp lệ", errors });
                            }

                            // Nếu không có lỗi, tiếp tục thêm dữ liệu
                            var test = new MultipleChoiceTest
                            {
                                Question = row["Question"].ToString(),
                                PlanA = row["PlanA"].ToString(),
                                PlanB = row["PlanB"].ToString(),
                                PlanC = row["PlanC"].ToString(),
                                PlanD = row["PlanD"].ToString(),
                                CorrectAnswer = row["CorrectAnswer"].ToString(),
                                TestId = id
                            };
                            multipleChoiceTests.Add(test);
                        }

                        // Lưu dữ liệu vào cơ sở dữ liệu nếu không có lỗi
                        _context.multipleChoiceTests.AddRange(multipleChoiceTests);
                        _context.SaveChanges();

                        return Ok(new { message = "File uploaded successfully!" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
            }
        }

        private bool MultipleChoiceTestExists(int id)
        {
            return (_context.multipleChoiceTests?.Any(e => e.MultipleChoiceTestId == id)).GetValueOrDefault();
        }
    }
}
