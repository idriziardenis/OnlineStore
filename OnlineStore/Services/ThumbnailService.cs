using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OnlineStore.Services
{
    public class ThumbnailService : IThumbnailService
    {
        public void GenerateThumbnail(int size, string inputPath, string outputPath)
        {
            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    int width, height;
                    if (image.Width > image.Height)
                    {
                        width = size;
                        height = size; /*Convert.ToInt32(image.Height * size / (double)image.Width);*/
                    }
                    else
                    {
                        width = size; /*Convert.ToInt32(image.Width * size / (double)image.Height);*/
                        height = size;
                    }
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(outputPath); // automatic encoder selected based on extension.
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //public void ResizeImage(string inputPath, string outputPath)
        //{
        //    using (Image<Rgba32> image = Image.Load(inputPath))
        //    {
        //        int width, height;
        //        if (image.Width > 1000)
        //        {
        //            width = Convert.ToInt32(image.Width * Convert.ToDecimal(0.2));
        //            height = Convert.ToInt32(image.Height * Convert.ToDecimal(0.2));
        //        }
        //        else
        //        {
        //            width = image.Width;
        //            height = image.Height;
        //        }

        //        image.Mutate(x => x.Resize(width, height));
        //        image.Save(outputPath);
        //    }
        //    }



    }
}
