using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8_Tamagotchi
{
    internal class Dog: Animal
    {
        /// <summary>
        /// Конструктор собаки.
        /// </summary>
        /// <param name="name">Имя собаки.</param>
        public Dog(string name) : base(name) { }

        /// <summary>
        /// Издать собачий звук.
        /// </summary>
        public override void MakeSound() => Console.WriteLine("- Гав-гав!\n");
    }
}
