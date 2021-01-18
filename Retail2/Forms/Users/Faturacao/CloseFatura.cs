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
    public partial class CloseFatura : Form
    {
        Order closing;
        Profile selected;
        public CloseFatura(Order o)
        {
            closing = o;
            InitializeComponent();
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked != true)
            {
                groupBox3.Size = new Size(81, 40);
            }
            else
            {
                groupBox3.Size = new Size(606, 115);
            }
        }

        private void CloseFatura_Load(object sender, EventArgs e)
        {
            label3.Text = closing.VALUE + "€";
            this.Text = this.Text + " " + closing.IDENTIFIER;
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            closing.DONE = true;
            closing.DATECLOSED = Time.get();
            closing.OCCURRENCE = checkBox2.Checked;
            List<String> l = new List<string>();
            foreach (String il in richTextBox1.Lines)
            {
                l.Add(il);
            }
            closing.OCCURRENCEINFO = Databases.compactList(l);
            List<String> l2 = new List<string>();
            foreach (String il in richTextBox2.Lines)
            {
                l2.Add(il);
            }
            closing.INFO = Databases.compactList(l2);
            
            if (selected != null)
            {
                closing.PEOPLEPROFILEID = selected.IDENTIFIER;
            } else
            {
                closing.PEOPLEPROFILEID = "Nenhum";
            }

            if (OrderManager.orderExistsNormal(closing))
            {
                OrderManager.editOrder(closing);
            } else {
                OrderManager.saveOrder(closing);
            }

            Data.checkoutDone = true;
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ProfileSearch pf = new ProfileSearch();
            pf.FormClosed += new FormClosedEventHandler(selProf);
            pf.ShowDialog();
        }

        private void selProf(object sender, FormClosedEventArgs e)
        {
            if (Data.profselect == true)
            {
                selected = Data.prof;
                textBox1.Text = selected.FIRSTNAME + " " + selected.LASTNAME;
                Data.profselect = false;
            }
        }
    }
}
