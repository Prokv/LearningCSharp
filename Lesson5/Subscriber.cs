using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lesson5
{
    internal class Subscriber
        {
        /// <summary>
        /// Имя абонента
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Номер телефона абонента
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Конструктор класса Subscriber (абонент)
        /// </summary>
        /// <param name="name">Имя абонента</param>
        /// <param name="phoneNumber">Номер телефона абонента</param>
        public Subscriber(string Name, string PhoneNumber)
        {
            this.Name = Name;
            this.PhoneNumber = PhoneNumber;
        }
    }
}
