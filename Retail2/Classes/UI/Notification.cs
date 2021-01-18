using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Retail2.Classes.Enum;

namespace Retail2.Classes.UI
{
    public partial class Notification : Form
    {
        public int seconds;

        public Boolean scroll = false;
        public int scrollspeedTicks = 400;

        public Notification(AlertType type, String text, int seconds)
        {
            InitializeComponent();

            this.seconds = seconds;

            switch(type)
            {
                case AlertType.SUCESS:
                    this.BackColor = Color.Green;
                    pictureBox1.Image = Retail2.Properties.Resources.sucess;
                    break;
                case AlertType.ERROR:
                    this.BackColor = Color.Red;
                    pictureBox1.Image = Retail2.Properties.Resources.error;
                    break;
                case AlertType.WARNING:
                    this.BackColor = Color.FromArgb(255, 207, 0);
                    pictureBox1.Image = Retail2.Properties.Resources.warning;
                    break;
                case AlertType.QUESTION:
                    this.BackColor = Color.FromArgb(1, 96, 218);
                    pictureBox1.Image = Retail2.Properties.Resources.ques;
                    break;
                case AlertType.INFO:
                    this.BackColor = Color.FromArgb(18, 14, 126);
                    pictureBox1.Image = Retail2.Properties.Resources.infoWhite;
                    break;
            }

            label1.Text = text;
        }
        public Notification(AlertType type, List<String> texts, int seconds)
        {
            InitializeComponent();

            this.seconds = seconds;

            switch (type)
            {
                case AlertType.SUCESS:
                    this.BackColor = Color.Green;
                    listBox1.BackColor = this.BackColor;
                    pictureBox1.Image = Retail2.Properties.Resources.sucess;
                    break;
                case AlertType.ERROR:
                    this.BackColor = Color.Red;
                    listBox1.BackColor = this.BackColor;
                    pictureBox1.Image = Retail2.Properties.Resources.error;
                    break;
                case AlertType.WARNING:
                    this.BackColor = Color.Yellow;
                    listBox1.BackColor = this.BackColor;
                    pictureBox1.Image = Retail2.Properties.Resources.warning;
                    break;
                case AlertType.QUESTION:
                    this.BackColor = Color.CadetBlue;
                    listBox1.BackColor = this.BackColor;
                    pictureBox1.Image = Retail2.Properties.Resources.ques;
                    break;
                case AlertType.INFO:
                    this.BackColor = Color.LightBlue;
                    listBox1.BackColor = this.BackColor;
                    pictureBox1.Image = Retail2.Properties.Resources.infoWhite;
                    break;
            }

            listBox1.Visible = true;
            listBox1.Items.AddRange(texts.ToArray());
        }


        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Alert_Load(object sender, EventArgs e)
        {
            if (scroll == true)
            {
                waitpls.Start();
            }

            this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height - 50;
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width - 10;
        }

        int elapsed = 0;

        private void Timer1_Tick(object sender, EventArgs e)
        {
            elapsed += 1;
            if (seconds == elapsed)
            {
                t1.Tick += new EventHandler(fadeOut);
                t1.Start();
            }
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            char[] chars = label1.Text.ToCharArray();
            char[] newChar = new char[chars.Length];
            int l = chars.Length;
            int k = 0;
            for (int j = 0; j < chars.Length; j++)
            {

                if (j + 1 < chars.Length)
                    newChar[j] = chars[j + 1];
                else
                {
                    newChar[l - 1] = chars[k];
                    //  k++;
                }
            }
            label1.Text = new string(newChar);
        }

        int b = 0;
        private void Waitpls_Tick(object sender, EventArgs e)
        {
            if (b == 1)
            {
                waitpls.Stop();
                label1.Text = label1.Text + "    ";
                scrollTimer.Interval = scrollspeedTicks;
                scrollTimer.Start();
            }
            b += 1;
        }

        private void Notification_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;   

            t1.Tick += new EventHandler(fadeOut);
            t1.Start();

            if (Opacity == 0) 
                e.Cancel = false;
        }

        private void fadeOut(object sender, EventArgs e)
        {
            if (Opacity <= 0)     //check if opacity is 0
            {
                t1.Stop();    //if it is, we stop the timer
                Close();   //and we try to close the form
            }
            else
                Opacity -= 0.05;
        }
    }
}
