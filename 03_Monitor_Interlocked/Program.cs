using System;
using System.Threading;

internal class Program
{
    private static void Main(string[] args)
    {
        FibonacciCounter counter = new FibonacciCounter();
        Thread[] threads = new Thread[] {
            new Thread(counter.CalculateFibonacci),
            new Thread(counter.CalculateFibonacci),
            new Thread(counter.CalculateFibonacci),
        };


        Console.WriteLine("Press any key to start the threads...");
        Console.ReadKey();
        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("All threads have completed.");
    }
}

public class FibonacciCounter
{
    private int n1 = 0;
    private int n2 = 1;
    private int evenCount = 0;
    private readonly object lockObject = new object();

    public void CalculateFibonacci()
    {
        for (int i = 0; i < 10; i++)
        {
            lock (lockObject)
            {
                int next = n1 + n2;
                n1 = n2;
                n2 = next;

                if (next % 2 == 0)
                {
                    evenCount++;
                }

                Console.WriteLine($"Iteration {i + 1}: n1 = {n1}, n2 = {n2}, evenCount = {evenCount}");
            }
            Thread.Sleep(100);
        }
    }
}