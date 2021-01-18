using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Forms.Admin.Settings
{
    public partial class SettingsForm : UI1
    {
        SettingsManager f = new SettingsManager();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SettingsManager.setDataPath(textBox1.Text);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = SettingsManager.getDataPath();
            Double d = Double.Parse(SettingsManager.getRefreshML());
            textBox2.Text = Time.ConvertMillisecondsToSeconds(d) + "";
            textBox3.Text = SettingsManager.getHouseName();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = AppDomain.CurrentDomain.BaseDirectory + @"Data";
        }

        private void Contastrip_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Button4_Click(object sender, EventArgs e)
        {
            ConfigurationManager.RefreshSection("appSettings");

            WindowConfigurator f = new WindowConfigurator();
            f.ShowDialog();

            Notification n = new Notification(Classes.Enum.AlertType.INFO, "As alterações serão aplicadas no próximo login.", 3);
            n.scrollspeedTicks = 70;
            n.scroll = true;
            n.ShowDialog();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = "1500";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            SettingsManager.setRefresh(Time.ConvertSecondsToMilliseconds(Double.Parse(textBox2.Text)));
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            SettingsManager.setHouseName(textBox3.Text);
        }
    }
}
