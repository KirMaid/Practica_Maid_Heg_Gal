using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Numerics;

namespace Shingles
{ 
    internal struct ShingleHashes: IEquatable<ShingleHashes>
    {
        //Конструктор с контрольными суммами хэшами,полученными различными способами кодировки(CRC32 MD 5 и т.д)
        public ShingleHashes(string sha1,
            string crc32,
            string md5,
           
            string mactri,
            string hmacMD5,
            string hmacSHA1,
            string ripemd,
            string hmacRIPEMD160,
            string hmacSHA256,
            string hmacSHA384,
            string hmacSHA512,
           
            string sha256, 
            string sha384,
            string sha512) : this()
        {
            this.sha1 = sha1;
            this.crc32 = crc32;
            this.md5 = md5;
            this.mactri = mactri;
            this.hmacMD5 = hmacMD5;
            this.hmacSHA1 = hmacSHA1;
            this.ripemd = ripemd;
            this.hmacRIPEMD160 = hmacRIPEMD160;
            this.hmacSHA256 = hmacSHA256;
            this.hmacSHA384 = hmacSHA384;
            this.hmacSHA512 = hmacSHA512;
            this.sha256 = sha256;
            this.sha384 = sha384;
            this.sha512 = sha512;
        }

        public string sha1 { get; set; }
        public string crc32 { get; set; }
        public string md5 { get; set; }

        public string mactri { get; set; }
        public string hmacMD5 { get; set; }
        public string hmacSHA1 { get; set; }
        public string ripemd { get; set; }
        public string hmacRIPEMD160 { get; set; }
        public string hmacSHA256 { get; set; }
        public string hmacSHA384 { get; set; }
        public string hmacSHA512 { get; set; }
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

            ShingleHashesArray.Add(this.mactri);
            ShingleHashesArray.Add(this.hmacMD5);
            ShingleHashesArray.Add(this.hmacSHA1);
            ShingleHashesArray.Add(this.ripemd);
            ShingleHashesArray.Add(this.hmacRIPEMD160);
            ShingleHashesArray.Add(this.hmacSHA256);
            ShingleHashesArray.Add(this.hmacSHA384);
            ShingleHashesArray.Add(this.hmacSHA512);
            ShingleHashesArray.Add(this.sha256);

            ShingleHashesArray.Add(this.sha384);
            ShingleHashesArray.Add(this.sha512);

            return ShingleHashesArray;
        }

