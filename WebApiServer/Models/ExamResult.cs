namespace WebApiServer.Models
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
        public int Score { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class Error
    {
        public string Question { get; set; }
        public string SelectedAnswer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
