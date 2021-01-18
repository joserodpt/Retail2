using Retail2.Classes.UI;
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
    public partial class AddForeignProduct : Form
    {
        public AddForeignProduct()
        {
            InitializeComponent();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            double price;
            bool isDouble = Double.TryParse(textBox3.Text, out price);

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                Notification n = new Notification(Classes.Enum.AlertType.INFO, "Insira um preço.", 1);
                n.ShowDialog(); return;
            }

            if (!isDouble)
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "O Preço não é um número.", 1);
                n.ShowDialog(); return;
            }

            Data.done = true;
            Data.id = "Added By User";
            Data.name = textBox2.Text;
            Data.price = price;

            this.Close();
        }
    }
}
