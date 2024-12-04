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
    public class CodeTestsController : ControllerBase
    {
        private readonly BackEndDbContext _context;

        public CodeTestsController(BackEndDbContext context)
        {
            _context = context;
        }

        // GET: api/CodeTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeTestViewModel>>> GetcodeTests()
        {
          if (_context.codeTests == null)
          {
              return NotFound();
          }
          var data=  _context.codeTests.ToList();
          List<CodeTestViewModel> codeTestViews = new List<CodeTestViewModel>();
            data.ForEach(e =>
            {
                var i = _context.tests.Find(e.TestId);
                CodeTestViewModel code=new CodeTestViewModel();
                code.CodeTestId = e.CodeTestId;
                code.Question = e.Question;
                code.FunctionDescription = e.FunctionDescription;

                code.SampleInput = e.SampleInput;
                code.SampleOutput = e.SampleOutput;
                code.Example = e.Example;
                code.ExampleReturn = e.ExampleReturn;

                code.Stdin1 = e.Stdin1;
                code.Stdin1Return = e.Stdin1Return;
                code.Stdin2 = e.Stdin2;
                code.Stdin2Return = e.Stdin2Return;
                code.Explanation = e.Explanation;

                code.CodeSnippetsDescription = e.CodeSnippetsDescription;
                code.LANGUAGEVERSIONS = e.LANGUAGEVERSIONS;

                code.TestId = e.TestId;
                code.TestName = i.Name;
                codeTestViews.Add(code);
            });
            return codeTestViews;
        }
        [HttpGet("Test")]
        public async Task<ActionResult<IEnumerable<CodeTestViewModel>>> GetcodeTestsByTest([FromQuery] int id)
        {
            if (_context.codeTests == null)
            {
                return NotFound();
            }
            var data = _context.codeTests.Where(e=>e.TestId==id).ToList();
            List<CodeTestViewModel> codeTestViews = new List<CodeTestViewModel>();
            data.ForEach(e =>
            {
                var i = _context.tests.Find(e.TestId);
                CodeTestViewModel code = new CodeTestViewModel();
                code.CodeTestId = e.CodeTestId;
                code.Question = e.Question;
                code.FunctionDescription = e.FunctionDescription;

                code.SampleInput = e.SampleInput;
                code.SampleOutput = e.SampleOutput;
                code.Example = e.Example;
                code.ExampleReturn = e.ExampleReturn;

                code.Stdin1 = e.Stdin1;
                code.Stdin1Return = e.Stdin1Return;
                code.Stdin2 = e.Stdin2;
                code.Stdin2Return = e.Stdin2Return;
                code.Explanation = e.Explanation;

                code.CodeSnippetsDescription = e.CodeSnippetsDescription;
                code.LANGUAGEVERSIONS = e.LANGUAGEVERSIONS;

                code.TestId = e.TestId;
                code.TestName = i.Name;
                codeTestViews.Add(code);
            });
            return codeTestViews;
        }
        // GET: api/CodeTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeTestViewModel>> GetCodeTest(int id)
        {
          if (_context.codeTests == null)
          {
              return NotFound();
          }
            var e = await _context.codeTests.FindAsync(id);

            if (e == null)
            {
                return NotFound();
            }
            var i = await _context.tests.FindAsync(e.TestId);
            CodeTestViewModel code = new CodeTestViewModel();
            code.CodeTestId = e.CodeTestId;
            code.Question = e.Question;
            code.FunctionDescription = e.FunctionDescription;

            code.SampleInput = e.SampleInput;
            code.SampleOutput = e.SampleOutput;
            code.Example = e.Example;
            code.ExampleReturn = e.ExampleReturn;

            code.Stdin1 = e.Stdin1;
            code.Stdin1Return = e.Stdin1Return;
            code.Stdin2 = e.Stdin2;
            code.Stdin2Return = e.Stdin2Return;
            code.Explanation = e.Explanation;

            code.CodeSnippetsDescription = e.CodeSnippetsDescription;
            code.LANGUAGEVERSIONS = e.LANGUAGEVERSIONS;

            code.TestId = e.TestId;
            code.TestName = i.Name;
            return code;
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
        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file, [FromQuery] int id)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is not selected or empty.");

            var codeTestList = new List<CodeTest>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Sử dụng dòng đầu tiên làm tiêu đề
                            }
                        });

                        var dataTable = dataSet.Tables[0];

                        foreach (DataRow row in dataTable.Rows)
                        {
                            var codeTest = new CodeTest
                            {
                               
                                Question = row["Question"].ToString() ?? string.Empty,
                                FunctionDescription = row["FunctionDescription"].ToString() ?? string.Empty,
                                SampleInput = row["SampleInput"].ToString() ?? string.Empty,
                                SampleOutput = row["SampleOutput"].ToString() ?? string.Empty,
                                Example = row["Example"].ToString() ?? string.Empty,
                                ExampleReturn = row["ExampleReturn"].ToString() ?? string.Empty,
                                Stdin1 = row["Stdin1"].ToString() ?? string.Empty,
                                Stdin1Return = row["Stdin1Return"].ToString() ?? string.Empty,
                                Stdin2 = row["Stdin2"].ToString() ?? string.Empty,
                                Stdin2Return = row["Stdin2Return"].ToString() ?? string.Empty,
                                Explanation = row["Explanation"].ToString() ?? string.Empty,
                                CodeSnippetsDescription = row["CodeSnippetsDescription"].ToString() ?? string.Empty,
                                LANGUAGEVERSIONS = row["LANGUAGEVERSIONS"].ToString() ?? string.Empty,
                                TestId= id
                            };

                            codeTestList.Add(codeTest);
                        }
                    }
                }
                await _context.codeTests.AddRangeAsync(codeTestList);
                await _context.SaveChangesAsync();
                // TODO: Lưu `codeTestList` vào database tại đây
                return Ok(new { Message = "File uploaded successfully.", Data = codeTestList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private bool CodeTestExists(int id)
        {
            return (_context.codeTests?.Any(e => e.CodeTestId == id)).GetValueOrDefault();
        }
    }
}
