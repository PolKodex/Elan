using System;
using System.IO;
using System.Net.Mime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace Elan.Common.Utils
{
    public static class ImageUtil
    {
        public static string Resize(string rawImage, int width, int height = 0)
        {
            int commaIndex = rawImage.IndexOf(',', StringComparison.Ordinal);
            var imagePrefix = rawImage.Substring(0, commaIndex + 1);
            var imageBase = rawImage.Substring(commaIndex + 1, rawImage.Length - commaIndex - 1);
            byte[] imageBytes = Convert.FromBase64String(imageBase);
            IImageFormat format;
            using (var image = Image.Load(imageBytes, out format))
            {
                if (width > image.Width)
                {
                    width = image.Width;
                }
                else if (height == 0)
                {
                    height = (int)(width * ((decimal)image.Height / image.Width));
                }

                image.Mutate(x => x
                    .Resize(width, height));

                using (var output = new MemoryStream())
                {
                    image.Save(output, format);
                    var resized = output.ToArray();
                    return imagePrefix + Convert.ToBase64String(resized);
                }
            }
        }
    }
}
