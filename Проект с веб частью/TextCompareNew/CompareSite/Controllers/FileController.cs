using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CompareSite.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Authorization;

namespace CompareSite.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        /// Максимальное кол-во файлов за одну загрузку
        int MAX_COUNT = 100;
        /// Максимальный объем файла
        long MAX_SIZE = 30 * 1024 * 1024;

        public IActionResult Index()
        {
            return View();
        }

        /// Загрузка файлов на сервер
        [HttpPost]
        public ActionResult Upload(IFormFileCollection files)
        {
            if (files.Count > MAX_COUNT)
                return PartialView("_Error", "Ошибка: " + "Превышено максимальное количество загружаемых за один раз файлов");

            foreach (var file in files)
            {
                if (file.Length > MAX_SIZE)
                    return PartialView("_Error", "Ошибка: " + "Превышено максимальный объем загружаемых за один раз файлов");
            }

            // Ответ от сервера
            string responseJson = null;
            TextFile currentTextFile = null;
            // Список файлов, полученных от сервера после добавления в БД
            List<TextFile> textFilesList = new List<TextFile>();

            foreach (var file in files)
            {
                try
                {
                    // Проверка типа файла
                    if (file.ContentType != DocxFile.ContentType && file.ContentType != "text/plain")
                        return PartialView("_Error", "Ошибка: неподдерживаемый входнгого тип файла " + file.FileName);

                    //fileData = FileWorker.GetFileBytes(file);
                    //textFile = new TextFile { FileName = fileName, Date = DateTime.Now, Content = fileData, Size = file.Length };
                    //string json = JsonConvert.SerializeObject(textFile);
                    //string formData = textFile.GetFormData();

                    // Отправка запроса на сервер
                    responseJson = Models.Request.POST("http://127.0.0.1:5001/api/files", file);

                    if (string.IsNullOrWhiteSpace(responseJson) || responseJson == "[]")
                        return Json(new { error = "Получен пустой ответ от сервера" });

                    // Десериализация ответа
                    currentTextFile = DeserializeFileFromJson(responseJson);
                    // Добавление полученного файла в список
                    textFilesList.Add(currentTextFile);
                }
                catch (Exception ex)
                {
                    //return PartialView("_Error", "Ошибка: " + ex.Message);
                    return Json(new { error = "Ошибка: " + ex.Message });
                }
            }

            ViewBag.TitleFilesTable = "На сервер загружено " + textFilesList.Count + " файлов.";
            return PartialView("_FilesTableResult", textFilesList);

            // для способа зигрузки большого кол-ва файлов
            // return PartialView("_FilesTableResultHARD", textFilesList);
        }

               /// Получить TextFile из JSON строки
        private TextFile DeserializeFileFromJson(string json)
        {
            TextFile textFile = null;
            try
            {
                // Чтение файла из ответа
                textFile = JsonConvert.DeserializeObject<TextFile>(json);
            }
            catch
            {
                DeserializeErrorFromJson(json);
                throw new Exception("Получен неизвестный ответ от сервера: " + json);
            }
            if (textFile == null)
            {
                throw new Exception("Не получен объект от сервера");
            }

            return textFile;
        }

        /// Получить List<TextFile> из JSON строки
        private List<TextFile> DeserializeFilesListFromJson(string json)
        {
            // Десериализуем список файлов
            List<TextFile> textFilesList = null;

            try
            {
                // Десериализуем список файлов
                textFilesList = JsonConvert.DeserializeObject<List<TextFile>>(json);
            }
            catch (Exception ex)
            {
                // Если список файлов не получен, десериализуем ошибку
                DeserializeErrorFromJson(json);
                throw new Exception("Получен неизвестный ответ от сервера: " + json);
            }

            // Если получено 0 файлов
            if (textFilesList.Count == 0)
                throw new Exception("Не получен объект от сервера"); // !!!!! Потом написать обработку error

            return textFilesList;
        }

        /// Получить ошибку из JSON строки
        private void DeserializeErrorFromJson(string json)
        {
            // Анонимный класс ошибки
            var error = new { error = "" };
            // Чтение ошибки из ответа
            var errorResponse = JsonConvert.DeserializeAnonymousType(json, error);
            // Если в ответе пришла ошибка, выводим ее пользователю
            if (errorResponse != null)
                throw new Exception(errorResponse.error);
        }

        /// Скачивание файла с сервера
        [HttpPost]
        public PartialViewResult Find(string select)
        {
            string responseJson = null;
            List<TextFile> textFilesList = new List<TextFile>();

            try
            {
                if (String.IsNullOrEmpty(select))
                    throw new Exception("Не введена поисковая строка.");
                // Отправка запроса на сервер
                responseJson = Models.Request.GET("http://127.0.0.1:5001/api/files/find", select);

                if (string.IsNullOrWhiteSpace(responseJson) || responseJson == "[]")
                    return PartialView("_Error", "По вашему запросу ничего не найдено");

                // Десериализация ответа
                textFilesList = DeserializeFilesListFromJson(responseJson);
            }
            catch (Exception ex)
            {
                return PartialView("_Error", "Ошибка: " + ex.Message);
            }
            ViewBag.TitleFilesTable = "По запросу \"" + select + "\" найдено " + textFilesList.Count + " результатов:";
            return PartialView("_FilesTableResult", textFilesList);
        }


        /// Скачивание файла с сервера
        [HttpGet]
        [Route("[controller]/download/{id:int}")]
        public ActionResult Download(int id)
        {
            TextFile textFile = null;
            string responseJson = null;

            try
            {
                responseJson = Models.Request.GET("http://127.0.0.1:5001/api/files", id.ToString());

                if (string.IsNullOrWhiteSpace(responseJson) || responseJson == "[]")
                    return Content("_Error", "Получен пустой ответ от сервера");

                // Десериализация ответа
                textFile = DeserializeFileFromJson(responseJson);
            }
            catch (Exception ex)
            {
                return Content("Ошибка: " + ex.Message);
            }

            return File(textFile.Content, System.Net.Mime.MediaTypeNames.Application.Octet, textFile.FileName);
        }

        /// Удаление файла с сервера
        [HttpPost]
        //[Route("[controller]/delete/{id:int}")]
        public JsonResult Delete(int id)
        {
            string responseJson = null;
            var answer = new { success = "", error = "" };
            try
            {
                responseJson = Models.Request.DELETE("http://127.0.0.1:5001/api/files", id.ToString());
                answer = JsonConvert.DeserializeAnonymousType(responseJson, answer);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(answer);
        }

        /// Изменение файла на сервере
        [HttpPost]
        //[Route("[controller]/delete/{id:int}")]
        public JsonResult Update(int id, string name, string description)
        {
            string responseJson = null;
            var answer = new { success = "", error = "" };
            var file = new { id = id, name = name, description = description };
            try
            {
                //string data = String.Format("{{id:{0},name:\"{1}\" , description:\"{2}\"}}", id, name, description);
                //var data = String.Format("{{\"id\":{0},\"name\":\"{1}\",\"description\":\"{2}\"}}" , id, name, description);
                string data = "id=" + id.ToString() + "&name=" + name + "&description=" + description;//JsonConvert.SerializeObject(file);
                responseJson = Models.Request.PUT("http://127.0.0.1:5001/api/files", data);
                answer = JsonConvert.DeserializeAnonymousType(responseJson, answer);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(answer);
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
