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
    public partial class ProfileSearch : Form
    {
        public ProfileSearch()
        {
            InitializeComponent();

            comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        List<Profile> pl = new List<Profile>();

        private void ProfileSearch_Load(object sender, EventArgs e)
        {
            pl = ProfileManager.loadProfiles();
            foreach (Profile p in pl)
            {
                comboBox2.Items.Add(p.FIRSTNAME + " " + p.LASTNAME);
            }
        }

        Profile sel;

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Profile p = pl[comboBox2.SelectedIndex];
            sel = p;

            textBox6.Text = p.FIRSTNAME;
            textBox2.Text = p.LASTNAME;
            textBox1.Text = p.ADRESS1;
            textBox3.Text = p.ADRESS2;
            comboBox1.Text = p.CITY;
            textBox4.Text = p.STATE;
            maskedTextBox2.Text = p.FISCAL + "";
            textBox5.Text = p.REFERENCE;
            textBox7.Text = p.INFO;
            maskedTextBox1.Text = p.PHONE + "";
            err.Text = p.EMAIL;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Data.profselect = true;
            Data.prof = sel;
            this.Close();
        }
    }
}
