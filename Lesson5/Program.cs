namespace Lesson5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Subscriber Subscriber=new Subscriber();
            SubscribersList SubscribersList = new SubscribersList();
            Phonebook Phonebook = new Phonebook();

            Phonebook.CreateFile(Phonebook.path);
            SubscribersList.Clear();
            Phonebook.ReadData(ref SubscribersList.Subscribers);

            for (int j=1; j<2;)
            {
                Console.WriteLine("Открыть меню для работы со справочником? [y/n]");
                ConsoleKeyInfo input = Console.ReadKey();
                Console.Clear();
                if (input.Key == ConsoleKey.Y)
                {
                    int i = UserMenu();

                    switch (i)
                    {
                        case 1:
                            Subscriber.Add();
                            if (SubscribersList.SearchNumberEquals(Subscriber.PhoneNumber))
                            {
                                Console.WriteLine("Такой номер уже существует в записной книжке.");
                                System.Threading.Thread.Sleep(1000);
                                Console.Clear();
                            }
                            else
                            {
                                SubscribersList.Add(Subscriber.Name, Subscriber.PhoneNumber);
                            }
                            break;
                        case 2:
                            Console.WriteLine("Введите номер телефона для поиска");
                            string phonenumber = Console.ReadLine();
                            SubscribersList.SearchPhonenumber(phonenumber);
                            break;
                        case 3:
                            Console.WriteLine("Введите имя для поиска");
                            string name = Console.ReadLine();
                            SubscribersList.SearchName(name);
                            break;
                        case 4:
                            SubscribersList.Clear();
                            break;
                        case 5:
                            SubscribersList.PrintAll();
                            break;
                    }

                }
                else
                {
                    j = 2;
                }
            }
            
           Phonebook.ClearData();
           Phonebook.AddData(SubscribersList.Subscribers);
        }

        public static int UserMenu ()
        {
            //Console.Clear();
            Console.WriteLine($"Выберите действия со справочником введя номер пункта меню: " +
       $"{Environment.NewLine} 1. Добавить нового пользователя." +
       $"{Environment.NewLine} 2. Поиск по номеру телефона." +
       $"{Environment.NewLine} 3. Поиск по Имени пользователя." +
       $"{Environment.NewLine} 4. Очистка справочника." +
       $"{Environment.NewLine} 5. Показать весь справочник.");
            int i;
            Int32.TryParse(Console.ReadLine(), out i);
            Console.Clear();
            return i;
        }
    }
}