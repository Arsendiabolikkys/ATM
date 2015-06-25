using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    public class ATM
    {
        private const int RatingsCount = 5;

        private Cash[] Money = new Cash[RatingsCount];
        private User CurrentUser;
        private int MaxCurrentCash;
        private int MinCurrentCash;        

        public ATM()
        {
            //чтение с файла
        }
    }
}
