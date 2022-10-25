using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    internal class UserInfo
    {
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Ответ пользователя на запрос
        /// </summary>
        public string UserResponse { get; set; }

        /// <summary>
        /// Номер книги в каталоге
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Начальная строка с которой выводится список
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Шаг для вывода списка книг
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Пользовательский список книг (в результате фильтрации или иного отбора)
        /// </summary>
        public List<BookInfo> UserBookList = new List<BookInfo>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <param name="UserResponse">Ответ пользователя</param>
        /// <param name="BookId">Номер книги в каталоге</param>
        /// <param name="StartLine">Начальная строка с которой выводится список</param>
        /// <param name="Step">Шаг для вывода книг</param>
        /// <param name="UserBookList">Пользовательский список книг</param>
        public UserInfo (long Id, string UserResponse, int BookId, int StartLine, int Step, List<BookInfo> UserBookList)
        {
            this.Id = Id;
            this.UserResponse = UserResponse;
            this.BookId = BookId;
            this.StartLine = StartLine;
            this.Step = Step;
            this.UserBookList = UserBookList;
        }
    }


}

