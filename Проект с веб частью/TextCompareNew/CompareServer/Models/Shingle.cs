using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Numerics;

namespace CompareServer.Models 
{ 
    public struct ShingleHashes: IEquatable<ShingleHashes> 
    {

        //Конструктор с контрольными суммами хэшами,полученными различными способами кодировки(CRC32 MD 5 и т.д)
        public ShingleHashes(string sha1,
            string crc32,
            string md5,

            string sha256,
            string sha384,
            string sha512) : this()
        {
            this.sha1 = sha1;
            this.crc32 = crc32;
            this.md5 = md5;
            this.sha256 = sha256;
            this.sha384 = sha384;
            this.sha512 = sha512;
        }
        
        public string sha1 { get; set; }
        public string crc32 { get; set; }
        public string md5 { get; set; }
        public string sha256 { get; set; }
        public string sha384 { get; set; }
        public string sha512 { get; set; }

        public int randomValue;

        //Текстовый массив с контрольными суммами хэшами,полученными различными способами кодировки(CRC32 MD 5 и т.д)
        public List<String> HashesArray()
        {
            List<String> ShingleHashesArray = new List<string>();
            ShingleHashesArray.Add(this.sha1);
            ShingleHashesArray.Add(this.crc32);
            ShingleHashesArray.Add(this.md5);
            ShingleHashesArray.Add(this.sha256);
            ShingleHashesArray.Add(this.sha384);
            ShingleHashesArray.Add(this.sha512);

            return ShingleHashesArray;
        }

        //Получаем случайную контрольную сумму хэша
        public String GetRandomHash()
        {
            var rand = new Random();
            randomValue = rand.Next(6);
            List<String> ShingleHashesArray = HashesArray();
            return ShingleHashesArray[randomValue];
            
        }

        //Получаем минимальную контрольную сумму хэша
        public String GetMinHash()
        { 
        var minHash = "";

            List<String> ShingleHashesArray = HashesArray();
            //для каждог вида хэша
            foreach(String hash in ShingleHashesArray)
            {
                //если minHash=="", то minHash= текущий_хэш
                if(minHash=="")
                {
                    minHash = hash;
                }
                //иначе 
                //если minHash.length>текущий_хэш.length
                else
                {
                    if(minHash.Length>hash.Length)
                    {
                        //то  minHash= текущий_хэш
                        minHash = hash;
                    }
                }
            }

            return  minHash;
        }


        //Получаем максимальную контрольную сумму хэша
        public String GetMaxHash()
        {
            var minHash = "";

            List<String> ShingleHashesArray = HashesArray();
            //для каждог вида хэша
            foreach (String hash in ShingleHashesArray)
            {
                //если minHash=="", то minHash= текущий_хэш
                if (minHash == "")
                {
                    minHash = hash;
                }
                //иначе 
                //если minHash.length>текущий_хэш.length
                else
                {
                    if (minHash.Length < hash.Length)
                    {
                        //то  minHash= текущий_хэш
                        minHash = hash;
                    }
                }
            }
            return minHash;
        }
        //Получаем  контрольную сумму хэша закодированную методом crc32 как одно из полей
        public String GetCRC32Hash()
        {
            return this.crc32;
        }

        public bool SearchHash(String hash, int id = -1) {

            var isFound = false;

            if (id == -1)
                 isFound = (this.sha1 == hash) || (this.crc32 == hash) || (this.md5 == hash) || (this.sha256 == hash)
                    || (this.sha384 == hash) || (this.sha512 == hash);
            else
                isFound = HashesArray()[id] == hash;

            if (isFound)
            {
                return true;
            }
            else {
                return false;
            }
        }

        //Сравнение двух массивов с конрольными суммами по 3 шифровкам
        public bool Equals(ShingleHashes other)
        {
            return (this.sha1 == other.sha1)
                && (this.crc32 == other.crc32)
                && (this.md5 == other.md5);
        }
    }

    public class Shingle
    {
        public const int Lenght = 10;
        public string selectionMethod { get; set; }

        public readonly String[] StopWords = {
                                                  "это", "как", "так",
                                                  "и", "в", "над",
                                                  "к", "до", "не",
                                                  "на", "но", "за",
                                                  "то", "с", "ли",
                                                  "а", "во", "от",
                                                  "со", "для", "о",
                                                  "же", "ну", "вы",
                                                  "бы", "что", "кто",
                                                  "он", "она"
                                              };

