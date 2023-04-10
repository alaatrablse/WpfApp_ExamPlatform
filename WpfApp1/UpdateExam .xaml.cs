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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Api;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for UpdateExam.xaml
    /// </summary>
    public partial class UpdateExam : Window
    {

        private ExamApiClient examApiClient = new ExamApiClient();

        public UpdateExam()
        {
            InitializeComponent();
        }

        private async void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                Exam exam = await examApiClient.GetExamAsyncByName(textName.Text);
                addExam addExamWindow = new addExam(exam);
                addExamWindow.Show();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            
        }
    }
}
