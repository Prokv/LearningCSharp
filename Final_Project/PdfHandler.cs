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
        internal PdfHandler()
        {
          
            DirectoryInfo dirName = new DirectoryInfo(pathDirectory);
            if (!dirName.Exists)
            {
                dirName.Create();
                Console.WriteLine("Каталог \"{0}\" создан.", pathDirectory);
            }
            else
            {
                Console.WriteLine("Файл \"{0}\" найден.", pathDirectory);
            }

            List<PdfMetaData> bookList = new List<PdfMetaData>();
            BooksList booksList = new BooksList(bookList);

            FileInfo[] files = dirName.GetFiles("*.pdf");
            Console.WriteLine("Файлы:");
            foreach (var file in files)
            {
                //Console.WriteLine(file.Name);
                booksList.AddNewBookMetaInfo(file.Name, file.FullName);
            }
            string outputInfo=booksList.OutputAll();
            Console.WriteLine(outputInfo);
        }
    }
}
