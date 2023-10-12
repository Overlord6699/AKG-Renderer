namespace CGA_1.Render
{
    // Встроенного не было почему-то, пришлось генерить
    public struct ColorRGB
    {
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;

        public int Color { get => (int)_value; }
        public byte R { get => (byte)((_value >> ARGBRedShift) & 0xFF); }
        public byte G { get => (byte)((_value >> ARGBGreenShift) & 0xFF); }
        public byte B { get => (byte)((_value >> ARGBBlueShift) & 0xFF); }


        private long _value;

        ColorRGB(byte r, byte g, byte b) => _value = (unchecked((uint)(r << ARGBRedShift | g << ARGBGreenShift | b << ARGBBlueShift | 255 << ARGBAlphaShift))) & 0xffffffff;


        public static ColorRGB operator *(float f, ColorRGB color) => new ColorRGB((byte)(f * color.R), (byte)(f * color.G), (byte)(f * color.B));



        // комбо цветов
        public static ColorRGB Yellow { get; } = new ColorRGB(255, 255, 0);
        public static ColorRGB Blue { get; } = new ColorRGB(0, 0, 255);
        public static ColorRGB Gray { get; } = new ColorRGB(127, 127, 127);
        public static ColorRGB Green { get; } = new ColorRGB(0, 255, 0);
        public static ColorRGB Red { get; } = new ColorRGB(255, 0, 0);
        public static ColorRGB Magenta { get; } = new ColorRGB(255, 0, 255);
    }
}