        public String StopSym = ".,!?:;-–\n\r\t()";


        //Замена всей хуйни на пробелы
        public String Canonize(String source)
        {
            String Result = source.ToLower();

            foreach (char c in StopSym)
            {
                String cc = "" + c;
                Result = Result.Replace(cc, " ");
            }

            foreach (string word in StopWords)
            {
                Result = Result.Replace((" " + word + " "), " ");
            }

            Result = Regex.Replace(Result, " +", " ").Trim();

            return Result;
        }


        public List<ShingleHashes> GetShingles(string Text) 
        {
            
            var crc32 = new CRC32();
            var sha1 = SHA1.Create();
            var md5 = MD5.Create();
            var sha256 = SHA256.Create();
            var sha384 = SHA384.Create();
            var sha512 = SHA512.Create();

            Encoding encoding = Encoding.UTF8;

            var Result = new List<ShingleHashes>();

            String[] Words = Text.Split(' ');

            for (int i = 0; i <= (Words.Count() - Lenght); i++)
            {
                var CurrentShingle = new List<string>();

                String ShingleText = "";

                for (int j = 0; j < Lenght; j++)
                {
                    CurrentShingle.Add(Words[i + j]);
                    ShingleText += (Words[i + j] + " ");
                }

                ShingleHashes p = new ShingleHashes(
                    encoding.GetString(sha1.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(crc32.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(md5.ComputeHash(encoding.GetBytes(ShingleText))),

                    encoding.GetString(sha256.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(sha384.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(sha512.ComputeHash(encoding.GetBytes(ShingleText)))
               );
                Result.Add(p);
            }
            return Result;
        }

        //Для подсчёта времени, не обращай внимания
        Stopwatch stopWatch = new Stopwatch();
        public string comparisonTime { get; set; }
        public string canonizeTime { get; set; }
        public string shinglingTime { get; set; }

        //Главный метод сравнения
        public int Compare(String TextA, String TextB)
        {
            stopWatch.Restart();
            String canonizedTextA = Canonize(TextA);
            String canonizedTextB = Canonize(TextB);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //{0:00}:{1:00}:
            this.canonizeTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            stopWatch.Restart();
            List<ShingleHashes> ShingleA = GetShingles(canonizedTextA);
            List<ShingleHashes> ShingleB = GetShingles(canonizedTextB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;

            this.shinglingTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
           
            int matches = 0;

            stopWatch.Restart();

            switch (this.selectionMethod)
            {
                case "Min Hash":
                    matches = CompareShinglesMinHash(ShingleA, ShingleB);
                    break;
                case "Max Hash":
                    matches = CompareShinglesMaxHash(ShingleA, ShingleB);
                    break;
                case "CRC32":
                    matches = CompareShinglesCRC32(ShingleA, ShingleB);
                    break;
                case "Random Hash Par":
                    matches = CompareShinglesRandomHashParallel(ShingleA, ShingleB);
                    break;
                case "Min Hash Par":
                    matches = CompareShinglesMinHashParallel(ShingleA, ShingleB);
                    break;
                case "Max Hash Par":
                    matches = CompareShinglesMaxHashParallel(ShingleA, ShingleB);
                    break;
                case "CRC32 Par":
                    matches = CompareShinglesCRC32Parallel(ShingleA, ShingleB);
                    break;
                default:
                    matches = CompareShinglesRandomHash(ShingleA, ShingleB);
                    break;
            }

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            this.comparisonTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            return 2*100*matches/(ShingleA.Count + ShingleB.Count);
        }

        public int CompareMin(String TextA, String TextB)
        {
            stopWatch.Restart();
            String canonizedTextA = Canonize(TextA);
            String canonizedTextB = Canonize(TextB);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //{0:00}:{1:00}:
            this.canonizeTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            stopWatch.Restart();
            List<ShingleHashes> ShingleA = GetShingles(canonizedTextA);
            List<ShingleHashes> ShingleB = GetShingles(canonizedTextB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;

            this.shinglingTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            int matches = 0;

            stopWatch.Restart();

                    matches = CompareShinglesMinHashParallel(ShingleA, ShingleB);

            

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            this.comparisonTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            return 2 * 100 * matches / (ShingleA.Count + ShingleB.Count);
        }



        public int CompareMax(String TextA, String TextB)
        {
            stopWatch.Restart();
            String canonizedTextA = Canonize(TextA);
            String canonizedTextB = Canonize(TextB);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //{0:00}:{1:00}:
            this.canonizeTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            stopWatch.Restart();
            List<ShingleHashes> ShingleA = GetShingles(canonizedTextA);
            List<ShingleHashes> ShingleB = GetShingles(canonizedTextB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;

            this.shinglingTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            int matches = 0;

            stopWatch.Restart();

                    matches = CompareShinglesMaxHashParallel(ShingleA, ShingleB);


            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            this.comparisonTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            return 2 * 100 * matches / (ShingleA.Count + ShingleB.Count);
        }



        public int CompareRandom(String TextA, String TextB)
        {
            stopWatch.Restart();
            String canonizedTextA = Canonize(TextA);
            String canonizedTextB = Canonize(TextB);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //{0:00}:{1:00}:
            this.canonizeTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            stopWatch.Restart();
            List<ShingleHashes> ShingleA = GetShingles(canonizedTextA);
            List<ShingleHashes> ShingleB = GetShingles(canonizedTextB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;

            this.shinglingTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            int matches = 0;

            stopWatch.Restart();

            matches = CompareShinglesRandomHashParallel(ShingleA, ShingleB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            this.comparisonTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            return 2 * 100 * matches / (ShingleA.Count + ShingleB.Count);
        }


        public int CompareCRC32(String TextA, String TextB)
        {
            stopWatch.Restart();
            String canonizedTextA = Canonize(TextA);
            String canonizedTextB = Canonize(TextB);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //{0:00}:{1:00}:
            this.canonizeTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            stopWatch.Restart();
            List<ShingleHashes> ShingleA = GetShingles(canonizedTextA);
            List<ShingleHashes> ShingleB = GetShingles(canonizedTextB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;

            this.shinglingTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            int matches = 0;

            stopWatch.Restart();

                    matches = CompareShinglesCRC32Parallel(ShingleA, ShingleB);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            this.comparisonTime = String.Format("{2:00}.{3:00000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);

            return 2 * 100 * matches / (ShingleA.Count + ShingleB.Count);
        }










        
        public int CompareShinglesMinHash(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB) {
            int matches = 0;
            foreach (ShingleHashes s in ShingleA)
            {

                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                for (int i = 0; i < ShingleB.Count; i++)
                {
                    if (ShingleB[i].SearchHash(s.GetMinHash()))
                    {
                        matches++;
                    }
                }
            }
            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;
        }

        //сравнение по минимуму параллельно
        public int CompareShinglesMinHashParallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            amatches = new int[16];
            for (int i = 0; i < 16; i++)
                amatches[i] = 0;

            ParallelOptions o = new ParallelOptions();
            o.MaxDegreeOfParallelism = System.Environment.ProcessorCount; // !

            ShingleAA = ShingleA;
            ShingleBB = ShingleB;

            //Кол-во шинглов на кол-во ядер процессора(Блок шинглов)
            num = (int)(ShingleA.Count / System.Environment.ProcessorCount);
            //Запуск параллельного выполнения
            Parallel.For(0, System.Environment.ProcessorCount, ProcessOneShingleBlockMin);
            

            int matches = 0;
            for (int i = 0; i < 16; i++)
                matches += amatches[i];

            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;

        }
        //сравнение по максимуму 
        public int CompareShinglesMaxHash(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            int matches = 0;
            foreach (ShingleHashes s in ShingleA)
            {

                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                for (int i = 0; i < ShingleB.Count; i++)
                {
                    if (ShingleB[i].SearchHash(s.GetMaxHash()))
                    {
                        matches++;
                    }
                }
            }
            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;
        }

        //сравнение по максимуму параллельно
        public int CompareShinglesMaxHashParallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {

            amatches = new int[16];
            for (int i = 0; i < 16; i++)
                amatches[i] = 0;

            ParallelOptions o = new ParallelOptions();
            o.MaxDegreeOfParallelism = System.Environment.ProcessorCount; // !

            ShingleAA = ShingleA;
            ShingleBB = ShingleB;

            //Кол-во шинглов на кол-во ядер процессора(Блок шинглов)
            num = (int)(ShingleA.Count / System.Environment.ProcessorCount);
            //Запуск параллельного выполнения
            Parallel.For(0, System.Environment.ProcessorCount, ProcessOneShingleBlockMax);
           

            int matches = 0;
            for (int i = 0; i < 16; i++)
                matches += amatches[i];

            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;

        }

        //непосредсвтенно поэлементное сравнение двух листов с хэшами
        public void ProcessOneShingleBlockMax(int idx)
        {
            int matches = 0;
            for (int j = 0; j < num; j++)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {
                    if (ShingleBB[i].SearchHash(s.GetMaxHash()))
                    {
                        matches++;
                    }
                }
            }

            amatches[idx] += matches;
            return;
        }

        public void ProcessOneShingleBlockMin(int idx)
        {
            int matches = 0;
            for (int j = 0; j < num; j++)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {
                    if (ShingleBB[i].SearchHash(s.GetMinHash()))
                    {
                        matches++;
                    }
                }
            }

            amatches[idx] += matches;
            return;
        }
        public void ProcessOneShingleBlockCRC32(int idx)
        {
            int matches = 0;
            for (int j = 0; j < num; j++)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {
                    if (ShingleBB[i].SearchHash(s.GetCRC32Hash()))
                    {
                        matches++;
                    }
                }
            }

            amatches[idx] += matches;
            return;
        }

        
        public void ProcessOneShingleBlock(int idx)
        {
            int matches = 0;
            for(int j = 0;  j < num; j++)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {
                    if (ShingleBB[i].SearchHash(s.GetRandomHash(), s.randomValue))
                    {
                        matches++;
                    }
                }
            }

            amatches[idx] += matches;
            return;
        }


        public List<ShingleHashes> ShingleAA;
        public List<ShingleHashes> ShingleBB;
        public int[] amatches = new int[16];
        int num = 0;

        public int CompareShinglesRandomHashParallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            amatches = new int[16];
            for (int i = 0; i < 16; i++)
                amatches[i] = 0;

            ParallelOptions o = new ParallelOptions();
            o.MaxDegreeOfParallelism = System.Environment.ProcessorCount; // !
 
            ShingleAA = ShingleA;
            ShingleBB = ShingleB;

            //Кол-во шинглов на кол-во ядер процессора(Блок шинглов)
            num = (int)(ShingleA.Count / System.Environment.ProcessorCount);
            //Запуск параллельного выполнения
            Parallel.For(0, System.Environment.ProcessorCount, ProcessOneShingleBlock);
            //Parallel.Invoke()

            int matches = 0;
            for (int i = 0; i < 16; i++)
                matches += amatches[i];

            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;
        }


        public int CompareShinglesRandomHash(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {

            var matches = 0;

            foreach (ShingleHashes s in ShingleA)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                for (int i = 0; i < ShingleB.Count; i++)
                {
                    if (ShingleB[i].SearchHash(s.GetRandomHash()))
                    {
                        matches++;
                    }
                }
            }

            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;
        }

        public int CompareShinglesCRC32(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            int matches = 0;
            foreach (ShingleHashes s in ShingleA)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                for (int i = 0; i < ShingleB.Count; i++)
                {
                    if (ShingleB[i].SearchHash(s.GetCRC32Hash()))
                    {
                        matches++;
                    }
                }
            }
            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;
        }

        
        public int CompareShinglesCRC32Parallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            amatches = new int[16];
            for (int i = 0; i < 16; i++)
                amatches[i] = 0;

            ParallelOptions o = new ParallelOptions();
            o.MaxDegreeOfParallelism = System.Environment.ProcessorCount; // !

            ShingleAA = ShingleA;
            ShingleBB = ShingleB;

            //Кол-во шинглов на кол-во ядер процессора(Блок шинглов)
            num = (int)(ShingleA.Count / System.Environment.ProcessorCount);
            //Запуск параллельного выполнения
            Parallel.For(0, System.Environment.ProcessorCount, ProcessOneShingleBlockCRC32);
            

            int matches = 0;
            for (int i = 0; i < 16; i++)
                matches += amatches[i];

            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }

            return matches;
        }

    }
}
