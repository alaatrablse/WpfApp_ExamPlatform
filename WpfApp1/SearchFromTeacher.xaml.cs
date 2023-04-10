using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Api;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SearchFromTeacher.xaml
    /// </summary>
    public partial class SearchFromTeacher : Window
    {
        private ExamApiClient examApiClient = new ExamApiClient();

        public SearchFromTeacher()
        {
            InitializeComponent();
        }

        private async void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textName.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }


            try
            {
                var exam = await examApiClient.GetExamAsyncByName(textName.Text);
                string formattedDate = exam.Date.ToString("dd:MM:yy");
                string timeString = exam.StartTime.ToString("HH:mm");

                var examDetails = $"Name: {exam.Name},\nTeacher Name: {exam.TeacherName}\n" +
                    $"Date of exam: {formattedDate}\nStart Time: {timeString}\n" +
                    $"Exam Duration (minutes): {exam.TotalTime}";

                LableExam.Content = examDetails;
                
            }
            catch (Exception)
            {
                LableExam.Content= "No existe";
            }
            textName.Text = "";
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Teacher open = new Teacher();
            open.Show();
            this.Close();
        }
    }
}
