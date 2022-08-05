using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8
{
    internal class NewException : Exception
    {   
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="message">Текст сообщения ошибки</param>
        public NewException(string message) : base(message) { }
    }
}
