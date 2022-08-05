using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson7._1
{
    internal class Animal
    {
        /// <summary>
        /// Название животного.
        /// </summary>
        public string? name;
        /// <summary>
        /// Вес животного.
        /// </summary>
        public int weight;
        /// <summary>
        /// Возраст животного.
        /// </summary>
        public int age;
        /// <summary>
        /// Конструктор класса Животные
        /// </summary>
        /// <param name="name">Название животного (вид)</param>
        /// <param name="weight">Вес животного</param>
        /// <param name="age">Возраст животного</param>
        public Animal(string name, int weight, int age)
        {
            this.name = name;
            this.weight = weight;
            this.age = age;
        }
        /// <summary>
        /// Конструктор класса Животные
        /// </summary>
        /// <param name="name">Название животного (вид)</param>
        /// <param name="weight">Вес животного</param>
        public Animal(string name, int weight)
        {
            this.name = name;
            this.weight = weight;
        }
        /// <summary>
        /// Конструктор класса Животные
        /// </summary>
        /// <param name="name">Название животного (вид)</param>
        public Animal(string name)
        {
            this.name = name;
        }

        public virtual void Voice()
        {
            Console.WriteLine("P-р-р-р-р-р");
        }
    }
}
