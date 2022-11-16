using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

namespace Final_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Запускаем класс для работы со списком книг.
            BookList BookList = BookList.getInstance();

            //Запускаем обработчик пдф файлов.
            FileHandler getPdfHandler = new FileHandler();

            //Запускаем обработчик событий телеграмм бота
            UpdateHandler UpdateHandler = new UpdateHandler();

            Console.ReadLine();
        }
    }
}
