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
        public static int stringNumber;
        public static int stepNumber=3;
        public static List<PdfMetaData> userBookList = new List<PdfMetaData>();

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
                                                                      "/Keyboard - открыть клавиатуру для работы с каталогом", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            if (message.Text == "/GetBooksCatalog")
            {
                stringNumber = 0;
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
                    ResizeKeyboard = true
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
                        InlineKeyboardButton.WithCallbackData("\U0001F464 Автор", "Filter_Author"),
                        InlineKeyboardButton.WithCallbackData("\U0001F520 Название", "Filter_Title"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("\U0001F46A Жанр", "Filter_Genre"),
                        InlineKeyboardButton.WithCallbackData("\U0001F481 Тема", "Filter_Keywords"),
                    },
                });
                await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Выберите способ фильтрации</strong>", ParseMode.Html, replyMarkup: keyboard);
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
                await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Выберите способ сортировки</strong>", ParseMode.Html, replyMarkup: keyboard);
                return;
            }

            if (message.Text == "Update")
            {
                PdfHandler.Update();
                stringNumber = 0;
                GetBooksCatalog(botClient, message, BooksList.bookList);
                return;
            }

            if (message.Text == "Forward")
            {
                stringNumber += stepNumber;
                if (stringNumber>=userBookList.Count)
                {
                    stringNumber=userBookList.Count;
                    await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли конца списка!</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                    return;
                }
                List<PdfMetaData> newUserBookList = new List<PdfMetaData>();
                for (int i = 0; i < userBookList.Count; i++)
                {
                    newUserBookList.Add(userBookList[i]);

                }
                GetBooksCatalog(botClient, message, newUserBookList);
                return;
            }

            //if (message.Text == "Backward")
            //{
            //    stringNumber -= stepNumber;
            //    if (stringNumber < 0)
            //    {
            //        stringNumber = 0;
            //        await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли начала списка!</strong>", ParseMode.Html);
            //        //return;
            //    }
            //    List<PdfMetaData> newUserBookList = new List<PdfMetaData>();
            //    for (int i = 0; i < userBookList.Count; i++)
            //    {
            //        newUserBookList.Add(userBookList[i]);

            //    }
            //    GetBooksCatalog(botClient, message, newUserBookList);
            //    return;
            //}

            if (answer!=null && answer.Contains("Filter"))
            {
                string field="";
                switch (answer)
                {
                    case "Filter_Author":
                        field = "Author";
                        answer = "Список книг отфильтрован по автору";
                        break;
                    case "Filter_Title":
                        field = "Title";
                        answer = "Список книг отфильтрован по названию";
                        break;
                    case "Filter_Genre":
                        field = "Genre";
                        answer = "Список книг отфильтрован по жанру";
                        break;
                    case "Filter_Keywords":
                        field = "Keywords";
                        answer = "Список книг отфильтрован по теме";
                        break;

                }
                string name = message.Text;
                List<PdfMetaData> outputInfo = BooksList.FilterList(field, name);
                stringNumber = 0;
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>{answer} :</strong>  <i>{name}</i>.", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
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
                        answer = "Автор успешно изменен";
                        break;
                    case "Transform_Title":
                        outputInfo = BooksList.SetTitleInList(bookId, name);
                        answer = "Название успешно изменено";
                        break;
                    case "Transform_Genre":
                        outputInfo = BooksList.SetGenreInList(bookId, name);
                        answer = "Жанр упешно изменен";
                        break;
                    case "Transform_Keywords":
                        outputInfo =  BooksList.SetKeywordsInList(bookId, name);
                        answer = "Тема упешно изменена";
                        break;

                }
                stringNumber = 0;
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>{answer} на:</strong>  <i>{name}</i>.", ParseMode.Html);
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
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,$"Напишите название {field}", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            if (callbackQuery.Data.StartsWith("Sort"))
            {
                string trend = callbackQuery.Data;
                List<PdfMetaData> outputInfo = BooksList.SortList(trend);
                stringNumber = 0;
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
                string outputInfo = "";
                int choiceOfUser = 0;
                string[] mes = callbackQuery.Data.Split("_", 2);
                bool isNumber = int.TryParse(mes[1], out choiceOfUser);
                outputInfo = BooksList.OutputById(choiceOfUser);
                InlineKeyboardMarkup keyboard = new(new[]
                                {
                                                    new[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("\U0001F464 Автор", $"Transform_Author_{choiceOfUser}"),
                                                        InlineKeyboardButton.WithCallbackData("\U0001F520 Название", $"Transform_Title_{choiceOfUser}"),
                                                    },
                                                    new[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("\U0001F46A Жанр", $"Transform_Genre_{choiceOfUser}"),
                                                        InlineKeyboardButton.WithCallbackData("\U0001F481 Тема", $"Transform_Keywords_{choiceOfUser}"),
                                                    },
                                                });
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "<strong>Выберите параметр для изменения:</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "<strong>Выбранная книга:\n</strong>" + outputInfo, ParseMode.Html, replyMarkup: keyboard);                
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
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Файл {fileName}, удален.", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                        break;           
                    }
                }
                PdfHandler.Update();
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
            int lastListPosition= stringNumber+stepNumber;
            if (lastListPosition > count) { lastListPosition = count; }
            string outputInfo = $"<strong>Список книг номера ({stringNumber+1}-{lastListPosition}) из {count}:</strong>";
            await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo, ParseMode.Html);
            if (count != 0)
            {
                for (int i = stringNumber; i < lastListPosition; i++)
                {
                    int bookId = bookList[i].Id;
                    outputInfo = BooksList.OutputById(bookId);
                    InlineKeyboardMarkup keyboard_1 = new(new[]
                    {
                                            new[]
                                            {
                                                InlineKeyboardButton.WithCallbackData("\U0001F4D6 Скачать", $"Download_{bookId}"),
                                                InlineKeyboardButton.WithCallbackData("\U0001F4DD Изменить", $"Change_{bookId}"),
                                                InlineKeyboardButton.WithCallbackData("\U0001F4DB Удалить", $"Delete_{bookId}"),
                                            },
                                        });
                    await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo, ParseMode.Html, replyMarkup: keyboard_1);
                }
                if (lastListPosition == count)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли конца списка!</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/GetBooksCatalog - получить список всех книг\n" +
                                      "/Keyboard - открыть клавиатуру для работы с каталогом");
                }
                else
                {
                    if (count > stepNumber)
                    {
                        ReplyKeyboardMarkup keyboard_2 = new(new[] { new KeyboardButton[] { "Forward" }, })
                        {
                            ResizeKeyboard = true
                        };
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Нажмите Forward-чтобы получить дополнительный список книг.", replyMarkup: keyboard_2);
                    }
                }
            }
            else
            {
                outputInfo = "<strong>Список файлов пуст. \U0001F625</strong>";
                await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo, ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            userBookList.Clear();
            for (int i = 0; i < count; i++)
            {
                userBookList.Add(bookList[i]);
            }
        }
    }
}
