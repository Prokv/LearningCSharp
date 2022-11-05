using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;

namespace Directum
{
    internal class MeetingList
    {
        public string path_txt = Path.Combine(Directory.GetCurrentDirectory(), "Список встреч.txt");

        public string path_json = Path.Combine(Directory.GetCurrentDirectory(), "DataBase_Meets.json");

        public List<Meeting> meetingList = new List<Meeting>();

        private static MeetingList instance;


        private MeetingList ()
        {
            this.meetingList = new List<Meeting>();
            if (!System.IO.File.Exists(path_txt))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(path_txt)) { }
                Console.WriteLine("Файл \"{0}\" создан.", path_txt);
            }

            if (!System.IO.File.Exists(path_json))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(path_json)) { }
                Console.WriteLine("Файл \"{0}\" создан.", path_json);
            }

        }     

        public async Task SerializeJSON(IEnumerable<Meeting> meet, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                await JsonSerializer.SerializeAsync<List<Meeting>>(fs, meet.ToList());
            }
        }

        public static MeetingList getInstance()
        {
            if (instance == null)
                instance = new MeetingList();
            return instance;
        }

        public void DataInFile()
        {
            File.WriteAllText(path_txt, string.Empty);

            List<string> AllLines = new List<string>();
            for (int i = 0; i < meetingList.Count; i++)
            {

                string str = ListToString(meetingList[i].Id);
                AllLines.Add(str);
            }            
            File.WriteAllLines(path_txt, AllLines);
        }
        public void AddNewMeet ()
        {
            Console.Clear();
            int id=1;
            int[] idArray=new int[meetingList.Count];
            for (int i = 0; i < meetingList.Count; i++)
            {
                idArray[i] = meetingList[i].Id;
            }
            if  (idArray.Length > 0) { id = idArray.Max() + 1; }
            string name = SetName();
            string description = SetDescription();

            Nullable<DateTime> setStartDate = SetStartDate();
            DateTime startTime;
            if (setStartDate.HasValue)
            {
                startTime=setStartDate.Value;
            }
            else { return; }

            Nullable<DateTime> setEndDate = SetEndDate(startTime);
            DateTime endTime;
            if (setEndDate.HasValue)
            {
                endTime = setEndDate.Value;
            }
            else { return; }

            if (Intersect(startTime, endTime))
            {
                return;
            }

            DateTime notificationTime = SetNotificationTime(startTime);

            int notificationFlag;
            if (notificationTime==startTime)
            {
                notificationFlag = 1;
            }
            else { notificationFlag = 0; }

            Meeting newMeeting = new Meeting(id, name, description, startTime, endTime, notificationTime, notificationFlag);
            meetingList.Add(newMeeting);
            Console.WriteLine("Добавлена информация о встрече:");
            PrintLine(id);
            return;
        }


        public string SetName()
        {
            Console.WriteLine("Введите название встречи");
            string? name = Console.ReadLine();
            return name;
        }

        public void RewriteName(int Id)
        {
            string name = SetName();
            meetingList[GetIndex(Id)].Name = name;
            Console.WriteLine("Название встречи изменено:");
            PrintLine(Id);
            return;
        }
        public string SetDescription()
        {
            Console.WriteLine("Введите описание встречи");
            string? descriptrion = Console.ReadLine();
            return descriptrion;
        }

        public void RewriteDescription(int Id)
        {
            string description = SetDescription();
            meetingList[GetIndex(Id)].Description = description;
            Console.WriteLine("Описание встречи изменено:");
            PrintLine(Id);
            return;
        }
        public Nullable<DateTime> SetStartDate()
        {
            Nullable<DateTime> dtDate;
            DateTime startTime;
            string strDate;
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            DateTimeStyles styles = DateTimeStyles.None;

            while (true)
            {
                Console.WriteLine($"Введите дату начала встречи в формате dd.mm.yyyy h:mm:ss (Пример: 01.01.2022 8:05:00)" +
                                  $"{Environment.NewLine}Для выхода введите exit");
                strDate = Console.ReadLine();
                if (strDate == "exit")
                {
                    dtDate = null;
                    break;
                }
                bool IsCorrect = DateTime.TryParse(strDate, provider, styles, out startTime);
                if (IsCorrect)
                {
                    if (startTime <= DateTime.Now)
                    {
                        Console.WriteLine("Начало встречи не может быть раньше текущего времени. Попробуйте ещё раз.");
                    }
                    else
                    {
                        Console.WriteLine($"Выбрана дата и время начала встречи:{startTime}");
                        dtDate=startTime;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Введен не верный формат даты и времени. Попробуйте ещё раз.");
                }
            }
            return dtDate;
        }

        public void RewriteStartDate(int Id)
        {
            int index = GetIndex(Id);
            int id = meetingList[index].Id;
            string name = meetingList[index].Name;
            string description = meetingList[index].Description;
            DateTime old_startTime = meetingList[index].StartTime;
            DateTime new_startTime = meetingList[index].StartTime;
            DateTime old_endTime = meetingList[index].EndTime;
            DateTime new_endTime = meetingList[index].EndTime;
            DateTime old_notificationTime = meetingList[index].NotificationTime;
            DateTime new_notificationTime;
            int old_notificationFlag = meetingList[index].NotificationFlag;
            int new_notificationFlag = 0;

            if (old_startTime<=DateTime.Now)
            {
                Console.WriteLine("Нельзя изменять время начала встречи, которая уже прошла. Попробуйте удалить встречу и создать заново.");
                return;
            }

            Nullable<DateTime> setStartDate = SetStartDate();

            if (setStartDate.HasValue)
            {
                new_startTime = setStartDate.Value;
            }
            else { return; }

            if (new_startTime> old_endTime)
            {
                new_endTime = new_startTime;
                Console.WriteLine($"Дата окончания встречи не может быть раньше даты начала, поэтому дата окончания изменена на {new_endTime}");
            }

            meetingList.RemoveAt(index);

            if (Intersect(new_startTime, new_endTime))
            {
                Meeting old_Meeting = new Meeting(id, name, description, old_startTime, old_endTime, old_notificationTime, old_notificationFlag);
                meetingList.Add(old_Meeting);
                return;
            }

            new_notificationTime = new_startTime - (old_startTime - old_notificationTime);
            Meeting new_Meeting = new Meeting(id, name, description, new_startTime, new_endTime, new_notificationTime, new_notificationFlag);
            meetingList.Add(new_Meeting);

            Console.WriteLine($"Время начала встречи изменено на: {new_startTime}");
            Console.WriteLine($"Дата начала встречи изменена, поэтому изменено время напоминания о встрече {new_notificationTime}");
            return;
        }

        public int GetIndex(int Id)
        {
            for (int i = 0; i < meetingList.Count; i++)
            {
                if (meetingList[i].Id == Id)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Intersect (DateTime startTime, DateTime endTime)
        {
            for (int i = 0; i < meetingList.Count; i++)
            {
                var date_1_start = meetingList[i].StartTime;
                var date_1_end = meetingList[i].EndTime;
                var date_2_start = startTime;
                var date_2_end = endTime;

                bool intersect = date_2_end >= date_1_start && date_2_start <= date_1_end;
                if (intersect)
                {
                    Console.WriteLine($"Невозможно запланировать встречу, её время пересекается с другой встречей:");
                    PrintLine(meetingList[i].Id);
                    return true;
                }
            }
            return false;
        }

        public Nullable<DateTime> SetEndDate (DateTime startTime)
        {
            Nullable<DateTime> dtDate;
            DateTime endTime;
            string strDate;
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            DateTimeStyles styles = DateTimeStyles.None;

            while (true)
            {
                Console.WriteLine($"Введите дату окончания встречи в формате dd.mm.yyyy h:mm:ss (Пример: 01.01.2022 8:05:00)" +
                                  $"{Environment.NewLine}Для выхода введите exit");
                strDate = Console.ReadLine();
                if (strDate == "exit")
                {
                    dtDate = null;
                    break;
                }
                bool IsCorrect = DateTime.TryParse(strDate, provider, styles, out endTime);
                if (IsCorrect)
                {
                    if (endTime <= startTime)
                    {
                        Console.WriteLine("Окончание встречи не может быть раньше её начала. Попробуйте ещё раз.");
                    }
                    else
                    {
                        Console.WriteLine($"Выбрана дата и время окончания встречи:{endTime}");
                        dtDate = endTime;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Введен не верный формат даты и времени. Попробуйте ещё раз.");
                }
            }
            return dtDate;
        }

        public void RewriteEndDate(int Id)
        {
            int index = GetIndex(Id);
            int id = meetingList[index].Id;
            string name = meetingList[index].Name;
            string description = meetingList[index].Description;
            DateTime startTime = meetingList[index].StartTime;
            DateTime old_endTime = meetingList[index].EndTime;
            DateTime new_endTime = meetingList[index].EndTime;
            DateTime notificationTime = meetingList[index].NotificationTime;
            int notificationFlag= meetingList[index].NotificationFlag;

            if (old_endTime <= DateTime.Now)
            {
                Console.WriteLine("Нельзя изменять время начала встречи, которая уже прошла. Попробуйте удалить встречу и создать заново.");
                return;
            }

            Nullable<DateTime> setEndDate = SetEndDate(startTime);

            if (setEndDate.HasValue)
            {
                new_endTime = setEndDate.Value;
            }
            else { return; }

            meetingList.RemoveAt(index);

            if (Intersect(startTime, new_endTime))
            {
                Meeting old_Meeting = new Meeting(id, name, description, startTime, old_endTime, notificationTime, notificationFlag);
                meetingList.Add(old_Meeting);
                return;
            }

            Meeting new_Meeting = new Meeting(id, name, description, startTime, new_endTime, notificationTime, notificationFlag);
            meetingList.Add(new_Meeting);

            Console.WriteLine($"Время окончания встречи изменено на: {new_endTime}");
            return;
        }

        public DateTime SetNotificationTime (DateTime startTime)
        {
            DateTime dtDate=startTime;
            TimeSpan interval;
            string strTS;

            while (true)
            {
                Console.WriteLine($"Задайте временной интервал для напоминания о начале встречи в формате h:mm:ss (Пример: 8:05:00)" +
                                  $"{Environment.NewLine}Для выхода введите exit");
                strTS = Console.ReadLine();
                if (strTS == "exit")
                {
                    break;
                }
                bool IsCorrect = TimeSpan.TryParse(strTS, out interval);
                if (IsCorrect)
                {
                    if (startTime-interval <= DateTime.Now)
                    {
                        Console.WriteLine("Напоминание о встрече не может быть раньше текущего времени.");
                    }
                    else
                    {
                        dtDate -= interval;
                        Console.WriteLine($"Установлено время напоминания о встрече:{dtDate}");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Введен не верный формат временного интервала. Попробуйте ещё раз.");
                }
            }
            return dtDate;
        }

        public void RewriteNotificationTime(int Id)
        {
            int index = GetIndex(Id);
            int id = meetingList[index].Id;
            DateTime startTime = meetingList[index].StartTime;
            DateTime notificationTime = SetNotificationTime(startTime);
            int notificationFlag = meetingList[index].NotificationFlag;

            if (notificationTime == startTime)
            {
                notificationFlag = 1;
            }
            else { notificationFlag = 0; }

            meetingList[index].NotificationTime = notificationTime;
            meetingList[index].NotificationFlag = notificationFlag;

            Console.WriteLine($"Установлено время напоминания о встрече: {notificationTime}");
            return;
        }

        public void PrintAll()
        {
            if (!IsEmpty())
            {
                for (int i = 0; i < meetingList.Count; i++)
                {
                    PrintLine(meetingList[i].Id);
                }
            }
            else { Console.WriteLine("Встречи не запланированы."); }

        }

        public bool IsEmpty ()
        {
            bool b = true;
            if (meetingList.Count > 0) { b = false; }
            return b;
        }

        public void PrintLine ( int id)
        {
            if (! IsEmpty())
            {
                string line = ListToString(id);
                if (line != null)
                {
                    Console.WriteLine(line);
                }
                else { Console.WriteLine("Задача с таким номером не найдена в списке."); }
            }
            else { Console.WriteLine("Встречи не запланированы."); }

        }

        public string ListToString (int id)
        {
            string outputInfo=null;
            int index=GetIndex(id);
            if (index != -1)
            {
                string NotificationTime = $"{meetingList[index].NotificationTime}";
                if (meetingList[index].NotificationTime == meetingList[index].StartTime)
                {
                    NotificationTime = "Не установлено.";
                }
                outputInfo = $"Номер: {meetingList[index].Id}, " +
                $"Название: {meetingList[index].Name}," +
                $"Описание: {meetingList[index].Description}, " +
                $"Старт: {meetingList[index].StartTime}, " +
                $"Окончание: {meetingList[index].EndTime}, " +
                $"Напоминание: {NotificationTime}";
            }
            return outputInfo;
        }

        public void Delete(int id)
        {
            string meet = null;
            for (int i = 0; i < meetingList.Count; i++)
            {
                if ( id == meetingList[i].Id)
                {
                    meet = ListToString(id);
                    meetingList.RemoveAt(i);
                    Console.WriteLine($"Удалена встреча {meet}");
                    return;
                }
            }
            if (meet == null)
            {
                Console.WriteLine("Встреч с таким номером не найдено. Попробуйте ещё раз.");
            }
            return;
        }

        public bool ContainsId(int Id)
        {
            int[] idArray = new int[meetingList.Count];
            for (int i = 0; i < meetingList.Count; i++)
            {
                idArray[i] = meetingList[i].Id;
            }
            foreach (int id in idArray)
            {
                if (id == Id) { return true;}
            }
            return false;
        }
    }
}
