using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Api;
using System.Net.Http;
using System.Text;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Teacher(object sender, RoutedEventArgs e)
        {
            Teacher newpage = new Teacher();
            this.Visibility = Visibility.Hidden;
            newpage.Show();
        }

        private async void Button_Click_Student(object sender, RoutedEventArgs e)
        {
            var examApiClient = new ExamApiClient();

            // GET all exams
            var exams = await examApiClient.GetAllExamsAsync();

            // display the list of exams
            StringBuilder sb = new StringBuilder();
            foreach (var exam in exams)
            {
                sb.AppendLine($" Name: {exam.Name}, Date: {exam.Date}");
                foreach (var q in exam.Questions) 
                {
                    sb.AppendLine($" Question: {q.QuestionText}, Date: {q.CorrectAnswerIndex}");
                    foreach(var o in q.Options)
                    {
                        sb.AppendLine($" Options: {o.Value}");
                    }
                    sb.AppendLine("*****************************");
                }
            }
            aa.Content = sb.ToString();
        }

    }
}
