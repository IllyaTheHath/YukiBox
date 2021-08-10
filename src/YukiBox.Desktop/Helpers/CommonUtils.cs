using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Helpers
{
    public static class CommonUtils
    {
        public static Uri GetAbsoluteUri(String path)
        {
            return new Uri($"pack://application:,,,/YukiBox;component/{path}");
        }

        public static Byte[] ImageToRGBABytes(Stream imageStream, out UInt32 height, out UInt32 width)
        {
            var img = new Bitmap(imageStream);
            var rgbaB = new Byte[4 * img.Width * img.Height];

            Int32 i = 0;

            for (var y = 0; y < img.Height; y++)
            {
                for (var x = 0; x < img.Width; x++)
                {
                    Color pix = img.GetPixel(x, y);

                    rgbaB[i++] = pix.R;
                    rgbaB[i++] = pix.G;
                    rgbaB[i++] = pix.B;
                    rgbaB[i++] = pix.A;
                }
            }

            height = (UInt32)img.Height;
            width = (UInt32)img.Width;

            return rgbaB;
        }
    }
}