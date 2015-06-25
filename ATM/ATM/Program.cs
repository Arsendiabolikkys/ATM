using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using ATM.Classes;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("Game", 1920, 1080); //creates a game with internal resolution 1920 x 1080
            game.SetWindow(1600, 900); //outputs the game to a window scaled down to 1600 x 900
            Classes.ATM atm = new Classes.ATM();
            atm.RunATM();
            game.Start(); //starts the game loop
        }
    }
}
