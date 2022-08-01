using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5
{
    internal class Phonebook
    {
        /// <summary>
        /// Путь к файлу в котором хранятся данные с абонентами
        /// </summary>
        public string path = Path.Combine(Directory.GetCurrentDirectory(), "phonebook.txt");

        private static Phonebook? instance;

        public static Phonebook getInstance()
        {
            if (instance == null)
                instance = new Phonebook();
            return instance;
        }

        /// <summary>
        /// Конструктор для работы с текстовым файлом в котором хранятся записи абонентов.
        /// Проверяет наличие файла, при отсутствии создаёт файл в указанной директории.
        /// </summary>
        private Phonebook()
        {
            if (!System.IO.File.Exists(path))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(path)) { }
                Console.WriteLine("Файл \"{0}\" создан.", path);
            }
            else
            {
                Console.WriteLine("Файл \"{0}\" найден.", path);
                return;
            }
        }
       /// <summary>
       /// Запись данных в файл
       /// </summary>
       /// <param name="Subscribers">Список абонентов</param>
        public void AddData(List<Subscriber> Subscribers)
        {
            List<string> AllLines = new List<string>();
            for (int i = 0; i < Subscribers.Count; i++)
            {
                
                string str = $"{Subscribers[i].Name},{Subscribers[i].PhoneNumber}";
                AllLines.Add(str);
            }
            File.WriteAllLines(path, AllLines);
        }

        /// <summary>
        /// Чтение данных из файла и запись в список Subscribers
        /// </summary>
        public void ReadData(List<Subscriber> Subscribers)
        {
            List<string> AllLines = new List<string>();
            AllLines = File.ReadAllLines(path).ToList();
            for (int i = 0; i < AllLines.Count; i++)
            {
                string[] subs = AllLines[i].Split(',');
                Subscriber Subscriber = new Subscriber(subs[0], subs[1]);
                Subscribers.Add(Subscriber);
            }
        }
        /// <summary>
        /// Очистка файла от данных
        /// </summary>
        public void ClearData()
        {
            File.WriteAllText(path, string.Empty);
        }

    }
}

