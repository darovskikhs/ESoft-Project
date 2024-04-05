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
        private Task _currentTask = null;
        private bool isEditing;

        public addEditTaskWindow()
        {
            InitializeComponent();
            foreach (var user in _context.User.ToList())
            {
                listUsers.Add(user.GetFullName());
            }
            cboxExecutor.ItemsSource = listUsers;
        }
        public addEditTaskWindow(Task task)
        {
            InitializeComponent();
            _currentTask = task;
            foreach (var user in _context.User.ToList())
            {
                listUsers.Add(user.GetFullName());
            }
            isEditing = true;
            tboxTitle.Text = task.Title;
            tboxDesc.Text = task.Description;
            cboxStatus.Text = task.Status;
            dpCreateDateTime.Text = task.CreateDateTime.ToString();
            dpDeadline.Text = task.Deadline.ToString();
            tboxDifficulty.Text = task.Difficulty.ToString();
            tboxTime.Text = task.Time.ToString();
            cboxNatureOfTheTask.Text = task.WorkType;
            foreach (var exUser in _context.Executor.ToList())
            {
                string G = FMLName.GetFullName(_context.User.FirstOrDefault(p => p.ID == exUser.ID));
                listUsers.Add(G);
            }
            var _exUser = _context.User.FirstOrDefault(p => p.ID == task.Executor.ID);
            cboxExecutor.ItemsSource = listUsers;
            cboxExecutor.Text = FMLName.GetFullName(_exUser).ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private int userID;

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var context = user10Entities.GetContext();
                if (_currentTask == null)
                {
                    List<User> usersBD = _context.User.ToList();
                    if (cboxExecutor.SelectedItem.ToString() is string user)
                    {
                        User userG = usersBD.First(p => p.GetFullName() == user);
                        userID = userG.ID;
                    }
                    var task = new Task()      
                    {
                        Title = tboxTitle.Text,
                        Description = tboxDesc.Text,
                        Deadline = DateTime.Parse(dpDeadline.Text),
                        CreateDateTime = DateTime.Parse(dpCreateDateTime.Text),
                        Difficulty = float.Parse(tboxDifficulty.Text),
                        Time = int.Parse(tboxTime.Text),
                        ExecutorID = userID,
                        Status = cboxStatus.Text,
                        WorkType = cboxNatureOfTheTask.Text,
                        IsDeleted = false

                    };

                    user10Entities.GetContext().Task.Add(task);
                    user10Entities.GetContext().SaveChanges();
                    this.Close();
                }
                else
                {
                    var existingTask = context.Task.FirstOrDefault(t => t.ID == _currentTask.ID);
                    List<User> usersBD = _context.User.ToList();
                    if (cboxExecutor.SelectedItem.ToString() is string user)
                    {
                        User userG = usersBD.First(p => p.GetFullName() == user);
                        userID = userG.ID;
                    }
                    if(existingTask != null)
                    {
                        existingTask.Title = tboxTitle.Text;
                        existingTask.Description = tboxDesc.Text;
                        existingTask.Deadline = DateTime.Parse(dpDeadline.Text);
                        existingTask.CreateDateTime = DateTime.Parse(dpCreateDateTime.Text);
                        existingTask.Difficulty = float.Parse(tboxDifficulty.Text);
                        existingTask.Time = int.Parse(tboxTime.Text);
                        existingTask.ExecutorID = userID;
                        existingTask.Status = cboxStatus.Text;
                        existingTask.WorkType = cboxNatureOfTheTask.Text;
                        existingTask.IsDeleted = false;

                    }
                    user10Entities.GetContext().SaveChanges();
                    this.Close();
                }
            }
        }

        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tboxTitle.Text))
                errorBuilder.AppendLine("Название задачи обязательно для заполнения;");

            float difficultyNumber = 1;
            if (float.TryParse(tboxDifficulty.Text, out difficultyNumber) == false || difficultyNumber > 50 || difficultyNumber < 1)
                errorBuilder.AppendLine("Сложность задачи превышает значение 50;");

            int durationMinutes = 0;
            if (int.TryParse(tboxTime.Text, out durationMinutes) == false || durationMinutes <= 0)
                errorBuilder.AppendLine("Время выполнения задачи должно быть положительным числом;");

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");
            return errorBuilder.ToString();
        }
    }
}
