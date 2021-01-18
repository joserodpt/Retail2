using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Retail2.Forms.Admin.Products
{
    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    Notification n = new Notification(Classes.Enum.AlertType.WARNING, "Valores por preencher.", 1);
                    n.ShowDialog(); return;
                }
            }

            Double res;
            bool isDouble = Double.TryParse(textBox3.Text, out res);

            if (!isDouble)
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "O preço não é um número.", 1);
                n.ShowDialog();
                return;
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
                            not.ShowDialog();
                            return;
                        }
                        Notification n = new Notification(Classes.Enum.AlertType.ERROR, "A Quantidade não é um número.", 1);
                        n.ShowDialog();
                        return;
                    }
                }
                {
                    String ident = Databases.getIdentifier(Classes.Enum.IdentifierType.PRODUCT);

                    if (pictureBox1.Image != null)
                    {
                        Imaging.saveImage(new Bitmap(pictureBox1.Image), 100, 78, 100, SettingsManager.getDataPath() + @"\Images\Products\" + ident + ".jpg");
                    }

                    Product p = new Product();
                    p.NAME = textBox2.Text;
                    p.PRICE = res;
                    p.DESCRIPTION = textBox5.Text;
                    p.CATEGORY = comboBox1.Text;
                    p.unlimitedSTOCK = checkBox1.Checked;
                    
                    if (valid == true)
                    {
                        p.STOCK = stock;
                    } else
                    {
                        p.STOCK = stockdef;
                    }

                    p.useOverlay = checkBox2.Checked;
                    p.INFO = textBox4.Text;
                    p.IDENTIFIER = ident;
                    p.DATEADDED = Time.get();

                    ProductManager.saveProduct(p);
                    this.Close();
                }
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhuma categoria selecionada.", 1);
                n.ShowDialog();
            }
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

        private void AddProduct_Load(object sender, EventArgs e)
        {
            foreach (Category c in CategoryManager.loadCategories())
            {
                comboBox1.Items.Add(c.NAME);
            }

            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.Filter = "JPG (*.jpg)|*.jpg| PNG (*.png)|*.png";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        ToolTip toolTip1 = new ToolTip();
        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
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
