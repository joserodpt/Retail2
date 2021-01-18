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
    public partial class EditProduct : Form
    {
        public Product prod;
        public EditProduct(Product p)
        {
            InitializeComponent();
            prod = p;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupBox2.Size = new Size(102, 60);
            }
            else
            {
                groupBox2.Size = new Size(272, 60);
            }
        }

        private void EditProduct_Load(object sender, EventArgs e)
        {
            foreach (Category c in CategoryManager.loadCategories())
            {
                comboBox1.Items.Add(c.NAME);
            }

            textBox2.Text = prod.NAME;
            textBox3.Text = prod.PRICE + "";
            textBox5.Text = prod.DESCRIPTION;
            textBox4.Text = prod.INFO;
            comboBox1.SelectedIndex = comboBox1.FindStringExact(prod.CATEGORY);
            checkBox1.Checked = prod.unlimitedSTOCK;
            textBox1.Text = prod.STOCK + "";
            checkBox2.Checked = prod.useOverlay;

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Existem valores por preencher.", 1);
                    n.ShowDialog(); return;
                }
            }

            Double res;
            bool isDouble = Double.TryParse(textBox3.Text, out res);

            if (!isDouble)
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "O Preço não é um número.", 1);
                n.ShowDialog(); return;
            }

            int stockdef = -1;
            int stock;
            bool valid = Int32.TryParse(textBox1.Text, out stock);
            if (comboBox1.SelectedIndex > -1)
            {
                if (checkBox1.Checked == false)
                {
                    if (string.IsNullOrEmpty(textBox1.Text.Trim()))
                    {
                        if (!valid)
                        {
                            Notification not = new Notification(Classes.Enum.AlertType.ERROR, "A Quantidade não é um número.", 1);
                            not.ShowDialog(); return;
                        }
                        Notification n = new Notification(Classes.Enum.AlertType.ERROR, "A Quantidade não é um número.", 1);
                        n.ShowDialog(); return;
                    }
                }
                {
                    prod.NAME = textBox2.Text;
                    prod.PRICE = res;
                    prod.DESCRIPTION = textBox5.Text;
                    prod.CATEGORY = comboBox1.Text;
                    prod.unlimitedSTOCK = checkBox1.Checked;

                    if (valid == true)
                    {
                        prod.STOCK = stock;
                    }
                    else
                    {
                        prod.STOCK = stockdef;
                    }

                    prod.useOverlay = checkBox2.Checked;
                    prod.INFO = textBox4.Text;

                    ProductManager.editProduct(prod);
                    this.Close();
                }
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhuma categoria selecionada.", 1);
                n.ShowDialog();
            }
        }

        ToolTip toolTip1 = new ToolTip();
        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text.Length > 15)
            {
                toolTip1.ToolTipTitle = "O nome é maior do que 15 caracteres.";
                toolTip1.ToolTipIcon = ToolTipIcon.Warning;
                toolTip1.IsBalloon = false;
                toolTip1.SetToolTip(textBox1, "O nome poderá não mostrar corretamente nas Janelas Fatura e Mesa.");
                toolTip1.Show("O nome poderá não mostrar corretamente nas Janelas Fatura e Mesa.", textBox2, 5, textBox2.Height - 5);
            }
            else
            {
                toolTip1.Hide(textBox1);
            }
        }
    }
}
