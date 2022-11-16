using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directum
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TimerCallback tm = new TimerCallback(Reminder);
            Timer timerReminder = new Timer (tm,null,0,10000);

            Menu menu = new Menu();
        }

        /// <summary>
        /// Напоминание о встрече
        /// </summary>
        /// <param name="obj">Объект класса object</param>
        static void Reminder(object obj)
        {
            MeetingList MeetingList = MeetingList.getInstance();

            for (int i = 0; i < MeetingList.meetingList.Count; i++)
            {
                if (MeetingList.meetingList[i].NotificationTime < DateTime.Now &&
                    MeetingList.meetingList[i].NotificationFlag == 0)
                {
                    string meet = MeetingList.ListToString(MeetingList.meetingList[i].Id);
                    MeetingList.meetingList[i].NotificationFlag = 1;
                    Console.WriteLine("!-----------------------------------------------!");
                    Console.WriteLine($"У вас запланирована встреча: {meet}");
                    Console.WriteLine("!-----------------------------------------------!");
                }
            }
        }
    }
}