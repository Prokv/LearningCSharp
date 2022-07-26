using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5
{
    internal class SubscribersList
    {
        public List<string> Subscribers = new List<string>();
        /// <summary>
        /// Конструктор для работы со списком записей справочника
        /// </summary>
        public SubscribersList() { }

        public void Add(string name, string phoneNumber)
        {
            Subscribers.Add(name);
            Subscribers.Add(phoneNumber);
        }
        public void Remove(string name) => Subscribers.Remove(name);
        public void Clear() => Subscribers.Clear();

        public void SearchName(string name)
        {
            List<int> IndexName = new List<int>();

            for (int i = 0; i < Subscribers.Count; i++)
            {
                if (Subscribers[i] == name) IndexName.Add(i);

            }
            if (IndexName.Count > 0)
            {
                for (int i = 0; i < IndexName.Count; i++)
                {
                    Console.WriteLine($"{Subscribers[IndexName[i]]}: {Subscribers[IndexName[i] + 1]}");
                }
            }
            else Console.WriteLine("Абонента с таким именем нет в справочнике.");
        }

        public void SearchPhonenumber(string phonenumber)
        {
            List<int> IndexPhonenumber = new List<int>();

            for (int i = 0; i < Subscribers.Count; i++)
            {
                if (Subscribers[i] == phonenumber) IndexPhonenumber.Add(i);
            }
            for (int i = 0; i < IndexPhonenumber.Count; i++)
            {
                Console.WriteLine($"{Subscribers[IndexPhonenumber[i-1]]}: {Subscribers[IndexPhonenumber[i]]}");
            }
        }
        public void PrintAll()
        {
            for (int i = 0; i < Subscribers.Count; i += 2)
            { 
                Console.WriteLine($"{Subscribers[i]}: {Subscribers[i + 1]}"); 
            } 
        }
        public bool SearchNumberEquals(string phonenumber)
        {
            return Subscribers.Contains(phonenumber);
        }
    }
}
