using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Forms;
using Retail2.Forms.Admin;
using Retail2.Forms.Users;
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

namespace Retail2
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        List<User> u = new List<User>();

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Estabelecimento: " + SettingsManager.getHouseName();
            if (UserManager.loadUsers().Count != 0)
            {
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

                u = UserManager.loadUsers();
                foreach (User us in u)
                {
                    comboBox1.Items.Add(us.FIRSTNAME + " " + us.LASTNAME);
                }
            } else
            {
                this.Hide();
                Welcome w = new Welcome(false);
                w.FormClosed += new FormClosedEventHandler(s);
                w.ShowDialog();
            }
        }

        private void s(object sender, FormClosedEventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            u = UserManager.loadUsers();
            foreach (User us in u)
            {
                comboBox1.Items.Add(us.FIRSTNAME + " " + us.LASTNAME);
            }
            toolStripStatusLabel2.Text = "Estabelecimento: " + SettingsManager.getHouseName();
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                Notification n = new Notification(Classes.Enum.AlertType.INFO, "Selecione um utilizador.", 1);
                n.ShowDialog();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                Notification n = new Notification(Classes.Enum.AlertType.INFO, "Insira a sua palavra-passe.", 1);
                n.ShowDialog();
                return;
            }

            User user = u[comboBox1.SelectedIndex];

            if (UserManager.compare(user, textBox2.Text))
            {
                if (user.ADMIN == true)
                {
                    this.Hide();

                    user.LOGININFO = Time.get();
                    user.ONLINE = true;
                    UserManager.updateLoginBoth(user);

                    MainFormAdmin f = new MainFormAdmin(user);
                    f.ShowDialog();
                } else
                {
                    this.Hide();

                    user.LOGININFO = Time.get();
                    user.ONLINE = true;
                    UserManager.updateLoginBoth(user);

                    MainForm f = new MainForm(user);
                    f.ShowDialog();
                }

                this.Close();
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Palavra passe incorreta. Se achar que a palavra passe está correta, por favor, contacte um Administrador.", 7);
                n.scrollspeedTicks = 70;
                n.scroll = true;
                n.ShowDialog();
                return;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }
    }
}
