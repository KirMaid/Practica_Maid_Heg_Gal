using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CompareSite.Models
{
    public class CompareResult
    {
        public TextFile TextDocument { get; set; }
        public int Percent { get; set; }
    }
}