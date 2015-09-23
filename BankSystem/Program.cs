﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    [Flags]
    enum PermissionTypes : int
    {
        None = 0,
        Read = 1,
        Write = 2,
        Modify = 4,
        Delete = 8,
        Create = 16,
        All = Read | Write | Modify | Delete | Create
    }
   
    class Program
    {
        static void Registration(BankAccount account)
        {
            Console.Write("Please enter your first name: ");
            account.name = GetCredentials();
            Console.Write("Please enter your last name: ");
            account.surname = GetCredentials();
            Console.Write("Please enter your birth date dd-mm-yyyy: ");
            account.year = getDate();
            account.id = RandomNumber(10000, 100000);
            account.pass = RandomNumber(100000, 1000000);
            Console.WriteLine("Your id is: {0}", account.id);
            Console.WriteLine("Your password is: {0}", account.pass);
            Console.WriteLine("Goodbye!");
        }

        static void Login(List<BankAccount> accounts)
        {
            Console.Write("Please enter your id: ");
            string id = Console.ReadLine();
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine();
            if (VerifyAccount(id, password, accounts) == true)
            {
                Console.Write("Please enter you password: ");
                string pass = Console.ReadLine();
                    SystemTray(id);
            }
            else
            {

            }
        }

        static bool VerifyAccount(string id, string password, List<BankAccount> accounts)
        {
            /*Need to start on this after reading from file is done!! 
            Important for Login!!!*/
            return true;
        }

        static void SystemTray(string id)
        {

        }
  
        static string GetCredentials()
        {
            bool valid = false;
            string name = "";
            while (!valid)
            {
                name = Console.ReadLine();
                if (System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Wrong input, only letters are allowed!");
                }
            }
            return name;
        }

        static string getDate()
        {
            string date = Console.ReadLine();
            return date;
        }

        static int RandomNumber(int i, int x)
        {
            Random rnd = new Random();
            int random = rnd.Next(i, x);
            return random;
        }

        static void GetAccountInformation(List<BankAccount> accounts)
        {
            string temp = "accounts.txt";
            if (!File.Exists(temp))
            {
                File.Create(temp);
            }
            else
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader("accounts.txt");
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    /*Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5} ", words[0], words[1], words[2], words[3], words[4], words[5]);*/
                    BankAccount account = new BankAccount();
                    account.name = words[0];
                    account.surname = words[1];
                    account.year = words[2];
                    account.id = int.Parse(words[3]);
                    account.pass = int.Parse(words[4]);
                    account.money = Convert.ToDouble(words[5]);
                    accounts.Add(account);
                }
            }
        }
        
        static void Main(string[] args)
        {


            /*BankAccount admin = new BankAccount();
            admin.permissions = PermissionTypes.Read | PermissionTypes.Write | PermissionTypes.Delete;
            bool canRead = ((PermissionTypes.Read & admin.permissions) == PermissionTypes.Read);
            Console.WriteLine(canRead);*/
            List<BankAccount> accounts = new List<BankAccount>();
            GetAccountInformation(accounts);
            foreach (var account in accounts)
            {
                Console.WriteLine(account);
            }
            Console.WriteLine("Welcome to Unsecured Bank system! Choose what you want to do:");
            Console.WriteLine("1. Register new account.");
            Console.WriteLine("2. Login with existing account.");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1: //Registration form
                    BankAccount account = new BankAccount();
                    Registration(account);
                    accounts.Add(account);         
                    break;
                case 2: //Login
                    Login(accounts);
                    break;
                default:

                    break;
            }
            Console.ReadLine();
        }
    }

}

//Sukurti registration forma +
//Sukurti login forma su -
