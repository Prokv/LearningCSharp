using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directum
{
    internal class Meeting
    {
        /// <summary>
        /// Идентификтор встречи
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название встречи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание встречи
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Дата и время начала встречи
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Дата и время окончания встречи
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Дата и время напоминания о встрече
        /// </summary>
        public DateTime NotificationTime { get; set; }
        /// <summary>
        /// Флаг о необходимости напоминания о встрече
        /// </summary>
        public int NotificationFlag { get; set; }
        /// <summary>
        /// Конструкто встречи
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="notificationTime"></param>
        /// <param name="notificationFlag"></param>
        public Meeting (int id, string name, string description, DateTime startTime, DateTime endTime, DateTime notificationTime, int notificationFlag) 
        {
            Id = id;
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            NotificationTime = notificationTime;
            NotificationFlag = notificationFlag;
        }
    }
}
