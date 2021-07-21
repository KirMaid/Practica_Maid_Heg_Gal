using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CompareServer.ViewModels
{
    /// Текстовый файл
    public class UploadFile
    {
        /*[FileSize(10240)]
        [FileTypes("jpg,jpeg,png")]*/
        public IFormFile File { get; set; }
    }

}