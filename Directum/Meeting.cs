using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directum
{
    internal class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime NotificationTime { get; set; }

        public int NotificationFlag { get; set; }

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
