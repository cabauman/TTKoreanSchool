extern alias SplatAlias;

using SplatAlias::System.Drawing;

namespace TTKoreanSchool.Utils
{
    public static class ColorPalette
    {
        public static Color BlueGrey100
        {
            get { return ColorUtil.FromHex("#CFD8DC"); }
        }

        public static Color BlueGrey500
        {
            get { return ColorUtil.FromHex("#607D8B"); }
        }

        public static Color Red200
        {
            get { return ColorUtil.FromHex("#ef9a9a"); }
        }

        public static Color Green200
        {
            get { return ColorUtil.FromHex("#a5d6a7"); }
        }

        public static Color Blue200
        {
            get { return ColorUtil.FromHex("#90caf9"); }
        }

        public static Color SilverMultRed200
        {
            get { return ColorUtil.FromHex("#b47474"); }
        }

        public static Color SilverMultGreen200
        {
            get { return ColorUtil.FromHex("#7ca17e"); }
        }

        public static Color SilverMultBlue200
        {
            get { return ColorUtil.FromHex("#6c98bb"); }
        }

        public static Color Amber
        {
            get { return ColorUtil.FromHex("#ffc107"); }
        }
    }
}