using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;

namespace Final_Project
{
    internal class FileHandler
    {
        /// <summary>
        /// Директория хранилища книг
        /// </summary>
        public static string pathDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Books");

        /// <summary>
        /// Информация о хранилище книг
        /// </summary>
        public static DirectoryInfo dirInfo = new DirectoryInfo(pathDirectory);

        /// <summary>
        /// Конструктор управления файлами в хранилище
        /// </summary>
        internal FileHandler()
        {         
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
                Console.WriteLine("Каталог \"{0}\" создан.", pathDirectory);
            }
            else
            {
                Console.WriteLine("Файл \"{0}\" найден.", pathDirectory);
            }

            BookList booksList = new BookList();
            Update();
        }

        /// <summary>
        /// Обновление информации в каталоге
        /// </summary>
        public static void Update()
        {
            BookList.ClearList();
            FileInfo[] files = dirInfo.GetFiles("*.pdf");
            Console.WriteLine("Книги в каталоге:");
            foreach (var file in files)
            {
                BookList.AddBookMetaInfo(file.Name, file.FullName);
            }
            for (int i = 0; i < BookList.bookList.Count; i++)
            {
                string fileInfo = BookList.GetBookInfoToString(BookList.bookList[i].Id);
                Console.WriteLine(fileInfo);
            }
        }

        /// <summary>
        /// Установить название Автора в файле(метаданные)
        /// </summary>
        /// <param name="PathFile">Полный путь к файлу</param>
        /// <param name="Author">Название автора</param>
        public static void SetAuthorInFile (string PathFile, string Author)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(PathFile);
            fInfo.Author = Author;
            fInfo.SaveNewInfo(PathFile);
        }

        /// <summary>
        ///  Установить название Книги в файле(метаданные)
        /// </summary>
        /// <param name="PathFile">Полный путь к  файлу</param>
        /// <param name="Title">Название книги</param>
        public static void SetTitleInFile(string PathFile, string Title)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(PathFile);
            fInfo.Title = Title;
            fInfo.SaveNewInfo(PathFile);
        }

        /// <summary>
        ///  Установить название Темы в файле(метаданные)
        /// </summary>
        /// <param name="PathFile">Полный путь к файлу</param>
        /// <param name="Keywords">Название темы</param>
        public static void SetKeywordsInFile(string PathFile, string Keywords)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(PathFile);
            fInfo.Keywords = Keywords;
            fInfo.SaveNewInfo(PathFile);
        }

        /// <summary>
        ///  Установить название Жанра в файле(метаданные)
        /// </summary>
        /// <param name="PathFile">Полный путь к файлу</param>
        /// <param name="Genre">Название жанра</param>
        public static void SetGenreInFile(string PathFile, string Genre)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(PathFile);
            string genre = Genre;
            fInfo.SetMetaInfo("Genre", genre);
            fInfo.SaveNewInfo(PathFile);
        }

        /// <summary>
        /// Удалить файл из директории
        /// </summary>
        /// <param name="PathFile">Полный путь к файлу</param>
        public static void DeleteFile (string PathFile)
        {
            File.Delete(PathFile);
        }
    }
}