        //Получаем случайную контрольную сумму хэша
        public String GetRandomHash()
        {
            var rand = new Random();
            //14 так как 14 способов кодировки и мы берём рандомную контрольную сумму хэша только одной кодировки
            randomValue = rand.Next(14);
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

        //Поиск хэша
        public bool SearchHash(String hash, int id = -1) {

            var isFound = false;

            if (id == -1)
                isFound = (this.sha1 == hash) || (this.crc32 == hash) || (this.md5 == hash)
                   || (this.mactri == hash) || (this.hmacMD5 == hash) || (this.hmacSHA1 == hash)
                   || (this.ripemd == hash) || (this.hmacRIPEMD160 == hash) || (this.hmacSHA256 == hash)
                   || (this.hmacSHA384 == hash) || (this.hmacSHA512 == hash) || (this.sha256 == hash)
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

    internal class Shingle
    {
        private const int Lenght = 10;
        public string selectionMethod { get; set; }

        private readonly String[] StopWords = {
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

        private String StopSym = ".,!?:;-–\n\r\t()";


        //Канонизация текста
        private String Canonize(String source)
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


        //Шифровка и добавление элементов в массив
        private List<ShingleHashes> GetShingles(string Text)
        {
            var crc32 = new CRC32();
            var md5 = MD5.Create();
            var sha1 = SHA1.Create();

            var mactri = MACTripleDES.Create();
            var hmacMD5 = HMACMD5.Create();
            var hmacSHA1 = HMACSHA1.Create();
            var ripemd = RIPEMD160Managed.Create();
            var hmacRIPEMD160 = HMACRIPEMD160.Create();
            var hmacSHA256 = HMACSHA256.Create();
            var hmacSHA384 = HMACSHA384.Create();
            var hmacSHA512 = HMACSHA512.Create();

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

                    encoding.GetString(mactri.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacMD5.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA1.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(ripemd.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacRIPEMD160.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA256.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA384.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA512.ComputeHash(encoding.GetBytes(ShingleText))),

                    encoding.GetString(sha256.ComputeHash(encoding.GetBytes(ShingleText))),

                    encoding.GetString(sha384.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(sha512.ComputeHash(encoding.GetBytes(ShingleText)))
               );
                Result.Add(p);
            }
            return Result;
        }

        private List<ShingleHashes> GetShinglesTest(string Text)
        {
            var crc32 = new CRC32();
            var md5 = MD5.Create();
            var sha1 = SHA1.Create();
            var mactri = MACTripleDES.Create();
            var hmacMD5 = HMACMD5.Create();
            var hmacSHA1 = HMACSHA1.Create();
            var ripemd = RIPEMD160Managed.Create();
            var hmacRIPEMD160 = HMACRIPEMD160.Create();
            var hmacSHA256 = HMACSHA256.Create();
            var hmacSHA384 = HMACSHA384.Create();
            var hmacSHA512 = HMACSHA512.Create();
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

                    encoding.GetString(mactri.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacMD5.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA1.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(ripemd.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacRIPEMD160.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA256.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA384.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(hmacSHA512.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(sha256.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(sha384.ComputeHash(encoding.GetBytes(ShingleText))),
                    encoding.GetString(sha512.ComputeHash(encoding.GetBytes(ShingleText)))
               );
                Result.Add(p);
            }
            return Result;
        }

        //Подсчёт времени
        Stopwatch stopWatch = new Stopwatch();
        public string comparisonTime { get; private set; }
        public string canonizeTime { get; private set; }
        public string shinglingTime { get; private set; }

        //Главный метод сравнения, половина кода вызов вышеперечисленных методов,другая половина работа с UI
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
            DateTime cmp1 = DateTime.Now;
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
            DateTime cmp2 = DateTime.Now;
            TimeSpan cmp = cmp2.Subtract(cmp1);
            this.comparisonTime = cmp.ToString();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            /*this.comparisonTime = String.Format("{2:00}.{3:00000}",
                cmp.Hours, cmp.Minutes, cmp.Seconds,
                cmp.Milliseconds);*/
            //Вычисление процента
            return 2*100*matches/(ShingleA.Count + ShingleB.Count);
        }

        //А вот тут уже сравнение по минимуму
        private int CompareShinglesMinHash(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB) {
            int matches = 0;
            foreach (ShingleHashes s in ShingleA)
            {
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
        private int CompareShinglesMinHashParallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
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
        //сравнение по максимуму 
        private int CompareShinglesMaxHash(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            int matches = 0;
            foreach (ShingleHashes s in ShingleA)
            {
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

        //Сравнение по максимуму параллельно 
        private int CompareShinglesMaxHashParallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
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

        //В методах с названиями аля ProcessOneShingleBlock происходит непосредственно поэлементное сравнение двух массивов с хэшами
        private void ProcessOneShingleBlockMax(int idx)
        {
            int matches = 0;
            for (int j = 0; j < num; j++)
            {
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {
                    if (ShingleBB[i].SearchHash(s.GetMaxHash()/*, s.randomValue*/))
                    {
                        matches++;
                    }
                }
            }

            amatches[idx] += matches;
            return;
        }

        private void ProcessOneShingleBlockMin(int idx)
        {
            int matches = 0;
            for (int j = 0; j < num; j++)
            {
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

        private void ProcessOneShingleBlockCRC32(int idx)
        {
            int matches = 0;
            for (int j = 0; j < num; j++)
            {
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {
                    if (ShingleBB[i].SearchHash(s.GetCRC32Hash()/*, s.randomValue*/))
                    {
                        matches++;
                    }
                }
            }

            amatches[idx] += matches;
            return;
        }

        
        private void ProcessOneShingleBlock(int idx)
        {
            //Поиск и сравнение по случайному шинглу
            int matches = 0;
            for(int j = 0;  j < num; j++)
            {
                //Выбираем из первого текста рандомный хэш и ищем его в списке хэшей второго шингла, idx - номер потока
                var s = ShingleAA[idx * num + j];
                for (int i = 0; i < ShingleBB.Count; i++)
                {   
                    if (ShingleBB[i].SearchHash(s.GetRandomHash(),s.randomValue))
                    {
                        matches++;
                    }
                }
            }
            amatches[idx] += matches;
            return;
        }


        private List<ShingleHashes> ShingleAA;
        private List<ShingleHashes> ShingleBB;
        private int[] amatches = new int[16];
        int num = 0;

        //Параллельная реализация сравнения по рандомному хэшу
        private int CompareShinglesRandomHashParallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            //Инициализация массива, в котором будут хранится результаты поиска.
            amatches = new int[16];
            for (int i = 0; i < 16; i++)
                amatches[i] = 0;

            //Настойка максимального числа параллельно выполняемых потоков для конкретной машины.
            ParallelOptions o = new ParallelOptions();
            o.MaxDegreeOfParallelism = System.Environment.ProcessorCount; // !
 
            ShingleAA = ShingleA;
            ShingleBB = ShingleB;

            //Число итераций в каждом цикле поиска паралельно выполняемого потока (Кол-во шинглов в первом тексте на кол-во ядер процессора)
            num = (int)(ShingleA.Count / System.Environment.ProcessorCount);
            //Запуск параллельного выполнения
            Parallel.For(0, System.Environment.ProcessorCount, ProcessOneShingleBlock);

            //Конечный подсчёт совпадений
            int matches = 0;
            for (int i = 0; i < 16; i++)
                matches += amatches[i];

            if (matches > ShingleA.Count)
            {
                matches = ShingleA.Count;
            }
            return matches;
        }


        private int CompareShinglesRandomHash(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {

            var matches = 0;

            foreach (ShingleHashes s in ShingleA)
            {
                //Выбираем из s рандомный хэш и ищем его в списке хэшей второго шингла
                for (int i = 0; i < ShingleB.Count; i++)
                {
                    if (ShingleB[i].SearchHash(s.GetRandomHash(),s.randomValue))
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

        private int CompareShinglesCRC32(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
        {
            int matches = 0;
            foreach (ShingleHashes s in ShingleA)
            {
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

        
        private int CompareShinglesCRC32Parallel(List<ShingleHashes> ShingleA, List<ShingleHashes> ShingleB)
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
    }
}
