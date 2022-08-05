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
        public int j = 0;

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        /// <param name="List">Имя списка</param>
        public void Add (T List)
        {
            count++;
            Array.Resize(ref this.List, count);
            this.List[j] = List;
            j++;
        }
        /// <summary>
        /// Очистка списка значений, сброс всех счетчиков
        /// </summary>
        public void Clear ()
        {
            count = 0;
            j = 0;
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
        public void Del (T name)
        {
            int identic=0;
            j = 0;
            for (int i = 0; i < count; i++)
            {
                if (Equals(List[i], name)) { identic++; }
                else
                {
                    List[j]=List[i];
                    j++;
                }
            }
            count=count-identic;
            Array.Resize(ref this.List, count);
        }
    }
}
