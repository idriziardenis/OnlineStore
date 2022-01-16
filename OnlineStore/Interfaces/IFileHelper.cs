﻿using Microsoft.AspNetCore.Http;
using OnlineStore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Interfaces
{
    public interface IFileHelper
    {
        void SaveFile(FileTypes type, IFormFile file, string folderName, string id, params int[] thumbnails);
        void SaveFavIcon(FileTypes type, IFormFile file, string folderName, string id);
        string GetProperFilePath(FileTypes type, Thumbnails thumbnail, string path);
        string GetProperFilePath(FileTypes type, Thumbnails thumbnail, string path, bool forLogo);
        string GetFavIconFilePath(string path);
        void SaveImage(FileTypes type, IFormFile file, string folderName, string id);
    }
}
