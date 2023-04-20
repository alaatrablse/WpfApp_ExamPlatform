using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Models;
using Path = System.IO.Path;
using System.Windows.Threading;
using WpfApp1.Api;
using System.Xml.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ExamPlatform.xaml
    /// </summary>
    public partial class ExamPlatform : Window
    {
        private ExamApiClient examApiClient = new ExamApiClient();

        private Exam ExameElement = new Exam();
        private ExamResult Results = new ExamResult();

        private List<int> Answer;

        private int index = 0;

        private DispatcherTimer timer;
        private TimeSpan timeRemaining;

        public ExamPlatform(Exam exam, ExamResult result)
        {
            InitializeComponent();
            Results = result;
            ExameElement = exam;

            Answer = Enumerable.Repeat(-1, exam.Questions.Count).ToList();

            random();
            getQ(index);


            //setup the Timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            DateTime time1 = DateTime.Today + exam.StartTime.TimeOfDay;
            TimeSpan timeElapsed = DateTime.Now - time1;
            double minutesElapsed = timeElapsed.TotalMinutes;
            timeRemaining = TimeSpan.FromMinutes(exam.TotalTime - minutesElapsed);
            timeText.Text = timeRemaining.ToString(@"hh\:mm\:ss");
            timer.Start();
        }

        private bool IsImageUrl(string url)
        {
            bool validUri = Uri.TryCreate(url, UriKind.Absolute, out Uri uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                && !string.IsNullOrEmpty(uri.Host);

            if (!validUri)
            {
                return false;
            }

            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string extension = Path.GetExtension(uri.LocalPath);

            return validExtensions.Contains(extension);
        }


        private void getQ(int indexQ)
        {
            int length = ExameElement.Questions.Count;
            if (indexQ >= length)
            {
                return;
            }


            Question question = ExameElement.Questions[indexQ];
            textNumQ.Text = $"Question {indexQ + 1}";

            lbAnswer.Items.Clear();



            foreach (Option p in question.Options)
            {
                lbAnswer.Items.Add(p.Value);
            }

            lbAnswer.SelectedIndex = Answer[indexQ];

            if (IsImageUrl(question.QuestionText))
            {
                TextQ.Visibility = Visibility.Collapsed;
                imgQ.Visibility = Visibility.Visible;
                imgQ.Source = new BitmapImage(new Uri(question.QuestionText));
            }
            else
            {
                TextQ.Visibility = Visibility.Visible;
                imgQ.Visibility = Visibility.Collapsed;
                TextQ.Text = question.QuestionText;
            }

            if (length == indexQ + 1)
            {
                if (length > 1)
                {
                    butPrevious.Visibility = Visibility.Visible;
                }
                butFinesh.Visibility = Visibility.Visible;
                butNext.Visibility = Visibility.Collapsed;
            }
            if (indexQ == 0)
            {
                if (length > 1)
                {
                    butNext.Visibility = Visibility.Visible;
                    butFinesh.Visibility = Visibility.Collapsed;
                }
                butPrevious.Visibility = Visibility.Collapsed;
            }
        }

        private void getAnswer()
        {
            int selctedindex = lbAnswer.SelectedIndex;
            Answer[index] = selctedindex;
        }



        private void butNext_Click(object sender, RoutedEventArgs e)
        {
            getAnswer();
            getQ(++index);
        }

        private void butPrevious_Click(object sender, RoutedEventArgs e)
        {
            getAnswer();
            getQ(--index);
        }

        private void butFinesh_Click(object sender, RoutedEventArgs e)
        {
            getAnswer();
            List<int> temp = new List<int>();
            for (int i = 0; i < Answer.Count; i++)
            {
                if (Answer[i] == -1)
                {
                    temp.Add(i);
                }
            }
            if (temp.Count > 0)
            {

                MessageBoxResult result = MessageBox.Show("You did not answer all the questions, are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Save();
            }
        }


        private async void Save()
        {
            int score = 0;
            int numQ = ExameElement.Questions.Count;

            for (int i = 0; i < numQ; i++)
            {
                if (ExameElement.Questions[i].CorrectAnswerIndex == Answer[i])
                {
                    score++;
                }
                else
                {
                    Error error = new Error();
                    error.Question = ExameElement.Questions[i].QuestionText;
                    error.SelectedAnswer = ExameElement.Questions[i].Options[Answer[i]].Value;
                    error.CorrectAnswer = ExameElement.Questions[i].Options[ExameElement.Questions[i].CorrectAnswerIndex].Value;
                    Results.Errors.Add(error);
                }
            }

            Results.Score = score;

            try
            {
                await examApiClient.CreateExamResultAsync(Results);

                MessageBox.Show($"Your score is {score}");
                MainWindow open = new MainWindow();
                open.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            timeRemaining -= TimeSpan.FromSeconds(1);
            timeText.Text = timeRemaining.ToString(@"hh\:mm\:ss");

            if (timeRemaining <= TimeSpan.Zero)
            {
                timer.Stop();
                MessageBox.Show("Time's up!");
                Save();
            }
        }


        private void random()
        {
            List<Question> questions = ExameElement.Questions;
            List<int> numbers = new List<int>();

            for (int i = 0; i < questions.Count; i++)
            {
                numbers = Enumerable.Range(0, questions[i].Options.Count).ToList();

                if (questions[i].RandomOrder)
                {
                    numbers = numbers.OrderBy(a => Guid.NewGuid()).ToList();
                    List<Option> options = ExameElement.Questions[i].Options.ToList();

                    for (int j = 0; j < options.Count; j++)
                    {
                        ExameElement.Questions[i].Options[j] = options[numbers[j]];
                    }
                    int temp = ExameElement.Questions[i].CorrectAnswerIndex;
                    ExameElement.Questions[i].CorrectAnswerIndex = numbers.IndexOf(temp);
                }
                numbers = Enumerable.Range(0, questions[i].Options.Count).ToList();
            }
        }
    }
}
