using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using WpfApp1.Api;
using WpfApp1.Models;
using Formatting = Newtonsoft.Json.Formatting;
using System.Windows.Controls;
using System.Linq;
using System.Globalization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for addExam.xaml
    /// </summary>
    public partial class addExam : Window
    {
        private List<Question> _questions;
        private ExamApiClient examApiClient = new ExamApiClient();
        
        private Exam examLocal = new Exam();


        public addExam(Exam exam)
        {
            InitializeComponent();
            _questions= new List<Question>();

            if(exam != null)
            {
                examLocal = exam;
                this.Title = "Update Exam";
                btnSaveExam.Content = "Save Update Exam";
                btnSaveOnComputer.Visibility = Visibility.Collapsed;
                txtExamName.Text =exam.Name;
                txtTeacherName.Text = exam.TeacherName;
                txtExamTime.Text = exam.StartTime.ToString("HH:mm");
                DatePickerExam.Text = exam.Date.ToString("dd/MM/yyyy");
                txtExamDuration.Text = exam.TotalTime.ToString();
                foreach(Question q in exam.Questions)
                {
                    lbQuestions.Items.Add(q.QuestionText);
                    _questions.Add(q);
                }
            }
            else
            {
                examLocal = null;
            }

        }

        private Exam getExam()
        {
            if (string.IsNullOrEmpty(txtExamName.Text) || string.IsNullOrEmpty(txtTeacherName.Text) || !DatePickerExam.SelectedDate.HasValue || string.IsNullOrEmpty(txtExamTime.Text) || string.IsNullOrEmpty(txtExamDuration.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return null;
            }
            if (_questions.Count < 1)
            {
                MessageBox.Show("Please Add question.");
                return null;
            }
            string pattern = "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
            Regex regex = new Regex(pattern);
            string input = txtExamTime.Text;

            if (!regex.IsMatch(input))
            {
                MessageBox.Show("Invalid input. Please enter a time in the format of HH:MM.");
                return null;
            }

            Exam exam = new Exam();
            exam.Name = txtExamName.Text;
            exam.TeacherName = txtTeacherName.Text;
            exam.Date = DatePickerExam.SelectedDate.Value;
            TimeSpan time = TimeSpan.Parse(txtExamTime.Text);
            exam.StartTime = exam.Date.Date + time;
            exam.TotalTime = int.Parse(txtExamDuration.Text);
            exam.Questions = _questions;

            if(examLocal != null)
            {
                exam.Id = examLocal.Id;
            }

            return exam;
        }


        private async void btnSaveExam_Click(object sender, RoutedEventArgs e)
        {            
            Exam exam = getExam();
            if (exam == null) { return; }
            try
            {
                var exams = await examApiClient.CreateExamAsync(exam);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
           

            Teacher teacherPage = new Teacher();
            teacherPage.Show();
            this.Close();
        }

        private void btnAddQuestions_Click(object sender, RoutedEventArgs e)
        {
            AddQuestion questionpage = new AddQuestion(null);
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
                var selectedQuestion = lbQuestions.SelectedItem as string;
                lbQuestions.Items.RemoveAt(lbQuestions.SelectedIndex);
                _questions.RemoveAll(q => q.QuestionText == selectedQuestion);

            }
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            if (lbQuestions.SelectedIndex != -1)
            {
                string selectedQuestionText = lbQuestions.SelectedValue.ToString();
                Question selectedQuestion = _questions.SingleOrDefault(q => q.QuestionText == selectedQuestionText);
                if (selectedQuestion != null)
                {
                    AddQuestion addExamWindow = new AddQuestion(selectedQuestion);
                    addExamWindow.OnClick += UpdateQ;
                    addExamWindow.ShowDialog();
                }
            }
        }

        private void UpdateQ(Question rules)
        {
            // find the question to update
            var questionToUpdate = _questions.Find(q => q.Id == rules.Id);

            // update the question if found
            if (questionToUpdate != null)
            {
                // apply the rules to the question
                questionToUpdate.QuestionText = rules.QuestionText;
                questionToUpdate.RandomOrder = rules.RandomOrder;
                questionToUpdate.Options = rules.Options;
                questionToUpdate.CorrectAnswerIndex = rules.CorrectAnswerIndex;

                questionToUpdate = _questions.Find(q => q.Id == rules.Id);
                lbQuestions.Items[lbQuestions.SelectedIndex] = rules.QuestionText;
            }
            

        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void btnSaveOnComputer_Click(object sender, RoutedEventArgs e)
        {
            Exam exam = getExam();
            if (exam == null) { return; }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string examFilePath = saveFileDialog.FileName;

                // Serialize the exam to JSON and write it to the file
                string examJson = JsonConvert.SerializeObject(exam, Formatting.Indented);
                File.WriteAllText(examFilePath, examJson);
                MessageBox.Show("Exam saved successfully.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Teacher teacherPage = new Teacher();
            teacherPage.Show();
            this.Close();
        }
    }
}
