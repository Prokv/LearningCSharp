namespace Lesson7._1
{
    internal class Program
    {
        static void Main()
        {
            Pet Мурка = new Pet("Мурка", "Кошка");
            Мурка.Voice();

            Animal Медведь = new Animal("Медведь");
            Медведь.Voice();
        }
    }
}