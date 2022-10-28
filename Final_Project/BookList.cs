using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;
using Newtonsoft.Json;

namespace Final_Project
{
    public class BookList
    {
        /// <summary>
        /// Список книг (каталог книг)
        /// </summary>
        public static List<BookInfo> bookList = new List<BookInfo>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        private static BookList? instance;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <returns></returns>
        public static BookList getInstance ()
        {
            if (instance == null)
                instance = new BookList();
            return instance;

        }
        public BookList ()
        {
         bookList=new List<BookInfo>();
        }

        /// <summary>
        /// Добавление информации о книге в список
        /// </summary>
        /// <param name="FileName">Название файла</param>
        /// <param name="PathFile">Полный путь файла</param>
        public static void AddBookMetaInfo(string FileName, string PathFile)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(PathFile);

            int Id = bookList.Count + 1;
            string Author = fInfo.Author;
            string Title = fInfo.Title;
            string Keywords = fInfo.Keywords;
            string Genre = fInfo.GetMetaInfo("Genre");

            if (String.IsNullOrEmpty(Author))
            {
                Author = "Не определено";
            }

            if (String.IsNullOrEmpty(Title))
            {
                Title = "Не определено";
            }

            if (String.IsNullOrEmpty(Keywords))
            {
                Keywords = "Не определено";
            }

            if (String.IsNullOrEmpty(Genre))
            {
                Genre = "Не определено";
            }

            BookInfo bookInfo = new BookInfo(Id, Author, Title, Keywords, Genre, FileName, PathFile);
            bookList.Add(bookInfo);
        }

        /// <summary>
        /// Установить автора
        /// </summary>
        /// <param name="Id">Номер книги в каталоге</param>
        /// <param name="Author">Имя автора</param>
        /// <returns></returns>
        public static List<BookInfo> SetAuthor (int Id, string Author)
        {
            List<BookInfo> outputInfo = new List<BookInfo>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Author = Author;
                    FileHandler.SetAuthorInFile(bookList[i].PathFile, Author);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Установить название книги
        /// </summary>
        /// <param name="Id">Номер книги в каталоге</param>
        /// <param name="Title">Название книги</param>
        /// <returns></returns>
        public static List<BookInfo> SetTitle(int Id, string Title)
        {
            List<BookInfo> outputInfo = new List<BookInfo>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Title = Title;
                    FileHandler.SetTitleInFile(bookList[i].PathFile, Title);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Установить тему книги
        /// </summary>
        /// <param name="Id">Номер книги в каталоге</param>
        /// <param name="Keywords">Тема</param>
        /// <returns></returns>
        public static List<BookInfo> SetKeywords(int Id, string Keywords)
        {
            List<BookInfo> outputInfo = new List<BookInfo>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Keywords = Keywords;
                    FileHandler.SetKeywordsInFile(bookList[i].PathFile, Keywords);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Установить жанр книги
        /// </summary>
        /// <param name="Id">Номер книги в каталоге</param>
        /// <param name="Genre">Жанр</param>
        /// <returns></returns>
        public static List<BookInfo> SetGenre(int Id, string Genre)
        {
            List<BookInfo> outputInfo = new List<BookInfo>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Genre = Genre;
                    FileHandler.SetGenreInFile(bookList[i].PathFile, Genre);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Очистить список
        /// </summary>
        public static void ClearList()
        {
            bookList.Clear();
        }

        /// <summary>
        /// Получить информацию о книге
        /// </summary>
        /// <param name="Id">Номер книги в каталоге</param>
        /// <returns></returns>
        public static string[] GetBookInfo(int Id)
        {
            string[] outputInfo = new string[6];
            outputInfo[0] ="Значение не найдено";

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    outputInfo[0] = bookList[i].Author;
                    outputInfo[1] = bookList[i].Title;
                    outputInfo[2] = bookList[i].Genre;
                    outputInfo[3] = bookList[i].Keywords;
                    outputInfo[3] = bookList[i].FileName;
                    outputInfo[4] = bookList[i].PathFile;
                    break;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Получить информацию о книге в виде строки
        /// </summary>
        /// <param name="Id">Номер книги в каталоге</param>
        /// <returns></returns>
        public static string GetBookInfoToString (int Id)
        {
            string outputInfo= "Значение не найдено";

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    outputInfo = $"<u>Автор</u>: {bookList[i].Author}\n" +
                                 $"<u>Название</u>: {bookList[i].Title}\n" +
                                 $"<u>Жанр</u>: {bookList[i].Genre}\n" +
                                 $"<u>Тема</u>: {bookList[i].Keywords}\n" +
                                 $"<u>Название файла</u>: {bookList[i].FileName}";
                    break;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Получить отфильтрованный список книг
        /// </summary>
        /// <param name="FilterBy">Название поля по которому выполняется фильтрация</param>
        /// <param name="Value">Значение по которому выполняется фильтрация</param>
        /// <returns></returns>
        public static List<BookInfo> GetFilterList(string FilterBy, string Value)
        {
            List <BookInfo> outputInfo = new List<BookInfo>();
            string field = FilterBy;
            string value=Value;

            switch (field)
            {
                case "Author":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Author.Contains(value))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "автору";
                    break;

                case "Title":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Title.Contains(value))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "названию";
                    break;

                case "Genre":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Genre.Contains(value))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "жанру";
                    break;

                case "Keywords":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Keywords.Contains(value))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "теме";
                    break;

            }
            return outputInfo;
        }

        /// <summary>
        /// Получить отсортированный список
        /// </summary>
        /// <param name="OrderBy">Поле и направление сортировки списка</param>
        /// <returns></returns>
        public static List<BookInfo> GetSortList(string OrderBy)
        {
            List<BookInfo> outputInfo = new List<BookInfo>();
            string ordBy = OrderBy;
            switch (ordBy)
            {
                case "Sort_Up_Author":
                    outputInfo=bookList.OrderBy(x => x.Author).ToList();
                    break;

                case "Sort_Down_Author":
                    outputInfo = bookList.OrderByDescending(x => x.Author).ToList();
                    break;
                case "Sort_Up_Genre":
                    outputInfo = bookList.OrderBy(x => x.Genre).ToList();
                    break;
                case "Sort_Down_Genre":
                    outputInfo = bookList.OrderByDescending(x => x.Genre).ToList();
                    break;
                case "Sort_Up_Title":
                    outputInfo = bookList.OrderBy(x => x.Title).ToList();
                    break;
                case "Sort_Down_Title":
                    outputInfo = bookList.OrderByDescending(x => x.Title).ToList();
                    break;
            }

            return outputInfo;
        }
    }
}
