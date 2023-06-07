using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Runtime.InteropServices;
using Avalonia;

namespace pannella.analoguepocket.gui.Helpers;
public class ImageRenderer
{
    public static Bitmap RenderBinImage(byte[] image, int width, int height, bool invert)
    {
        var bitmap = new WriteableBitmap(new PixelSize(width + 1, height), new Vector(96, 96), PixelFormat.Rgba8888);

        using (var context = bitmap.Lock())
        {
            int stride = context.RowBytes;
            int bufferSize = stride * height;
            var buffer = new byte[bufferSize];

            Marshal.Copy(context.Address, buffer, 0, bufferSize);

            int index = 0;

            for (int x = width; x > 0; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    byte value = image[index];
                    byte brightness = invert ? (byte)(255 - value) : value;

                    int pixelIndex = y * stride + x * 4;
                    buffer[pixelIndex + 0] = brightness;
                    buffer[pixelIndex + 1] = brightness;
                    buffer[pixelIndex + 2] = brightness;
                    buffer[pixelIndex + 3] = 255;

                    index += 2;
                }
            }

            Marshal.Copy(buffer, 0, context.Address, bufferSize);
        }

        using (var stream = new System.IO.MemoryStream())
        {
            bitmap.Save(stream);
            stream.Position = 0;

            var avaloniaBitmap = new Bitmap(stream);

            // You can now use the 'avaloniaBitmap' object as an Avalonia bitmap

            return avaloniaBitmap;
        }
    }
}