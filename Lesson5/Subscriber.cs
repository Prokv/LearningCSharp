using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lesson5
{
    internal class Subscriber
    {
        private string name;
        private string phoneNumber;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PhoneNumber 
        { 
            get { return phoneNumber; } 
            set { phoneNumber = value; }
        }


        /// <summary>
        /// Конструктор для создания абонента.
        /// </summary>
        public Subscriber()
        {
            Name = "";
            PhoneNumber = "";
        }
        public void Add()
        {
            Console.WriteLine($"Введите имя пользователя:");
            Name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"Введите номер телефона:");
            PhoneNumber = Console.ReadLine();
            Console.Clear();
        }
        public void Print() => Console.WriteLine($"{Name} {PhoneNumber}");
    }
}
