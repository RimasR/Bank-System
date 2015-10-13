using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankSystem
{
    [Flags]                                                                                 //FLAGS
    public enum PermissionTypes : int
    {
        None = 0,
        Read = 1,
        Write = 2,
        Modify = 4,
        Delete = 8,
        Create = 16,
        All = Read | Write | Modify | Delete | Create
    }
   
    public class Program
    {
        static void Registration(BankAccounts accounts, List<Log> eventLog)
        {
            BankAccount account = new BankAccount();
            Console.Write("Please enter your first name: ");
            account.Name = ReadCredentials().UppercaseFirstLetter();
            Console.Write("Please enter your last name: ");
            account.Surname = ReadCredentials().UppercaseFirstLetter(); //EXTENTION METHOD
            Console.Write("Please enter your birth date dd-mm-yyyy: ");
            account.year = ReadDate();
            Random rnd = new Random();
            account.id = RandomNumber(rnd, 10000, 100000);
            account.pass = RandomNumber(rnd, 100000, 1000000);
            Console.WriteLine("Your id is: {0}", account.id);
            Console.WriteLine("Your password is: {0}", account.pass);
            account.GiveMoney(tempMoney : 100000);                                        //OPTIONAL ARGUMENT
            Console.WriteLine("Please save this information!");
            if (account.Name == "admin" || account.Name == "Admin")
            {
                GivePermissions("admin", account);
            }
            else
            {
                GivePermissions("normal", account);
            }
            accounts.Add(account);
            eventLog.Add(new Log() { id = account.id, debugTime = DateTime.Now, debug = "Registered new account" });
            Console.Read();
        }

        static void Login(BankAccounts accounts, List<Log> eventLog)
        {
            foreach (BankAccount c in accounts)
            {
                Console.WriteLine(c.id);
                Console.WriteLine(c.pass);
                Console.WriteLine("---------");
            }

            BankAccount account = new BankAccount();
            bool valid = false;
            while (!valid)
            {
                Console.Write("Please enter your id: ");
                string id = ReadLoginInfo("id");
                Console.Write("Please enter your password: ");
                string password = ReadLoginInfo("pass");
                if (accounts.Count() > 0)
                {
                    foreach (BankAccount c in accounts.ToList())
                    {
                        if ((c.id == id) && (c.pass == password))
                        {
                            valid = true;
                            account = c;
                            Console.WriteLine("Logged in successfully!");
                            eventLog.Add(new Log() { id = account.id, debugTime = DateTime.Now, debug = "Logged in" });
                            SystemTray(account, accounts, eventLog);
                        }
                    }
                }
                Console.WriteLine("Wrong id or password!");
            }

            
        }

        static void SystemTray(BankAccount account, BankAccounts accounts, List<Log> eventLog)
        {
            Console.Clear();
            Console.WriteLine("Welcome back " + account.Name + "! Available options: ");
            bool valid = true;
            while (valid)
            {
                Console.WriteLine("1. Transfer money to other account");
                Console.WriteLine("2. Show your credentials");
                Console.WriteLine("3. Delete account");
                Console.WriteLine("4.Exit \n What would you like to do?");
                switch (ReadInt())
                {
                    case 1:
                        if (account.money > 0)
                        {
                            Console.WriteLine("To what ID do you want to send the money to? ");
                            Console.WriteLine(" ----------------------------------- \n ID List for clarification:");
                            foreach (BankAccount acc in accounts)
                            {
                                Console.WriteLine(acc.id + "\n ---------");
                            }
                            string id = ReadLoginInfo("id");
                            Console.WriteLine("How much money do you want to transfer? ");
                            int tempMoney = int.Parse(Console.ReadLine());
                            double money = tempMoney;                                                                       //DATA WIDENING
                            if (money < account.money)
                            {
                                var found = accounts.FirstOrDefault(c => c.id == id);                                      //LINQ METHOD
                                if (found != null)
                                {
                                    Console.WriteLine("ID found, money transfered.");
                                    account.money = account.money - money;
                                    found.money = found.money + money;
                                    eventLog.Add(new Log() { id = account.id, debugTime = DateTime.Now, debug = "Transfered money to other account" });
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("You don't have any money to send!");
                        }
                        break;
                    case 2:
                        if (account != null)
                        {
                            Console.Clear();
                            Console.WriteLine("Name: {0}", account.Name);
                            Console.WriteLine("Surname: {0}", account.Surname);
                            Console.WriteLine("Birth date: {0}", account.year);
                            Console.WriteLine("Money: {0}", account.money);
                            eventLog.Add(new Log() { id = account.id, debugTime = DateTime.Now, debug = "Showed the credentials of an account" });
                        } else
                        {
                            Console.WriteLine("There is no such account.");
                        }
                        break;
                    case 3:
                        if ((PermissionTypes.Delete & account.permissions) == PermissionTypes.Delete)
                        {
                            Console.WriteLine("Do you really want to delete this account? Y/N");
                            string temp = Console.ReadLine();
                            switch (temp)
                            {
                                case "Y":
                                    var deleteThis = accounts.SingleOrDefault(c => c.id == account.id);
                                    if (deleteThis != null)
                                    {
                                        eventLog.Add(new Log() { id = account.id, debugTime = DateTime.Now, debug = "Deleted account" });
                                        accounts.Remove(acc : deleteThis);                  //NAMED ARGUMENT
                                        account = null;
                                        Console.WriteLine("Deleted successfully!");
                                    }
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
                        if(account != null)
                        {
                            eventLog.Add(new Log() { id = account.id, debugTime = DateTime.Now, debug = "Logged off" });
                        }

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

        static void Exit(BankAccounts accounts, List<Log> eventLog)
        {
            StreamWriter file = new StreamWriter("accounts.txt");
            foreach (BankAccount c in accounts)                                                     //USAGE OF IENUMERABLE IN foreach
            {
                string account = c.toString();
                Console.WriteLine(account);
                Console.WriteLine("---------------");
                file.WriteLine(account);
            }
            file.Close();
            eventLog.Sort();                                                                        //USAGE OF ICOMPARABLE
            string temp = "EventLog.txt";
            if (!File.Exists(temp))
            {
                File.Create(temp);
            }
            else
            {
                StreamReader readDebug = new StreamReader("EventLog.txt");
                string text = readDebug.ReadToEnd();
                readDebug.Close();
                StreamWriter writeDebug = new StreamWriter("EventLog.txt");
                writeDebug.WriteLine(text);
                foreach (var element in eventLog)
                {
                    Console.WriteLine(element);
                    writeDebug.WriteLine(element);
                }
                writeDebug.Close();
            }
            Console.WriteLine("Good bye!");
            Console.ReadLine();
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
            
        static void GivePermissions(string value, BankAccount account)
        {
            switch (value)
            {
                case "normal":
                    account.permissions = PermissionTypes.Read | PermissionTypes.Write;
                    break;
                case "admin":
                    account.permissions = PermissionTypes.All;
                    break;
                default:
                    Console.WriteLine("Something's wrong");
                    break;
            }
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

        static int ReadInt()
        {
            int tempInt = 0;
            bool valid = false;
            while (!valid)
            {
                string temp = Console.ReadLine();
                if (!string.IsNullOrEmpty(temp) && Regex.IsMatch(temp, "^[0-9]*$"))
                {
                    valid = true;
                    tempInt = int.Parse(temp);
                }
                else
                {
                    Console.WriteLine("Wrong input!");
                }
            }
            return tempInt;
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

        static string RandomNumber(Random rnd, int i, int x)
        {
            int random = rnd.Next(i, x);
            string temp = random.ToString();
            return temp;
        }

        static void GetAccountInformation(BankAccounts accounts)
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
                    BankAccount account = new BankAccount();
                    account.Name = words[0];
                    account.Surname = words[1];
                    account.year = words[2];
                    account.id = words[3];
                    account.pass = words[4];
                    account.money = Convert.ToDouble(words[5]);
                    if(words[0] == "Admin" || words[1] == "Admin")
                    {
                        GivePermissions("Admin", account);
                    }
                    else
                    {
                        GivePermissions("normal", account);
                    }
                    accounts.Add(account);
                }
                file.Close();
            }
        }
        
        static void Main(string[] args)
        {
            BankAccounts accounts = new BankAccounts();
            accounts.Create();
            GetAccountInformation(accounts);
            List<Log> eventLog = new List<Log>();
            Console.WriteLine("Welcome to Unsecured Bank system!");
            while (true)
            {
                Console.WriteLine("1. Register new account.");
                Console.WriteLine("2. Login with existing account.");
                Console.WriteLine("3. Exit system");
                switch (ReadInt())
                {
                    case 1:
                        Registration(accounts, eventLog);
                        Console.Clear();
                        break;
                    case 2:
                        Login(accounts, eventLog);
                        Console.Clear();
                        break;
                    case 3:
                        Exit(accounts, eventLog);
                        Console.Clear();
                        break;
                    default:
                        Console.Write("Wrong input! Please enter a number: ");
                        Console.Read();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
