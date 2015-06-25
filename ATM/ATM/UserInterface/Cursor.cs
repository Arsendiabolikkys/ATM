﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace ATM.UserInterface
{
    class Cursor : Entity
    {
        private int _cooldown = 20;
        public Cursor()
        {
            SetGraphic(new Image(Assets.Cursor));
            Graphic.CenterOrigin();
        }

        public override void Update()
        {
            _cooldown--;
            var Y = Game.Input.MouseY;
            var X = Game.Input.MouseX;

            Graphic.X = X;
            Graphic.Y = Y;
            if (_cooldown == 0)
            {
                RemoveSelf();
                GameScene.Instance.Add(new Cursor());
            }
        }
    }
}
