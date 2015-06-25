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
        private Card CurrentCard;     

        public ATM()
        {
            Money = new List<Cash>();
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
        }

        public void RunATM()
        {
            
        }
    }
}
