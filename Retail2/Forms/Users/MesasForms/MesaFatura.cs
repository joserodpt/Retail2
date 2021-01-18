using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Retail2.Forms.Users.Mesas
{
    public partial class MesaFatura : Form
    {
        User log;
        TableUI ord;

        public MesaFatura(User u, TableUI t)
        {
            ord = t;
            log = u;
            InitializeComponent();
            this.Text = "Pedido da Mesa " + ord.Table.NUMBER;
        }

        Size lastSize;
        private void MesaFatura_ResizeEnd(object sender, EventArgs e)
        {
            if (lastSize != this.Size)
            {
                lastSize = this.Size;
                filterProducts();
                displayProducts();
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex < toolStripComboBox1.Items.Count - 1)
            {
                toolStripComboBox1.SelectedIndex = toolStripComboBox1.SelectedIndex + 1;
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex > 0)
            {
                toolStripComboBox1.SelectedIndex = toolStripComboBox1.SelectedIndex - 1;
            }
        }

        private void filterProducts()
        {
            show.Clear();

            foreach (Product p in ProductManager.loadProducts())
            {
                if (toolStripComboBox1.Text == "Todas")
                {
                    show.Add(p);
                }
                if (toolStripComboBox1.Text == p.CATEGORY)
                {
                    show.Add(p);
                }
            }
        }

        List<Product> show = new List<Product>();

        private void displayProducts()
        {
            foreach (Control c in panel1.Controls)
            {
                c.Hide();
                c.Dispose();
                panel1.Controls.Remove(c);
            }

            Point[] p = new Point[show.Count];

            Shower[] prod = new Shower[show.Count];
            for (int i = 0; i < prod.GetLength(0); i++)
            {
                String name = show[i].NAME;
                String ide = show[i].IDENTIFIER;
                prod[i] = new Shower();
                prod[i].Height = 90;
                prod[i].Width = 90;
                prod[i].Name = "shower";
                prod[i].ProductIdentifier = ide;
                prod[i].Click += (sender2, e2) => addItem(sender2, e2, ide);
                prod[i].SizeMode = PictureBoxSizeMode.StretchImage;

                p[i] = new Point();
                p[i].X = i * 7;
                p[i].Y = 0;

                if (show[i].useOverlay == true)
                {
                    prod[i].Image = Imaging.makeShower(Databases.getImage(show[i].IDENTIFIER));
                }
                else
                {
                    prod[i].Image = Databases.getImage(show[i].IDENTIFIER);
                }

                prod[i].Paint += new PaintEventHandler((sender, e) =>
                {
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    string text = name;

                    SizeF textSize = e.Graphics.MeasureString(text, Font);
                    PointF locationToDraw = new PointF();
                    locationToDraw.X = (90 / 2) - (textSize.Width / 2);
                    locationToDraw.Y = 74;

                    e.Graphics.DrawString(text, Font, Brushes.Black, locationToDraw);
                });

                prod[i].PointToClient(p[i]);
                prod[i].Show();
            }

            FlowLayoutPanel pan = new FlowLayoutPanel();
            pan.Location = new Point(0, 0);
            pan.Size = panel1.Size;
            pan.Controls.AddRange(prod);
            pan.AutoScroll = true;

            panel1.Controls.Add(pan);
        }
        List<string> IndexIdentifiers = new List<string>();
        private void addItem(object sender2, EventArgs e2, string ide)
        {
            Product p = ProductManager.getProduct(ide);

            if (p.unlimitedSTOCK == false)
            {
                if (p.STOCK >= 1)
                {
                    p.STOCK -= 1;
                    ProductManager.editStockNumb(p);

                    DataTable dataTable = (DataTable)ver.DataSource;
                    DataRow drToAdd = dataTable.NewRow();

                    drToAdd["Produto"] = p.NAME;
                    drToAdd["Valor"] = p.PRICE + "€";

                    dataTable.Rows.Add(drToAdd);
                    dataTable.AcceptChanges();

                    ord.Table.ORDER.VALUE += p.PRICE;

                    if (IndexIdentifiers == null)
                    {
                        IndexIdentifiers = new List<string>();
                    }
                    IndexIdentifiers.Add(p.IDENTIFIER);

                    if (dataTable.Rows.Count > 0)
                    {
                        ord.Table.ORDER.Empty = false;
                    }

                    label4.Text = ord.Table.ORDER.VALUE + "€";

                    if (ord.Table.ORDER.EVENTS == null)
                    {
                        ord.Table.ORDER.EVENTS = log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")";
                    }
                    else
                    {
                        ord.Table.ORDER.EVENTS = String.Concat(ord.Table.ORDER.EVENTS, "§" + log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")");
                    }
                }
                else
                {
                    Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Produto esgotado.", 1);
                    n.ShowDialog();
                }
            } else
            {
                DataTable dataTable = (DataTable)ver.DataSource;
                DataRow drToAdd = dataTable.NewRow();

                drToAdd["Produto"] = p.NAME;
                drToAdd["Valor"] = p.PRICE + "€";

                dataTable.Rows.Add(drToAdd);
                dataTable.AcceptChanges();

                ord.Table.ORDER.VALUE += p.PRICE;

                if (IndexIdentifiers == null)
                {
                    IndexIdentifiers = new List<string>();
                }
                IndexIdentifiers.Add(p.IDENTIFIER);

                if (dataTable.Rows.Count > 0)
                {
                    ord.Table.ORDER.Empty = false;
                }

                label4.Text = ord.Table.ORDER.VALUE + "€";

                if (ord.Table.ORDER.EVENTS == null)
                {
                    ord.Table.ORDER.EVENTS = log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")";
                }
                else
                {
                    ord.Table.ORDER.EVENTS = String.Concat(ord.Table.ORDER.EVENTS, "§" + log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")");
                }
            }
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            OrderInfo d = new OrderInfo(ord.Table.ORDER);
            d.ShowDialog();
        }

        private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterProducts();
            displayProducts();
        }

        private void Ver_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && ver.CurrentCell.Selected)
            {
                int selectedrowindex = ver.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = ver.Rows[selectedrowindex];

                Product p = ProductManager.getProduct(IndexIdentifiers[selectedrowindex]);

                if (IndexIdentifiers[selectedrowindex] == "Added By User")
                {
                    string a = Convert.ToString(selectedRow.Cells["Produto"].Value);
                    Double d = Convert.ToDouble(selectedRow.Cells["Valor"].Value.ToString().Replace("€", ""));

                    ord.Table.ORDER.EVENTS = String.Concat(ord.Table.ORDER.EVENTS, "§" + log.IDENTIFIER + " removeu " + a + " (" + Time.get() + ")");
                    ord.Table.ORDER.VALUE = ord.Table.ORDER.VALUE - d;
                    label4.Text = ord.Table.ORDER.VALUE + "€";
                }
                else
                {
                    ord.Table.ORDER.EVENTS = String.Concat(ord.Table.ORDER.EVENTS, "§" + log.IDENTIFIER + " removeu " + p.NAME + " (" + Time.get() + ")");
                    ord.Table.ORDER.VALUE = ord.Table.ORDER.VALUE - p.PRICE;
                    label4.Text = ord.Table.ORDER.VALUE + "€";
                }

                ver.Rows.RemoveAt(selectedrowindex);
                IndexIdentifiers.RemoveAt(selectedrowindex);

                if (ver.Rows.Count == 0)
                {
                    ord.Table.ORDER.Empty = true;
                }

                if (p.unlimitedSTOCK == false)
                {
                    p.STOCK += 1;
                    ProductManager.editStockNumb(p);
                }
            }
        }

        private void MesaFatura_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveOrder();
        }

        private void saveOrder()
        {
            Order o = ord.Table.ORDER;

            o.CONTENTLIST = Databases.compactData(((DataTable)ver.DataSource));
            o.INDEXLIST = Databases.compactList(IndexIdentifiers);

            ord.Table.ORDER = o;

            if (!OrderManager.orderExistsNormal(o))
            {
                OrderManager.saveOrder(o);
            }
            else
            {
                OrderManager.editOrder(o);
            }

            Table t = new Table();
            t.NUMBER = ord.Table.ORDER.TABLE;

            t.AVAILABLE = 1;
            ord.BackColor = Color.IndianRed;
            t.STATUS = 2;
            TableManager.updateStatus(t);
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            AddForeignProduct p = new AddForeignProduct();
            p.FormClosing += new FormClosingEventHandler(updvalor);
            p.Show();
        }

        private void updvalor(object sender, FormClosingEventArgs e)
        {
            if (Data.done == true)
            {
                ord.Table.ORDER.Empty = false;

                DataTable dataTable = (DataTable)ver.DataSource;
                DataRow drToAdd = dataTable.NewRow();

                drToAdd["Produto"] = Data.name;
                drToAdd["Valor"] = Data.price + "€";

                dataTable.Rows.Add(drToAdd);
                dataTable.AcceptChanges();

                try
                {
                    ord.Table.ORDER.CONTENTLIST = Databases.compactData(dataTable);
                }
                catch (Exception s)
                {
                    MessageBox.Show(s.StackTrace);
                }
                ord.Table.ORDER.VALUE += Data.price;

                if (IndexIdentifiers == null)
                {
                    IndexIdentifiers = new List<string>();
                }
                IndexIdentifiers.Add(Data.id);


                label4.Text = ord.Table.ORDER.VALUE + "€";


                if (ord.Table.ORDER.EVENTS == null)
                {
                    ord.Table.ORDER.EVENTS = log.IDENTIFIER + " adicionou um produto não listado " + Data.name + " no valor de " + Data.price + "€" + " (" + Time.get() + ")";
                }
                else
                {
                    ord.Table.ORDER.EVENTS = String.Concat(ord.Table.ORDER.EVENTS, "§" + log.IDENTIFIER + " adicionou um produto não listado " + Data.name + " no valor de " + Data.price + "€" + " (" + Time.get() + ")");
                }
            }
            Data.done = false;
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            closeFatura();
        }

        private void closeFatura()
        {
            if (ord.Table.ORDER.Empty == false)
            {
                try
                {
                    ord.Table.ORDER.CONTENTLIST = Databases.compactData(((DataTable)ver.DataSource));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace);
                }

                ord.Table.ORDER.INDEXLIST = Databases.compactList(IndexIdentifiers);

                CloseFatura f = new CloseFatura(ord.Table.ORDER);
                f.FormClosed += new FormClosedEventHandler(checkClose);
                f.Show();
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhum pedido foi iniciado.", 1);
                n.ShowDialog();
            }
        }

        private void checkClose(object sender, FormClosedEventArgs e)
        {
            if (Data.checkoutDone == true)
            {
                this.Close();
                Data.checkoutDone = false;
                ord.Table.ORDER = null;

                ord.Table.AVAILABLE = 0;
                ord.BackColor = Color.LightGreen;
                ord.Table.STATUS = 3;
                ord.Invalidate();
                TableManager.updateStatus(ord.Table);
            }
        }

        private void MesaFatura_Load(object sender, EventArgs e)
        {
            this.Text = "Fatura da mesa " + ord.Table.NUMBER;

            ver.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            toolStripComboBox1.Items.Add("Todas");
            foreach (Category s in CategoryManager.loadCategories())
            {
                toolStripComboBox1.Items.Add(s.NAME);
            }

            toolStripComboBox1.SelectedIndex = 0;

            if (string.IsNullOrEmpty(ord.Table.ORDER.CONTENTLIST) == true)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Produto", typeof(string));
                dt.Columns.Add("Valor", typeof(string));

                ver.DataSource = dt;
            } else
            {
                ver.DataSource = Databases.uncompactTable(ord.Table.ORDER.CONTENTLIST);
                IndexIdentifiers = Databases.uncompactList(ord.Table.ORDER.INDEXLIST);

                label4.Text = ord.Table.ORDER.VALUE + "€";
            }

            filterProducts();
            displayProducts();
        }

        private void ProdutoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}