using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Deadliner
{
    class Task : INotifyPropertyChanged
    {
        /// <summary>
        /// Дата и время создания задачи
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата и время дедлайна задачи
        /// </summary>
        private DateTime _deadline;
        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Deadline"));
            }
        }

        /// <summary>
        /// Оставшееся время
        /// </summary>
        public string TimeLeft { get; set; }

        /// <summary>
        /// Текст задачи
        /// </summary>
        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set
            {
                _taskDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaskDescription"));
            }
        }

        public bool IsTermless { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Task()
        {
            Created = DateTime.Now;
            _deadline = Created.AddDays(1);//По умолчанию конец ровно через сутки. Может быть, если добалять столбцы прямо через датагрид
            _taskDescription = "New task";
        }

        public string CountTimeLeft()
        {
            if (IsTermless == true)//Если время вышло, возвращает Time is up!
            {
                return "Termless";
            }

            DateTime now = DateTime.Now;

            if (now >= Deadline)//Если время вышло, возвращает Time is up!
            {
                return "Time is up!";
            }

            if (Deadline.Year - now.Year >= 3)//Если осталось 3 или больше лет, возвращает кол-во лет
            {
                return $"{(Deadline.Year - now.Year).ToString()} years";
            }

            int yearsDif = Deadline.Year - now.Year;
            int monthsDif = yearsDif * 12 + (Deadline.Month - now.Month);
            if (monthsDif >= 3)//Если осталось 3 или больше месяцев, возвращает кол-во месяцев
            {
                return $"{monthsDif.ToString()} months";
            }

            TimeSpan diff = Deadline - now;
            if (diff.TotalDays >= 2)//Если осталось 2 или больше дней, возвращает кол-во дней
            {
                return $"{Math.Round(diff.TotalDays).ToString()} days";
            }

            if (diff.TotalHours >= 3)//Если осталось 3 или больше часов, возвращает кол-во часов
            {
                return $"{Math.Round(diff.TotalHours).ToString()} hours";
            }

            if (diff.TotalMinutes >= 1)//Если осталось 1 или больше минут, возвращает кол-во минут
            {
                return $"{Math.Round(diff.TotalMinutes).ToString()} minutes";
            }

            return $"{Math.Round(diff.TotalSeconds).ToString()} seconds";
        }
    }
}
