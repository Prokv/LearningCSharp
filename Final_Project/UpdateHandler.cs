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
    internal class UpdateHandler
    {
        /// <summary>
        /// Обработчик типов сообщений
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="update">Сообщение</param>
        /// <param name="cancellationToken">Ключ доступа к боту (токен)</param>
        /// <returns></returns>
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                UserSettingsList.Add(update.Message.From.Id);
                await HandleMessage(botClient, update.Message);
                return;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                UserSettingsList.Add(update.CallbackQuery.From.Id);
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
        }

        /// <summary>
        /// Обработчик текстовых сообщений
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public static async Task HandleMessage(ITelegramBotClient botClient, Message message)
        {
            long userId = message.From.Id;
            object[] userInfo = UserSettingsList.GetUserInfo(userId);
            string userReturn = userInfo[0].ToString();
            int bookId = Convert.ToInt32(userInfo[1]);
            int lineNumber = Convert.ToInt32(userInfo[2]);
            int stepNumber = Convert.ToInt32(userInfo[3]);
            List<BookInfo> userBookList = UserSettingsList.GetUserBookList(userId);

            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "/GetBooksCatalog - получить список всех книг\n" +
                                                                      "/Keyboard - открыть клавиатуру для работы с каталогом", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            if (message.Text == "/GetBooksCatalog")
            {
                lineNumber = 0;
                userInfo[2] = lineNumber;
                UserSettingsList.SetUserInfo(userId, userInfo);
                GetBooksCatalog(botClient, message, BookList.bookList, userId);
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
                FileHandler.Update();
                lineNumber = 0;
                userInfo[2] = lineNumber;
                UserSettingsList.SetUserInfo(userId, userInfo);
                GetBooksCatalog(botClient, message, BookList.bookList, userId);
                return;
            }

            if (message.Text == "Forward")
            {
                lineNumber += stepNumber;
                if (lineNumber>=userBookList.Count)
                {
                    lineNumber=userBookList.Count;
                    await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли конца списка!</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                    return;
                }
                userInfo[2] = lineNumber;
                UserSettingsList.SetUserInfo(userId, userInfo);
                List<BookInfo> newUserBookList = new List<BookInfo>();
                for (int i = 0; i < userBookList.Count; i++)
                {
                    newUserBookList.Add(userBookList[i]);

                }
                GetBooksCatalog(botClient, message, newUserBookList, userId);
                return;
            }

            if (userReturn!=null && userReturn.Contains("Filter"))
            {
                string field="";
                string user_mes = "";
                switch (userReturn)
                {
                    case "Filter_Author":
                        field = "Author";
                        user_mes = "Список книг отфильтрован по автору";
                        break;
                    case "Filter_Title":
                        field = "Title";
                        user_mes = "Список книг отфильтрован по названию";
                        break;
                    case "Filter_Genre":
                        field = "Genre";
                        user_mes = "Список книг отфильтрован по жанру";
                        break;
                    case "Filter_Keywords":
                        field = "Keywords";
                        user_mes = "Список книг отфильтрован по теме";
                        break;

                }
                string value = message.Text;
                List<BookInfo> outputInfo = BookList.GetFilterList(field, value);
                lineNumber = 0;
                userReturn = "";
                userInfo[0] = userReturn;
                userInfo[2] = lineNumber;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>{user_mes} :</strong>  <i>{value}</i>.", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                await GetBooksCatalog(botClient, message, outputInfo, userId);
                return;
            }

            if (userReturn != null && userReturn.Contains("Transform"))
            {
                string name = message.Text;
                List<BookInfo> outputInfo = new List<BookInfo> ();
                switch (userReturn)
                {
                    case "Transform_Author":
                        outputInfo = BookList.SetAuthor(bookId, name);
                        userReturn = "Автор успешно изменен";
                        break;
                    case "Transform_Title":
                        outputInfo = BookList.SetTitle(bookId, name);
                        userReturn = "Название успешно изменено";
                        break;
                    case "Transform_Genre":
                        outputInfo = BookList.SetGenre(bookId, name);
                        userReturn = "Жанр упешно изменен";
                        break;
                    case "Transform_Keywords":
                        outputInfo =  BookList.SetKeywords(bookId, name);
                        userReturn = "Тема упешно изменена";
                        break;

                }
                lineNumber = 0;
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>{userReturn} на:</strong>  <i>{name}</i>.", ParseMode.Html);
                await GetBooksCatalog(botClient, message, outputInfo, userId);
                userReturn = "";
                bookId = 0;
                userInfo[0] = userReturn;
                userInfo[1] = bookId;
                userInfo[2] = lineNumber;
                UserSettingsList.SetUserInfo(userId, userInfo);
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat.Id, $"Нажмите /start чтобы ознакомиться с набором команд стартового меню");
        }

        /// <summary>
        /// Обработчик сообщений типа Callback
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="callbackQuery">Сообщение</param>
        /// <returns></returns>
        public static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {      
            long userId = callbackQuery.From.Id;
            object[] userInfo = UserSettingsList.GetUserInfo(userId);
            string userReturn = userInfo[0].ToString();
            int bookId = Convert.ToInt32(userInfo[1]);
            int lineNumber = Convert.ToInt32(userInfo[2]);
            int stepNumber = Convert.ToInt32(userInfo[3]);
            List<BookInfo> userBookList = UserSettingsList.GetUserBookList(userId);

            if (callbackQuery.Data.StartsWith("Filter"))
            {
                string field = "";
                userReturn = callbackQuery.Data;
                switch (userReturn)
                {
                    case "Filter_Author":
                        field = "автора";
                        break;

                    case "Filter_Title":
                        field = "книги";
                        break;

                    case "Filter_Genre":
                        field = "жанра";
                        break;

                    case "Filter_Keywords":
                        field = "темы";
                        break;
                }

                userInfo[0] = userReturn;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,$"Напишите название {field}", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            if (callbackQuery.Data.StartsWith("Sort"))
            {
                string ordBy = callbackQuery.Data;
                List<BookInfo> outputInfo = BookList.GetSortList(ordBy);
                lineNumber = 0;
                userInfo[2] = lineNumber;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await GetBooksCatalog(botClient, callbackQuery.Message, outputInfo, userId);
                return;
            }

            if (callbackQuery.Data.StartsWith("Download"))
            {
                int choiceOfUser = 0;
                string[] mes = callbackQuery.Data.Split("_", 2);
                bool isNumber = int.TryParse(mes[1], out choiceOfUser);
                if (isNumber)
                {
                    string[] outputInfo = BookList.GetBookInfo(choiceOfUser);
                    if (outputInfo[0] == "Значение не найдено")
                    {
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Книга с таким номером не найдена!");
                        return;
                    }
                    else
                    {
                        string emogiRocet = "\U0001F680";
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Ожидайте {emogiRocet}  идёт загрузка книги");
                        await using Stream stream = System.IO.File.OpenRead(outputInfo[4]);
                        await botClient.SendDocumentAsync(
                                                            chatId: callbackQuery.Message.Chat.Id,
                                                            document: new InputOnlineFile(content: stream, fileName: outputInfo[3])
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
                outputInfo = BookList.GetBookInfoToString(choiceOfUser);
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
                string mes1=string.Join("_", mes[0], mes[1]);
                switch (mes1)
                {
                    case "Transform_Author":
                        field = "автора";
                        break;

                    case "Transform_Title":
                        field = "книги";
                        break;

                    case "Transform_Genre":
                        field = "жанра";
                        break;

                    case "Transform_Keywords":
                        field = "темы";
                        break;
                }
                userInfo[0] = mes1;
                UserSettingsList.SetUserInfo(userId, userInfo);
                int choiceOfUser = 0;
                bool isNumber = int.TryParse(mes[2], out choiceOfUser);
                if (isNumber)
                {
                    bookId = choiceOfUser;
                    userInfo[1] = bookId;
                    UserSettingsList.SetUserInfo(userId, userInfo);
                }
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Напишите название {field}");
                return;
            }

            if (callbackQuery.Data.StartsWith("Delete"))
            {
                string[] mes = callbackQuery.Data.Split("_", 2);
                int choiceOfUser = 0;
                int count = BookList.bookList.Count;
                string pathFile = "";
                string fileName = "";

                bool isNumber = int.TryParse(mes[1], out choiceOfUser);
                for (int i = 0; i <count; i++)
                {
                    if (choiceOfUser == BookList.bookList[i].Id)
                    {
                        pathFile = BookList.bookList[i].PathFile;
                        fileName = BookList.bookList[i].FileName;
                        FileHandler.DeleteFile(pathFile);
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Файл {fileName}, удален.", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                        break;           
                    }
                }
                FileHandler.Update();
                return;
            }
        }

        /// <summary>
        /// Обработчик ошибок
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="exception">Ошибка</param>
        /// <param name="cancellationToken">Токен</param>
        /// <returns></returns>
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            //await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Произошла ошибка в работе телеграмм бота. Обратитесь к администратору!");
        }

        /// <summary>
        /// Получить каталог книг
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="message">Сообщение</param>
        /// <param name="bookList">Список книг</param>
        /// <param name="UserId">ИД пользователя</param>
        /// <returns></returns>
        public static async Task GetBooksCatalog (ITelegramBotClient botClient, Message message, List<BookInfo> bookList, long UserId)
        {
            long userId = UserId;
            object[] userInfo = UserSettingsList.GetUserInfo(userId);
            int lineNumber = Convert.ToInt32(userInfo[2]);
            int stepNumber = Convert.ToInt32(userInfo[3]);
            List<BookInfo> userBookList = UserSettingsList.GetUserBookList(userId);

            int count = bookList.Count;
            int lastListPosition= lineNumber+stepNumber;
            if (lastListPosition > count) { lastListPosition = count; }
            string outputInfo = $"<strong>Список книг номера ({lineNumber + 1}-{lastListPosition}) из {count}:</strong>";
            if (count!=1)
            {  
                await botClient.SendTextMessageAsync(message.Chat.Id, outputInfo, ParseMode.Html);
            }
            if (count != 0)
            {
                for (int i = lineNumber; i < lastListPosition; i++)
                {
                    int bookId = bookList[i].Id;
                    outputInfo = BookList.GetBookInfoToString(bookId);
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
                    if (count != 1)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли конца списка!</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                    }                   
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
                UserSettingsList.SetUserBookList(userId, userBookList);
            }
        }
    }
}
