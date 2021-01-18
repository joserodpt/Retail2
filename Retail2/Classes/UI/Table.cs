using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail2.Classes
{
    public class Table
    {
        public int NUMBER { get; set; }
        public String SIZE { get; set; }
        public String LOC { get; set; }
        public int AVAILABLE { get; set; }
        public int STATUS { get; set; }
        public String INFO { get; set; }
        public Order ORDER { get; set; }
        public Zone ZONE { get; set; }

    }
}
