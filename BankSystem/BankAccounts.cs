using BankSystem;
using System;
using System.Collections.Generic;
using System.Collections;

namespace BankSystem
{
    public struct BankAccounts : IEnumerable<BankAccount>                               //STRUCT , GENERIC
    {
        private List<BankAccount> accountList;

        public void Create()
        {
            accountList = new List<BankAccount>();
        }

        public void Add(BankAccount acc)
        {
            accountList.Add(acc);
        }

        public IEnumerator<BankAccount> GetEnumerator()
        {
            return accountList.GetEnumerator();
        }

        public void Remove(BankAccount acc)
        {
            accountList.Remove(acc);
        }

        public int Count(BankAccount acc)
        {
            return accountList.Count;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}