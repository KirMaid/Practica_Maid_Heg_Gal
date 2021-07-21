using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompareServer.Models
{
    /// Текстовый файл
    public class TextFile
    {

        /// id файла
        public int Id { get => mId; }
        private int mId;
        /// Имя файла
        public string FileName { get => mFileName; }
        private string mFileName;
        /// Название файла
        public string Name { get => mName; }
        private string mName;
        /// Описание файла
        public string Description { get => mDescription; }
        private string mDescription;
        /// Дата добавления файла
        public DateTime Date { get => mDate; }
        private DateTime mDate;
        /// Данные
        public byte[] Content { get => mContent; }
        private byte[] mContent;

        /// Текст
        public string Text { get => mText; }
        private string mText;

        /// Набор хэшей шинглов, рассчитанных для файла
        public int[] Shingles { get => mShingles; }
        public string ShinglesString
        {
            // get
            // {
            //     if (mShingles == null)
            //         return null;
            //     else if (mShingles.Length == 0)
            //         return null;

            //     int size = mShingles.Length;
            //     string shingles = "";
            //     for (int i = 0; i < size - 1; i++)
            //     {
            //         shingles += mShingles[i].ToString() + ";";
            //     }

            //     // Для последнего элемента
            //     shingles += mShingles[size - 1].ToString();

            //     return shingles;
            // }

            get
            {
                return JsonConvert.SerializeObject(mShingles);
            }
        }
        private int[] mShingles;
        /// Размер файла в байтах
        public long Size { get => mSize; }
        private long mSize;
        /// Путь к файлу
        public string Path { get => mPath; }
        private string mPath;

        /// Тип файла
        public string ContentType { get => mContentType; }
        private string mContentType;

        /// Каталог с файлами на сервере
        public string CatalogFilesDataBase { get => Startup.PathFiles; }
        /// Каталог с шинглами на сервере
        public string CatalogShinglesDataBase { get => Startup.PathShingles; }
        /// Строка подключения к бд
        public string ConnectionString { get => Startup.ConnectionString; }
        public TextFile()
        {

        }

        /// Конструктор
        public TextFile(int id, string fileName, int[] shingles)
        {
            this.mId = id;
            this.mFileName = string.IsNullOrWhiteSpace(fileName) ? null : fileName;
            this.mDate = DateTime.MinValue;
            this.mShingles = shingles;
        }


        /// Получение текстового файла из объекта IFormFile
        public TextFile(IFormFile file)
        {
            if (file == null)
                throw new Exception("Пустой файл");

            this.mFileName = file.FileName;
            this.mContent = FileWorker.GetFileBytes(file);//getFileTextBytes(file);
            this.mText = GetFileText(file);
            this.mShingles = GetShingles(mText);
            this.mContentType = file.ContentType;
            this.mSize = file.Length;
            this.mDate = DateTime.Now;
        }

        /// Получение текста из файла
        private string GetFileText(IFormFile file)
        {
            string text = null;
            byte[] bytes = null;

            // Если этот файл имеет формат docx
            if (file.ContentType == DocxFile.ContentType)
            {
                DocxFile docxFile = new DocxFile(file);
                text = docxFile.Text;
                //  bytes = Encoding.UTF8.GetBytes(text);
                //  string text2 = System.Text.Encoding.UTF8.GetString(bytes);

                //bytes = FileWorker.GetFileBytes(file);

            }
            else if (file.ContentType == "text/plain")
            {
                bytes = FileWorker.GetFileBytes(file);
                text = System.Text.Encoding.UTF8.GetString(bytes);
            }
            else
                throw new Exception("Некорректный формат файла");

            return text;
        }

        /// Конструктор
        public TextFile(int id, string fileName, string name, string description,
        DateTime date, byte[] content, string shingles, string path, long size)
        {
            this.mId = id;
            this.mFileName = string.IsNullOrWhiteSpace(fileName) ? null : fileName;
            this.mName = string.IsNullOrWhiteSpace(name) ? null : name;
            this.mDescription = string.IsNullOrWhiteSpace(description) ? null : description;
            this.mDate = date;
            this.mContent = null;

            // Исправить!!!
            // if (!String.IsNullOrEmpty(shingles))
            // {
            //     if (shingles.Substring(shingles.Length - 1) == "-")
            //     {
            //         shingles = shingles.Remove(shingles.Length - 2);
            //     }
            //     else if (shingles.Substring(shingles.Length - 1) == ";")
            //         shingles = shingles.Remove(shingles.Length - 1);
            // }

            //this.mShingles = shingles == null ? null : shingles.Split(';').Select(x => int.Parse(x)).ToArray();

            this.mShingles = string.IsNullOrWhiteSpace(shingles) ? null : JsonConvert.DeserializeObject<int[]>(shingles);

            this.mPath = string.IsNullOrWhiteSpace(path) ? null : path;
            this.mSize = size;
        }

        /// Получение файла по id.
        /// mode = 1: Id, FileName, Path + Content
        /// mode = 2: Id, FileName, Name, Description, Date, Size.
        /// mode = 3: Id, FileName, Shingles.
        public TextFile(int id, int mode)
        {
            TextFile dbFile = null;
            using (var dbProvider = new DbProvider(ConnectionString))
                dbFile = dbProvider.Get(id, mode);

            this.mId = dbFile.Id;
            this.mFileName = dbFile.FileName;
            this.mName = dbFile.Name;
            this.mDescription = dbFile.Description;
            this.mDate = dbFile.Date;
            this.mShingles = dbFile.Shingles;
            this.mPath = dbFile.Path;
            this.mSize = dbFile.Size;

            if (mode == 1)
                this.mContent = FileWorker.ReadFile(this.Path);
        }

        /// Получение файла с сервера
        public void GetContent()
        {
            this.mContent = FileWorker.ReadFile(this.Path);
        }

        /// Расчет шинглов для файла
        private int[] GetShingles(string text)
        {
            //string text = System.Text.Encoding.UTF8.GetString(this.mContent);
            //this.mShingles = LibRepository.GetShingles(text).Where(x => x != 0).ToArray();
            return LibRepository.GetShingles(text).Where(x => x != 0).ToArray();
        }

        /// Сохранение файла на сервере и добавление его в БД
        public int Save()
        {
            // Добавление файла в БД
            using (var dbProvider = new DbProvider(ConnectionString))
                this.mId = dbProvider.Add(this);

            // Путь к файлу
            string directoryFileName = String.Format("{0}_{1}", this.mId, this.mFileName);
            this.mPath = String.Format("{0}{1}", this.CatalogFilesDataBase, directoryFileName);

            // Обновление пути к файлу
            this.Update(this.mName, this.mDescription);

            // Сохранение в директории на сервере
            FileWorker.SaveFile(this.mContent, this.mPath);


            // Сохранение шинглов
            FileWorker.SaveShinglesFile(this.mShingles, String.Format("{0}{1}.txt", this.CatalogShinglesDataBase, this.mId));


            return this.mId;
        }

        /// Обновление инфо о файле
        public int Update(string name, string description)
        {
            this.mName = name;
            this.mDescription = description;

            // Обновление информации о файле
            using (var dbProvider = new DbProvider(ConnectionString))
                return dbProvider.Update(this);
        }

        /// Удаление файла с сервера
        public int Delete()
        {
            // Имяфайла в директории на сервере
            string directoryFileName = String.Format("{0}_{1}", this.mId, this.mFileName);
            // Удаление в директории на сервере
            this.mPath = this.CatalogFilesDataBase + directoryFileName;
            FileWorker.DeleteFile(this.mPath);

            // Удаление файла из БД
            using (var dbProvider = new DbProvider(ConnectionString))
                return dbProvider.Delete(this.mId);
        }

    }
}
