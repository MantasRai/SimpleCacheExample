using System;
using System.Configuration;
using System.Threading;
using CacheSimpleExample.Controllers;
using CacheSimpleExample.Interfaces;
using CacheSimpleExample.Repository;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace CacheSimpleExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            var connectionString = ConfigurationManager.ConnectionStrings["local"].ConnectionString;

            container.Register(Component.For<ICache>().ImplementedBy<Cache>());
            container.Register(Component.For<IDAL>().ImplementedBy<DAL>().DependsOn(Dependency.OnValue("connectionString", connectionString)));

            var cache = container.Resolve<ICache>();

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