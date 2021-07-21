using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompareSite.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;


namespace CompareSite.Controllers
{
    [Authorize]
    public class CompareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// Сравнение двух текстов
        [HttpPost]
        public string CompareTwoTexts(string textOne, string textTwo, IFormFile fileTextOne, IFormFile fileTextTwo)
        {
            string text1 = null;
            string text2 = null;
            string result = null;
            try
            {
                if (fileTextOne != null)
                    text1 = FileWorker.GetFileText(fileTextOne);
                else if (textOne != null)
                    text1 = textOne;

                if (fileTextTwo != null)
                    text2 = FileWorker.GetFileText(fileTextTwo);
                else if (textTwo != null)
                    text2 = textTwo;

                result = Comparer.CompareTwoTexts(text1, text2); //
            }
            catch (Exception ex)
            {
                return string.Format(@"{{""error"":""{0}""}}", ex.Message);
            }

            return result;
        }

        [HttpPost]
        public string CompareTwoTextsRandom(string textOne, string textTwo, IFormFile fileTextOne, IFormFile fileTextTwo)
        {
            string text1 = null;
            string text2 = null;
            string result = null;
            try
            {
                if (fileTextOne != null)
                    text1 = FileWorker.GetFileText(fileTextOne);
                else if (textOne != null)
                    text1 = textOne;

                if (fileTextTwo != null)
                    text2 = FileWorker.GetFileText(fileTextTwo);
                else if (textTwo != null)
                    text2 = textTwo;

                result = Comparer.CompareTwoTextsRandom(text1, text2); //
            }
            catch (Exception ex)
            {
                return string.Format(@"{{""error"":""{0}""}}", ex.Message);
            }

            return result;
        }


        [HttpPost]
        public string CompareTwoTextsMax(string textOne, string textTwo, IFormFile fileTextOne, IFormFile fileTextTwo)
        {
            string text1 = null;
            string text2 = null;
            string result = null;
            try
            {
                if (fileTextOne != null)
                    text1 = FileWorker.GetFileText(fileTextOne);
                else if (textOne != null)
                    text1 = textOne;

                if (fileTextTwo != null)
                    text2 = FileWorker.GetFileText(fileTextTwo);
                else if (textTwo != null)
                    text2 = textTwo;

                result = Comparer.CompareTwoTextsMax(text1, text2); //
            }
            catch (Exception ex)
            {
                return string.Format(@"{{""error"":""{0}""}}", ex.Message);
            }

            return result;
        }

        public string CompareTwoTextsMin(string textOne, string textTwo, IFormFile fileTextOne, IFormFile fileTextTwo)
        {
            string text1 = null;
            string text2 = null;
            string result = null;
            try
            {
                if (fileTextOne != null)
                    text1 = FileWorker.GetFileText(fileTextOne);
                else if (textOne != null)
                    text1 = textOne;

                if (fileTextTwo != null)
                    text2 = FileWorker.GetFileText(fileTextTwo);
                else if (textTwo != null)
                    text2 = textTwo;

                result = Comparer.CompareTwoTextsMin(text1, text2); //
            }
            catch (Exception ex)
            {
                return string.Format(@"{{""error"":""{0}""}}", ex.Message);
            }

            return result;
        }







        ////////////////////////////////////////////////
        /// Сравнение двух текстов
        [HttpPost]
        public PartialViewResult CompareTextWithBase(string textOne, IFormFile fileTextOne)
        {
            string textToCmp = null;
            List<CompareResult> resultList = null;

            try
            {
                // был выбран файл
                if (fileTextOne != null)
                {
                    textToCmp = FileWorker.GetFileText(fileTextOne);
                    ViewBag.TitleFileCmpRes = fileTextOne.FileName;
                }
                // Если введен текст
                else if (textOne != null)
                {
                    textToCmp = textOne;
                    ViewBag.TitleFileCmpRes = "\"" + textOne.Substring(0, 10) + "...\"";
                }

                resultList = Comparer.CompareTextWithBase(textToCmp);
            }
            catch (Exception ex)
            {
                return PartialView("_Error", "Ошибка: " + ex.Message);
            }

            // В одном из файлов было сохранено время соравнения
            CompareResult timeResult = resultList.Where(x => x.TextDocument == null).First();
            // Сохранение времени для отобрадения (в секундах)
            ViewBag.TimeCmp = (double)timeResult.Percent / 1000;
            resultList.Remove(timeResult);

            return PartialView("_CompareFilesResult", resultList);
        }

        public IActionResult Compare()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
