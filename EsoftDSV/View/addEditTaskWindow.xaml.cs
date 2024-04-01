using System;
using System.CodeDom.Compiler;
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

namespace EsoftDSV.View
{
    /// <summary>
    /// Логика взаимодействия для addEditTaskWindow.xaml
    /// </summary>
    public partial class addEditTaskWindow : Window
    {
        private user10Entities _context = new user10Entities();
        private List<string> listUsers = new List<string>();
        public addEditTaskWindow()
        {
            InitializeComponent();
            foreach (var user in _context.User.ToList())
            {
                listUsers.Add(user.GetFullName());
            }
            cboxExecutor.ItemsSource = listUsers;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
