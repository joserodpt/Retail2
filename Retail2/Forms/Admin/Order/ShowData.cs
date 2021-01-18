using Retail2.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Forms.Admin.Order
{
    public partial class ShowData : Form
    {
        public ShowData(int i, DataTable dt, List<String> l)
        {
            InitializeComponent();

            if (i == 0)
            {
                dataGridView1.Visible = true;
                dataGridView1.DataSource = dt;
                this.Size = dataGridView1.Size;
            }
            if (i == 1)
            {
                listBox1.Visible = true;
                foreach (String s in l)
                {
                    String id = s.Substring(0, 5);
                    if (UserManager.getUser(id) != null)
                    {
                        listBox1.Items.Add(s.Replace(id, UserManager.getUser(id).FIRSTNAME + " " + UserManager.getUser(id).LASTNAME));
                    } else
                    {
                        listBox1.Items.Add(s.Replace(id, "Eliminado"));
                    }
                }
                this.Size = listBox1.Size;
            }
        }

        private void ShowData_Load(object sender, EventArgs e)
        {

        }
    }
}
