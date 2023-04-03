using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{

    public class Question
    {
        public Question()
        {
            Options = new List<QuestionOption>();
        }
       
        public Question(string questionText, List<QuestionOption> options, int correctAnswerIndex, bool isRandomOptionArrangement)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            IsRandomOptionArrangement = isRandomOptionArrangement;
        }

        public string QuestionText { get; set; }
        public List<QuestionOption> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public bool IsRandomOptionArrangement { get; set; }
    }

}
