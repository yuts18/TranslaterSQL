using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslaterSQL
{
    public class FlashCard
    {
        public int Id { get; set; }
        public string Original { get; set; }
        public string Translation { get; set; }
        public int IntervalDays { get; set; }
        public float EaseFactor { get; set; }
        public int Repetitions { get; set; }
    }
}
