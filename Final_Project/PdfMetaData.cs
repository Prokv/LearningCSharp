using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    internal class PdfMetaData
    { 
        /// <summary>
        /// Номер книги в каталоге
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Автор произведения
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Название произведения
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Краткое содержание произведения
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// Жанр произведения
        /// </summary>
        public string Genre { get; set; }
        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string PathFile { get; set; } 


        public PdfMetaData (int Id, string Author, string Title, string Keywords, string Genre, string FileName, string PathFile)
        {
            this.Id = Id;
            this.Author = Author;
            this.Title = Title;
            this.Keywords = Keywords;                
            this.Genre = Genre;
            this.FileName = FileName;
            this.PathFile = PathFile;

        }

    }
}
