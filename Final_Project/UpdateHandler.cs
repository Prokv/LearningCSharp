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
    public class UpdateHandler
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
            if (update.Type == UpdateType.Message) //&& update?.Message?.Text != null
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
            string userResponse = userInfo[0].ToString();
            int bookId = Convert.ToInt32(userInfo[1]);
            int startLine = Convert.ToInt32(userInfo[2]);
            int step = Convert.ToInt32(userInfo[3]);
            List<BookInfo> userBookList = UserSettingsList.GetUserBookList(userId);

            if (userResponse != null && userResponse.Contains("Filter"))
            {
                string field = "";
                string user_mes = "";
                switch (userResponse)
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
                startLine = 0;
                userResponse = "";
                userInfo[0] = userResponse;
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>{user_mes} :</strong>  <i>{value}</i>.", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                await PrintCatalog(botClient, message, outputInfo, userId);
                return;
            }

            if (userResponse != null && userResponse.Contains("Rewrite"))
            {
                string name = message.Text;
                List<BookInfo> outputInfo = new List<BookInfo>();
                switch (userResponse)
                {
                    case "Rewrite_Author":
                        outputInfo = BookList.SetAuthor(bookId, name);
                        userResponse = "Автор успешно изменен";
                        break;
                    case "Rewrite_Title":
                        outputInfo = BookList.SetTitle(bookId, name);
                        userResponse = "Название успешно изменено";
                        break;
                    case "Rewrite_Genre":
                        outputInfo = BookList.SetGenre(bookId, name);
                        userResponse = "Жанр упешно изменен";
                        break;
                    case "Rewrite_Keywords":
                        outputInfo = BookList.SetKeywords(bookId, name);
                        userResponse = "Тема упешно изменена";
                        break;

                }
                startLine = 0;
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>{userResponse} на:</strong>  <i>{name}</i>.", ParseMode.Html);
                await PrintCatalog(botClient, message, outputInfo, userId);
                userResponse = "";
                bookId = 0;
                userInfo[0] = userResponse;
                userInfo[1] = bookId;
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                return;
            }

            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "/catalog - получить список всех книг\n" +
                                                                      "/keyboard - открыть клавиатуру для работы с каталогом", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            if (message.Text == "/catalog")
            {
                startLine = 0;
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                PrintCatalog(botClient, message, BookList.bookList, userId);
                return;
            }

            if (message.Text == "/keyboard")
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
                startLine = 0;
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await PrintCatalog(botClient, message, BookList.bookList, userId);
                return;
            }

            if (message.Text == "Forward")
            {
                startLine += step;
                if (startLine>=userBookList.Count)
                {
                    startLine=userBookList.Count;
                    await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли конца списка!</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                    return;
                }
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                List<BookInfo> newUserBookList = new List<BookInfo>();
                for (int i = 0; i < userBookList.Count; i++)
                {
                    newUserBookList.Add(userBookList[i]);

                }
                await PrintCatalog(botClient, message, newUserBookList, userId);
                return;
            }

            if (message.Document != null)
            {
                var document = message.Document;
                try
                {
                    var file = await botClient.GetFileAsync(document.FileId);
                    string fP = Path.Combine(FileHandler.pathDirectory, document.FileName);
                    using (var fs = new FileStream(fP, FileMode.Create))
                    {
                        await botClient.DownloadFileAsync(file.FilePath, fs);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error downloading: " + ex.Message);
                }

                FileHandler.Update();

                List<BookInfo> outputInfo = new List<BookInfo>();
                for (int i = 0; i < BookList.bookList.Count; i++)
                {
                    if (BookList.bookList[i].FileName == document.FileName)
                    {
                        outputInfo.Add(BookList.bookList[i]);
                    }
                }
                bookId = 0;
                startLine = 0;
                userInfo[1] = bookId;
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await botClient.SendTextMessageAsync(message.Chat.Id, $"<strong>В хранилище успешно загружена книга:</strong>  <i>{document.FileName}</i>.", ParseMode.Html);
                await PrintCatalog(botClient, message, outputInfo, userId);
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
            string userResponse = userInfo[0].ToString();
            int bookId = Convert.ToInt32(userInfo[1]);
            int startLine = Convert.ToInt32(userInfo[2]);
            int step = Convert.ToInt32(userInfo[3]);

            if (callbackQuery.Data.StartsWith("Filter"))
            {
                string field = "";
                userResponse = callbackQuery.Data;
                switch (userResponse)
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

                userInfo[0] = userResponse;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,$"Напишите название {field}", replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                return;
            }

            if (callbackQuery.Data.StartsWith("Sort"))
            {
                string ordBy = callbackQuery.Data;
                List<BookInfo> outputInfo = BookList.GetSortList(ordBy);
                startLine = 0;
                userInfo[2] = startLine;
                UserSettingsList.SetUserInfo(userId, userInfo);
                await PrintCatalog(botClient, callbackQuery.Message, outputInfo, userId);
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
                        string emogiClock = "\U000023F3";
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Ожидайте {emogiClock}  идёт загрузка книги");
                        await using Stream stream = System.IO.File.OpenRead(outputInfo[4]);

                        InlineKeyboardMarkup keyboard = new(new[]
                                                            {
                                                                new[]
                                                                {
                                                                    InlineKeyboardButton.WithCallbackData("Открепить", $"Unpin"),
                                                                    InlineKeyboardButton.WithCallbackData("Закрепить", $"Pin"),
                                                                },
                                                            });
                        await botClient.SendDocumentAsync(
                                                            chatId: callbackQuery.Message.Chat.Id,
                                                            document: new InputOnlineFile(content: stream, fileName: outputInfo[3]),
                                                            replyMarkup: keyboard
                                                            );
                        return;
                    }
                }
            }

            switch (callbackQuery.Data)
            {
                case "Pin":
                    await botClient.PinChatMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    break;
                case "Unpin":
                    await botClient.UnpinChatMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    break;
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
                                                        InlineKeyboardButton.WithCallbackData("\U0001F464 Автор", $"Rewrite_Author_{choiceOfUser}"),
                                                        InlineKeyboardButton.WithCallbackData("\U0001F520 Название", $"Rewrite_Title_{choiceOfUser}"),
                                                    },
                                                    new[]
                                                    {
                                                        InlineKeyboardButton.WithCallbackData("\U0001F46A Жанр", $"Rewrite_Genre_{choiceOfUser}"),
                                                        InlineKeyboardButton.WithCallbackData("\U0001F481 Тема", $"Rewrite_Keywords_{choiceOfUser}"),
                                                    },
                                                });
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "<strong>Выберите параметр для изменения:</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "<strong>Выбранная книга:\n</strong>" + outputInfo, ParseMode.Html, replyMarkup: keyboard);                
                return;
            }

            if (callbackQuery.Data.StartsWith("Rewrite"))
            {
                string field = "";
                string[] mes = callbackQuery.Data.Split("_", 3);
                string mes1=string.Join("_", mes[0], mes[1]);
                switch (mes1)
                {
                    case "Rewrite_Author":
                        field = "автора";
                        break;

                    case "Rewrite_Title":
                        field = "книги";
                        break;

                    case "Rewrite_Genre":
                        field = "жанра";
                        break;

                    case "Rewrite_Keywords":
                        field = "темы";
                        break;
                }

                int choiceOfUser = 0;
                bool isNumber = int.TryParse(mes[2], out choiceOfUser);
                if (isNumber)
                {
                    bookId = choiceOfUser;
                    userInfo[1] = bookId;
                    userInfo[0] = mes1;
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
                        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "/catalog - получить список всех книг\n" +
                                                                                            "/keyboard - открыть клавиатуру для работы с каталогом");
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
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            //await botClient.SendTextMessageAsync(botClient.Message, "Произошла ошибка в работе телеграмм бота. Обратитесь к администратору!");
        }

        /// <summary>
        /// Получить каталог книг
        /// </summary>
        /// <param name="botClient">Экземпляр бота</param>
        /// <param name="message">Сообщение</param>
        /// <param name="bookList">Список книг</param>
        /// <param name="UserId">ИД пользователя</param>
        /// <returns></returns>
        public static async Task PrintCatalog (ITelegramBotClient botClient, Message message, List<BookInfo> bookList, long UserId)
        {
            long userId = UserId;
            object[] userInfo = UserSettingsList.GetUserInfo(userId);
            int startLine = Convert.ToInt32(userInfo[2]);
            int step = Convert.ToInt32(userInfo[3]);
            List<BookInfo> userBookList = UserSettingsList.GetUserBookList(userId);
            string text = "";
            string outputInfo = "";

            int count = bookList.Count;
            int finLine= startLine+step;
            if (finLine > count) { finLine = count; }
            text = $"<strong>Список книг номера ({startLine + 1}-{finLine}) из {count}:</strong>";

            if (count > 0)
            {
                if (count > 1)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Html);
                }

                for (int i = startLine; i < finLine; i++)
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
                if (finLine == count)
                {
                    if (count != 1)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "<strong>Вы достигли конца списка!</strong>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
                    }                   
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/catalog - получить список всех книг\n" +
                                      "/keyboard - открыть клавиатуру для работы с каталогом");
                }
                else
                {
                    if (count > step)
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
                text = "<strong>Список файлов пуст. \U0001F625</strong>";
                await botClient.SendTextMessageAsync(message.Chat.Id, text, ParseMode.Html, replyMarkup: new ReplyKeyboardRemove()).ConfigureAwait(false);
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
