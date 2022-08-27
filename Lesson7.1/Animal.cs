using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson7._1
{
    internal abstract class Animal
    {

        private string? name;

        /// <summary>
        /// Название
        /// </summary>
        public string ? Name { get { return name; } }

        private int weight;

        /// <summary>
        /// Вес
        /// </summary>
        public int? Weight { get { return weight; } }

        private int age;

        /// <summary>
        /// Возраст
        /// </summary>
        public int? Age { get { return age; } }

        /// <summary>
        /// Конструктор
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
        /// Конструктор
        /// </summary>
        /// <param name="name">Название животного (вид)</param>
        /// <param name="weight">Вес животного</param>
        public Animal(string name, int weight)
        {
            this.name = name;
            this.weight = weight;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название животного (вид)</param>
        public Animal(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// Издать голос
        /// </summary>
        public virtual void MakeVoice()
        {
            Console.WriteLine("P-р-р-р-р-р");
        }
    }
}
