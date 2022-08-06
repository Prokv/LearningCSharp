namespace Lesson7._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyList<int> newList = new MyList<int>();
            int n = 10;
            for (int i = 0; i < n; i++)
            {
                newList.Add(i);
                Console.WriteLine(newList[i]);
            }
            Console.WriteLine("------------");
            Console.WriteLine("Третий элемент поменяли на 4");
            newList[3] = 4;
            newList.Print();
            Console.WriteLine("------------");
            Console.WriteLine("Удалили все элементы равные 4");
            newList.DelName(4); 
            newList.Print();
            Console.WriteLine($"Количество элементов в массиве: {newList.Count}");
            Console.WriteLine("------------");
            Console.WriteLine("Удалили элемент с индексом 4");
            newList.DelIndex(4);
            newList.Print();
            Console.WriteLine($"Количество элементов в массиве: {newList.Count}");
            Console.WriteLine("------------");
            Console.WriteLine("Удалили элементы в массиве с индексами 3,7,5");
            int[] nums = {3,7,5};
            newList.DelIndex(nums);
            newList.Print();
            Console.WriteLine($"Количество элементов в массиве: {newList.Count}");
        }
    }
}