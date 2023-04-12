using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1
{
    public delegate void INPUT_WERTE(Question rulses);

    /// <summary>
    /// Interaction logic for AddQuestion.xaml
    /// </summary>
    public partial class AddQuestion : Window
    {
        public event INPUT_WERTE OnClick;
        private Question rulses;
        public AddQuestion(Question question)
        {
            InitializeComponent();
            rulses = new Question();

            if(question != null)
            {

                this.Title = "Update Question";
                labelTitle.Content = "Update Question";
                butSave.Content = "Update";

                questiontext.Text = question.QuestionText;

                rulses.Id= question.Id;
                rulses.QuestionText = question.QuestionText;
                foreach(Option option in question.Options)
                {
                    listboxoptions.Items.Add(option.Value);
                    rulses.Options.Add(option);
                    ComboBoxAnswer.Items.Add($"Option {listboxoptions.Items.Count}");
                }
                ComboBoxAnswer.SelectedIndex = question.CorrectAnswerIndex;
                rulses.CorrectAnswerIndex = question.CorrectAnswerIndex;
                CheckBoxRandomize.IsChecked = question.RandomOrder;
                rulses.RandomOrder = question.RandomOrder;


            }

        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addoption.Text))
            {
                listboxoptions.Items.Add(addoption.Text);
                ComboBoxAnswer.Items.Add($"Option {listboxoptions.Items.Count}");

                rulses.QuestionText = questiontext.Text;
                Option newOption = new Option();
                newOption.Value = addoption.Text;
                rulses.Options.Add(newOption);
                addoption.Text = "";
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(questiontext.Text) || listboxoptions.Items.Count < 2)
            {
                MessageBox.Show("Not all fields are completed correctly.\n Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                rulses.QuestionText = questiontext.Text;
                rulses.RandomOrder = (bool)CheckBoxRandomize.IsChecked;
                rulses.CorrectAnswerIndex = ComboBoxAnswer.SelectedIndex;
                OnClick(rulses);
                this.Close();
            }

        }


        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (listboxoptions.SelectedIndex != -1)
            {
                rulses.Options.RemoveAt(listboxoptions.SelectedIndex);
                listboxoptions.Items.RemoveAt(listboxoptions.SelectedIndex);
                int lastIndex = ComboBoxAnswer.Items.Count - 1;
                if (lastIndex >= 0)
                {
                    ComboBoxAnswer.Items.RemoveAt(lastIndex);
                }
                
            }
        }
    }
}