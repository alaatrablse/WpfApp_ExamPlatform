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
using WpfApp1.Api;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class StartExam : Window
    {

        private ExamApiClient examApiClient = new ExamApiClient();

        public StartExam()
        {
            InitializeComponent();
        }

        private async void Button_Click_Go(object sender, RoutedEventArgs e)
        {
            if(textName.Text == "" || textName.Text == null)
            {
                MessageBox.Show("The input is empty!!");
                return;
            }
            try
            {
                var exam = await examApiClient.GetExamAsyncByName(textName.Text);

                if (exam.Date.Date > DateTime.Today)
                {
                    MessageBox.Show("The exam is not for today");
                    return;
                }
                else if (exam.Date.Date < DateTime.Today)
                {
                    MessageBox.Show("The exam is over");
                    return;
                }
                else if (exam.StartTime.AddMinutes(exam.TotalTime) <= DateTime.Now)
                {
                    MessageBox.Show("The exam is over");
                    return;
                }
               

                if (exam != null)
                {
                    Student Window = new Student(exam);
                    Window.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error Server");
                }
            }
            catch (Exception )
            {
                MessageBox.Show("Not Found Exam, Try again !!");
                return;
            }

           
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            MainWindow newpage = new MainWindow();
            newpage.Show();
            this.Close();
        }
    }
}
