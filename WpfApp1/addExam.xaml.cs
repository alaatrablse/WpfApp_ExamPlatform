using System;
using System.Collections.Generic;
using System.Data;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for addExam.xaml
    /// </summary>
    public partial class addExam : Window
    {
        private List<Question> _questions;
        public addExam()
        {
            InitializeComponent();
            _questions= new List<Question>();
        }

        private void CreateExamButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveExam_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
