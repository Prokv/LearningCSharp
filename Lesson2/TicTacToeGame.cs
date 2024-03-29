﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson2
{
    internal class TicTacToeGame
    {
        /// <summary>
        /// Проверка наличия выигрышной комбинации.
        /// </summary>
        public static void WinCheck(string name, string[] Field, ref int i)
        {
            int[,] WinArray = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };

            
            for (int r = 0; r < 8; r++)
            {
                if (Field[WinArray[r, 0]] == Field[WinArray[r, 1]] & Field[WinArray[r, 1]] == Field[WinArray[r, 2]])
                {
                    Console.WriteLine($"Победа игрока: {name}. Игра окончена.\a");
                    i = Field.Length + 2;
                }
            }
        }
        /// <summary>
        /// Раскраска поля в зависимости от хода игрока.
        /// </summary>
        public static void Cell_Color(string[] Field, int min, int max)
        {
            Console.Write("|");
            for (int j = min; j < max; j++)
            {
                if (Field[j] == "X")
                { Console.ForegroundColor = ConsoleColor.Red; }
                else
                    if (Field[j] == "O")
                { Console.ForegroundColor = ConsoleColor.Green; }
                else Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(Field[j]);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("|");
            }
        }
        /// <summary>
        /// Построение игровой решетки поля.
        /// </summary>
        public static void Grid(string[] Field)
        {
            int min = 0;
            int max = 3;

            Console.WriteLine("-------");
            for (int i = 0; i < 3; i++)
            {
                Cell_Color(Field, min, max);
                Console.WriteLine();
                min = max;
                max +=3;
            }
            Console.WriteLine("-------");
        }
        /// <summary>
        /// Определяет очередность хода и правильность выбора поля для хода.
        /// </summary>
        public static void StepCheck(string name, string[] Field, ref int i)
        {
            int step;
            Console.Write($"Ход игрока {name}:");
            if (int.TryParse(Console.ReadLine(), out int numValue))
            {
                step = numValue - 1;
                if (numValue > 0 & numValue <= Field.Length)
                {
                    if (Field[step] != "X" && Field[step] != "O")
                    {
                        Console.Clear();
                        Field[step] = name;
                        i++;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Ход игрока {name}: {numValue}. Поле занято, ход невозможен! Повторите попытку.");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Ход игрока {name}: {numValue}. Ход невозможен. Повторите попытку.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Ход игрока {name}.Некорректный ввод. Введите цифровое значение! Повторите попытку.");
            }
        }
    }
}
