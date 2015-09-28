using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            bool valid = false;
            while (!valid)
            {
                Console.Write("Please enter your id: ");
                string id = ReadLoginInfo("id");
                Console.Write("Please enter your password: ");
                string password = ReadLoginInfo("pass");
                foreach (BankAccount c in accounts)
                {
                    if ((c.id == id) && (c.pass == password))
                    {
                        valid = true;
                        account = c;
                        Console.WriteLine("Logged in successfully!");
                        SystemTray(account, accounts);
                    }
                }
                Console.WriteLine("Wrong id or password!");
            }

            
        }

        static void SystemTray(BankAccount account, List<BankAccount> accounts)
        {
            Console.Clear();
            Console.WriteLine("Welcome back " + account.name + "! Avialable options: ");
            bool valid = true;
            while (valid)
            {
                Console.WriteLine("1. Transfer money to other account");
                Console.WriteLine("2. Show your credentials");
                Console.Write("3. Delete account \n What would you like to do?");
                string i = Console.ReadLine();
                switch (int.Parse(i))
                {
                    case 1:
                        Console.WriteLine("To what ID do you want to send the money to? ");
                        string id = ReadLoginInfo("id");
                        Console.WriteLine("How much money do you want to transfer? ");
                        int money = int.Parse(Console.ReadLine());
                        var found = accounts.FirstOrDefault(c => c.id == id);
                        account.money = account.money - money;
                        found.money = found.money + money;
                        //Write accounts id, verify if real
                        //Choose amount to send, verify if you have it
                        //Clone object from list, clone.money = normal.money
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Name: {0}", account.name);
                        Console.WriteLine("Surname: {0}", account.surname);
                        Console.WriteLine("Birth date: {0}", account.year);
                        Console.WriteLine("Money: {0}", account.money);
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
                                default:

                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You do not have permission to delete account!");
                            Console.Read();
                            Console.Clear();
                        }
                        break;
                    case 4:
                        valid = false;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Wrong input!");
                        Console.Read();
                        Console.Clear();
                        break;
                }
            }            
        }

        static void Exit(List<BankAccount> accounts)
        {
            StreamWriter file = new StreamWriter("accounts.txt");
            foreach (BankAccount c in accounts)
            {
                string account = c.toString();
                file.WriteLine(account);
            }
            Console.WriteLine("Good bye!");
            file.Close();
            Environment.Exit(0);
        }

        static string ReadCredentials()
        {
            bool valid = false;
            string name = "";
            while (!valid)
            {
                name = Console.ReadLine();
                if (Regex.IsMatch(name, @"^[a-zA-Z]+$"))
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
            Regex validDate = new Regex(@"\d{2}-\d{2}-\d{4}");
            bool valid = false;
            string date = null;
            while (!valid)
            {
                date = Console.ReadLine();
                if (validDate.IsMatch(date) == true)
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Wrong input! Year input is dd-mm-yyyy");
                }
            }
            return date;
        }

        static string ReadLoginInfo(string type)
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
                if (!string.IsNullOrEmpty(temp) && Regex.IsMatch(temp, "^[0-9]*$"))
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
            return id.ToString();
        }

        static string RandomNumber(int i, int x)
        {
            Random rnd = new Random();
            int random = rnd.Next(i, x);
            string temp = random.ToString();
            return temp;
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
                StreamReader file = new StreamReader("accounts.txt");
                while ((line = file.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    //Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5} ", words[0], words[1], words[2], words[3], words[4], words[5]);
                    BankAccount account = new BankAccount();
                    account.name = words[0];
                    account.surname = words[1];
                    account.year = words[2];
                    account.id = words[3];
                    account.pass = words[4];
                    account.money = Convert.ToDouble(words[5]);
                    account.permissions = PermissionTypes.Read | PermissionTypes.Write;
                    accounts.Add(account);
                }
                file.Close();
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
            Console.WriteLine("Welcome to Unsecured Bank system!");
            while (true)
            {
                Console.WriteLine("1. Register new account.");
                Console.WriteLine("2. Login with existing account.");
                Console.WriteLine("3. Exit system");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        BankAccount account = new BankAccount();
                        Registration(account);
                        accounts.Add(account);
                        break;
                    case 2:
                        Login(accounts);
                        Console.Clear();
                        break;
                    case 3:
                        Exit(accounts);
                        Console.Clear();
                        break;
                    default:
                        Console.Write("Wrong input! Please enter a number 1 or 2");
                        Console.Read();
                        Console.Clear();
                        break;
                }
            }
        }
    }

}
