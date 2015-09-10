using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Unsecured Bank system! Choose what you want to do:");
            Console.WriteLine("1. Register new account.");
            Console.WriteLine("2. Login with existing account.");
            int i = Console.Read();
            switch (i)
            {
                case 1:
                    //Registration();
                    break;
                case 2:
                    //Login();
                    break;
            }
        }
    }
}

//Sukurti registration forma
//Sukurti login forma su 
