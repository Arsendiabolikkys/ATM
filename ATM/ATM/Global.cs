using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using System.IO;


namespace ATM
{
    public static class Assets // это строки к картинкам

    {
        public static string Cursor = @"../../UserInterface/Resources/cursor.png";
        public static string Label;
        public static string Background = @"../../UserInterface/Resources/background.png";
        public static string Button = @"../../UserInterface/Resources/button.png";
    }

    public class Global //тут статик объект основного бизнесс класса и объект сесси для юзера
    {
        public static Classes.ATM atm = new Classes.ATM();
        public static Session User;
    }

    public enum Controls // перечисление для кнопок
    {
        Left,
        Right
    }
}
