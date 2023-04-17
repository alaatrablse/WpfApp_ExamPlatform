using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiServer.Data;
using WebApiServer.Models;

namespace WebApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : ControllerBase
    {
        private readonly ExamDbContext _context;

        public ExamResultController(ExamDbContext context)
        {
            _context = context;
        }

        // GET: api/ExamResult
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamResult>>> GetExamResults()
        {
            return await _context.ExamResults.Include(e => e.Errors).ToListAsync();
        }

        // GET: api/ExamResult/5/2004
        [HttpGet("{id}/{examid}")]
        public async Task<ActionResult<ExamResult>> GetExamResult(int id, int examid)
        {
            var examResult = await _context.ExamResults.Include(e => e.Errors)
                .FirstOrDefaultAsync(e => e.StudentId == id && e.ExamId == examid);


            if (examResult == null)
            {
                return NotFound();
            }

            return examResult;
        }

        // PUT: api/ExamResult/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamResult(int id, ExamResult examResult)
        {
            if (id != examResult.Id)
            {
                return BadRequest();
            }

            _context.Entry(examResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamResultExists(id))
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

        // POST: api/ExamResult
        [HttpPost]
        public async Task<ActionResult<ExamResult>> PostExamResult(ExamResult examResult)
        {
            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync();

            return Ok(examResult);
        }

        // DELETE: api/ExamResult/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamResult(int id)
        {
            var examResult = await _context.ExamResults.FindAsync(id);
            if (examResult == null)
            {
                return NotFound();
            }

            _context.ExamResults.Remove(examResult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamResultExists(int id)
        {
            return _context.ExamResults.Any(e => e.Id == id);
        }
    }


}
