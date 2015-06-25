using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace ATM.UserInterface
{
    delegate void ClickAction();

    class Button : Entity
    {    
        private int ClickCooldown = 30;
        private Image NormalImage;
        private Image DisableImage;
        public ClickAction onClick = delegate() { };
        public Button(string path, float x, float y, string disablePath, ClickAction func)
        {
           
            if (!String.IsNullOrEmpty(disablePath))
            {
                DisableImage = new Image(disablePath);
                SetGraphic(DisableImage);
            }
            else
            {
                NormalImage = new Image(path);
                SetGraphic(NormalImage);
                Graphic.CenterOrigin();
            }

            onClick = func;

            this.X = x;
            this.Y = y;
        }

        public override void Update()
        {
            if (Game.Input.MouseX > X &&
                Game.Input.MouseX < X + Graphic.Width &&
                Game.Input.MouseY > Y &&
                Game.Input.MouseY < Y + Graphic.Height &&
                ClickCooldown <= 0 && Global.User.Controller.Button(Controls.Left).Down)
            {
                onClick();
                ClickCooldown = 30;
            }
            ClickCooldown--;
        }
    }
}
