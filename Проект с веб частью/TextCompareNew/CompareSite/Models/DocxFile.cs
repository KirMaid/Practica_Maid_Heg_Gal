using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CompareSite.Models
{
    /// Класс для работы с документами формата .docx
    class DocxFile
    {
        /// Имя файла
        public string FileName { get => mFileName; }
        private string mFileName;

        /// Текст
        public string Text { get => mText; }
        private string mText;
        /// Тип файла
        public static string ContentType { get => "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; }

        public DocxFile(IFormFile file)
        {
            // Провекрка типа документа
            if (file.ContentType.ToLower() != ContentType)
                throw new Exception("Некорректный тип файла");

            this.mFileName = file.FileName;
            this.mText = getDocxData(file);
        }
        
        /// Получить текст из файла
        private string getDocxData(IFormFile file)
        {
            Body bodyDoc = null;
            string text = null;

            using (Stream stream = file.OpenReadStream())
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(file.OpenReadStream(), false))
                {
                    bodyDoc = doc.MainDocumentPart.Document.Body;
                    text = bodyDoc.InnerText;
                }
            }

            return text;
        }

        public bool CheckContentType(string type)
        {


            return true;
        }
    }
}