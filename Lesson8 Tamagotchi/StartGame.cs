using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8_Tamagotchi
{
    internal class StartGame
    {
        public StartGame()
        {
            int numberOfMenu;
            do
            {
                numberOfMenu = StartMenu();

                    if (numberOfMenu != 9)
                    {
                        Console.WriteLine("Дай имя своему питомцу");
                        string? animalName = Console.ReadLine();
                        Animal animal;

                        switch (numberOfMenu)
                        {
                            case 1://Кошка
                                animal = new Cat(animalName); 
                                GameMenu(animal);
                                break;
                            case 2://Собака
                                animal = new Dog(animalName);
                                GameMenu(animal);
                                break;
                    }
                    }
                    else Console.WriteLine("Игра окончена.");
            }
            while (numberOfMenu != 9);
        }
        public int StartMenu()
        {
            int choiceOfUser;
            int i=0;
            Console.WriteLine($"Выберите питомца: " +
       $"{Environment.NewLine} {++i}. Котенок." +
       $"{Environment.NewLine} {++i}. Щенок." +
       $"{Environment.NewLine} 9. Выход из игры");
           do
           {
                bool isNumber = int.TryParse(Console.ReadLine(), out choiceOfUser);
                if (choiceOfUser>0 && choiceOfUser<=i)
                {
                    return choiceOfUser;
                    break;
                }
                if (choiceOfUser == 9)
                {
                    break;
                }
                else Console.WriteLine("Кажется, ты нажал что-то не то... Попробуй еще раз.");
            }
            while (choiceOfUser !=9);
            return choiceOfUser;
        }

        public void GameMenu (Animal animal)
        {
            Console.WriteLine("Отлично! Теперь ты можешь:\n" +
                 "Напоить питомца, нажав - 1\n" +
                 "Покормить питомца, нажав - 2\n" +
                 "Поиграть с питомцем, нажав - 3\n" +
                 "Нажми 0, чтобы выйти\n");

            int choose;

            do
            {
                bool success = int.TryParse(Console.ReadLine(), out choose);

                switch (choose)
                {
                    case 1:
                        animal.Drink();
                        break;
                    case 2:
                        animal.Eat();
                        break;
                    case 3:
                        animal.Play();
                        break;
                    //case 0:
                    //    animal.Die(null, null);
                    //    break;
                    default:
                        break;
                }
            } while (choose != 0);

        }
    }
}
