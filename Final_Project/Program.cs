﻿using System;
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
            ITelegramBotClient botClient = new TelegramBotClient("5520714688:AAFw0UIaWAx0pCP8NfWQ0I7zQ9rH9KWudZE");
            Console.WriteLine("Запущен бот " + botClient.GetMeAsync().Result.FirstName);

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
