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
            account.name = Console.ReadLine();
            Console.Write("Please enter your last name: ");
            account.surname = Console.ReadLine();
            Console.Write("Please enter your birth date dd-mm-yyyy: ");
            account.year = Console.ReadLine();
            account.id = RandomNumber(10000, 100000);
            account.pass = RandomNumber(100000, 1000000);    
        }

        static int RandomNumber(int i, int x)
        {
            Random rnd = new Random();
            int random = rnd.Next(i, x);
            return random;
        }

        static void Login()
        {

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
        double money;
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
