using System;
using System.Collections.Generic;
using System.Threading;

namespace Bar_simulator
{
    class Program
    {
        static List<Visitors> listCustomers = new List<Visitors>();
        static List<Drinks> listDrinks = new List<Drinks>();

        static bool isOpen;
        static int orderNumber = -1;
        static int maxVisitosCount;
        static int anger;
        static string[] products = { "Bloody Mary", "Blue Lagoon", "Serena", "Blue Hawaii" };

        static Semaphore _pool;

        static Random rand;
        static void Main(string[] args)
        {
            maxVisitosCount = 5;

            _pool = new Semaphore(0, maxVisitosCount);
            rand = new Random();
            isOpen = true;

            Console.Write("Customers count:");
            string n = Console.ReadLine();

            for (int i = 0; i < int.Parse(n); i++)
            {
                listCustomers.Add(new Visitors(rand.Next(20) + 10, 0, rand.Next(500) + 10));
            }

            for (int i = 0; i < products.Length; i++)
            {
                double sale = rand.Next(30);
                listDrinks.Add(new Drinks(products[rand.Next(4)], rand.Next(50) + 50 - sale, rand.Next(20) + 1, sale));
            }


            foreach (var item in listDrinks)
            {
                Console.WriteLine("{0}, Price:{1}, Count:{2}, Sale:{3}", item.name, item.price, item.count, item.sale);
            }

            Thread barThread = new Thread(Bar);
            barThread.Start();

            foreach (var item in listCustomers)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Customers));

                t.Start(item);
            }
            Thread.Sleep(1000);


            _pool.Release(maxVisitosCount);
        }

        private static void Bar()
        {
            double temp;
            while (isOpen)
            {
                temp = rand.Next(20);

                if (temp == 5)
                    isOpen = false;
                
                Thread.Sleep(3000);
                
                anger++;
            }

            Console.WriteLine("The Bar is closed!");
            _pool.Release();
        }

        private static void Customers(object obj)
        {
            Visitors visitors = (Visitors)obj;

            Console.WriteLine("Visitor: {3}, Age:{0}, Anger:{1}, Budget:{2} entered the queue!", visitors.age, visitors.anger, visitors.budget, listCustomers.IndexOf(visitors));

            _pool.WaitOne();

            visitors.anger = anger;
            anger = 0;

            if (!isOpen)
            {
                Console.WriteLine("The Bar is closed!");

                return;
            }

            if (visitors.age <= 18)
            {
                _pool.Release();
                return;
            }

            listCustomers.Find(p => p == visitors).anger = visitors.anger;

            if (visitors.anger > 0)
            {
                Console.WriteLine("Visitor: {3}, Age:{0}, Anger:{1}, Budget:{2} LEFT the queue!", visitors.age, visitors.anger, visitors.budget, listCustomers.IndexOf(visitors));
                _pool.Release();
                return;
            }

            Console.WriteLine("Visitor: {3}, Age:{0}, Anger:{1}, Budget:{2} entered the Bar!", visitors.age, visitors.anger, visitors.budget, listCustomers.IndexOf(visitors));

            Thread.Sleep(3000);
            orderNumber = rand.Next(3);

            if (listDrinks[orderNumber].count > 0 && visitors.budget > listDrinks[orderNumber].price)
            {
                listDrinks[orderNumber].count--;
                Console.WriteLine("Visitor {0} now have {1}", listCustomers.IndexOf(visitors), listCustomers.Find(p => p == visitors).budget -= listDrinks[orderNumber].price);
            }

            Console.WriteLine("Visitor {0} left the Bar!", listCustomers.IndexOf(visitors));
            
            _pool.Release(); 

        }
    }
}
