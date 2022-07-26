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
        public string path= Path.Combine(Directory.GetCurrentDirectory(), "phonebook.txt");
        /// <summary>
        /// Конструктор для работы с текстовым файлом в котором хранятся записи абонентов.
        /// </summary>
        public Phonebook() { }

        public void CreateFile(string phonebook)
        {

            if (!System.IO.File.Exists(phonebook))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(phonebook)) { }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", phonebook);
                return;
            }
        }
        public void AddData(List <string> Subscribers) => File.WriteAllLines(path, Subscribers);

        public void ReadData(ref List<string> Subscribers) => Subscribers = File.ReadAllLines(path).ToList();

        public void ClearData() => File.WriteAllText(path, string.Empty);

    }
}
