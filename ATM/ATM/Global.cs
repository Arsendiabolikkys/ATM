using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using System.IO;


namespace ATM
{
    public static class Assets
    {
        public static string Cursor = @"../../UserInterface/Resources/cursor.png";
        public static string Label;
        public static string Background = @"../../UserInterface/Resources/background.png";
        public static string Button = @"../../UserInterface/Resources/button.png";
    }

    class Global
    {
        public static Session User;
    }

    public enum Controls
    {
        Left,
        Right
    }
}
