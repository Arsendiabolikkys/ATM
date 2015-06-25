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
        private Image BackgroundImage; // Тут хранится изображение

        public Background(string path)
        {
            BackgroundImage = new Image(path);//Устанавливаем его по пути
            SetGraphic(BackgroundImage);//сетапим как графику
        }
    }
}
