using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lesson5
{
    internal class SubscribersList
    {
        /// <summary>
        /// Пополняемый список абонентов
        /// </summary>
        public List<Subscriber> Subscribers = new List<Subscriber>();
        /// <summary>
        /// Конструктор для работы со списком записей справочника
        /// </summary>
        public SubscribersList() { }

        /// <summary>
        /// Добавляет нового абонента в списоок.
        /// </summary>
        public void AddSubscriber()
        {
            Console.WriteLine("Введите имя:");
            string? newName = Console.ReadLine();
            Console.WriteLine("Введите номер:");
            string? newPhoneNumber = Console.ReadLine();

            if (ISNewSubscriberCorrect(newName, newPhoneNumber))
            {
                Subscriber newSubscriber = new Subscriber (newName, newPhoneNumber);
                Subscribers.Add(newSubscriber);
                Console.WriteLine("Пользователь добавлен");
            }

        }

        /// <summary>
        /// Проверяет данные абонента
        /// </summary>
        /// <param name="name">имя абонента</param>
        /// <param name="phoneNumber">номер</param>
        /// <returns>корректно ли введены данные</returns>
        private bool ISNewSubscriberCorrect(string name, string phoneNumber)
        {
            string strPattern = @"(\+7|8|\b)[\(\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[)\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)";
            if (name == null || phoneNumber == null)
            {
                Console.WriteLine("Отсутствуют данные");
                return false;
            }

            if (Regex.IsMatch(phoneNumber, strPattern))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Введенное значение не является номером");
                return false;
            }

            for (int i = 0; i < Subscribers.Count; i++)
            {
                if (Subscribers[i].PhoneNumber == phoneNumber)
                {
                    Console.WriteLine("Такой номер уже существует");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Поиск абонентов по имени
        /// </summary>
        /// <param name="name">Имя абонента</param>
        public void SearchName(string name)
        {
            List<Subscriber> IndexName = new List<Subscriber>();
            IndexName.Clear();

            for (int i = 0; i < Subscribers.Count; i++)
            {
                if (Subscribers[i].Name == name)
                {
                    IndexName.Add(Subscribers[i]);
                }
            }
            if (IndexName.Count > 0)
            {
                PrintAll(IndexName);
            }
            else Console.WriteLine("Абонента с таким именем нет в справочнике.");
        }
        /// <summary>
        /// Поиск абонентов по номеру телефона
        /// </summary>
        /// <param name="phonenumber">Номер телефона для поиска</param>
        public void SearchPhonenumber(string phonenumber)
        {
            List<Subscriber> IndexPhonenumber = new List<Subscriber>();
            IndexPhonenumber.Clear();

            for (int i = 0; i < Subscribers.Count; i++)
            {
                if (Subscribers[i].PhoneNumber == phonenumber)
                {
                 IndexPhonenumber.Add(Subscribers[i]);
                }
            }
            if (IndexPhonenumber.Count > 0)
            {
                PrintAll(IndexPhonenumber);
            }
            else Console.WriteLine("Абонента с таким номером телефона нет в справочнике.");
        }
        /// <summary>
        /// Выведение на консоль полного списка абонентов
        /// </summary>
        /// <param name="Subscribers">Список абонентов</param>
        public void PrintAll(List<Subscriber> Subscribers)
        {
            if (Subscribers.Count!=0)
            {
                for (int i = 0; i < Subscribers.Count; i++)
                {
                    Console.WriteLine($"{Subscribers[i].Name} , {Subscribers[i].PhoneNumber}");
                }
            }
            else Console.WriteLine("Список абонентов пуст.");
        }
        /// <summary>
        /// Очистка списка абонентов
        /// </summary>
        public void Clear()
        {
            Subscribers.Clear();
        }
    }
}
