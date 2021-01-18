using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Forms.Read;
using Retail2.Forms.Users;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Retail2.Forms.Admin
{
    public partial class MainFormAdmin : UI1
    {
        User logg;
        public MainFormAdmin(User u)
        {
            logg = u;
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = Databases.getSize(SettingsManager.getWindowSize(0));
            this.CenterToScreen();

            toolStripStatusLabel1.Text = "Utilizador: " + logg.FIRSTNAME + " " + logg.LASTNAME + " | Último Login: " + logg.LOGININFO;
        }

        private void MenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private bool fatbut;
        private bool tabbut;
        private bool admbut;
        private bool messbut;

        Form f1;
        Form f2;
        Form f3;
        Form f4;
        Form f5;
        private bool tabstock;

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (fatbut == false)
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.Fixed3D;
                fatbut = true;

                if (f1 == null)
                {
                    Faturação f = new Faturação(logg);
                    f.TopLevel = false;
                    panel1.Controls.Add(f);
                    f.Show();
                    f1 = f;
                }
                else
                {
                    f1.Show();
                }
            }
            else
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.None;
                fatbut = false;
                f1.Hide();
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            if (tabbut == false)
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.Fixed3D;
                tabbut = true;

                if (f2 == null)
                {
                    MesasFo f = new MesasFo(logg);
                    f.TopLevel = false;
                    panel1.Controls.Add(f);
                    f.Show();
                    f2 = f;
                }
                else
                {
                    f2.Show();
                }
            }
            else
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.None;
                tabbut = false;
                f2.Hide();
            }
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            if (admbut == false)
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.Fixed3D;
                admbut = true;

                if (f3 == null)
                {
                    Administrar f = new Administrar(logg);
                    f.TopLevel = false;
                    panel1.Controls.Add(f);
                    f.Show();
                    f3 = f;
                }
                else
                {
                    f3.Show();
                }
            }
            else
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.None;
                admbut = false;
                f3.Hide();
            }
        }

        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logg.ONLINE = false;
            UserManager.updateLoginState(logg);

            ConfigurationManager.RefreshSection("appSettings");

            this.Hide();
            Login l = new Login();
            l.ShowDialog();
            this.Close();
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            if (tabstock == false)
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.Fixed3D;
                tabstock = true;

                if (f4 == null)
                {
                    Stocks f = new Stocks();
                    f.TopLevel = false;
                    panel1.Controls.Add(f);
                    f.Show();
                    f4 = f;
                }
                else
                {
                    f4.Show();
                }
            }
            else
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.None;
                tabstock = false;
                f4.Hide();
            }
        }

        private void CozinhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders o = new Orders();
            o.ShowDialog();
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            logg.ONLINE = false;
            UserManager.updateLoginState(logg);

            ConfigurationManager.RefreshSection("appSettings");

            this.Hide();
            Login l = new Login();
            l.ShowDialog();
            this.Close();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            EditUser a = new EditUser(logg, true);
            a.Text = "Definições da Conta";
            a.ShowDialog();
        }

        private void SobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (tabstock == false)
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.Fixed3D;
                tabstock = true;

                if (f5 == null)
                {
                    MessageBoard f = new MessageBoard(logg);
                    f.TopLevel = false;
                    panel1.Controls.Add(f);
                    f.Show();
                    f5 = f;
                }
                else
                {
                    f5.Show();
                }
            }
            else
            {
                PictureBox picBox = (PictureBox)(sender);
                picBox.BorderStyle = BorderStyle.None;
                tabstock = false;
                f5.Hide();
            }
        }
    }
}