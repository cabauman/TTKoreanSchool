using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace TTKoreanSchool.iOS.Utils
{
    public static class ColorUtil
    {
        public static UIColor Red200
        {
            get { return HexToUIColor("#ef9a9a"); }
        }

        public static UIColor Green200
        {
            get { return HexToUIColor("#a5d6a7"); }
        }

        public static UIColor Blue200
        {
            get { return HexToUIColor("#90caf9"); }
        }

        public static UIColor SilverMultRed200
        {
            get { return HexToUIColor("#b47474"); }
        }

        public static UIColor SilverMultGreen200
        {
            get { return HexToUIColor("#7ca17e"); }
        }

        public static UIColor SilverMultBlue200
        {
            get { return HexToUIColor("#6c98bb"); }
        }

        public static UIColor BlueGrey
        {
            get { return HexToUIColor("#607D8B"); }
        }

        public static UIColor Amber
        {
            get { return HexToUIColor("#ffc107"); }
        }

        public static UIColor HexToUIColor(string hexString)
        {
            hexString = hexString.Replace("#", string.Empty);

            if(hexString.Length == 3)
            {
                hexString = hexString + hexString;
            }

            if(hexString.Length != 6)
            {
                throw new Exception("Invalid hex string");
            }

            int red = int.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            int green = int.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            int blue = int.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            return UIColor.FromRGB(red, green, blue);
        }
    }
}