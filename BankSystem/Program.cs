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
            account.name = ReadCredentials();
            Console.Write("Please enter your last name: ");
            account.surname = ReadCredentials();
            Console.Write("Please enter your birth date dd-mm-yyyy: ");
            account.year = ReadDate();
            account.id = RandomNumber(10000, 100000);
            account.pass = RandomNumber(100000, 1000000);
            Console.WriteLine("Your id is: {0}", account.id);
            Console.WriteLine("Your password is: {0}", account.pass);
            Console.WriteLine("Please save this information!");
        }

        static void Login(List<BankAccount> accounts)
        {
            BankAccount account = new BankAccount();
            Console.Write("Please enter your id: ");
            int id = ReadLoginInfo("id");
            Console.Write("Please enter your password: ");
            int password = ReadLoginInfo("pass");
            foreach (BankAccount c in accounts)
            {
                if ((c.id == id) && (c.pass == password))
                {
                    account = c;
                    Console.WriteLine("Logged in successfully!");
                    SystemTray(account, accounts);
                }
            }
            
        }

        static void SystemTray(BankAccount account, List<BankAccount> accounts)
        {
            Console.WriteLine("Welcome back " + account.name + "! Avialable options: ");
            Console.WriteLine("1. Transfer money to other account");
            Console.WriteLine("2. Show your credentials");
            Console.Write("3. Delete account \n What would you like to do?");
            string i = Console.ReadLine();
            switch (int.Parse(i))
            {
                case 1:
                    Console.Write("To what ID do you want to send the money to? ");
                    int id = ReadLoginInfo("id");
                    Console.Write("How much money do you want to transfer? ");
                    int money = int.Parse(Console.ReadLine());
                    var found = accounts.FirstOrDefault(c => c.id == id);
                    account.money = account.money - money;                    
                    found.money = found.money + money;
                    //Write accounts id, verify if real
                    //Choose amount to send, verify if you have it
                    //Clone object from list, clone.money = normal.money
                    break;
                case 2:
                    Console.Write("Name: {0}", account.name);
                    Console.Write("Surname: {0}", account.surname);
                    Console.Write("Birth date: {0}", account.year);
                    Console.Write("Money: {0}", account.money);
                    break;
                case 3:
                    if ((PermissionTypes.Delete & account.permissions) == PermissionTypes.Delete)
                    {
                        Console.WriteLine("Do you really want to delete this account? Y/N");
                        string temp = Console.ReadLine();
                        switch (temp)
                        {
                            case "Y":
                                account = null;
                                break;
                        }
                    }
                        break;
            }
            bool canDelete = ((PermissionTypes.Delete & account.permissions) == PermissionTypes.Delete);
            
        }

        static string ReadCredentials()
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
                    Console.WriteLine("Wrong input!");
                }
            }
            return name;
        }

        static string ReadDate()
        {
            //Reikia pabaigti
            string date = Console.ReadLine();
            return date;
        }

        static int ReadLoginInfo(string type)
        {
            int id = 0;
            bool valid = false;
            int length = 0;
            switch (type)
            {
                case "id":
                    length = 5;
                    break;
                case "pass":
                    length = 6;
                    break;
                default:

                    break;
            }
            while (!valid)
            {
                string temp = Console.ReadLine();
                if (!string.IsNullOrEmpty(temp) && System.Text.RegularExpressions.Regex.IsMatch(temp, "^[0-9]*$"))
                {
                    id = int.Parse(temp);
                    if (temp.Length == length)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong input!");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input!");
                }
            }
            return id;
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
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5} ", words[0], words[1], words[2], words[3], words[4], words[5]);
                    BankAccount account = new BankAccount();
                    account.name = words[0];
                    account.surname = words[1];
                    account.year = words[2];
                    account.id = int.Parse(words[3]);
                    account.pass = int.Parse(words[4]);
                    account.money = Convert.ToDouble(words[5]);
                    account.permissions = PermissionTypes.Read | PermissionTypes.Write;
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
            foreach (BankAccount c in accounts)
            {
                Console.WriteLine(c.name);
            }
            Console.WriteLine("Welcome to Unsecured Bank system! Choose what you want to do:");
            Console.WriteLine("1. Register new account.");
            Console.WriteLine("2. Login with existing account.");
            while (true)
            {
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        BankAccount account = new BankAccount();
                        Registration(account);
                        accounts.Add(account);
                        break;
                    case 2:
                        Login(accounts);
                        break;
                    default:

                        break;
                }
            }
            Console.ReadLine();
        }
    }

}

//Sukurti registration forma +
//Sukurti login forma su -
