using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiServer.Data;
using WebApiServer.Models;

namespace WebApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly ExamDbContext _context;

        public ExamController(ExamDbContext context)
        {
            _context = context;
        }

        // GET: api/Exam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetExams()
        {
            return await _context.Exams
                .Select(e => new { e.Id, e.Name })
                .ToListAsync();
        }

        // GET: api/Exam/MyExamName
        [HttpGet("{name}")]
        public async Task<ActionResult<Exam>> GetExamByName(string name)
        {
            var exam = await _context.Exams.Include(e => e.Questions)
            .ThenInclude(q => q.Options).FirstOrDefaultAsync(e => e.Name == name);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }

        private async Task<ActionResult<Exam>> GetExam(int id)
        {
            var exam = await _context.Exams.FindAsync(id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }

        // PUT: api/Exam/5
        [HttpPut("{id}")]
        public IActionResult PutExam(int id, Exam exam)
        {
            if (id != exam.Id)
            {
                return BadRequest();
            }

            // get the existing exam object from the database
            var existingExam = _context.Exams.Include(e => e.Questions).ThenInclude(q => q.Options).FirstOrDefault(e => e.Id == exam.Id);

            if (existingExam != null)
            {
                // check which questions and options have been removed
                var removedQuestions = existingExam.Questions.Where(q => !exam.Questions.Any(eq => eq.Id == q.Id)).ToList();
                var removedOptions = existingExam.Questions.SelectMany(q => q.Options).Where(o => !exam.Questions.SelectMany(eq => eq.Options).Any(qo => qo.Id == o.Id)).ToList();

                // remove questions and options from the context if they have been removed from the exam
                removedQuestions.ForEach(q => _context.Entry(q).State = EntityState.Deleted);
                removedOptions.ForEach(o => _context.Entry(o).State = EntityState.Deleted);

                // update the remaining questions and options in the database
                foreach (var question in exam.Questions)
                {
                    var existingQuestion = existingExam.Questions.FirstOrDefault(q => q.Id == question.Id);
                    if (existingQuestion != null)
                    {
                        // update the question properties
                        existingQuestion.QuestionText = question.QuestionText;
                        existingQuestion.CorrectAnswerIndex = question.CorrectAnswerIndex;
                        existingQuestion.RandomOrder = question.RandomOrder;

                        // check which options have been removed
                        var removedQuestionOptions = existingQuestion.Options.Where(o => !question.Options.Any(qo => qo.Id == o.Id)).ToList();

                        // remove the options from the context if they have been removed from the question
                        removedQuestionOptions.ForEach(o => _context.Entry(o).State = EntityState.Deleted);

                        // update the remaining options in the question
                        foreach (var option in question.Options)
                        {
                            var existingOption = existingQuestion.Options.FirstOrDefault(o => o.Id == option.Id);
                            if (existingOption != null && option.Id != 0)
                            {
                                // update the option value
                                existingOption.Value = option.Value;
                            }
                            else
                            {
                                // add a new option to the question
                                existingQuestion.Options.Add(new Option
                                {
                                    Value = option.Value
                                });
                            }
                        }
                    }
                    else
                    {
                        // add a new question to the exam
                        existingExam.Questions.Add(new Question
                        {
                            QuestionText = question.QuestionText,
                            CorrectAnswerIndex = question.CorrectAnswerIndex,
                            RandomOrder = question.RandomOrder,
                            Options = question.Options.Select(o => new Option
                            {
                                Value = o.Value
                            }).ToList()
                        });
                    }
                }

                // update the exam properties
                existingExam.Name = exam.Name;
                existingExam.Date = exam.Date;
                existingExam.TeacherName = exam.TeacherName;
                existingExam.StartTime = exam.StartTime;
                existingExam.TotalTime = exam.TotalTime;

                // save the changes to the database
                _context.SaveChanges();

                return Ok(existingExam);
            }
            else
            {
                return NotFound();
            }
        }


        // POST: api/Exam
        //if exam id exist go to PutExam
        [HttpPost]
        public async Task<ActionResult<Exam>> PostExam(Exam exam)
        {
            if (_context.Exams.FirstOrDefault(e => e.Id == exam.Id) != null)
            {
                var existingExam = await _context.Exams.Include(e => e.Questions)
                .ThenInclude(q => q.Options).FirstOrDefaultAsync(e => e.Id == exam.Id);

                // Update the existing exam with the new values
                if (existingExam != null)
                {
                    if (existingExam.Name != exam.Name)
                    {
                        if (_context.Exams.FirstOrDefault(e => e.Name == exam.Name) != null)
                        {
                            return BadRequest("An exam with the same name already exists.");
                        }
                    }

                    PutExam(existingExam.Id, exam);
                }
                else
                {
                    return BadRequest("Server error");
                }

                return Ok(existingExam);
            }
            if (_context.Exams.FirstOrDefault(e => e.Name == exam.Name) != null)
            {
                return BadRequest("An exam with the same name already exists.");
            }

                // If the exam with the same name doesn't exist, create a new exam
                _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            return Ok(exam);
        }


        // DELETE: api/Exam/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}
