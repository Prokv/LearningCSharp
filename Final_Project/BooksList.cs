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
        List<PdfMetaData> bookList = new List<PdfMetaData>();

        public BooksList (List<PdfMetaData> bookList)
        {
            this.bookList = bookList;

        }

        public void AddNewBookMetaInfo(string FileName, string PathFile)
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

        public void SetAuthor (int Id, string Author)
        {

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Author = Author;
                }
            }
        }

        public void SetTitle(int Id, string Title)
        {

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Title = Title;
                }
            }
        }

        public void SetKeywords(int Id, string Keywords)
        {

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Keywords = Keywords;
                }
            }
        }

        public void SetGenre(int Id, string Genre)
        {

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                    bookList[i].Genre = Genre;
                }
            }
        }

        public void ClearListData()
        {
            bookList.Clear();
        }

        public string OutputAll()
        {
            string outputInfo="";

            if (bookList.Count != 0)
            {
                for (int i = 0; i < bookList.Count; i++)
                {
                    string bookInfo= OutputById(bookList[i].Id);
                    outputInfo = outputInfo+"\n" + bookInfo;
                }
            }
            else 
            {
                outputInfo = "Список файлов пуст.";
            }
            return outputInfo;
            //Console.WriteLine(outputInfo);
        }

        public string OutputById(int Id)
        {
            string outputInfo="Значение не найдено";

            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList[i].Id == Id)
                {
                   outputInfo = $"Номер в каталоге: {bookList[i].Id}\n" +
                                $"Автор: {bookList[i].Author}\n" +
                                $"Название: {bookList[i].Title}\n" +
                                $"Жанр: {bookList[i].Genre}\n" +
                                $"Название файла: {bookList[i].FileName}";
                    break;
                }
            }
            return outputInfo;
            //Console.WriteLine(outputInfo);
        }
    }
}
