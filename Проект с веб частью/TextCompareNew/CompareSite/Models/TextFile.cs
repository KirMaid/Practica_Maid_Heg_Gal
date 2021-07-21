using System;
using System.IO;
using System.Linq;

namespace CompareSite.Models
{
    /// Текстовый файл
    [Serializable]
    public class TextFile
    {
        /// id файла
        public int Id { get => mId; set => mId = value; }
        private int mId;
        /// Имя файла
        public string FileName { get => mFileName; set => mFileName = value; }
        private string mFileName;
        /// Название файла
        public string Name { get => mName; set => mName = value; }
        private string mName;
        /// Описание файла
        public string Description { get => mDescription; set => mDescription = value; }
        private string mDescription;
        /// Дата добавления файла
        public DateTime Date { get => mDate; set => mDate = value; }
        private DateTime mDate;
        /// Данные
        public byte[] Content { get => mContent; set => mContent = value; }
        private byte[] mContent;
        /// Набор хэшей шинглов, рассчитанных для файла
        public int[] Shingles { get => mShingles; set => mShingles = value; }
        public string ShinglesString
        {
            get
            {
                if (mShingles == null)
                    return null;

                int size = mShingles.Length;
                string shingles = "";
                for (int i = 0; i < size; i++)
                {
                    shingles += mShingles[i].ToString() + ";";
                }

                return shingles;
            }
        }
        private int[] mShingles;
        /// Размер файла в байтах
        public long Size { get => mSize; set => mSize = value; }
        private long mSize;
        /// Путь к файлу
        public string Path { get => mPath; set => mPath = value; }
        private string mPath;


        public TextFile()
        {

        }
    }
}