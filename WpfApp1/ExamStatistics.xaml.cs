using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using WpfApp1.Api;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ExamStatistics.xaml
    /// </summary>
    public partial class ExamStatistics : Window
    {
        private ExamApiClient examApiClient = new ExamApiClient();

        private List<Exam> Exams = new List<Exam>();
        private List<ExamResult> results = new List<ExamResult>();
        private ExamResult result = new ExamResult();
        private int totalscor = 0;
        public ExamStatistics()
        {
            InitializeComponent();
            ChooseExam();

        }


        private async void ChooseExam()
        {
            try
            {
                var exams = await examApiClient.GetAllExamsAsync();
                Exams = exams;
                foreach (var exam in exams)
                {
                    CBName.Items.Add(exam.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private async void CBName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textNum.Text = "0";
            textAverage.Text = "0";
            CBStudentName.Items.Clear();
            CBStudentName.Items.Add("Choose ...");
            if (CBName.SelectedIndex > 0)
            {
                // Get the selected item
                string selectedItem = CBName.SelectedItem.ToString();
                Exam exam = Exams.FirstOrDefault(ex => ex.Name.Equals(selectedItem));


                try
                {
                    results = await examApiClient.GetExamStaticAsync(exam.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if(results == null || results.Count == 0)
                {
                    return;
                }

                double average = 0;
                int sum = 0;
                totalscor = results[0].Score + results[0].Errors.Count;

                foreach (ExamResult item in results)
                {
                    sum += item.Score;
                    CBStudentName.Items.Add(item.StudentName);
                }
                average = sum / results.Count;

                textNum.Text = results.Count.ToString();
                textAverage.Text = average.ToString() + " from " + totalscor.ToString();
            }
        }

        private void CBStudentName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textSocre.Text = "0/0";
            CBError.Items.Clear();
            CBError.Items.Add("Choose ...");
            if (CBName.SelectedIndex > 0)
            {
                // Get the selected item
                string selectedItem = CBStudentName.SelectedItem.ToString();
                result = results.FirstOrDefault(r => r.StudentName == selectedItem);
                if (result != null)
                {
                    textSocre.Text = result.Score.ToString() + "/" + totalscor.ToString();

                    for (int i = 0; i < result.Errors.Count; i++)
                    {
                        CBError.Items.Add($"Error {i + 1}");
                    }
                }
            }
        }


        private void CBError_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textData.Text = "";

            if (CBName.SelectedIndex > 0)
            {
                string s = CBError.SelectedItem.ToString();
                int number;
                if (int.TryParse(s.Substring(6), out number))
                {
                    Error error = result.Errors[number - 1];
                    string str = $"Question: {error.Question}";
                    str += $"\nSelected Answer: {error.SelectedAnswer}";
                    str += $"\nCorrect Answer: {error.CorrectAnswer}";
                    textData.Text = str;
                }
            }
        }


        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Teacher teacherPage = new Teacher();
            teacherPage.Show();
            this.Close();
        }
    }
}
