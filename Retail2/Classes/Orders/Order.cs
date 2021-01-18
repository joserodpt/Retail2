using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retail2.Classes.Enum;

namespace Retail2.Classes
{
    public class Order
    {
        public int ID { get; set; }
        public String CREATORUSERID { get; set; }
        //mesas = 1; fat = 0
        public int ORDERTYPE { get; set; }
        public String CONTENTLIST { get; set; }
        public Boolean Empty { get; set; }
        public String INDEXLIST { get; set; }
        public String EVENTS { get; set; }
        public Double VALUE { get; set; }
        public int TABLE { get; set; }
        public String DATECREATED { get; set; }
	    public String DATECLOSED { get; set; }
        public Boolean DONE { get; set; }
	    public String INFO { get; set; }
		public Boolean OCCURRENCE { get; set; }
		public String OCCURRENCEINFO { get; set; }
        public String PAYMENTDETAILS { get; set; }
        public String PEOPLEPROFILEID { get; set; }
		public String IDENTIFIER { get; set; }
    }
}
