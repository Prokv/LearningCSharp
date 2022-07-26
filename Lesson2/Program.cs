namespace Lesson2 // Игра крестики-нолики на 2-х человек
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Игра крестики-нолики."+ $"{Environment.NewLine}Хотите сыграть в игру?" +
                $"{Environment.NewLine}[Нажмите Y если согласны или Любую клавишу если нет]");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine();
            if (input.Key == ConsoleKey.Y)
            {
                string[] Field = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string name;

                Console.WriteLine("Игра начата. Начинает игрок Х, выбирая позицию на стартовом поле");
                TicTacToeGame.Grid(Field);
                for (int i = 1; i <= Field.Length;)
                {
                    if (i % 2 > 0)
                    {
                        name = "X";
                    }
                    else {
                        name = "O";
                    }
                    TicTacToeGame.StepCheck(name, Field, ref i);
                    TicTacToeGame.Grid(Field);
                    if (i > 4) TicTacToeGame.WinCheck(name, Field, ref i);
                    if (i == Field.Length+1) Console.WriteLine("Ничья!Победила дружба.\a");
                }
            }
            else Console.WriteLine("Отказ от игры!");
        }
    }
}