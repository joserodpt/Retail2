using System;
using System.Windows.Forms;

namespace Retail2.Classes
{
    public partial class UI2 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | 0x200;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
