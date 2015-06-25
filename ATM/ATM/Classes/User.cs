using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    public class User
    {
        public string CardNumber { get; private set; }
        public string Password { get; private set; }
        public double AmountOfMoney { get; private set; }

        public User(string cardNumber = null, string password = null, double amountOfMoney = 0)
        {
            CardNumber = cardNumber;
            Password = password;
            AmountOfMoney = amountOfMoney;
        }
    }
}
