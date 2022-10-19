using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf;

namespace Final_Project
{
    internal class PdfHandler
    {
        public static string pathDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Books");

        public static DirectoryInfo dirName = new DirectoryInfo(pathDirectory);
        internal PdfHandler()
        {         
            if (!dirName.Exists)
            {
                dirName.Create();
                Console.WriteLine("Каталог \"{0}\" создан.", pathDirectory);
            }
            else
            {
                Console.WriteLine("Файл \"{0}\" найден.", pathDirectory);
            }

            BooksList booksList = new BooksList();
            Update();
        }

        public static void Update()
        {
            BooksList.ClearListData();
            FileInfo[] files = dirName.GetFiles("*.pdf");
            Console.WriteLine("Файлы:");
            foreach (var file in files)
            {
                BooksList.AddNewBookMetaInfo(file.Name, file.FullName);
            }
            for (int i = 0; i < BooksList.bookList.Count; i++)
            {
                string fileInfo = BooksList.OutputById(BooksList.bookList[i].Id);
                Console.WriteLine(fileInfo);
            }
        }

        public static void SetAuthorInFile (string FileFullName, string Author)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(FileFullName);
            fInfo.Author = Author;
            fInfo.SaveNewInfo(FileFullName);
        }

        public static void SetTitleInFile(string FileFullName, string Title)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(FileFullName);
            fInfo.Title = Title;
            fInfo.SaveNewInfo(FileFullName);
        }

        public static void SetKeywordsInFile(string FileFullName, string Keywords)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(FileFullName);
            fInfo.Keywords = Keywords;
            fInfo.SaveNewInfo(FileFullName);
        }

        public static void SetGenreInFile(string FileFullName, string Genre)
        {
            Aspose.Pdf.Facades.PdfFileInfo fInfo = new Aspose.Pdf.Facades.PdfFileInfo(FileFullName);
            string genre = Genre;
            fInfo.SetMetaInfo("Genre", genre);
            fInfo.SaveNewInfo(FileFullName);
        }

        public static void DeleteFile (string Path)
        {
            File.Delete(Path);
        }
    }
}
