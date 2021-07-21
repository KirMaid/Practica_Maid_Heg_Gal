using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;

using System.Xml;

namespace CompareSite.Models
{
    /// Класс для работы с файлами
    public static class FileWorker
    {
        /// Получение текста из файла
        public static string GetFileText(IFormFile file)
        {
            string text = null;

            // Если этот файл имеет формат docx
            if (file.ContentType == DocxFile.ContentType)
            {
                DocxFile docxFile = new DocxFile(file);
                text = docxFile.Text;
            }
            else if (file.ContentType == "text/plain")
            {
                byte[] bytes = GetFileBytes(file);
                text = System.Text.Encoding.UTF8.GetString(bytes);
            }
            else
                throw new Exception("Некорректный формат файла");

            return text;
        }

        /// Получение byte из файла
        public static byte[] GetFileBytes(IFormFile file)
        {
            if (file == null)
                throw new System.ArgumentException("Пустой файл");

            byte[] bytes = null;
            BinaryReader reader = new BinaryReader(file.OpenReadStream());
            bytes = reader.ReadBytes((int)file.Length);

            return bytes;
        }

        public static Stream GenerateStreamFromString(string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(byteArray);
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }


        public static string GetTextFromDoc(IFormFile file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string text = null;

            using (Stream responseStream = file.OpenReadStream())
            {
                xmlDoc.Load(responseStream);
            }

            text = xmlDoc.OuterXml;

            return "";
        }
    }
}