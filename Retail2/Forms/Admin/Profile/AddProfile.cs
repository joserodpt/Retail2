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

namespace Retail2.Forms.Admin.Profile
{
    public partial class AddProfile : Form
    {
        public AddProfile()
        {
            InitializeComponent();
        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void AddProfile_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (err.Text.Contains("@"))
            {
                if (err.Text.Contains(".com") || err.Text.Contains(".pt"))
                {
                    Notification n = new Notification(Classes.Enum.AlertType.SUCESS, "E-Mail correto.", 1);
                    n.ShowDialog();
                }
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.SUCESS, "E-Mail desconhecido.", 1);
                n.ShowDialog();
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Classes.Profile p = new Classes.Profile();

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
            p.IDENTIFIER = Databases.getIdentifier(Classes.Enum.IdentifierType.PROFILE);
            p.DATECREATED = Utils.Time.get();

            ProfileManager.saveProfile(p);
            Data.editdone2 = true;
            this.Close();
        }
    }
}
