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
using System.ComponentModel;

namespace Deadliner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SaveLoad _saveLoad;
        private BindingList<Task> _tasks;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _saveLoad = new SaveLoad();

            try
            {
                _tasks = _saveLoad.Load();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this.Close();
            }

            UpdateTimeLeft();
            NewTaskDate_Calendar.SelectedDate = DateTime.Now.AddDays(1);

            dgTasks.ItemsSource = _tasks;
            _tasks.ListChanged += DataChanged;
        }

        /// <summary>
        /// Добавлен/удален столбец или изменено значение
        /// </summary>
        private void DataChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                UpdateTimeLeft();
                TrySave();
            }
        }

        /// <summary>
        /// Добавить задачу
        /// </summary>
        private void AddTask_Button_Click(object sender, RoutedEventArgs e)
        {
            _tasks.Add(new Task()
            {
                Created = DateTime.Now,
                Deadline = ConstructDateTime(NewTaskDate_Calendar.SelectedDate.Value, int.Parse(NewTaskHour_TextBox.Text), int.Parse(NewTaskMinute_TextBox.Text)),
                TaskDescription = NewTaskName_TextBox.Text
        });
        }

        private void TrySave()
        {
            try
            {
                _saveLoad.Save(_tasks);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                this.Close();
            }
        }

        /// <summary>
        /// Изменена выбранная дата на календаре
        /// </summary>
        private void NewTaskDate_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            NewTaskDateTimeChanged();
        }

        /// <summary>
        /// Убран фокус с текстбокса ввода часа
        /// </summary>
        private void NewTaskHour_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int hours;
            try
            {
                hours = int.Parse((sender as TextBox).Text);
            }
            catch (Exception)
            {
                (sender as TextBox).Text = "00";
                return;
            }

            if (hours >= 24 || hours < 0)
            {
                NewTaskHour_TextBox.Text = "00";
            }

            NewTaskDateTimeChanged();
        }

        /// <summary>
        /// Убран фокус с текстбокса ввода минуты
        /// </summary>
        private void NewTaskMinute_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int minutes;
            try
            {
                minutes = int.Parse((sender as TextBox).Text);
            }
            catch (Exception)
            {
                (sender as TextBox).Text = "00";
                return;
            }

            if (minutes >= 60 || minutes < 0)
            {
                NewTaskMinute_TextBox.Text = "00";
            }

            NewTaskDateTimeChanged();
        }

        /// <summary>
        /// Изменена дата/час/минута новой задачи
        /// </summary>
        private void NewTaskDateTimeChanged() {

            DateTime dateTime = ConstructDateTime(NewTaskDate_Calendar.SelectedDate.Value, int.Parse(NewTaskHour_TextBox.Text), int.Parse(NewTaskMinute_TextBox.Text));
            DeadlineDateTime_Label.Content = $"Deadline: {dateTime.Day}.{dateTime.Month}.{dateTime.Year} {dateTime.Hour}:{dateTime.Minute}";
            SetAddButton(dateTime);
        }

        /// <summary>
        /// нужно ли активировать/деактивировать кнопку добавления
        /// </summary>
        private void SetAddButton(DateTime dateTime)
        {
            if (dateTime <= DateTime.Now)
            {
                if (AddTask_Button.IsEnabled != false)
                {
                    AddTask_Button.IsEnabled = false;
                }
            }
            else
            {
                if (AddTask_Button.IsEnabled != true)
                {
                    AddTask_Button.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Собирает DateTime из введенных данных
        /// </summary>
        /// <param name="choosenDay">Дата с календаря</param>
        /// <param name="hour">Час</param>
        /// <param name="minute">Минута</param>
        /// <returns>Новый DateTime</returns>
        private DateTime ConstructDateTime(DateTime choosenDay, int hour, int minute)
        {
            return new DateTime(choosenDay.Year, choosenDay.Month, choosenDay.Day, hour, minute, 0);
        }

        /// <summary>
        /// Обнуляет текстбокс часов, когда пользователь нажимает на него
        /// </summary>
        private void NewTaskHour_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        /// <summary>
        /// Обнуляет текстбокс минут, когда пользователь нажимает на него
        /// </summary>
        private void NewTaskMinute_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
        }

        /// <summary>
        /// Проверяет описание задачи, когда теряет фокус
        /// </summary>
        private void NewTaskName_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text)) {
                (sender as TextBox).Text = "New task";
            }
        }

        private void UpdateTimeLeft() {
            foreach (Task task in _tasks) {
                task.TimeLeft = task.CountTimeLeft();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            TrySave();
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            _tasks.ListChanged -= DataChanged;
            UpdateTimeLeft();
            dgTasks.Items.Refresh();
            _tasks.ListChanged += DataChanged;
        }
    }
}
