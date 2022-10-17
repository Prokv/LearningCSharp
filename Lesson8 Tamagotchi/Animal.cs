using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8_Tamagotchi
{
    /// <summary>
    /// Описание абстрактного животного с его функциями
    /// </summary>
    internal abstract class Animal
    {
        /// <summary>
        /// Имя животного.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Таймер голода.
        /// </summary>
        Timer TimerToEat;

        /// <summary>
        /// таймер жажды.
        /// </summary>
        Timer TimerToDrink;

        /// <summary>
        /// Таймер скуки.
        /// </summary>
        Timer TimerToPlay;

        /// <summary>
        /// Таймер смерти от жажды.
        /// </summary>
        System.Timers.Timer TimerToDieByThirst;

        /// <summary>
        /// Таймер смерти от голода.
        /// </summary>
        System.Timers.Timer TimerToDieByHunger;

        /// <summary>
        /// Таймер смерти от скуки.
        /// </summary>
        System.Timers.Timer TimerToDieByBoredom;

        /// <summary>
        /// Конструктор животного.
        /// </summary>
        /// <param name="name">Имя животного.</param>
        public Animal(string name)
        {
            Name = name;
            MakeSound();

            TimerToEat = new Timer(WantToEat, null, 8000, 10000);
            TimerToDrink = new Timer(WantToDrink, null, 5000, 7000);
            TimerToPlay = new Timer(WantToPlay, null, 13000, 15000);

            TimerToDieByThirst = new System.Timers.Timer(10000);
            TimerToDieByThirst.Elapsed += Die;
            TimerToDieByHunger = new System.Timers.Timer(12000);
            TimerToDieByHunger.Elapsed += Die;
            TimerToDieByBoredom = new System.Timers.Timer(15000);
            TimerToDieByBoredom.Elapsed += Die;
        }

        /// <summary>
        /// Издать звук животного.
        /// </summary>
        public abstract void MakeSound();

        /// <summary>
        /// Вызвать желание есть.
        /// </summary>
        /// <param name="obj">Объект типа object.</param>
        public void WantToDrink(Object obj)
        {
            Console.WriteLine("- Я хочу пить");
            TimerToDieByThirst.Enabled = true;
        }

        /// <summary>
        /// Вызвать желание есть.
        /// </summary>
        /// <param name="obj">Объект типа object.</param>
        public void WantToEat(Object obj)
        {
            Console.WriteLine("- Я хочу есть");
            TimerToDieByHunger.Enabled = true;
        }

        /// <summary>
        /// Вызвать желание играть.
        /// </summary>
        /// <param name="obj">Объект типа object.</param>
        public void WantToPlay(Object obj)
        {
            Console.WriteLine("- Я хочу играть");
            TimerToDieByBoredom.Enabled = true;
        }

        /// <summary>
        /// Напоить животного.
        /// </summary>
        public void Drink()
        {
            TimerToDieByThirst.Enabled = false;
            Console.WriteLine("- Спасибо! Вода великолепна!");
        }

        /// <summary>
        /// Покормить животного.
        /// </summary>
        public void Eat()
        {
            TimerToDieByHunger.Enabled = false;
            Console.WriteLine("- Спасибо! Я сыт");
        }

        /// <summary>
        /// Поиграть с животным.
        /// </summary>
        public void Play()
        {
            TimerToDieByBoredom.Enabled = false;
            Console.WriteLine("- Спасибо! С тобой так весело!");
        }

        /// <summary>
        /// Вызвать смерть животного.
        /// </summary>
        /// <param name="obj">Объект типа object.</param>
        /// <param name="e">Данные события.</param>
        public void Die(Object obj, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("- Я умер! Пока");
            Environment.Exit(0);
            //TimerToEat.Change();
        }
    }
}
