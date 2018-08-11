using System;
using System.Threading;
using CacheSimpleExample.Controllers;
using CacheSimpleExample.Interfaces;

namespace CacheSimpleExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            ICache cache = new Cache();

            var date = cache.GetCachedData();
            Console.WriteLine(date);

            Console.WriteLine("waiting 1 sec");
            Thread.Sleep(1000);
            date = cache.GetCachedData();
            Console.WriteLine(date);

            Console.WriteLine("waiting 3 sec");
            Thread.Sleep(3000);
            date = cache.GetCachedData();
            Console.WriteLine(date);

            Console.ReadKey();
        }
    }
}