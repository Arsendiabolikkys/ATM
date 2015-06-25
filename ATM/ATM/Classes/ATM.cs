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
        private int MinAvailableMoney;
        private int MaxAvailableMoney;

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
                SetMinMax();
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

        private double CurrentCardAvailableMoney()
        {
            return CurrentCard.ShowAvailableMoney();
        }

        private void SaveATM()
        {
            CurrentCard.SaveCard();
            using (StreamWriter sw = new StreamWriter(@"data\ATM.txt", false, System.Text.Encoding.Default))
            {
                foreach (var item in Money)
                {
                    sw.WriteLine(item.Rating);
                    sw.WriteLine(item.Count);
                }
            }
        }

        private void SetMinMax()
        {
            for (int i = 0; i < RatingsCount; ++i)
            {
                if (Money[i].Count > 0)
                {
                    MinAvailableMoney = Money[i].Rating;
                    break;
                }
            }
            for (int i = RatingsCount - 1; i > 0; --i)
            {
                if (Money[i].Count > 0)
                {
                    MaxAvailableMoney = Money[i].Rating;
                    break;
                }
            }
        }

        private void TakeMoney(int sum)
        {
            int count = 0;
            int[] counts = new int[RatingsCount];
            bool enable = false;
            for (int i = 0; i < RatingsCount; ++i) count += Money[i].Count;
            for (counts[0] = 0; counts[0] < count && !enable && sum >= MinAvailableMoney; ++counts[0])
            {
                for (counts[1] = 0; counts[1] < count && !enable && sum >= MinAvailableMoney; ++counts[1])
                {
                    for (counts[2] = 0; counts[2] < count && !enable && sum >= MinAvailableMoney; ++counts[2])
                    {
                        for (counts[3] = 0; counts[3] < count && !enable && sum >= MinAvailableMoney; ++counts[3])
                        {
                            for (counts[4] = 0; counts[4] < count && !enable && sum >= MinAvailableMoney; ++counts[4])
                            {
                                if ((Money[0].Rating * counts[0] + Money[1].Rating * counts[1] + Money[2].Rating * counts[2] + Money[3].Rating * counts[3] + Money[4].Rating * counts[4]) == sum)
                                {
                                    if (Money[0].Count >= counts[0] && Money[1].Count >= counts[1] &&
                                        Money[2].Count >= counts[2] && Money[3].Count >= counts[3] &&
                                        Money[4].Count >= counts[4])
                                    {
                                        for (int k = 0; k < RatingsCount; ++k)
                                        {
                                            Money[k].Count -= counts[k];
                                        }                                        
                                        sum = 0;
                                        enable = true;
                                        //printf("take your money!\n");
                                        //if (count500) printf("%d - 500\n", count500);
                                        //if (count200) printf("%d - 200\n", count200);
                                        //if (count100) printf("%d - 100\n", count100);
                                        //if (count50) printf("%d - 50\n", count50);
                                        //if (count20) printf("%d - 20\n", count20);
                                        //if (count10) printf("%d - 10\n", count10);
                                        //printf("press any key to continue\n");
                                        //getch();
                                    }
                                }
                            }
                        }
                    }
                }
            }                         
            if (!enable)
            {
                //не валидная сумма
                //printf("incorrect operation\nPress any key to continue\n");
                //getch();
            }
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
