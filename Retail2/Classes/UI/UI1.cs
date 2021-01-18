using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Classes
{
    public partial class UI1 : Form
    {
        const int WM_NCHITTEST = 0x0084;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x40000;
                cp.ClassStyle = cp.ClassStyle | 0x200;
                return cp;
            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UI1
            // 
            this.ClientSize = new System.Drawing.Size(120, 16);
            this.Name = "UI1";
            this.ResumeLayout(false);

        }


    }
}
