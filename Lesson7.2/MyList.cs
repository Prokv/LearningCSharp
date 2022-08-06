using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson7._2
{
    internal class MyList <T>
    {
        /// <summary>
        /// Начальный список
        /// </summary>
        public T[] List=new T[0];

        private int count=0;
        /// <summary>
        /// Количество содержащихся элементов в массиве
        /// </summary>
        public int Count { get { return count; } }
        public int index = 0;

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        /// <param name="List">Имя списка</param>
        public void Add (T List)
        {
            count++;
            Array.Resize(ref this.List, count);
            this.List[index] = List;
            index++;
        }
        /// <summary>
        /// Очистка списка значений, сброс всех счетчиков
        /// </summary>
        public void Clear ()
        {
            count = 0;
            index = 0;
            List = new T[0];
        }

        /// <summary>
        /// Индексатор для возврата значения по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }
        /// <summary>
        /// Удаление элемента массива по его значению
        /// </summary>
        /// <param name="name">Значение элемента массива, которое требуется удалить</param>
        public void DelName (T name)
        {
            int countEqual=0;
            index = 0;
            for (int i = 0; i < count; i++)
            {
                if (Equals(List[i], name)) { countEqual++; }
                else
                {
                    List[index]=List[i];
                    index++;
                }
            }
            count=count-countEqual;
            Array.Resize(ref this.List, count);
        }
        /// <summary>
        /// Удаление элемента массива по его индексу
        /// </summary>
        /// <param name="index">Индекс элемента массива</param>
        public void DelIndex (int index)
        {
            for (int i = index; i < count; i++)
            {
                List[i]=List[i+1];
                --count;
            }
            Array.Resize(ref this.List, count);
        }
        /// <summary>
        /// Удаление элемента массива по его индексу, взятому из массива индексов
        /// </summary>
        /// <param name="index"></param>
        public void DelIndex (int [] indexMass)
        {
            for (int i = 0; i < indexMass.Length; i++)
            {
                index=indexMass[i];
                for (int j = index; j < count; j++)
                {
                    List[j] = List[j + 1];
                    --count;
                }
            }
            Array.Resize(ref this.List, count);
        }
        /// <summary>
        /// Вывод в консоль содержимого списка
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine(this.List[i]);
            }
        }
    }
}
