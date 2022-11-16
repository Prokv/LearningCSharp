using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Directum
{
    internal class Menu
    {
        MeetingList MeetingList = MeetingList.getInstance();
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Menu()
        {
            int numberOfMenu;
            do
            {
                numberOfMenu = UserMenu();
                if (numberOfMenu != 0)
                {
                    switch (numberOfMenu)
                    {
                        case 1://Добавить новую встречу
                            MeetingList.AddNewMeet();
                            Console.WriteLine("Нажмите любую клавишу для продолжения.");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 2://Удалить встречу
                            Console.Clear();
                            MeetingList.PrintAll();
                            if (!MeetingList.IsEmpty())
                            {
                                Console.WriteLine("Введите номер встречи которую собираетесь удалить:");
                                string choisOfUser = Console.ReadLine();
                                bool IsCorrect = int.TryParse(choisOfUser, out int id);
                                if (MeetingList.ContainsId(id))
                                {
                                    MeetingList.Delete(id);
                                }
                                else
                                {
                                    Console.WriteLine("Нет встреч с таким номером.Попробуйте ещё раз.");
                                }
                            }
                            Console.WriteLine("Нажмите любую клавишу для продолжения.");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 3://Изменить данные о встрече
                            Console.Clear();
                            MeetingList.PrintAll();
                            if (! MeetingList.IsEmpty())
                            {
                                Console.WriteLine("Введите номер встречи которую собираетесь изменить:");
                                string choisOfUser = Console.ReadLine();
                                bool IsCorrect = int.TryParse(choisOfUser, out int id);
                                if (MeetingList.ContainsId(id))
                                {
                                    Console.Clear();
                                    RewriteMeet(id);
                                }
                                else 
                                { 
                                    Console.WriteLine("Нет встреч с таким номером.Попробуйте ещё раз.");
                                }
                            }
                            Console.WriteLine("Нажмите любую клавишу для продолжения.");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 4://Показать список встреч
                            MeetingList.PrintAll();
                            Console.WriteLine("Нажмите любую клавишу для продолжения.");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 5://Сохранить список встреч в файл
                            Console.Clear();
                            MeetingList.DataInFile();
                            Console.WriteLine($"Все внесенные изменения сохранены в файл: {MeetingList.getInstance().path_txt}");
                            Console.WriteLine("Нажмите любую клавишу для продолжения.");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 9://Выход из меню
                            MeetingList.SerializeJSON(MeetingList.meetingList ,MeetingList.path_json);
                            break;
                        default:
                            Console.WriteLine("Неверный номер");
                            break;
                    }
                }
                else Console.WriteLine("Некорректный ввод, введите номер повторно.");
            }
            while (numberOfMenu != 9);
        }
        /// <summary>
        /// Основное меню
        /// </summary>
        /// <returns></returns>
        public int UserMenu()
        {
            int choiceOfUser;
            Console.WriteLine($"Выберите действия введя номер пункта меню: " +
       $"{Environment.NewLine} 1. Добавить новую встречу." +
       $"{Environment.NewLine} 2. Удалить встречу." +
       $"{Environment.NewLine} 3. Изменить данные о встрече." +
       $"{Environment.NewLine} 4. Показать список встреч." +
       $"{Environment.NewLine} 5. Сохранить список встреч в файл." +
       $"{Environment.NewLine} 9. Выход из меню");
            bool isNumber = int.TryParse(Console.ReadLine(), out choiceOfUser);
            if (isNumber)
            {
                Console.Clear();
                return choiceOfUser;
            }
            else return 0;
        }
        /// <summary>
        /// Меню внесения изменений в созданную встречу
        /// </summary>
        /// <param name="Id">ИД встречи</param>
        public void RewriteMeet(int Id)
        {
            int choiceOfUser;
            while (true)
            {
                Console.WriteLine($"Выберите действия введя номер пункта меню: " +
                        $"{Environment.NewLine} 1. Изменить название встречи " +
                        $"{Environment.NewLine} 2. Изменить описание встречи." +
                        $"{Environment.NewLine} 3. Изменить дату начала встречи." +
                        $"{Environment.NewLine} 4. Изменить дату окончания встречи." +
                        $"{Environment.NewLine} 5. Изменить время напоминания о встрече." +
                        $"{Environment.NewLine} 9. Выход из меню");
                bool isNumber = int.TryParse(Console.ReadLine(), out choiceOfUser);
                if (isNumber)
                {
                    switch (choiceOfUser)
                    {
                        case 1:
                            MeetingList.RewriteName(Id);
                            break;
                        case 2:
                            MeetingList.RewriteDescription(Id);
                            break;
                        case 3:
                            MeetingList.RewriteStartDate(Id);
                            break;
                        case 4:
                            MeetingList.RewriteEndDate(Id);
                            break;
                        case 5:
                            MeetingList.RewriteNotificationTime(Id);
                            break;
                        case 9:
                            return;
                    }
                }
                else {
                    Console.WriteLine("Введены некорректные символы, попробуйте ещё раз.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения.");
                    Console.ReadLine();
                }
            }
        }
    }
}
