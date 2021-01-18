using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail2.Classes
{
    public class Product
    {
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }
        public Double PRICE { get; set; }
        public Boolean unlimitedSTOCK { get; set; }
        public int STOCK { get; set; }
        public String DATEADDED { get; set; }
        public String CATEGORY { get; set; }
        public String INFO { get; set; }
        public Boolean useOverlay { get; set; }
        public String IDENTIFIER { get; set; }
    }
}
