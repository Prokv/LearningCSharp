namespace Lesson2 // Игра крестики-нолики на 2-х человек
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"NewLine: {Environment.NewLine}  Хотите сыграть в игру?" +
                $"{Environment.NewLine}  [Нажмите Y если согласны или Любую клавишу если нет]");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine();
            if (input.Key == ConsoleKey.Y) 
            {
                Console.WriteLine("Игра начата. Начинает игрок Х, выбирая позицию на стартовом поле"); 
                string[] Pos = new string[] {"1","2","3","4","5","6","7","8","9"};
                Console.WriteLine("-------");
                Console.WriteLine("|" + Pos[0] + "|" + Pos[1] + "|" + Pos[2] + "|");
                Console.WriteLine("|" + Pos[3] + "|" + Pos[4] + "|" + Pos[5] + "|");
                Console.WriteLine("|" + Pos[6] + "|" + Pos[7] + "|" + Pos[8] + "|");
                Console.WriteLine("-------");
                int[] movesX = new int[9];
                int[] movesO = new int[9];
                int step;
                for (int i = 1; i <= Pos.Length;)
                {
                    if (i % 2 > 0)
                    {
                        Console.Write("Ход игрока Х: ");
                        if (int.TryParse(Console.ReadLine(), out int numValue))
                        {
                            step = numValue - 1;
                            if (numValue > 0 & numValue < 10)
                            { if (Pos[step] != "X" & Pos[step] != "O")
                                {
                                    movesX[step] = step + 1;
                                    Pos[step] = "X";
                                    //Console.WriteLine("[{0}]", string.Join(", ", movesX)); // Проверка что содержится в массиве ходов игрока Х
                                    Console.WriteLine("-------");
                                    Console.Write("|");
                                    for (int j = 0; j < 3; j++)
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
                                    Console.WriteLine();
                                    Console.Write("|");
                                    for (int j = 3; j < 6; j++)
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
                                    Console.WriteLine();
                                    Console.Write("|");
                                    for (int j = 6; j < 9; j++)
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
                                    Console.WriteLine();
                                    Console.WriteLine("-------");
                                    i++;
                                }
                                else
                                {
                                    Console.WriteLine("Ход игрока Х: Поле занято, ход невозможен!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ход игрока Х: Ход невозможен");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ввод");
                        }
                    }
                    else {
                        Console.Write("Ход игрока O: ");
                        if (int.TryParse(Console.ReadLine(), out int numValue))
                        {
                            step = numValue - 1;
                            if (numValue > 0 & numValue < 10)
                            {
                                if (Pos[step] != "X" & Pos[step] != "O")
                                {
                                    movesO[step] = step + 1;
                                    Pos[step] = "O";
                                    //Console.WriteLine("[{0}]", string.Join(", ", movesO)); // Проверка что содержится в массиве ходов игрока O
                                    Console.WriteLine("-------");
                                    Console.Write("|");
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (Pos[j] == "O")
                                        { Console.ForegroundColor = ConsoleColor.Green; }
                                        else
                                            if (Pos[j] == "X")
                                        { Console.ForegroundColor = ConsoleColor.Red; }
                                        else Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write(Pos[j]);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write("|");
                                    }
                                    Console.WriteLine();
                                    Console.Write("|");
                                    for (int j = 3; j < 6; j++)
                                    {
                                        if (Pos[j] == "O")
                                        { Console.ForegroundColor = ConsoleColor.Green; }
                                        else
                                            if (Pos[j] == "X")
                                        { Console.ForegroundColor = ConsoleColor.Red; }
                                        else Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write(Pos[j]);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write("|");
                                    }
                                    Console.WriteLine();
                                    Console.Write("|");
                                    for (int j = 6; j < 9; j++)
                                    {
                                        if (Pos[j] == "O")
                                        { Console.ForegroundColor = ConsoleColor.Green; }
                                        else
                                            if (Pos[j] == "X")
                                        { Console.ForegroundColor = ConsoleColor.Red; }
                                        else Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write(Pos[j]);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write("|");
                                    }
                                    Console.WriteLine();
                                    Console.WriteLine("-------");
                                    i++;
                                }
                                else
                                {
                                    Console.WriteLine("Ход игрока O: Поле занято, ход невозможен!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ход игрока O: Ход невозможен");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некорректный ввод");
                        }
                    }
                }
                                
            }
            else {
                Console.WriteLine("Отказ от игры!"); 
                return; }

        }

    }
}