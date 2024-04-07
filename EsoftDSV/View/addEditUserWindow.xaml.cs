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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EsoftDSV.View
{
    /// <summary>
    /// Логика взаимодействия для addEditUserWindow.xaml
    /// </summary>
    public partial class addEditUserWindow : Window
    {
        private user10Entities _context = new user10Entities();
        private User _currentUser = null;
        public addEditUserWindow()
        {
            InitializeComponent();
        }

        public addEditUserWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            tboxFirstName.Text = user.FirstName;
            tboxLastName.Text = user.LastName;
            tboxMiddleName.Text = user.MiddleName;
            tboxLogin.Text = user.Login;
            tboxPassword.Text = user.Password;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var context = user10Entities.GetContext();
            if (_currentUser == null)
            {
                var user = new User()
                {
                    FirstName = tboxFirstName.Text,
                    MiddleName = tboxMiddleName.Text,
                    LastName = tboxLastName.Text,
                    Login = tboxLogin.Text,
                    Password = tboxPassword.Text,
                    IsDeleted = false

                };

                user10Entities.GetContext().User.Add(user);
                user10Entities.GetContext().SaveChanges();
                this.Close();
            }
            else
            {
                var existingUser = context.User.FirstOrDefault(t => t.ID == _currentUser.ID);
                if (existingUser != null)
                {
                    existingUser.FirstName = tboxFirstName.Text;
                    existingUser.MiddleName = tboxMiddleName.Text;
                    existingUser.LastName = tboxLastName.Text;
                    existingUser.Login = tboxLogin.Text;
                    existingUser.Password = tboxPassword.Text;
                    existingUser.IsDeleted = false;

                }
                user10Entities.GetContext().SaveChanges();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
