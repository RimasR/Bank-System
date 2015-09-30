using System;
using System.Collections;

namespace BankSystem
{
    public class BankAccount
    {
        public PermissionTypes permissions = PermissionTypes.None;
        public string name
        { get; set; }
        public string surname
        { get; set; }
        public string id;
        public string pass;
        public string year;
        public double money
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

        public string toString()
        {
            return name + " " + surname + " " + year + " " + id + " " + pass + " " + money;
        }
    }
}