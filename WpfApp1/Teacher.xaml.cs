﻿using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Teacher.xaml
    /// </summary>
    public partial class Teacher : Window
    {
        public Teacher()
        {
            InitializeComponent();
        }


        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            addExam open=new addExam(null);
            open.Show();
            this.Close();
        }

        private void Button_Click_search(object sender, RoutedEventArgs e)
        {
            SearchFromTeacher open = new SearchFromTeacher();
            open.Show();
            this.Close();
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            UpdateExam open = new UpdateExam();
            open.Show();
            this.Close();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            MainWindow open = new MainWindow();
            open.Show();
            this.Close();
        }
    }
}
