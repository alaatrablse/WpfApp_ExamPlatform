namespace WebApiServer.Models
{
    public class Question
    {
        public Question()
        {
            Options = new List<Option>();
        }

        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public bool RandomOrder { get; set; }
        public List<Option> Options { get; set; }
    }

    public class Option
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }


}



