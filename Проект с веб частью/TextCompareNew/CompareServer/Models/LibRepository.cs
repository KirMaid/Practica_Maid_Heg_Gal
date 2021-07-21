using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace CompareServer.Models
{
    
    public partial class LibRepository
    {
        private static string DirectorySingles = Startup.PathShingles;
        //private const string LibSource = "LibPackShingle.dll";//@"D:\web service windows\CompareServer\bin\Debug\netcoreapp2.0\LibPackShingle.dll";  // "/home/andrey/lib/libPackShingle.so";      // "/home/andrey/lib/libPackShingle.so"
        //private const string LibSource = "/home/polezhaev/Desktop/TextCompareNEW/lib/libPackShingle.so";
        private const string LibSource = "D:\\meh\\ПРОЕКТ С ДОКУМЕНТАЦИЕЙ\\TextCompareNew\\CompareServer\\bin\\Debug\\netcoreapp2.0\\LibPackShingle.dll";
       
        static Shingle s = new Shingle();
        
        



        // Сравнение двух текстов
        public static string CompareTwoTexts(string text1, string text2)
        {
            string error = "";

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            {
                error = "Не введен текст для сравнения.";
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            int result = -1;
            Stopwatch watch = null;
            long timespan = 0;

            try
            {
                // Старт времени
                watch = Stopwatch.StartNew();
                // Сравнение
                result = CmpTwoTexts(text1, text2);
                //result = s.CompareCRC32(text1, text2);
                watch.Stop();
                timespan = watch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            return string.Format(@"{{""success"":""{0}"", ""timeSpan"":""{1}""}}", result, timespan);
        }


        public static string CompareTwoTextsRandom(string text1, string text2)
        {
            string error = "";

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            {
                error = "Не введен текст для сравнения.";
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            int result = -1;
            Stopwatch watch = null;
            long timespan = 0;

            try
            {
                // Старт времени
                watch = Stopwatch.StartNew();
                // Сравнение
                result = s.CompareRandom(text1, text2);
                watch.Stop();
                timespan = watch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            return string.Format(@"{{""success"":""{0}"", ""timeSpan"":""{1}""}}", result, timespan);
        }

        public static string CompareTwoTextsMax(string text1, string text2)
        {
            string error = "";

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            {
                error = "Не введен текст для сравнения.";
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            int result = -1;
            Stopwatch watch = null;
            long timespan = 0;

            try
            {
                // Старт времени
                watch = Stopwatch.StartNew();
                // Сравнение
                result = s.CompareMax(text1, text2);
                watch.Stop();
                timespan = watch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            return string.Format(@"{{""success"":""{0}"", ""timeSpan"":""{1}""}}", result, timespan);
        }


        public static string CompareTwoTextsMin(string text1, string text2)
        {
            string error = "";

            if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            {
                error = "Не введен текст для сравнения.";
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            int result = -1;
            Stopwatch watch = null;
            long timespan = 0;

            try
            {
                // Старт времени
                watch = Stopwatch.StartNew();
                // Сравнение
                result = s.CompareMin(text1, text2);
                watch.Stop();
                timespan = watch.ElapsedMilliseconds;
                
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return string.Format(@"{{""error"":""{0}""}}", error);
            }

            return string.Format(@"{{""success"":""{0}"", ""timeSpan"":""{1}""}}", result, timespan);
        }









        public static int[] GetShingles(string text)
        {
            int[] shingles = new int[text.Length];
            GetShinglesArray(text, shingles, text.Length);

            return shingles;
        }

        /// Шинглы считываются из файлов
        public static List<CompareResult> CompareTextWithBase_Files(string text)
        {
            Stopwatch watch = new Stopwatch();

            // Результат сравнения с БД
            List<CompareResult> result = new List<CompareResult>() { };

            // Расчет шинглов для поступившего текста
            int[] shingles1 = GetShingles(text).Where(x => x != 0).ToArray();

            //TextFile tmp = new TextFile(105, 3);
            //result.Add(new CompareResult { TextDocument = new TextFile(105, 1), Percent = 55 });

            watch = Stopwatch.StartNew();
            // Cписок файлов для сравнения
            List<int[]> files = FileWorker.ReadShingleFiles(DirectorySingles);
            // Получение списка файлов для сравнения
            watch.Stop();
            long timespan1 = watch.ElapsedMilliseconds;
            int percent = 0;
            // Старт времени
            watch = Stopwatch.StartNew();
            // Сравнение
            foreach (int[] shingles2 in files)
            {
                percent = CmpTwoShingles(shingles1, shingles2, shingles1.Length, shingles2.Length);

                if (percent > 0)
                    result.Add(new CompareResult { TextDocument = new TextFile(), Percent = percent });
            }
            watch.Stop();
            long timespan2 = watch.ElapsedMilliseconds;
            // Результат сохраняем в качестве файла коллекции
            result.Add(new CompareResult { TextDocument = null, Percent = (int)timespan1 });

            return result;
        }

        /// Шинглы считываются из файлов
        public static List<CompareResult> CompareTextWithBase_DB(string text)
        {
            Stopwatch watch = new Stopwatch();

            // Результат сравнения с БД
            List<CompareResult> result = new List<CompareResult>() { };

            // Расчет шинглов для поступившего текста
            int[] shingles1 = GetShingles(text).Where(x => x != 0).ToArray();

            //TextFile tmp = new TextFile(105, 3);
            //result.Add(new CompareResult { TextDocument = new TextFile(105, 1), Percent = 55 });
            List<int[]> files = null;
            watch = Stopwatch.StartNew();
            // Cписок файлов для сравнения
            using (var dbProvider = new DbProvider(Startup.ConnectionString))
                files = dbProvider.GetShinglesArray();
            // Получение списка файлов для сравнения
            watch.Stop();
            long timespan1 = watch.ElapsedMilliseconds;
            int percent = 0;
            // Старт времени
            watch = Stopwatch.StartNew();
            // Сравнение
            foreach (int[] shingles2 in files)
            {
                percent = CmpTwoShingles(shingles1, shingles2, shingles1.Length, shingles2.Length);

                if (percent > 0)
                    result.Add(new CompareResult { TextDocument = new TextFile(), Percent = percent });
            }
            watch.Stop();
            long timespan2 = watch.ElapsedMilliseconds;
            // Результат сохраняем в качестве файла коллекции
            result.Add(new CompareResult { TextDocument = null, Percent = (int)(timespan1+timespan2) });
            return result;
        }

        public static List<CompareResult> CompareTextWithBase(string text)
        {
            Stopwatch watch = new Stopwatch();

            // Результат сравнения с БД
            List<CompareResult> result = new List<CompareResult>() { };

            // Расчет шинглов для поступившего текста
            int[] shingles = GetShingles(text).Where(x => x != 0).ToArray();

            //TextFile tmp = new TextFile(105, 3);
            //result.Add(new CompareResult { TextDocument = new TextFile(105, 1), Percent = 55 });

            // Cписок файлов для сравнения
            List<TextFile> files;
            watch = Stopwatch.StartNew();
            // Получение списка файлов для сравнения
            using (var dbProvider = new DbProvider(Startup.ConnectionString))
                files = dbProvider.GetShingles();
            watch.Stop();
            long timespan1 = watch.ElapsedMilliseconds;

            // Старт времени
            watch = Stopwatch.StartNew();
            // Сравнение
            foreach (TextFile file in files)
            {
                int percent = CmpTwoShingles(shingles, file.Shingles, shingles.Length, file.Shingles.Length);

                if (percent > 0)
                   result.Add(new CompareResult { TextDocument = file, Percent = percent });
            }
            watch.Stop();
            long timespan2 = watch.ElapsedMilliseconds;
            // Результат сохраняем в качестве файла коллекции
            result.Add(new CompareResult { TextDocument = null, Percent = (int)(timespan1 + timespan2) });

            return result;
        }

        [DllImport(LibSource)]
        private static extern int CmpTwoTexts(string text1, string text2);

        [DllImport(LibSource)]
        private static extern string CmpTextWithBase(string text);

        [DllImport(LibSource)]
        private static extern void GetShinglesArray(string text, int[] shingles, int sizeText);

        [DllImport(LibSource)]
        private static extern int CmpTwoShingles(int[] shingles1, int[] shingles2, int size1, int size2);
    }
}
