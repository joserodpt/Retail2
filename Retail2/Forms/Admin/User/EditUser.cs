using Retail.Managers;
using Retail2.Classes;
using Retail2.Classes.UI;
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

namespace Retail2.Forms.Admin
{
    public partial class EditUser : Form
    {
        User current;
        bool normal;

        public EditUser(User u, Boolean ed)
        {
            current = u;
            normal = ed;
            InitializeComponent();
        }

        private void EditUser_Load(object sender, EventArgs e)
        {
            textBox1.Text = current.FIRSTNAME;
            textBox2.Text = current.LASTNAME;
            textBox3.Text = Transformer.Decrypt(current.PASSWORD, current.IDENTIFIER);
            checkBox1.Checked = current.ADMIN;

            String perms = current.PERMISSIONS;
            string[] tok = perms.Split(';');
            if (tok[0] == "True")
            {
                checkBox3.Checked = true;
            }
            if (tok[1] == "True")
            {
                checkBox4.Checked = true;
            }
            if (tok[2] == "True")
            {
                checkBox2.Checked = true;
            }

            textBox4.Text = current.INFO;

            if (normal)
            {
                groupBox2.Visible = false;
            }
        }

        private void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox5.Checked)
            {
                textBox3.PasswordChar = '*';
            } else
            {
                textBox3.PasswordChar = '\0';
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    Notification not = new Notification(Classes.Enum.AlertType.ERROR, "Existem valores por preencher.", 1);
                    not.ShowDialog(); return;
                }
            }

            User u = new User();

            u.FIRSTNAME = textBox1.Text;
            u.LASTNAME = textBox2.Text;
            u.ADMIN = checkBox1.Checked;
            u.ONLINE = false;
            u.LOGININFO = current.LOGININFO;
            u.PERMISSIONS = checkBox3.Checked + ";" + checkBox4.Checked + ";" + checkBox2.Checked;
            u.IDENTIFIER = current.IDENTIFIER;
            u.INFO = textBox4.Text;
            u.PASSWORD = Transformer.Encrypt(textBox3.Text, u.IDENTIFIER);

            Data.editdone1 = true;
            UserManager.editUser(u);
            this.Close();
        }
    }
}
