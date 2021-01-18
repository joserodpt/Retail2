using System;

namespace Retail2.Classes
{
    public class User
    {
        public String FIRSTNAME { get; set; }
        public String LASTNAME { get; set; }
        public String PASSWORD { get; set; }
        public Boolean ADMIN { get; set; }
        public Boolean ONLINE { get; set; }
        public String LOGININFO { get; set; }
        //mesas, faturas, stocks
        public String PERMISSIONS { get; set; }
        public String INFO { get; set; }
        public String IDENTIFIER { get; set; }

    }
}
