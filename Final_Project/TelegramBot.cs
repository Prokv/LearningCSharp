using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InputFiles;

namespace Final_Project
{
    internal class TelegramBot
    {
        
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    //await using stream stream = system.io.file.openread(@"c:\users\admin\documents\учеба c#\learningcsharp\test1\bin\debug\net6.0\books\джек швайгер-биржевые маги.pdf");
                    //await botclient.senddocumentasync(
                    //                                   chatid:  message.chat.id,
                    //                                   document: new inputonlinefile(content: stream, filename: "джек швайгер-биржевые маги.pdf"),
                    //                                   caption: "получи фашист гранату"
                    //                                  );
                    //string outputInfo = BooksList.OutputAll();
                    //(message.Chat.Id, "Олег, привет");
                    return;
                }

                await botClient.SendTextMessageAsync(
                                                    message.Chat,
                                                    "Номер в каталоге\n" +
                                                    "1\n" + 
                                                    "Автор\n" +
                                                    "Джек Швайгер\n"+
                                                    "Название\n" +
                                                    "Биржевые маги\n" +
                                                    "Жанр\n" +
                                                    "Не определено");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

    }
}
