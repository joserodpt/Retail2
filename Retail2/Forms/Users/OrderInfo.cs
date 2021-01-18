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

namespace Retail2.Forms.Users
{
    public partial class OrderInfo : Form
    {
        Order u;
        public OrderInfo(Order o)
        {
            u = o;
            InitializeComponent();
        }

        private void OrderInfo_Load(object sender, EventArgs e)
        {
            this.Text = "Pedido ID " + u.IDENTIFIER;
            textBox1.Text = UserManager.getUserFirstName(u.CREATORUSERID);

            if (u.ORDERTYPE == 0)
            {
                textBox3.Text = "Regular";
            }
            if (u.ORDERTYPE == 1)
            {
                textBox3.Text = "Mesa";
            }

            if (u.TABLE != -1)
            {
                textBox4.Text = u.TABLE + "";
            }
            else
            {
                textBox4.Text = "Nenhuma";
            }
            textBox5.Text = u.DATECREATED;
            foreach (String s in Databases.uncompactList(u.EVENTS))
            {
                String id = s.Substring(0, 5);
                listBox1.Items.Add(s.Replace(id, UserManager.getUser(id).FIRSTNAME + " " + UserManager.getUser(id).LASTNAME));
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
