using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InputFiles;

namespace Final_Project
{
    internal class TelegramBot
    {
        
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandleMessage(botClient, update.Message);
                return;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
        }

        public static async Task HandleMessage(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "/GetBooksCatalog - получить список всех книг\n"+
                                                                      "/Keyboard - открыть клавиатуру для работы с каталогом");
                return;
            }

            if (message.Text == "/GetBooksCatalog")
            {
                string outputInfo = BooksList.OutputAll();
                await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo);
                return;
            }

            if (message.Text == "/Keyboard")
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                    {
                    new KeyboardButton[] {"Filter", "Update"},
                    new KeyboardButton[] {"Sort", "Book"}
                    })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Действия по командам:\n" +
                                                                        "Filter-установить фильтр для списка книг\n" +
                                                                        "Update-обновить список книг\n" +
                                                                        "Sort-сортировать список\n" +
                                                                        "Book-получить книгу по номеру в каталоге", replyMarkup: keyboard);
                return;
            }
            
            if (message.Text == "Book")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введите номер книги в формате <Книга_***> где вместо *** подставьте номер книги из каталога");
                return;

            }

            if (message.Text.Contains("Книга"))
            {
                int choiceOfUser;
                string[] mes = message.Text.Split("_", 2);
                bool isNumber = int.TryParse(mes[1], out choiceOfUser);
                if (isNumber)
                {
                    string[] outputInfo = BooksList.GetFileName(choiceOfUser);
                    if (outputInfo[0] == "Файл не найден")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Книга с таким номером не найдена!");
                        return;
                    }
                    else
                    {
                        await using Stream stream = System.IO.File.OpenRead(outputInfo[1]);
                        await botClient.SendDocumentAsync(
                                                            chatId:  message.Chat.Id,
                                                            document: new InputOnlineFile(content: stream, fileName: outputInfo[0])
                                                            );
                        return;
                    }

                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Не верно введен номер книги, попробуйте ещё раз");
                    return;
                }
            }

            if (message.Text == "/inline")
            {
                InlineKeyboardMarkup keyboard = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Buy 50c", "buy_50c"),
                        InlineKeyboardButton.WithCallbackData("Buy 100c", "buy_100c"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Sell 50c", "sell_50c"),
                        InlineKeyboardButton.WithCallbackData("Sell 100c", "sell_100c"),
                    },
                });
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose inline:", replyMarkup: keyboard);
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat.Id, $"Нажмите /start чтобы ознакомиться с набором команд стартового меню");
        }

        public static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("buy"))
            {
                await botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    $"Вы хотите купить?"
                );
                return;
            }
            if (callbackQuery.Data.StartsWith("sell"))
            {
                await botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    $"Вы хотите продать?"
                );
                return;
            }
            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"You choose with data: {callbackQuery.Data}"
                );
            return;
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

    }
}
