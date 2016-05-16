using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;

namespace BankSystem
{
    [Flags]                                                                                 //FLAGS
    public enum PermissionTypes
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
        //
        ///
        ////
        /////Registration blank to register new account
        private static void Registration(BankAccounts accounts, List<Log> eventLog)  
        {
            BankAccount account = new BankAccount();
            Write("Please enter your first name: ");
            account.Name = ReadCredentials().UppercaseFirstLetter();
            Write("Please enter your last name: ");
            account.Surname = ReadCredentials().UppercaseFirstLetter(); //EXTENTION METHOD
            Write("Please enter your birth date dd-mm-yyyy: ");
            account.year = ReadDate();
            Random rnd = new Random();
            account.id = RandomNumber(rnd, 10000, 100000);
            account.pass = RandomNumber(rnd, 100000, 1000000);
            WriteLine($"Your id is: {0}", account.id);
            WriteLine($"Your password is: {account.pass}");
            account.GiveMoney();                                        //OPTIONAL ARGUMENT
            WriteLine("Please save this information!");
            if (account.Name == "admin" || account.Name == "Admin")
            {
                GivePermissions("admin", account);
            }
            else
            {
                GivePermissions("normal", account);
            }
            accounts.Add(account);
            eventLog.Add(new Log { id = account.id, debugTime = DateTime.Now, debug = "Registered new account" });
            Read();
        }
        //
        ///
        ////
        /////Login menu
        static void Login(BankAccounts accounts, List<Log> eventLog)
        {
            foreach (BankAccount c in accounts)
            {
                WriteLine(c.id);
                WriteLine(c.pass);
                WriteLine("---------");
            }

            bool valid = false;
            while (!valid)
            {
                Write("Please enter your id: ");
                string id = ReadLoginInfo("id");
                Write("Please enter your password: ");
                string password = ReadLoginInfo("pass");
                if (accounts.Any())
                {
                    foreach (BankAccount c in accounts.ToList())
                    {
                        if ((c.id == id) && (c.pass == password))
                        {
                            valid = true;
                            var account = c;
                            WriteLine("Logged in successfully!");
                            eventLog.Add(new Log { id = account.id, debugTime = DateTime.Now, debug = "Logged in" });
                            SystemTray(account, accounts, eventLog);
                        }
                    }
                }
                WriteLine("Wrong id or password!");
            }

            
        }
        //
        ///
        ////
        /////System menu for an account
        static void SystemTray(BankAccount account, BankAccounts accounts, List<Log> eventLog)
        {
            Clear();
            WriteLine("Welcome back " + account.Name + "! Available options: ");
            bool valid = true;
            while (valid)
            {
                WriteLine("1. Transfer money to other account");
                WriteLine("2. Show your credentials");
                WriteLine("3. Delete account");
                WriteLine("4.Exit \n What would you like to do?");
                switch (ReadInt())
                {
                    case 1:
                        if (account != null && account.money > 0)
                        {
                            WriteLine("To what ID do you want to send the money to? ");
                            WriteLine(" ----------------------------------- \n ID List for clarification:");
                            foreach (BankAccount acc in accounts)
                            {
                                WriteLine(acc.id + "\n ---------");
                            }
                            string id = ReadLoginInfo("id");
                            WriteLine("How much money do you want to transfer? ");
                            int tempMoney = int.Parse(ReadLine());
                            double money = tempMoney;                                                                       //DATA WIDENING
                            if (money < account.money)
                            {
                                var found = accounts.FirstOrDefault(c => c.id == id);                                      //LINQ METHOD
                                /* var found = from foundId in accounts
                                   where foundId.id==id
                                   select foundId;*/
                                if (found != null)
                                {
                                    WriteLine("ID found, money transfered.");
                                    account.money = account.money - money;
                                    found.money = found.money + money;
                                    eventLog.Add(new Log { id = account.id, debugTime = DateTime.Now, debug = "Transfered money to other account" });
                                }
                            } else
                            {
                                WriteLine("You don't have that much money.");
                            }
                        }
                        else
                        {
                            WriteLine("You don't have any money to send!");
                        }
                        break;
                    case 2:
                        if (account != null)
                        {
                            Clear();
                            WriteLine("Name: {0}", account.Name);
                            WriteLine("Surname: {0}", account.Surname);
                            WriteLine("Birth date: {0}", account.year);
                            WriteLine("Money: {0}", account.money);
                            eventLog.Add(new Log { id = account.id, debugTime = DateTime.Now, debug = "Showed the credentials of an account" });
                        } else
                        {
                            WriteLine("There is no such account.");
                        }
                        break;
                    case 3:
                        if (account != null && (PermissionTypes.Delete & account.permissions) == PermissionTypes.Delete)
                        {
                            WriteLine("Do you really want to delete this account? Y/N");
                            string temp = ReadLine();
                            switch (temp)
                            {
                                case "Y":
                                    var deleteThis = accounts.SingleOrDefault(c => c.id == account.id);
                                    if (deleteThis != null)
                                    {
                                        eventLog.Add(new Log { id = account.id, debugTime = DateTime.Now, debug = "Deleted account" });
                                        accounts.Remove(acc : deleteThis);                  //NAMED ARGUMENT
                                        account = null;
                                        WriteLine("Deleted successfully!");
                                    }
                                    break;
                                case "N":
                                    Clear();
                                    WriteLine("Account is not deleted.");
                                    break;
                            }
                        }
                        else
                        {
                            WriteLine("You do not have permission to delete account!");
                            Read();
                            Clear();
                        }
                        break;
                    case 4:
                        if(account != null)
                        {
                            eventLog.Add(new Log { id = account.id, debugTime = DateTime.Now, debug = "Logged off" });
                        }

                        valid = false;
                        Clear();
                        break;
                    default:
                        WriteLine("Wrong input!");
                        Read();
                        Clear();
                        break;
                }
            }            
        }
        //
        ///
        ////
        /////Exit function, saving and storing all info into files
        static void Exit(BankAccounts accounts, List<Log> eventLog)
        {
            StreamWriter file = new StreamWriter("accounts.txt");
            foreach (BankAccount c in accounts)                                                     //USAGE OF IENUMERABLE IN foreach
            {
                string account = c.toString();
                WriteLine(account);
                WriteLine("---------------");
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
                    WriteLine(element);
                    writeDebug.WriteLine(element);
                }
                writeDebug.Close();
            }
            WriteLine("Press any key to exit.");
            ReadKey();
            Environment.Exit(0);
        }
        //
        ///
        ////
        /////Function to validate weather or not the input is letters only.
        private static string ReadCredentials()
        {
            bool valid = false;
            string name = "";
            while (!valid)
            {
                name = ReadLine();
                if (name != null && Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    valid = true;
                }
                else
                {
                    WriteLine("Wrong input!");
                }
            }
            return name;
        }

        //
        ///
        ////
        /////Function to give certain permissions to an account

        static void GivePermissions(string value, BankAccount account)
        {
            switch (value)
            {
                case "Normal":
                case "normal":
                    account.permissions = PermissionTypes.Read | PermissionTypes.Write;
                    break;
                case "Admin":
                case "admin":
                    account.permissions = PermissionTypes.All;
                    break;
                default:
                    WriteLine("Something's wrong");
                    break;
            }
        }
        
        /////Function to validate whether or not the input is a date
        private static string ReadDate()
        {
            var validDate = new Regex(@"\d{2}-\d{2}-\d{4}"); //REGEX
            var valid = false;
            string date = null;
            while (!valid)
            {
                date = ReadLine();
                if (date != null && validDate.IsMatch(date))
                {
                    valid = true;
                }
                else
                {
                    WriteLine("Wrong input! Year input is dd-mm-yyyy");
                }
            }
            return date;
        }

        //
        ///
        ////
        /////Function to validate weather or not the input is digits only.

        static int ReadInt()
        {
            int tempInt = 0;
            bool valid = false;
            while (!valid)
            {
                string temp = ReadLine();
                if (!string.IsNullOrEmpty(temp) && Regex.IsMatch(temp, "^[0-9]*$"))
                {
                    valid = true;
                    tempInt = int.Parse(temp);
                }
                else
                {
                    WriteLine("Wrong input!");
                }
            }
            return tempInt;
        }
        //
        ///
        ////
        /////Function to validate weather or not username and password is in good format.

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
                {
                    break;
                }
            }
            while (!valid)
            {
                string temp = ReadLine();
                if (!string.IsNullOrEmpty(temp) && Regex.IsMatch(temp, "^[0-9]*$"))
                {
                    id = int.Parse(temp);
                    if (temp.Length == length)
                    {
                        valid = true;
                    }
                    else
                    {
                        WriteLine("Wrong input!");
                    }
                }
                else
                {
                    WriteLine("Wrong input!");
                }
            }
            return id.ToString();
        }
        //
        ///
        ////
        /////Generates random number from i number to x number

        static string RandomNumber(Random rnd, int i, int x)
        {
            int random = rnd.Next(i, x);
            string temp = random.ToString();
            return temp;
        }

        //
        ///
        ////
        /////Reads account information from files

        static void GetAccountInformation(BankAccounts accounts)
        {
            string temp = "accounts.txt";
            if (!File.Exists(temp))
            {
                File.Create(temp);
            }
            else
            {
                using (StreamReader file = new StreamReader("accounts.txt"))
                {

                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var words = line.Split(' ');
                        var account = new BankAccount
                        {
                            Name = words[0],
                            Surname = words[1],
                            year = words[2],
                            id = words[3],
                            pass = words[4],
                            money = Convert.ToDouble(words[5])
                        };
                        if (words[0] == "Admin" || words[1] == "Admin")
                        {
                            GivePermissions("Admin", account);
                        }
                        else
                        {
                            GivePermissions("normal", account);
                        }
                        accounts.Add(account);
                    }
                }
                    //file.Close();
            }
        }

        //
        ///
        ////
        /////First menu to choose actions from
        public static void Main(string[] args)
        {
            BankAccounts accounts = new BankAccounts();
            accounts.Create();
            GetAccountInformation(accounts);
            var eventLog = new List<Log>();
            WriteLine("Welcome to Unsecured Bank system!");
            while (true)
            {
                WriteLine("1. Register new account.");
                WriteLine("2. Login with existing account.");
                WriteLine("3. Exit system");
                switch (ReadInt())
                {
                    case 1:
                        Registration(accounts, eventLog);
                        Clear();
                        break;
                    case 2:
                        Login(accounts, eventLog);
                        Clear();
                        break;
                    case 3:
                        Exit(accounts, eventLog);
                        Clear();
                        break;
                    default:
                        Write("Wrong input! Please enter a number: ");
                        Read();
                        Clear();
                        break;
                }
            }
        }
    }
}
