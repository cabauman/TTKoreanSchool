using System.Drawing;
using System.Globalization;

namespace TTKoreanSchool.Utils
{
    public static class ColorUtil
    {
        public static Color FromHex(string hexString)
        {
            int argb = int.Parse(hexString.Replace("#", string.Empty), NumberStyles.HexNumber);

            return Color.FromArgb(argb);
        }
    }
}