using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using WpfApp1.Api;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for addExam.xaml
    /// </summary>
    public partial class addExam : Window
    {
        private List<Question> _questions;
        private ExamApiClient examApiClient = new ExamApiClient();

        public addExam()
        {
            InitializeComponent();
            _questions= new List<Question>();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            Teacher teacherWindow = new Teacher();
            teacherWindow.Visibility = Visibility.Visible;
        }

        private async void btnSaveExam_Click(object sender, RoutedEventArgs e)
        {            
            if (string.IsNullOrEmpty(txtExamName.Text) || string.IsNullOrEmpty(txtTeacherName.Text) || !DatePickerExam.SelectedDate.HasValue || string.IsNullOrEmpty(txtExamTime.Text) || string.IsNullOrEmpty(txtExamDuration.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            if(_questions.Count <1)
            {
                MessageBox.Show("Please Add question.");
                return;
            }

            Exam exam = new Exam();
            exam.Name = txtExamName.Text;
            exam.TeacherName = txtTeacherName.Text;
            exam.Date = DatePickerExam.SelectedDate.Value;
            exam.StartTime = DateTime.Parse(txtExamTime.Text);
            exam.TotalTime = int.Parse(txtExamDuration.Text);
            exam.Questions = _questions;

            var exams = await examApiClient.CreateExamAsync(exam);

            this.Close();
        }

        private void btnAddQuestions_Click(object sender, RoutedEventArgs e)
        {
            AddQuestion questionpage = new AddQuestion();
            questionpage.OnClick += AddQ;
            questionpage.ShowDialog();
        }


        private void AddQ(Question rulses)
        {
            _questions.Add(rulses);
            lbQuestions.Items.Add(rulses.QuestionText.ToString());
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (lbQuestions.SelectedIndex != -1)
            {
                lbQuestions.Items.RemoveAt(lbQuestions.SelectedIndex);
            }
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
            }
        }



    }
}
