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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EsoftDSV.View
{
    /// <summary>
    /// Логика взаимодействия для taskPage.xaml
    /// </summary>
    public partial class taskPage : Page
    {
        public taskPage()
        {
            InitializeComponent();
            dgTasks.ItemsSource = user10Entities.GetContext().Task.ToList();
        }

        private void LoadGrid()
        {
            dgTasks.ItemsSource = user10Entities.GetContext().Task.ToList();
        }

        private void UpdateTasks()
        {
            var tasks = App._context.Task.ToList();

            if (cbStatus.SelectedItem == cbiPlanned)
                tasks = tasks.Where(p => p.Status == cbiPlanned.Content.ToString()).ToList();

            if (cbStatus.SelectedItem == cbiExecute)
                tasks = tasks.Where(p => p.Status == cbiExecute.Content.ToString()).ToList();

            if (cbStatus.SelectedItem == cbiExecuted)
                tasks = tasks.Where(p => p.Status == cbiExecuted.Content.ToString()).ToList();

            if (cbStatus.SelectedItem == cbiCanceled)
                tasks = tasks.Where(p => p.Status == cbiCanceled.Content.ToString()).ToList();

            tasks = tasks.Where(p => p.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();

            dgTasks.ItemsSource = tasks;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            addEditTaskWindow addEdit = new addEditTaskWindow();
            addEdit.Show();
            
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentTask = dgTasks.SelectedItem as Task;
            if (currentTask != null)
            {
                addEditTaskWindow addEdit = new addEditTaskWindow(currentTask);
                addEdit.Show();
                LoadGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentTask = dgTasks.SelectedItem as Task;
            if (MessageBox.Show($"Вы уверены, что хотите удалить следующую задачу - " + $"{currentTask.Title}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                user10Entities.GetContext().Task.Remove(currentTask);
                user10Entities.GetContext().SaveChanges();
                UpdateTasks();
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTasks();
        }

        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTasks();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Clear();
            cbStatus.SelectedIndex = -1;
            UpdateTasks();
        }
    }
}
