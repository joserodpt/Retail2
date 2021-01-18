using Retail2.Classes;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Forms.Admin.Settings
{
    public partial class WindowConfigurator : UI1
    {
        public WindowConfigurator()
        {
            InitializeComponent();
        }

        private void WindowConfigurator_Load(object sender, EventArgs e)
        {

        }

        private void WindowConfigurator_Load_1(object sender, EventArgs e)
        {
            this.Size = Databases.getSize(SettingsManager.getWindowSize(0));
            this.CenterToScreen();
        }

        Boolean formmax = false;

        private void Max_Click(object sender, EventArgs e)
        {
            if (formmax == false)
            {
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
                formmax = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                formmax = false;
            }
        }

        private void Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsManager.setWindowSize(0, this.Size.Width + ";" + this.Size.Height);
            if (f != null)
            {
                SettingsManager.setWindowSize(1, f.Size.Width + ";" + f.Size.Height);
            }
            this.Close();
        }

        private void WindowConfigurator_SizeChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Tamanho desta Janela: " + this.Size.Width + " por " + this.Size.Height;
        }

        Boolean show = false;
        Form f;
        private void Button1_Click(object sender, EventArgs e)
        {
            if (show == false)
            {
                f = new DummyWindow();
                f.TopLevel = false;

                f.Size = Databases.getSize(SettingsManager.getWindowSize(1));


                panel1.Controls.Add(f);
                f.Show();

                show = true;
            } else
            {
                f.Hide();
                f.Show();
            }
        }
    }
}
