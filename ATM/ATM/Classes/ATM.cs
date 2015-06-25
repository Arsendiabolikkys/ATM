using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATM.Classes
{
    public class ATM
    {
        private int RatingsCount;

        private List<Cash> Money;
        private List<Card> AvailableCards;
        private Card CurrentCard;
   

        public ATM()
        {
            Money = new List<Cash>();
            AvailableCards = new List<Card>();

            string path = @"data\ATM.txt";
            string text;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                while (!String.IsNullOrEmpty(text = sr.ReadLine()))
                {
                    Money.Add(new Cash(Int32.Parse(text), Int32.Parse(sr.ReadLine())));
                }
                RatingsCount = Money.Count;
            }

            path = @"data\Cards.txt";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                while (!String.IsNullOrEmpty(text = sr.ReadLine()))
                {
                    AvailableCards.Add(new Card(text, sr.ReadLine(), Double.Parse(sr.ReadLine())));
                }
            }
        }

        public void RunATM()
        {
            Console.WriteLine("card number - ");
            string cardNumber = Console.ReadLine();
            if (CardExist(cardNumber))
            {
                Console.WriteLine("password - ");
                string password = Console.ReadLine();
                if (IsValidPassword(cardNumber, password))
                {
                    CurrentCard = AvailableCards.Find(x => x.CardNumber.Equals(cardNumber));
                    Menu();
                }
                else 
                {
                    Console.WriteLine("wrong pass");
                }
            }
            else 
            {
                Console.WriteLine("card is not exist");
            }
        }

        private void Menu()
        {

        }

        private bool CardExist(string cardNumber)
        {
            foreach (var card in AvailableCards)
            {
                if (card.CardNumber == cardNumber) return true;
            }
            return false;
        }

        private bool IsValidPassword(string cardNumber, string password)
        {
            return AvailableCards.Find(x => x.CardNumber.Equals(cardNumber)).Password == password;
        }
    }
}
