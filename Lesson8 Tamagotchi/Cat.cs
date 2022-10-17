using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8_Tamagotchi
{
    internal class Cat : Animal
    {
        /// <summary>
        /// Конструктор кошки.
        /// </summary>
        /// <param name="name">Имя кошки.</param>
        public Cat(string name) : base(name) { }

        /// <summary>
        /// Издать кошачий звук.
        /// </summary>
        public override void MakeSound() => Console.WriteLine("- Мяяяяу!\n");
    }
}

