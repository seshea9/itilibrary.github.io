using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace ITI_Libraly_Api.Utilities
{
    public class Resized
    {
        public static byte[] resizeImage(byte[] image, int newWidth = 100, int newHeight = 100)
        {
            
            var memoryStream = new MemoryStream(image);
            
            var originalImage = new Bitmap(memoryStream);
            var width = originalImage.Width * ((float)newHeight / (float)originalImage.Height);
            var height = originalImage.Height * ((float)newWidth / (float)originalImage.Width);
            var resized = new Bitmap(originalImage,(int)width,newHeight);
            var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(originalImage, 0, 0, width, newHeight);
            //graphics.DrawImage(originalImage, resized);
            var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
