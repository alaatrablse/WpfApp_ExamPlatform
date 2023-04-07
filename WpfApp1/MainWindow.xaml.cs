using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Api;
using System.Net.Http;
using System.Text;

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

        private async void Button_Click_Student(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
