using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson5
{
    internal class GetUserMenu
    {
        SubscribersList SubscribersList = new SubscribersList();


        public GetUserMenu()
        {           
            SubscribersList.Clear();
            Phonebook.getInstance().ReadData(SubscribersList.Subscribers);

            int numberOfMenu;
            do
            {
                numberOfMenu = UserMenu();
                if (numberOfMenu != 0)
                {
                    switch (numberOfMenu)
                    {
                        case 1://Добавить нового пользователя
                            SubscribersList.AddSubscriber();
                            break;
                        case 2://Поиск по номеру телефона
                            Console.WriteLine("Введите номер телефона для поиска");
                            string? phonenumber = Console.ReadLine();
                            SubscribersList.SearchPhonenumber(phonenumber);
                            break;
                        case 3://Поиск по Имени пользователя
                            Console.WriteLine("Введите имя для поиска");
                            string? name = Console.ReadLine();
                            SubscribersList.SearchName(name);
                            break;
                        case 4://Очистка справочника
                            SubscribersList.Clear();
                            int count = SubscribersList.Subscribers.Count;
                            Console.WriteLine($"Список очищен. Количество текущих записей: {count}");
                            break;
                        case 5://Показать весь справочник
                            SubscribersList.PrintAll(SubscribersList.Subscribers);
                            break;
                        case 9://Выход из меню
                            Console.WriteLine($"Все внесенные изменения сохранены в файл: {Phonebook.getInstance().path}");
                            break;
                        default:
                            Console.WriteLine("Неверный номер");
                            break;
                    }
                }
                else Console.WriteLine("Некорректный ввод, введите номер повторно.");
            }
            while (numberOfMenu != 9);

            Phonebook.getInstance().ClearData();
            Phonebook.getInstance().AddData(SubscribersList.Subscribers);
        }
        public int UserMenu()
        {
            int choiceOfUser;
            Console.WriteLine($"Выберите действия со справочником введя номер пункта меню: " +
       $"{Environment.NewLine} 1. Добавить нового пользователя." +
       $"{Environment.NewLine} 2. Поиск по номеру телефона." +
       $"{Environment.NewLine} 3. Поиск по Имени пользователя." +
       $"{Environment.NewLine} 4. Очистка справочника." +
       $"{Environment.NewLine} 5. Показать весь справочник." +
       $"{Environment.NewLine} 9. Выход из меню");
            bool isNumber = int.TryParse(Console.ReadLine(), out choiceOfUser);
            if (isNumber)
            {
                Console.Clear();
                return choiceOfUser;
            }
            else return 0;
        }
    }
}
