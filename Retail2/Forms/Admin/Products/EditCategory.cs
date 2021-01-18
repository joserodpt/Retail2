using Retail2.Classes;
using Retail2.Classes.UI;
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

namespace Retail2.Forms.Admin.Products
{
    public partial class EditCategory : Form
    {
        Category curr;
        public EditCategory(Category c)
        {
            curr = c;
            InitializeComponent();
        }

        private void EditCategory_Load(object sender, EventArgs e)
        {
            label3.Text = "Nome Original: " + curr.NAME;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                curr.NAME = textBox1.Text;

                CategoryManager.editCat(curr);
                this.Close();
            } else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nome inválido.", 1);
                n.ShowDialog();
            }
        }
    }
}
