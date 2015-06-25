using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace ATM.UserInterface
{
    private delegate void ClickAction();

    class Button : Entity
    {    
        private int ClickCooldown = 30;
        private Image NormalImage;
        private Image DisableImage;
        public ClickAction onClick = delegate() { };
        public Button(string path, float x, float y, string disablePath)
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
            }

            this.X = x;
            this.Y = y;
        }

        public override void Update()
        {
            if (Game.Input.MouseX > X &&
                Game.Input.MouseX < X + Graphic.Width &&
                Game.Input.MouseY > Y &&
                Game.Input.MouseY < Y + Graphic.Height &&
                ClickCooldown <= 0 /* &&TODO::*/)
            {
                onClick();
                ClickCooldown = 30;
            }
            ClickCooldown--;
        }
    }
}
