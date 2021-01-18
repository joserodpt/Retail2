using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Classes
{
    public class TableUI : PictureBox
    {
        public Table Table { get; set; }
    }
    public class TableUIAdmin : Button
    {
        public Table Table { get; set; }

        public TableUIAdmin()
        {
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        }
    }
}
