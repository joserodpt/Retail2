using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Retail2.Forms.Users
{
    public partial class Stocks : Form
    {
        public Stocks()
        {
            InitializeComponent();
        }

        private void Stocks_Load(object sender, EventArgs e)
        {
            List<Product> prods = new List<Product>();

            toolStripComboBox1.Items.Add("Tudo");
            toolStripComboBox1.SelectedIndex = 0;

            toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            toolStripComboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            toolStripComboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            foreach (Product p in ProductManager.loadProducts())
            {
                prods.Add(p);
                toolStripComboBox1.Items.Add(p.NAME);
            }
            showProducts(0, prods);
        }

        private void showProducts(int mode, List<Product> prods)
        {
            foreach (Control o in panel1.Controls)
            {
                o.Dispose();
            }

            Point[] p = new Point[prods.Count];

            StockViewer[] ord = new StockViewer[prods.Count];
            for (int i = 0; i < ord.GetLength(0); i++)
            {
                ord[i] = new StockViewer(prods[i], mode);
                //ord[i].Click += (sender2, e2) => addItem(sender2, e2, ide);

                p[i] = new Point();
                p[i].X = i * 7;
                p[i].Y = 0;

                ord[i].PointToClient(p[i]);
                ord[i].Show();
            }

            FlowLayoutPanel pan = new FlowLayoutPanel();
            pan.Size = panel1.Size;
            pan.Controls.AddRange(ord);
            pan.AutoScroll = true;
            pan.Dock = DockStyle.Fill;

            panel1.Controls.Add(pan);
        }

        private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text == "Tudo")
            {
                List<Product> prods2 = new List<Product>();

                foreach (Product p in ProductManager.loadProducts())
                {
                    prods2.Add(p);
                }
                showProducts(0, prods2);
                return;
            }


            List<Product> prods = new List<Product>();

            foreach (Product p in ProductManager.loadProducts())
            {

                if (p.NAME.Contains(toolStripComboBox1.Text))
                {
                    prods.Add(p);
                }
            }
            showProducts(1, prods);
        }
    }
}
