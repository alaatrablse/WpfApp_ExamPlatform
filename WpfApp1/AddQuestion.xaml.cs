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
        public AddQuestion()
        {
            InitializeComponent();
            rulses = new Question();

        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addoption.Text))
            {
                listboxoptions.Items.Add(addoption.Text);
                ComboBoxAnswer.Items.Add($"Option {listboxoptions.Items.Count}");

                rulses.QuestionText = questiontext.Text;
                QuestionOption option = new QuestionOption();
                option.OptionText = addoption.Text;
                rulses.Options.Add(option);
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
                rulses.IsRandomOptionArrangement = (bool)CheckBoxRandomize.IsChecked;
                OnClick(rulses);
                this.Close();
            }

        }


        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (listboxoptions.SelectedIndex != -1)
            {
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