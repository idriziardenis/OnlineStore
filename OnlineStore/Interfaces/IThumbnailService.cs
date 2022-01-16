using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IThumbnailService
    {
        void GenerateThumbnail(int size, string inputPath, string outputPath);
        //void ResizeImage(string inputPath, string outputPath);
    }
}
