extern alias SplatAlias;

using SplatAlias::System.Drawing;
using System;
using System.Globalization;

namespace TTKoreanSchool.Utils
{
    public static class ColorUtil
    {
        public static Color FromHex(string hexString)
        {
            hexString = hexString.Replace("#", "");

            int red = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int green = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int blue = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return Color.FromArgb(red, green, blue);
        }
    }
}