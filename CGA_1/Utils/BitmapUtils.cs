using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CGA_1.Utils
{
    public static class BitmapUtils
    {

        public static Bitmap BuildBitmap(Bitmap bmp, int[] buffer, int width, int height)
        {
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}
