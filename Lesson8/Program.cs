namespace Lesson8
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите текст:");
            try
            {
                string line = Console.ReadLine();
                if (line == "")
                {
                    throw new NewException("Ввод пустой строки недопустим");
                }
            }
            catch (NewException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally { Console.WriteLine("Конец программы"); }

        }
    }
}