namespace Lesson2 // Игра крестики-нолики на 2-х человек
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Игра крестики-нолики."+ $"{Environment.NewLine}Хотите сыграть в игру?" +
                $"{Environment.NewLine}[Нажмите Y если согласны или Любую клавишу если нет]");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine();
            if (input.Key == ConsoleKey.Y) 
            {
                string[] Pos = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string name;

                Console.WriteLine("Игра начата. Начинает игрок Х, выбирая позицию на стартовом поле");
                GameField(Pos);
                for (int i = 1; i <= Pos.Length;)
                {
                    if (i % 2 > 0)
                    {
                        name = "X";
                        i= StepCheck(name, Pos, i);
                    }
                    else {
                        name = "O";
                        i= StepCheck(name, Pos, i);
                    }
                    if (i == Pos.Length+1)
                    {
                        Console.WriteLine("Ничья!Победила дружба.");
                    }
                }
            }
            else {
                Console.WriteLine("Отказ от игры!");  
                  }
        }
        public static int StepCheck (string name, string[] Pos, int i ) //Проверка правильности хода
        {
            int step;
            Console.Write("Ход игрока "+name+":");
            if (int.TryParse(Console.ReadLine(), out int numValue))
            {
                step = numValue - 1;
                if (numValue > 0 & numValue <= Pos.Length)
                {
                    if (Pos[step] != "X" & Pos[step] != "O")
                    {
                        Console.Clear();
                        Pos[step] = name;
                        GameField(Pos);
                        if (i > 4)
                        {
                            i = WinCheck(name, Pos, i); //Проверка на выигрыш
                        }
                        else { i++; }
                        return i;
                    }
                    else
                    {
                        Console.WriteLine("Ход игрока "+ name+": Поле занято, ход невозможен!");
                        return i;
                    }
                }
                else
                {
                    Console.WriteLine("Ход игрока "+ name+": Ход невозможен");
                    return i;
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод");
                return i;
            }
        }
    
        public static void GameField(string [] Pos) // Отрисовка полей игры
        {
            int min=0;
            int max=3;
            
            Console.WriteLine("-------");
            for (int i = 0; i < 3; i++)
            {
                Cell_Color(Pos, min, max);
                Console.WriteLine();
                min = max;
                max = max + 3;
            }
            Console.WriteLine("-------");
        }
        public static void Cell_Color(string[] Pos, int min, int max) //Раскраска полей цветом
        {
            Console.Write("|");
            for (int j=min; j < max; j++)
            {
                if (Pos[j] == "X")
                { Console.ForegroundColor = ConsoleColor.Red; }
                else
                    if (Pos[j] == "O")
                { Console.ForegroundColor = ConsoleColor.Green; }
                else Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(Pos[j]);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("|");
            }
        }
        public static int WinCheck(string name, string[] Pos, int i) //Проверка выигрышной комбинации
        {
            int[,] array = new int[8,3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } }; //массив с выигрышными комбинациями

            for (int r = 0; r < 8; r++) //Перебор строк
            {
                if (Pos[array[r, 0]] == Pos[array[r, 1]] & Pos[array[r, 1]]== Pos[array[r, 2]])
                    {
                        Console.WriteLine("Победа игрока " + name + ". Игра окончена");
                        i = Pos.Length + 2;
                        return i;
                    }
            }
            i++;
            return i;
        }
    }
}