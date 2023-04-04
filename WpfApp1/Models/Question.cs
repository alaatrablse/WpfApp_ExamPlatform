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
            Options = new List<string>();
        }
       
        public Question(string questionText, List<string> options, int correctAnswerIndex, bool randomOrder)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            RandomOrder = randomOrder;
        }

        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public bool RandomOrder { get; set; }
    }

}
