using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATM.Classes
{
    public class Card
    {
        public string CardNumber { get; private set; }
        public string Password { get; private set; }
        public double AmountOfMoney { get; set; }

        public Card(string cardNumber = null, string password = null, double amountOfMoney = 0)
        {
            CardNumber = cardNumber;
            Password = password;
            AmountOfMoney = amountOfMoney;
        }

        public double ShowAvailableMoney()
        {
            return AmountOfMoney;
        }

        public void SaveCard()
        {
            string path = @"data\Cards.txt";
            string text;
            List<Card> cards = new List<Card>();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                while (!String.IsNullOrEmpty(text = sr.ReadLine()))
                {
                    cards.Add(new Card(text, sr.ReadLine(), Double.Parse(sr.ReadLine())));
                }
            }
            cards.Find(x => x.CardNumber.Equals(this.CardNumber)).AmountOfMoney = this.AmountOfMoney;
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                foreach (var item in cards)
                {
                    sw.WriteLine(item.CardNumber);
                    sw.WriteLine(item.Password);
                    sw.WriteLine(item.AmountOfMoney);
                }
            }
        }

        
    }
}
