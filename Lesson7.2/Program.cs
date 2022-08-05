namespace Lesson7._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyList<int> list1 = new MyList<int>();
            int n = 6;
            for (int i = 0; i < n; i++)
            {
                list1.Add(i);
                Console.WriteLine(list1[i]);
            }
            list1[3] = 4;
            Console.WriteLine("------------");
            Console.WriteLine(list1[3]); //получаем элемент по индексу
            Console.WriteLine("------------");
            Console.WriteLine(list1.Count); //получаем количество элементов в массиве

            list1.Del(4);
            Console.WriteLine("------------");
            Console.WriteLine(list1[4]);
            Console.WriteLine("------------");
            Console.WriteLine(list1.Count);
            Console.WriteLine("------------");
            for (int i = 0; i < list1.Count; i++)
            {
                Console.WriteLine(list1[i]);
            }
        }
    }
}