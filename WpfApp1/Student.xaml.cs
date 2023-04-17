using System;
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
using System.Windows.Threading;
using WpfApp1.Api;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        private ExamApiClient examApiClient = new ExamApiClient();
        Exam examGloabl;

        public Student(Exam exam)
        {
            InitializeComponent();

            if (exam.StartTime > DateTime.Now)
            {

                // Set button visibility to Collapsed initially
                butStart.Visibility = Visibility.Collapsed;
                textNote.Visibility= Visibility.Visible;
                // Create a timer to check the time every minute
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            if (exam == null)
            {
                examGloabl = new Exam();
            }
            else
            {
                examGloabl = exam;
            }

        }

        private async void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(TextName.Text) || string.IsNullOrEmpty(TextId.Text))
            {
                MessageBox.Show("Please fill in both name and Id.");
                return;
            }


            string StudentName = TextName.Text;
            int StudentId = int.Parse(TextId.Text);

            ExamResult element = new ExamResult();
            element.StudentId = StudentId;
            element.StudentName = StudentName;
            element.ExamId = examGloabl.Id;



            try
            {
                await examApiClient.GetExamResultAsync(StudentId,examGloabl.Id);
                
                MessageBox.Show("You've been tested before");
                MainWindow open = new MainWindow();
                open.Show();
                this.Close();

            }
            catch (Exception)
            {

                ExamPlatform open = new ExamPlatform(examGloabl, element) ;
                open.Show();
                this.Close();
                return;
            }

        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Check if the current time is after 12 o'clock
            if (DateTime.Now >= examGloabl.StartTime)
            {
                // Set button visibility to Visible
                butStart.Visibility = Visibility.Visible;

                textNote.Visibility = Visibility.Collapsed;
            }
        }

    }
}
