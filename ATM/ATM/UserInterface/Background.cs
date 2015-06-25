﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;


namespace ATM.UserInterface
{
    class Background : Entity
    {
        private Image BackgroundImage;

        public Background(string path)
        {
            BackgroundImage = new Image(path);
            SetGraphic(BackgroundImage);
        }
    }
}
