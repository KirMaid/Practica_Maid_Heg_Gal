using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CompareSite.Models
{
    public static class Comparer
    {

        private static string CmpTwoTextsUrl = "http://127.0.0.1:5001/api/compare/two-texts";
        private static string CmpTwoTextsUrl2 = "http://127.0.0.1:5001/api/compare/two-texts-random";
        private static string CmpTwoTextsUrl3 = "http://127.0.0.1:5001/api/compare/two-texts-max";
        private static string CmpTwoTextsUrl4 = "http://127.0.0.1:5001/api/compare/two-texts-min";
        private static string CmpTextWithBaseUrl = "http://127.0.0.1:5001/api/compare/text-with-base";

        public static string CompareTwoTexts(string text1, string text2)
        {
            // Валидация
            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
                throw new Exception("Не введен текст для сравнения.");
            // Отправка запроса
            //string response = Request.POST(CmpTwoTextsUrl, string.Format("text1={0}&text2={1}", text1, text2));

            var parameters = new[]
            {
        new KeyValuePair<string, string>("text1", text1),
        new KeyValuePair<string, string>("text2", text2)
            };

            string response = Request.POST_Stream(CmpTwoTextsUrl, parameters);

            return response;
        }


        public static string CompareTwoTextsRandom(string text1, string text2)
        {
            // Валидация
            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
                throw new Exception("Не введен текст для сравнения.");
            // Отправка запроса
            //string response = Request.POST(CmpTwoTextsUrl, string.Format("text1={0}&text2={1}", text1, text2));

            var parameters = new[]
            {
        new KeyValuePair<string, string>("text1", text1),
        new KeyValuePair<string, string>("text2", text2)
            };

            string response = Request.POST_Stream(CmpTwoTextsUrl2, parameters);

            return response;
        }

        public static string CompareTwoTextsMax(string text1, string text2)
        {
            // Валидация
            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
                throw new Exception("Не введен текст для сравнения.");
            // Отправка запроса
            //string response = Request.POST(CmpTwoTextsUrl, string.Format("text1={0}&text2={1}", text1, text2));

            var parameters = new[]
            {
        new KeyValuePair<string, string>("text1", text1),
        new KeyValuePair<string, string>("text2", text2)
            };

            string response = Request.POST_Stream(CmpTwoTextsUrl3, parameters);

            return response;
        }

        public static string CompareTwoTextsMin(string text1, string text2)
        {
            // Валидация
            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
                throw new Exception("Не введен текст для сравнения.");
            // Отправка запроса
            //string response = Request.POST(CmpTwoTextsUrl, string.Format("text1={0}&text2={1}", text1, text2));

            var parameters = new[]
            {
        new KeyValuePair<string, string>("text1", text1),
        new KeyValuePair<string, string>("text2", text2)
            };

            string response = Request.POST_Stream(CmpTwoTextsUrl4, parameters);

            return response;
        }




        public static List<CompareResult> CompareTextWithBase(string text)
        {
            // Валидация
            if (string.IsNullOrEmpty(text))
                throw new Exception("Не введен текст для сравнения.");
            // Отправка запроса
            //string response = Request.POST(cmpTextWithBaseUrl, string.Format("text={0}", text));

            var parameters = new[]
            {
                new KeyValuePair<string, string>("text", text),
                new KeyValuePair<string, string>("empty", null)
            };

            string response = Request.POST_Stream(CmpTextWithBaseUrl, parameters);

            return DeserializeFilesListFromJson(response);
        }

        /// Получить List<CompareResult> из JSON строки
        private static List<CompareResult> DeserializeFilesListFromJson(string json)
        {
            List<CompareResult> textFilesList = null;

            try
            {
                // Десериализуем список файлов
                textFilesList = JsonConvert.DeserializeObject<List<CompareResult>>(json);
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
        private static void DeserializeErrorFromJson(string json)
        {
            // Анонимный класс ошибки
            var error = new { error = "" };
            // Чтение ошибки из ответа
            var errorResponse = JsonConvert.DeserializeAnonymousType(json, error);
            // Если в ответе пришла ошибка, выводим ее пользователю
            if (errorResponse != null)
                throw new Exception(errorResponse.error);
        }
    }
}