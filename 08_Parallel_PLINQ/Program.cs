internal class Program
{
    private static void Main(string[] args)
    {
        // Task 1
        Parallel.For(1, 10, Factorial);
        Console.WriteLine(new string('-', 40));

        // Task 2
        Parallel.For(225, 240, SumNumbers);
        Console.WriteLine(new string('-', 40));
        
        // Task 3
        Parallel.For(5, 8+1, TableMult);
        Console.WriteLine(new string('-', 40));

        // Task 4
        List<int> ints = File.ReadAllText("../../../ints.txt").Split(',').Select(int.Parse).ToList();
        Parallel.ForEach(ints, Factorial);
        Console.WriteLine(new string('-', 40));

        // Task 5
        var sum = ints.AsParallel()
                      .Sum();
        var max = ints.AsParallel()
                      .Max();
        var min = ints.AsParallel()
                      .Min();

        Console.WriteLine($"Ints: {string.Join(',', ints)}\nSum: {sum} \nMax: {max} \nMin: {min}");
    }

    static void Factorial(int number)
    {
        int res = 1;
        for (int i = 1; i < number; i++)
        {
            res *= i;
        }
        Console.WriteLine($"Factorial {number} = {res}");
    }

    static void SumNumbers(int number)
    {
        int res = 0;
        string strnumb = number.ToString();
        int count = strnumb.Length;
        for (int i = 0; i < count; i++)
        {
            res += int.Parse(strnumb[i].ToString());
        }
        Console.WriteLine($"Number {number}:\nCount of digits: {count} \nSum of digits: {res}");
    }


    static void TableMult(int number)
    {
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"{number}*{i} = {number*i}");
        }
    }


}