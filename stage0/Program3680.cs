
using System;
namespace stage0 {
    partial class Program 
    {
        static void Main(string[] args)
        {
            Welcome3680();
            Welcome1705();
            Console.ReadKey();
        }
        private static void Welcome3680()
        {
            Console.Write("enter your name: ");
            string username = Console.ReadLine();
            Console.WriteLine(username + " welcom to my first console application");
        }
        static partial void Welcome1705();


    }
}
