using System;
using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class Exam
    {
        public Exam()
        {
            Questions = new List<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalTime { get; set; }
        public List<Question> Questions { get; set; }
    }
}
