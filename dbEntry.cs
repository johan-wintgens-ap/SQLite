using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLlite
{
    class dbEntry
    {
        public int id { get; set; }
        public string naamVak { get; set; }
        public int score { get; set; }
        public DateTime datum { get; set; }
        public string opmerking { get; set; }
        public int credit { get; set; }

        public override string ToString()
        {
            return naamVak;
        }
    }
}
