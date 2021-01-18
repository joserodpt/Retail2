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

namespace Retail2.Forms.Admin.Products
{
    public partial class AddCategory : Form
    {
        public AddCategory()
        {
            InitializeComponent();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                Category c = new Category();
                c.NAME = textBox1.Text;
                c.IDENTIFIER = Databases.getIdentifier(Classes.Enum.IdentifierType.CATEGORY);

                CategoryManager.saveCat(c);
                this.Close();
            } else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Introdução inválida.", 1);
                n.ShowDialog();
            }
        }
    }
}
