using System;

namespace CompareServer.Models
{
    public class CompareResult
    {
        /// Текстовый файл
        public TextFile TextDocument { get; set; }
        /// Процент совпадений
        public int Percent { get; set; }
        /// Время сравнения
        public long TimeSpan { get; set; }
    }
}