﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;


namespace ATM.UI
{
    class Label : Entity
    {
        private bool _isFloat;
        public Label(float x, float y, string text, Color color)
        {
            SetGraphic(new Text());
            ((Text)Graphic).OutlineColor = Color.Black;
            ((Text)Graphic).OutlineThickness = 2;
            ((Text)Graphic).String = text;
            ((Text)Graphic).Color = color;
            X = x;
            Y = y;
        }

        public Label(float x, float y, string text, Color color, int Lifespan, bool isFloat = false, int size = 16)
        {
            SetGraphic(new Text(size));
            ((Text)Graphic).OutlineColor = Color.Black;
            ((Text)Graphic).OutlineThickness = 2;
            ((Text)Graphic).String = text;
            ((Text)Graphic).Color = color;
            X = x;
            Y = y;
            LifeSpan = Lifespan;
            _isFloat = isFloat;
        }

        public override void Update()
        {
            if (_isFloat) Y -= 0.3f;
        }
    }
}
