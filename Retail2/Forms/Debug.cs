using Retail2.Classes.UI;
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

namespace Retail2.Forms
{
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AlertType at = AlertType.SUCESS;

            if (textBox2.Text == "0")
            {
                at = AlertType.SUCESS;
            }
            if (textBox2.Text == "1")
            {
                at = AlertType.ERROR;
            }
            if (textBox2.Text == "2")
            {
                at = AlertType.WARNING;
            }
            if (textBox2.Text == "3")
            {
                at = AlertType.INFO;
            }
            if (textBox2.Text == "4")
            {
                at = AlertType.QUESTION;
            }

            Notification n = new Notification(at, textBox1.Text, Int32.Parse(textBox3.Text));
            n.scrollspeedTicks = Int32.Parse(textBox4.Text);
            n.scroll = checkBox1.Checked;
            n.ShowDialog();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            AlertType at = AlertType.SUCESS;

            if (textBox2.Text == "0")
            {
                at = AlertType.SUCESS;
            }
            if (textBox2.Text == "1")
            {
                at = AlertType.ERROR;
            }
            if (textBox2.Text == "2")
            {
                at = AlertType.WARNING;
            }
            if (textBox2.Text == "3")
            {
                at = AlertType.INFO;
            }
            if (textBox2.Text == "4")
            {
                at = AlertType.QUESTION;
            }

            Notification n = new Notification(at, richTextBox1.Text, Int32.Parse(textBox3.Text));
            n.ShowDialog();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
