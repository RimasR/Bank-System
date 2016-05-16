namespace BankSystem
{
    public class BankAccount                                        //CLASS
    {
        public PermissionTypes permissions = PermissionTypes.None;
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private string surname;
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
            }
        }

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

        public void GiveMoney(int tempMoney = 100000)
        {
            money = tempMoney;
        }

        public string toString()
        {
            return name + " " + surname + " " + year + " " + id + " " + pass + " " + money;
        }
    }
}