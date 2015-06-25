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
        private int ClickCooldown = 30; //задержка при повторном нажатии на кнопку  
        private Image NormalImage;
        private Image DisableImage;
        public ClickAction onClick = delegate() { };//делегат для хранения функции при нажатии
        public Button(string path, float x, float y, string disablePath, ClickAction func)
        {
           
            if (!String.IsNullOrEmpty(disablePath))//если кнопка дисейбл, то для неё изображение
            {
                DisableImage = new Image(disablePath);
                SetGraphic(DisableImage);
            }
            else
            {//а тут для обычной
                NormalImage = new Image(path);
                SetGraphic(NormalImage);
                Graphic.CenterOrigin();
            }

            onClick = func;//тут функцию присваеваем в делегат

            this.X = x;
            this.Y = y;//положение кнопки
        }

        public override void Update() // тут проверка или мышка клацнула на кнопке
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
