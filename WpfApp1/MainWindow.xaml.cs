using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Teacher(object sender, RoutedEventArgs e)
        {
            Teacher newpage = new Teacher();
            newpage.Show();
            this.Close();
        }

        private void Button_Click_Student(object sender, RoutedEventArgs e)
        {

        }

    }
}
