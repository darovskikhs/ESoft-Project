using EsoftDSV.Class;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EsoftDSV.View
{
    /// <summary>
    /// Логика взаимодействия для employeesPage.xaml
    /// </summary>
    public partial class employeesPage : Page
    {
        private static bool IfUser()
        {
            var currentUser = GetCurrent.CurrentUser;
            var isManager = user10Entities.GetContext().Manager.Any(manager => manager.ID == currentUser.ID);
            if (isManager) return true;
            else return false;
        }
        public employeesPage()
        {
            InitializeComponent();
            if (IfUser() == false)
            {
                statusBorder.Visibility = Visibility.Hidden;
                tbSearch.Visibility = Visibility.Hidden;
                btnClear.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }
            dgEmployees.ItemsSource = user10Entities.GetContext().User.ToList();
        }

        private void LoadGrid()
        {
            dgEmployees.ItemsSource = user10Entities.GetContext().Task.ToList();
        }

        private void UpdateUsers()
        {
            var users = App._context.User.ToList();
            users = users.Where(p => p.FirstName.ToLower().Contains(tbSearch.Text.ToLower()) || p.MiddleName.ToLower().Contains(tbSearch.Text.ToLower()) || p.LastName.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            dgEmployees.ItemsSource = users;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = dgEmployees.SelectedItem as User;
            if (MessageBox.Show($"Вы уверены, что хотите удалить из списка следующего сотрудника - " + $"{currentUser.MiddleName} " + $"{currentUser.FirstName} " + $"{currentUser.LastName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                user10Entities.GetContext().User.Remove(currentUser);
                user10Entities.GetContext().SaveChanges();
                UpdateUsers();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = dgEmployees.SelectedItem as User;
            if (currentUser != null)
            {
                addEditUserWindow addEdit = new addEditUserWindow(currentUser);
                addEdit.Show();
                LoadGrid();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            addEditUserWindow addEdit = new addEditUserWindow();
            addEdit.Show();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Clear();
            UpdateUsers();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }
    }
}
