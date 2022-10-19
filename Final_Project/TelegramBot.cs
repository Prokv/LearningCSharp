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
        public static string answer;
        public static int bookId;

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
                await botClient.SendTextMessageAsync(message.Chat.Id, "/GetBooksCatalog - получить список всех книг\n" +
                                                                      "/Keyboard - открыть клавиатуру для работы с каталогом");
                return;
            }

            if (message.Text == "/GetBooksCatalog")
            {
                GetBooksCatalog(botClient, message, BooksList.bookList);
                return;
            }

            if (message.Text == "/Keyboard")
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                    {
                    new KeyboardButton[] {"Filter", "Update"},
                    new KeyboardButton[] {"Sort"}
                    })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Действия по командам:\n" +
                                                                        "Filter-установить фильтр для списка книг\n" +
                                                                        "Update-обновить список книг\n" +
                                                                        "Sort-сортировать список", replyMarkup: keyboard);
                return;
            }

            if (message.Text == "Filter")
            {

                InlineKeyboardMarkup keyboard = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Автор", "Filter_Author"),
                        InlineKeyboardButton.WithCallbackData("Название", "Filter_Title"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Жанр", "Filter_Genre"),
                        InlineKeyboardButton.WithCallbackData("Тема", "Filter_Keywords"),
                    },
                });
                await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите способ фильтрации", replyMarkup: keyboard);
                return;
            }

            if (message.Text == "Sort")
            {
                string emogiUp = "\U00002B06";
                string emogiDown = "\U00002B07";
                InlineKeyboardMarkup keyboard = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Автор "+emogiDown, "Sort_Up_Author"),
                        InlineKeyboardButton.WithCallbackData("Жанр "+emogiDown, "Sort_Up_Genre"),
                        InlineKeyboardButton.WithCallbackData("Название "+emogiDown, "Sort_Up_Title"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Автор "+emogiUp, "Sort_Down_Author"),
                        InlineKeyboardButton.WithCallbackData("Жанр "+emogiUp, "Sort_Down_Genre"),
                        InlineKeyboardButton.WithCallbackData("Название "+emogiUp, "Sort_Down_Title"),
                    },
                });
                await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите способ сортировки", replyMarkup: keyboard);
                return;
            }

            if (message.Text == "Update")
            {
                PdfHandler.Update();
                GetBooksCatalog(botClient, message, BooksList.bookList);
                return;
            }

            if (answer!=null && answer.Contains("Filter"))
            {
                string field="";
                switch (answer)
                {
                    case "Filter_Author":
                        field = "Author";
                        break;
                    case "Filter_Title":
                        field = "Title";
                        break;
                    case "Filter_Genre":
                        field = "Genre";
                        break;
                    case "Filter_Keywords":
                        field = "Keywords";
                        break;

                }
                string name = message.Text;
                List<PdfMetaData> outputInfo = BooksList.FilterList(field, name);
                await GetBooksCatalog(botClient, message, outputInfo);
                answer = "";
                return;
            }

            if (answer != null && answer.Contains("Transform"))
            {
                string name = message.Text;
                List<PdfMetaData> outputInfo = new List<PdfMetaData> ();
                switch (answer)
                {
                    case "Transform_Author":
                        outputInfo = BooksList.SetAuthorInList(bookId, name);
                        break;
                    case "Transform_Title":
                        outputInfo = BooksList.SetTitleInList(bookId, name);
                        break;
                    case "Transform_Genre":
                        outputInfo = BooksList.SetKeywordsInList(bookId, name);
                        break;
                    case "Transform_Keywords":
                        outputInfo = BooksList.SetGenreInList(bookId, name);
                        break;

                }
                await GetBooksCatalog(botClient, message, outputInfo);
                answer = "";
                bookId = 0;
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Нажмите /start чтобы ознакомиться с набором команд стартового меню");
        }

        public static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data.StartsWith("Filter"))
            {
                string field = "";
                switch (callbackQuery.Data)
                {
                    case "Filter_Author":
                        field = "автора";
                        answer = "Filter_Author";
                        break;

                    case "Filter_Title":
                        field = "книги";
                        answer = "Filter_Title";
                        break;

                    case "Filter_Genre":
                        field = "жанра";
                        answer = "Filter_Genre";
                        break;

                    case "Filter_Keywords":
                        field = "темы";
                        answer = "Filter_Keywords";
                        break;
                }
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,$"Напишите название {field}");
                return;
            }

            if (callbackQuery.Data.StartsWith("Sort"))
            {
                string trend = callbackQuery.Data;
                List<PdfMetaData> outputInfo = BooksList.SortList(trend);
                await GetBooksCatalog(botClient, callbackQuery.Message, outputInfo);
                return;
            }

            if (callbackQuery.Data.StartsWith("Download"))
            {
                int choiceOfUser = 0;
                string[] mes = callbackQuery.Data.Split("_", 2);
                bool isNumber = int.TryParse(mes[1], out choiceOfUser);
                if (isNumber)
                {
                    string[] outputInfo = BooksList.GetFileName(choiceOfUser);
                    if (outputInfo[0] == "Файл не найден")
                    {
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Книга с таким номером не найдена!");
                        return;
                    }
                    else
                    {
                        string emogiRocet = "\U0001F680";
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Ожидайте {emogiRocet}  идёт загрузка книги");
                        await using Stream stream = System.IO.File.OpenRead(outputInfo[1]);
                        await botClient.SendDocumentAsync(
                                                            chatId: callbackQuery.Message.Chat.Id,
                                                            document: new InputOnlineFile(content: stream, fileName: outputInfo[0])
                                                            );
                        return;
                    }
                }
            }

            if (callbackQuery.Data.StartsWith("Change"))
            {
                int choiceOfUser = 0;
                string[] mes = callbackQuery.Data.Split("_", 2);
                bool isNumber = int.TryParse(mes[1], out choiceOfUser);

                InlineKeyboardMarkup keyboard = new(new[]
                                {
                                                    new[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("Автор", $"Transform_Author_{choiceOfUser}"),
                                                        InlineKeyboardButton.WithCallbackData("Название", $"Transform_Title_{choiceOfUser}"),
                                                    },
                                                    new[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("Жанр", $"Transform_Genre_{choiceOfUser}"),
                                                        InlineKeyboardButton.WithCallbackData("Тема", $"Transform_Keywords_{choiceOfUser}"),
                                                    },
                                                });
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите параметр для изменения", replyMarkup: keyboard);
                return;
            }

            if (callbackQuery.Data.StartsWith("Transform"))
            {
                string field = "";
                string[] mes = callbackQuery.Data.Split("_", 3);
                switch (mes[1])
                {
                    case "Author":
                        field = "автора";
                        answer = "Transform_Author";
                        break;

                    case "Title":
                        field = "книги";
                        answer = "Transform_Title";
                        break;

                    case "Genre":
                        field = "жанра";
                        answer = "Transform_Genre";
                        break;

                    case "Keywords":
                        field = "темы";
                        answer = "Transform_Keywords";
                        break;
                }
                int choiceOfUser = 0;
                bool isNumber = int.TryParse(mes[2], out choiceOfUser);
                if (isNumber)
                {
                    bookId = choiceOfUser;
                }
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Напишите название {field}");
                return;
            }

            if (callbackQuery.Data.StartsWith("Delete"))
            {
                string[] mes = callbackQuery.Data.Split("_", 2);
                int choiceOfUser = 0;
                int count = BooksList.bookList.Count;
                string pathFile = "";
                string fileName = "";

                bool isNumber = int.TryParse(mes[1], out choiceOfUser);
                for (int i = 0; i <count; i++)
                {
                    if (choiceOfUser == BooksList.bookList[i].Id)
                    {
                        pathFile = BooksList.bookList[i].PathFile;
                        fileName = BooksList.bookList[i].FileName;
                        PdfHandler.DeleteFile(pathFile);
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Файл {fileName}, удален.");
                        break;           
                    }
                }
                return;
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            //await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Произошла ошибка в работе телеграмм бота. Обратитесь к администратору!");
        }

        public static async Task GetBooksCatalog (ITelegramBotClient botClient, Message message, List<PdfMetaData> bookList)
        {
            int count = bookList.Count;
            string outputInfo = "<strong>Доступный список книг:</strong>";
            await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo, ParseMode.Html);
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    int bookId = bookList[i].Id;
                    outputInfo = BooksList.OutputById(bookId);
                    InlineKeyboardMarkup keyboard = new(new[]
                    {
                                            new[]
                                            {
                                                InlineKeyboardButton.WithCallbackData("Скачать", $"Download_{bookId}"),
                                                InlineKeyboardButton.WithCallbackData("Изменить", $"Change_{bookId}"),
                                                InlineKeyboardButton.WithCallbackData("Удалить", $"Delete_{bookId}"),
                                            },
                                        });
                    await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo, replyMarkup: keyboard);
                }
            }
            else
            {
                outputInfo = "Список файлов пуст.";
                await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo);
            }

        }

    }
}
