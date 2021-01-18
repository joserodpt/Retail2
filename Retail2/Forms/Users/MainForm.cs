using Retail2.Classes;
using Retail2.Forms.Admin;
using Retail2.Forms.Read;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Retail2.Forms.Users
{
    public partial class MainForm : UI1
    {
        User logg;
        public MainForm(User u)
        {
            logg = u;
            InitializeComponent();
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

        Form f1;
        Form f2;
        Form f3;
        private bool tabstock;

        private void noPerm()
        {
            MessageBox.Show("Não tens permissão para usar este recurso.", "Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            String perm = logg.PERMISSIONS;
            string[] s = perm.Split(';');
            if (s[1] == "True")
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
            else
            { noPerm(); }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            String perm = logg.PERMISSIONS;
            string[] s = perm.Split(';');
            if (s[0] == "True")
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
            else
            {
                noPerm();
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

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            String perm = logg.PERMISSIONS;
            string[] s = perm.Split(';');
            if (s[2] == "True")
            {
                if (tabstock == false)
                {
                    PictureBox picBox = (PictureBox)(sender);
                    picBox.BorderStyle = BorderStyle.Fixed3D;
                    tabstock = true;

                    if (f3 == null)
                    {
                        Stocks f = new Stocks();
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
                    tabstock = false;
                    f3.Hide();
                }
            }
            else
            {
                noPerm();
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void CozinhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders r = new Orders();
            r.ShowDialog();
        }
    }
}