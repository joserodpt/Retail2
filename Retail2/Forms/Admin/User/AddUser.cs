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
    public partial class AddUser : Form
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Existem valores por preencher.", 1);
                    n.ShowDialog(); return;
                }
            }

            User u = new User();

            u.FIRSTNAME = textBox1.Text;
            u.LASTNAME = textBox2.Text;
            u.ADMIN = checkBox1.Checked;
            u.ONLINE = false;
            u.LOGININFO = "None";
            u.PERMISSIONS = checkBox3.Checked + ";" + checkBox4.Checked + ";" + checkBox2.Checked;
            u.IDENTIFIER = Databases.getIdentifier(Classes.Enum.IdentifierType.USER);
            u.INFO = textBox4.Text;
            u.PASSWORD = Transformer.Encrypt(textBox3.Text, u.IDENTIFIER);

            UserManager.saveUser(u);
            Data.editdone1 = true;
            this.Close();
        }
    }
}
