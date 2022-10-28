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
        static void Main(string[] args)
        {

            //Создаем директорию для хранения Pdf файлов (книг). Запускаем обработчик пдф файлов в этой директории.//
            FileHandler getPdfHandler = new FileHandler();

            //Создаем и запускаем Телеграмм Бота//
            ITelegramBotClient botClient = new TelegramBotClient("****"); //токен по запросу получаем.
            Console.WriteLine("Запущен бот " + botClient.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            botClient.StartReceiving(
                UpdateHandler.HandleUpdateAsync,
                UpdateHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

            Console.ReadLine();
        }
    }
}
