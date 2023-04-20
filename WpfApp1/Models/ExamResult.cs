using System;
using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class ExamResult
    {
        public ExamResult()
        {
            Errors = new List<Error>();
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int ExamId { get; set; }
        public int Score { get; set; }
        public List<Error> Errors { get; set; }

        public static implicit operator ExamResult(List<Exam> v)
        {
            throw new NotImplementedException();
        }
    }

    public class Error
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string SelectedAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
