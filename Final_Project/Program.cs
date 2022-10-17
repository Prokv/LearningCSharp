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
    internal class Program
    {
        static void Main(string[] args)
        {

            //Создаем директорию для хранения Pdf файлов (книг). Запускаем обработчик пдф файлов в этой директории.//
            PdfHandler getPdfHandler = new PdfHandler();

            //Создаем и запускаем Телеграмм Бота//
<<<<<<< HEAD
            ITelegramBotClient botClient = new TelegramBotClient("*******************");
            Console.WriteLine("Запущен бот " + botClient.GetMeAsync().Result.FirstName);
=======
            ITelegramBotClient bot = new TelegramBotClient("******");
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
>>>>>>> 008ab3420908f5da960196184407979ccc6d919c

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            botClient.StartReceiving(
                TelegramBot.HandleUpdateAsync,
                TelegramBot.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.ReadLine();
        }
    }
}
