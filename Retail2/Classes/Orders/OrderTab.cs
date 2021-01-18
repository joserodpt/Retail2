using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Retail2.Classes
{
    public class OrderTab : TabPage
    {
        public Boolean Empty { get; set; }
        public Order Order { get; set; }
        public DataTable List { get; set; }
        public List<String> IndexIdentifiers{ get; set; }
    }
}
