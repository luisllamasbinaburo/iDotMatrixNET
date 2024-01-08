namespace iDotMatrixNET.Classes
{
    public class Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public Color()
        {
            Red = 0;
            Blue = 0;
            Green = 0;
        }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Color(string hexColor)
        {
            byte[] bytes = ToByteArray(hexColor);
            Red = bytes[0];
            Green = bytes[1];
            Blue = bytes[2];
        }

        internal static byte GetBrightness(byte Red, byte Green, byte Blue)
        {
            int maxx = 0;

            if (Red > maxx) maxx = Red;
            if (Green > maxx) maxx = Green;
            if (Blue > maxx) maxx = Blue;

            maxx = maxx * 100 / 255;

            var brightness = Convert.ToByte(maxx);

            return brightness;
        }

        private static byte[] ToByteArray(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            int indexer;
            if (hexString[0] == '#')
                indexer = 1;
            else
                indexer = 0;

            for (int i = indexer; i < hexString.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return bytes;
        }
    }
}