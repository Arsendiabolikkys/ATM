using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    public class Cash
    {
        public int Rating { get; set; }
        public int Count { get; set; }

        public Cash(int rating = 20, int count = 0)
        {
            Rating = rating;
            Count = count;
        }
    }
}
