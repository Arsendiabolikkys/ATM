using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Business;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseManager parseManager = new ParseManager();
            parseManager.SaveToXml();
            Console.ReadLine();
        }
    }
}
