using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;
using Newtonsoft.Json;

namespace Final_Project
{
    internal class BooksList
    {
        public static List<PdfMetaData> bookList = new List<PdfMetaData>();

        private static BooksList? instance;

        public static BooksList getInstance ()
        {
            if (instance == null)
                instance = new BooksList();
            return instance;

        }
        public BooksList ()
        {
         bookList=new List<PdfMetaData>();
        }

        public static void AddNewBookMetaInfo(string FileName, string PathFile)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(PathFile);

            int newId = bookList.Count + 1;
            string newAuthor = fInfo.Author;
            string newTitle = fInfo.Title;
            string newKeywords = fInfo.Keywords;
            string newGenre = fInfo.GetMetaInfo("Genre");

            if (String.IsNullOrEmpty(newAuthor))
            {
                newAuthor = "Не определено";
            }

            if (String.IsNullOrEmpty(newTitle))
            {
                newTitle = "Не определено";
            }

            if (String.IsNullOrEmpty(newKeywords))
            {
                newKeywords = "Не определено";
            }

            if (String.IsNullOrEmpty(newGenre))
            {
                newGenre = "Не определено";
            }

            PdfMetaData newMetaData = new PdfMetaData(newId, newAuthor, newTitle, newKeywords, newGenre, FileName, PathFile);
            bookList.Add(newMetaData);
        }

        public static List<PdfMetaData> SetAuthorInList (int Id, string Author)
        {
            List<PdfMetaData> outputInfo = new List<PdfMetaData>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Author = Author;
                    PdfHandler.SetAuthorInFile(bookList[i].PathFile, Author);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        public static List<PdfMetaData> SetTitleInList(int Id, string Title)
        {
            List<PdfMetaData> outputInfo = new List<PdfMetaData>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Title = Title;
                    PdfHandler.SetTitleInFile(bookList[i].PathFile, Title);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        public static List<PdfMetaData> SetKeywordsInList(int Id, string Keywords)
        {
            List<PdfMetaData> outputInfo = new List<PdfMetaData>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Keywords = Keywords;
                    PdfHandler.SetKeywordsInFile(bookList[i].PathFile, Keywords);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        public static List<PdfMetaData> SetGenreInList(int Id, string Genre)
        {
            List<PdfMetaData> outputInfo = new List<PdfMetaData>();
            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Genre = Genre;
                    PdfHandler.SetGenreInFile(bookList[i].PathFile, Genre);
                    outputInfo.Add(bookList[i]);
                    break;
                }
            }
            return outputInfo;
        }

        public static void ClearListData()
        {
            bookList.Clear();
        }

        public static string OutputById(int Id)
        {
            string outputInfo="Значение не найдено";

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                   outputInfo = $"Автор: {bookList[i].Author}\n" +
                                $"Название: {bookList[i].Title}\n" +
                                $"Жанр: {bookList[i].Genre}\n" +
                                $"Название файла: {bookList[i].FileName}";
                    break;
                }
            }
            return outputInfo;
        }

        public static string[] GetFileName (int Id)
        {
            string[] outputInfo = new string[2];
            outputInfo[0]="Файл не найден";

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    outputInfo[0] = bookList[i].FileName;
                    outputInfo[1] = bookList[i].PathFile;
                    break;
                }
            }
            return outputInfo;
        }

        public static List<PdfMetaData> FilterList(string Field, string Name)
        {
            List <PdfMetaData> outputInfo = new List<PdfMetaData>();
            string field = Field;
            string name=Name;

            switch (field)
            {
                case "Author":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Author.Contains(name))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "автору";
                    break;

                case "Title":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Title.Contains(name))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "названию";
                    break;

                case "Genre":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Genre.Contains(name))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "жанру";
                    break;

                case "Keywords":
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        if (bookList[i].Keywords.Contains(name))
                        {
                            outputInfo.Add(bookList[i]);
                        }
                    }
                    field = "теме";
                    break;

            }
            return outputInfo;
        }
        public static List<PdfMetaData> SortList(string Trend)
        {
            List<PdfMetaData> outputInfo = new List<PdfMetaData>();
            string trend = Trend;
            switch (trend)
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
