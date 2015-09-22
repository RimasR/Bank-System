namespace BankSystem
{
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