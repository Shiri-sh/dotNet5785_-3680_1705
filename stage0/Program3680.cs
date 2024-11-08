
using System;
namespace stage0 {
    partial class program 
    {
        static void Main(string[] args)
        {
            Welcome3680();
            Welcome1705();
            Console.ReadKey();
        }

        private static void welcom3680()
        {
            Console.Write("enter your name: ");
            string username = Console.ReadLine();
            /*----------------------------------------------------------------------*/
            Console.WriteLine(username + " welcom to my first console application");
        }
        static partial void welcom1705();
     
 
    }
}
