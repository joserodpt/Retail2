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

namespace Retail2.Forms.Admin.Profile
{
    public partial class EditProfile : Form
    {
        Classes.Profile p;
        public EditProfile(Classes.Profile pp)
        {
            p = pp;
            InitializeComponent();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            p.FIRSTNAME = textBox6.Text;
            p.LASTNAME = textBox2.Text;
            p.ADRESS1 = textBox1.Text;
            p.ADRESS2 = textBox3.Text;
            p.CITY = comboBox1.Text;
            p.STATE = textBox4.Text;
            maskedTextBox2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            p.FISCAL = Int32.Parse(maskedTextBox2.Text);
            p.REFERENCE = textBox5.Text;
            p.INFO = textBox7.Text;
            maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            p.PHONE = Int32.Parse(maskedTextBox1.Text);
            p.EMAIL = err.Text;

            ProfileManager.editProfile(p);
            Data.editdone2 = true;
            this.Close();
        }

        private void EditProfile_Load(object sender, EventArgs e)
        {
            textBox6.Text = p.FIRSTNAME;
            textBox2.Text = p.LASTNAME;
            textBox1.Text = p.ADRESS1;
            textBox3.Text = p.ADRESS2;
            comboBox1.Text = p.CITY;
            textBox4.Text = p.STATE;
            maskedTextBox2.Text = p.FISCAL+ "";
            textBox5.Text = p.REFERENCE;
            textBox7.Text = p.INFO;
            maskedTextBox1.Text = p.PHONE + "";
            err.Text = p.EMAIL;
        }
    }
}
