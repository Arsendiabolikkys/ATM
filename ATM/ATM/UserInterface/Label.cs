﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;


namespace ATM.UserInterface
{
    class Label : Entity
    {
        private bool _isFloat;
        public Label(float x, float y, string text, Color color)
        {
            SetGraphic(new Text());
            Graphic.CenterOrigin();
            ((Text)Graphic).OutlineColor = Color.Black;//цвет границы
            //((Text)Graphic).OutlineThickness = 2;
            ((Text)Graphic).String = text;//сам текст лейбла устанавливаем
            ((Text)Graphic).Color = color;//...
            X = x;//положение по иксу 
            Y = y;//по игрику
        }

        public Label(float x, float y, string text, Color color, int size=16)
        {
            SetGraphic(new Text(size));
            Graphic.CenterOrigin();
            ((Text)Graphic).OutlineColor = Color.Black;  //просто перегруженный метод который ещё и размер задаёт
            //((Text)Graphic).OutlineThickness = 2;
            ((Text)Graphic).String = text;
            ((Text)Graphic).Color = color;
            X = x;
            Y = y;
        }

        public Label(float x, float y, string text, Color color, int Lifespan, bool isFloat = false, int size = 16)
        {
            SetGraphic(new Text(size));
            Graphic.CenterOrigin();
            ((Text)Graphic).OutlineColor = Color.Black;
            //((Text)Graphic).OutlineThickness = 2;
            ((Text)Graphic).String = text;
            ((Text)Graphic).Color = color;  // а тут ещё и время жизни задаётся
            X = x;
            Y = y;
            LifeSpan = Lifespan;
            _isFloat = isFloat;
        }

        public override void Update()
        {
            if (_isFloat) Y -= 0.3f;  // это чтобы лейбл постепенно уезжаел если он подвижный
        }
    }
}
