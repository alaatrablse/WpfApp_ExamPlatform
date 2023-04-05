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
            Options = new List<Option>();
        }

        public string QuestionText { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public bool RandomOrder { get; set; }
        public List<Option> Options { get; set; }
    }

    public class Option
    {
        public string Value { get; set; }
    }

}
