using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CompareServer.Models
{
    /// Класс работы с файлами на сервере (не БД)
    public static class FileWorker
    {

        /// Сохранение файла в директории
        public static void SaveFile(byte[] data, string path)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Create);
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                stream.Close();
            }
        }

        /// Сохранение файла в директории
        public static void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// Получение контента из обьекта файла IFormFile
        public static byte[] GetFileBytes(IFormFile file)
        {
            if (file == null)
                throw new System.ArgumentException("Пустой файл");

            byte[] bytes = null;
            BinaryReader reader = null;

            try
            {
                reader = new BinaryReader(file.OpenReadStream());
                bytes = reader.ReadBytes((int)file.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                reader.Close();
            }

            return bytes;
        }

        /// Получение контента из обьекта файла IFormFile
        public static byte[] ReadFile(string path)
        {
            byte[] data = null;
            FileStream stream = null;

            try
            {
                stream = new FileStream(path, FileMode.Open);
                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

            }
            finally
            {
                stream.Close();
            }

            return data;
        }

        /// Получение шиглов файлов директории
        public static List<int[]> ReadShingleFiles(string path)
        {
            // Шинглы файлов
            List<int[]> files = new List<int[]>();
            // Набор шинглов файла
            //List<int> array = new List<int>();
            // Получение списка файлов директории
            string[] dirs = Directory.GetFiles(path, "*.txt");

            Parallel.ForEach(dirs, dir =>
            //foreach (string dir in dirs)
            {
		List<int>  array = new List<int>();

                // Чтение файла
                using (var reader = new StreamReader(dir))
                {
                    
                    while (!reader.EndOfStream)
                        array.Add(Convert.ToInt32(reader.ReadLine()));
                }
                files.Add(array.ToArray());
            }
            );

            return files;
        }

        /// Сохранение файла в директории
        public static void SaveShinglesFile(int[] shingles, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < shingles.Length; i++)
                    writer.WriteLine(shingles[i]);
            }
        }
    }

}
