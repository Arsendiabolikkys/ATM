using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATM.Classes;
using Otter;

namespace ATM
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Game game = new Game("Game", 800, 600); //creates a game with internal resolution 1920 x 1080
            Global.User = game.AddSession("User");


            Global.User.Controller.AddButton(Controls.Left);

            Global.User.Controller.Button(Controls.Left).AddMouseButton(MouseButton.Left);

            //Game settings
            game.SetWindow(800, 600); //outputs the game to a window scaled down to 1600 x 900
            game.FirstScene = new GameScene();
            Classes.ATM atm = new Classes.ATM();
            game.Start(); //starts the game loop
        }
    }
}
