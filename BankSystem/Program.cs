using System;
using System.Collections.Generic;
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
        }

        static void Login()
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

        static void Main(string[] args)
        {


            /*BankAccount admin = new BankAccount();
            admin.permissions = PermissionTypes.Read | PermissionTypes.Write | PermissionTypes.Delete;
            bool canRead = ((PermissionTypes.Read & admin.permissions) == PermissionTypes.Read);
            Console.WriteLine(canRead);*/
            List<BankAccount> accounts = new List<BankAccount>();

            Console.WriteLine("Welcome to Unsecured Bank system! Choose what you want to do:");
            Console.WriteLine("1. Register new account.");
            Console.WriteLine("2. Login with existing account.");
            string i = Console.ReadLine();
            switch (i)
            {
                case "1":
                    //Registration
                    BankAccount account = new BankAccount();
                    Registration(account);
                    accounts.Add(account);         
                    break;
                case "2":
                    //Login
                    Login();
                    break;
                default:

                    break;
            }
            Console.ReadLine();
        }
    }

    class BankAccount
    {
        public PermissionTypes permissions = PermissionTypes.None;
        public string name
        { get; set; }
        public string surname
        { get; set; }
        public int id;
        public int pass;
        public string year;
        double money
        { get; set; }
        public BankAccount()
        {

        }
        public BankAccount(string name, string surname, string year)
        {
            this.name = name;
            this.surname = surname;
            this.year = year;
        }
    }
}

//Sukurti registration forma +
//Sukurti login forma su -
