using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson7._1
{
    internal class Pet : Animal
    {
        private string? alias;
        /// <summary>
        /// Кличка животного
        /// </summary>
        public string Alias { get { return alias; } set { alias = value; } }
        /// <summary>
        /// Конструктор класса Домашние животные
        /// </summary>
        /// <param name="alias">Кличка животного</param>
        /// <param name="name">Название животного (вид)</param>
        public Pet(string alias, string name) : base(name)
        {
            this.Alias = alias;
        }
        public override void MakeVoice()
        {
            Console.WriteLine("Мяу-мяу");
        }
    }
}
