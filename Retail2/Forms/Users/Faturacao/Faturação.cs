using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Retail2.Forms.Users
{
    public partial class Faturação : UI2
    {
        User log;
        public Faturação(User u)
        {
            log = u;
            InitializeComponent();

            this.Size = Databases.getSize(SettingsManager.getWindowSize(1));
        }

        List<Product> show = new List<Product>();

        private void Faturação_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Add("Todas");
            foreach (Category c in CategoryManager.loadCategories())
            {
                toolStripComboBox1.Items.Add(c.NAME);
            }
            toolStripComboBox1.SelectedIndex = 0;

            try
            {
                filterProducts();
                displayProducts();

                panel1.VerticalScroll.Enabled = true;
                panel1.VerticalScroll.Visible = true;

                panel1.AutoScroll = true;

                if (OrderManager.loadOrders().Count != 0)
                {
                    foreach (Order o in OrderManager.loadOrders())
                    {
                        if (o.ORDERTYPE == 0) {
                            if (o.DONE == false)
                            {
                                if (o.CREATORUSERID == log.IDENTIFIER)
                                {
                                    addFatura(true, o);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception swa) { Console.Write(swa.StackTrace); }
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

        private void addItem(object sender2, EventArgs e2, String s)
        {
            if (tabControl1.SelectedTab == null)
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhum pedido foi iniciado.", 1);
                n.ShowDialog(); return;
            }


            TabPage tab = tabControl1.SelectedTab;
            OrderTab selectedTab = (OrderTab)tab;

            selectedTab.Empty = false;

            foreach (DataGridView rb in selectedTab.Controls.OfType<DataGridView>())
            {
                Product p = ProductManager.getProduct(s);

                if (p.unlimitedSTOCK == false)
                {
                    if (p.STOCK >= 1)
                    {
                        p.STOCK -= 1;
                        ProductManager.editStockNumb(p);

                        DataTable dataTable = (DataTable)rb.DataSource;
                        DataRow drToAdd = dataTable.NewRow();

                        drToAdd["Produto"] = p.NAME;
                        drToAdd["Valor"] = p.PRICE + "€";

                        dataTable.Rows.Add(drToAdd);
                        dataTable.AcceptChanges();

                        selectedTab.List = dataTable;

                        selectedTab.Order.VALUE += p.PRICE;

                        if (selectedTab.IndexIdentifiers == null)
                        {
                            selectedTab.IndexIdentifiers = new List<string>();
                        }
                        selectedTab.IndexIdentifiers.Add(p.IDENTIFIER);

                        foreach (Label l in selectedTab.Controls.OfType<Label>())
                        {
                            if (l.Name == "priceshow")
                            {
                                l.Text = selectedTab.Order.VALUE + "€";
                            }
                        }

                        if (selectedTab.Order.EVENTS == null)
                        {
                            selectedTab.Order.EVENTS = log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")";
                        }
                        else
                        {
                            selectedTab.Order.EVENTS = String.Concat(selectedTab.Order.EVENTS, "§" + log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")");
                        }
                    }
                    else
                    {
                        Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Produto esgotado.", 1);
                        n.ShowDialog();
                    }
                }
                else
                {
                    DataTable dataTable = (DataTable)rb.DataSource;
                    DataRow drToAdd = dataTable.NewRow();

                    drToAdd["Produto"] = p.NAME;
                    drToAdd["Valor"] = p.PRICE + "€";

                    dataTable.Rows.Add(drToAdd);
                    dataTable.AcceptChanges();

                    selectedTab.List = dataTable;

                    selectedTab.Order.VALUE += p.PRICE;

                    if (selectedTab.IndexIdentifiers == null)
                    {
                        selectedTab.IndexIdentifiers = new List<string>();
                    }
                    selectedTab.IndexIdentifiers.Add(p.IDENTIFIER);

                    foreach (Label l in selectedTab.Controls.OfType<Label>())
                    {
                        if (l.Name == "priceshow")
                        {
                            l.Text = selectedTab.Order.VALUE + "€";
                        }
                    }

                    if (selectedTab.Order.EVENTS == null)
                    {
                        selectedTab.Order.EVENTS = log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")";
                    }
                    else
                    {
                        selectedTab.Order.EVENTS = String.Concat(selectedTab.Order.EVENTS, "§" + log.IDENTIFIER + " adicionou " + p.NAME + " (" + Time.get() + ")");
                    }
                }
            }
        }
        protected override void OnResizeBegin(EventArgs e)
        {
            SuspendLayout();
            base.OnResizeBegin(e);
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            ResumeLayout();
            base.OnResizeEnd(e);
        }

        Size lastSize;
        private void Faturação_ResizeEnd(object sender, EventArgs e)
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

        private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterProducts();
            displayProducts();
        }

        private void DsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filterProducts();
            displayProducts();
        }

        public void saveOrders()
        {
            foreach (TabPage p in tabControl1.TabPages)
            {
                OrderTab ot = (OrderTab)p;
                Order o = ot.Order;

                if (!OrderManager.orderExistsNormal(o))
                {
                    if (ot.Empty == false)
                    {
                        foreach (DataGridView d in ot.Controls.OfType<DataGridView>())
                        {
                            o.CONTENTLIST = Databases.compactData(((DataTable)d.DataSource));
                        }

                        o.INDEXLIST = Databases.compactList(ot.IndexIdentifiers);
                        o.TABLE = -1;
                        OrderManager.saveOrder(o);
                    }
                    else
                    {
                        tabControl1.TabPages.Remove(ot);
                    }
                }
                else
                {
                    if (ot.Empty == false)
                    {
                        foreach (DataGridView d in ot.Controls.OfType<DataGridView>())
                        {
                            o.CONTENTLIST = Databases.compactData(((DataTable)d.DataSource));
                        }

                        o.INDEXLIST = Databases.compactList(ot.IndexIdentifiers);
                        o.TABLE = -1;
                        OrderManager.editOrder(o);
                    }
                    else
                    {
                        tabControl1.TabPages.Remove(ot);
                    }
                }
            }
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            saveOrders();
        }

        int fatn = OrderManager.getOrderNumber();

        private void addFatura(Boolean source, Order souce)
        {
            OrderTab tp = new OrderTab();
            tp.Padding = new System.Windows.Forms.Padding(3);
            tp.Size = new System.Drawing.Size(159, 280);
            tp.TabIndex = tabControl1.TabPages.Count + 1;
            tp.UseVisualStyleBackColor = true;

            if (source == false)
            {
                tp.Text = fatn + 1 + "";

                Order o = new Order();
                o.CREATORUSERID = log.IDENTIFIER;
                o.ORDERTYPE = 0;
                o.ID = fatn + 1;
                o.DATECREATED = Time.get();
                o.DONE = false;
                o.IDENTIFIER = Databases.getIdentifier(Classes.Enum.IdentifierType.ORDER);

                tp.Order = o;

                tp.Empty = true;
            }
            else
            {
                tp.Empty = false;
                tp.Text = souce.ID + "";
                tp.Order = souce;
                tp.IndexIdentifiers = Databases.uncompactList(tp.Order.INDEXLIST);
            }

            fatn += 1;

            DataGridView dv = new DataGridView();
            dv.AllowUserToAddRows = false;
            dv.AllowUserToDeleteRows = false;
            dv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dv.EnableHeadersVisualStyles = false;
            dv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dv.Location = new System.Drawing.Point(3, 3);
            dv.ReadOnly = true;
            dv.RowHeadersVisible = false;
            dv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dv.Size = new System.Drawing.Size(146, 213);
            dv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            dv.KeyDown += (sender2, e2) => delProduct(sender2, e2, dv);

            if (source == true)
            {
                dv.DataSource = Databases.uncompactTable(souce.CONTENTLIST);
            }
            else
            {
                DataTable dbUsers = new DataTable();
                dbUsers.Columns.Add("Produto", typeof(string));
                dbUsers.Columns.Add("Valor", typeof(string));
                dv.DataSource = dbUsers;
            }

            tp.Controls.Add(dv);

            Label l = new Label();
            l.AutoSize = true;
            l.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l.Location = new System.Drawing.Point(0, 218);
            l.Size = new System.Drawing.Size(56, 20);
            l.Text = "Total:";
            l.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            tp.Controls.Add(l);

            Label l2 = new Label();
            l2.AutoSize = true;
            l2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l2.Location = new System.Drawing.Point(54, 218);
            l2.Size = new System.Drawing.Size(0, 24);
            l2.AllowDrop = true;
            l2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            if (source == true)
            {
                l2.Text = souce.VALUE + "€";
            }
            else
            {
                l2.Text = "0€";
            }
            l2.Name = "priceshow";
            tp.Controls.Add(l2);

            PictureBox p3 = new PictureBox();
            p3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            p3.Image = global::Retail2.Properties.Resources.info;
            p3.Location = new System.Drawing.Point(68, 244);
            p3.Name = "pictureBox3";
            p3.Size = new System.Drawing.Size(30, 30);
            p3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            p3.TabIndex = 5;
            p3.TabStop = false;
            p3.Click += (sender2, e2) => infoTab();
            tp.Controls.Add(p3);

            PictureBox p2 = new PictureBox();
            p2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            p2.Image = global::Retail2.Properties.Resources.close_fatura;
            p2.Location = new System.Drawing.Point(124, 244);
            p2.Name = "pictureBox2";
            p2.Size = new System.Drawing.Size(30, 30);
            p2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            p2.TabIndex = 4;
            p2.TabStop = false;
            p2.Click += (sender2, e2) => closeTab();
            tp.Controls.Add(p2);

            PictureBox p1 = new PictureBox();
            p1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            p1.Image = global::Retail2.Properties.Resources.add;
            p1.Location = new System.Drawing.Point(7, 244);
            p1.Name = "pictureBox1";
            p1.Size = new System.Drawing.Size(30, 30);
            p1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            p1.TabIndex = 3;
            p1.Click += (sender2, e2) => addUnknownValue();
            p1.TabStop = false;
            tp.Controls.Add(p1);

            tabControl1.TabPages.Add(tp);
        }

        private void delProduct(object sender2, KeyEventArgs e2, DataGridView dv)
        {
            if (e2.KeyCode == Keys.Delete)
            {
                Int32 selectedRowCount = dv.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    for (int i = 0; i < selectedRowCount; i++)
                    {
                        TabPage tab = tabControl1.SelectedTab;
                        OrderTab selectedTab = (OrderTab)tab;

                        int selectedrowindex = dv.SelectedCells[0].RowIndex;

                        if (selectedTab.IndexIdentifiers[selectedrowindex] != "Added By User")
                        {
                            Product p = ProductManager.getProduct(selectedTab.IndexIdentifiers[selectedrowindex]);

                            DataGridViewRow selectedRow = dv.Rows[selectedrowindex];

                            dv.Rows.RemoveAt(dv.SelectedRows[0].Index);

                            if (dv.Rows.GetRowCount(DataGridViewElementStates.Visible) == 0)
                            {
                                selectedTab.Empty = true;
                            }

                            selectedTab.IndexIdentifiers.RemoveAt(selectedrowindex);

                            selectedTab.List = (DataTable)dv.DataSource;

                            selectedTab.Order.VALUE -= p.PRICE;

                            foreach (Label l in selectedTab.Controls.OfType<Label>())
                            {
                                if (l.Name == "priceshow")
                                {
                                    l.Text = selectedTab.Order.VALUE + "€";
                                }
                            }

                            if (p.unlimitedSTOCK == false)
                            {
                                p.STOCK += 1;
                                ProductManager.editStockNumb(p);
                            }

                            selectedTab.Order.EVENTS = String.Concat(selectedTab.Order.EVENTS, "§" + log.IDENTIFIER + " removeu " + p.NAME + " (" + Time.get() + ")");
                        }
                        else
                        {
                            DataGridViewRow selectedRow = dv.Rows[selectedrowindex];

                            if (dv.Rows.GetRowCount(DataGridViewElementStates.Visible) == 0)
                            {
                                selectedTab.Empty = true;
                            }

                            selectedTab.IndexIdentifiers.RemoveAt(selectedrowindex);

                            selectedTab.List = (DataTable)dv.DataSource;

                            String nom = dv.Rows[selectedrowindex].Cells["Produto"].Value.ToString();
                            Double d = Double.Parse(dv.Rows[selectedrowindex].Cells["Valor"].Value.ToString().Replace("€", ""));

                            dv.Rows.RemoveAt(dv.SelectedRows[0].Index);

                            selectedTab.Order.VALUE -= d;

                            foreach (Label l in selectedTab.Controls.OfType<Label>())
                            {
                                if (l.Name == "priceshow")
                                {
                                    l.Text = selectedTab.Order.VALUE + "€";
                                }
                            }

                            selectedTab.Order.EVENTS = String.Concat(selectedTab.Order.EVENTS, "§" + log.IDENTIFIER + " removeu " + nom + " (" + Time.get() + ")");
                        }
                    }
                }
            }
        }

        private void closeTab()
        {
            OrderTab ot = (OrderTab)tabControl1.SelectedTab;
            Order o = ot.Order;

            if (ot.Empty == false)
            {
                foreach (DataGridView d in ot.Controls.OfType<DataGridView>())
                {
                    o.CONTENTLIST = Databases.compactData(((DataTable)d.DataSource));
                }

                o.INDEXLIST = Databases.compactList(ot.IndexIdentifiers);
                o.TABLE = -1;

                CloseFatura f = new CloseFatura(o);
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
                this.tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                Data.checkoutDone = false;
            }
        }

        private void addUnknownValue()
        {
            AddForeignProduct p = new AddForeignProduct();
            p.FormClosing += new FormClosingEventHandler(updvalor);
            p.Show();
        }

        private void updvalor(object sender, FormClosingEventArgs e)
        {
            if (Data.done == true)
            {
                if (tabControl1.SelectedTab == null)
                {
                    Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhum pedido foi iniciado.", 1);
                    n.ShowDialog();
                    return;
                }

                OrderTab selectedTab = (OrderTab)tabControl1.SelectedTab;
                selectedTab.Empty = false;

                foreach (DataGridView rb in selectedTab.Controls.OfType<DataGridView>())
                {
                    DataTable dataTable = (DataTable)rb.DataSource;
                    DataRow drToAdd = dataTable.NewRow();

                    drToAdd["Produto"] = Data.name;
                    drToAdd["Valor"] = Data.price + "€";

                    dataTable.Rows.Add(drToAdd);
                    dataTable.AcceptChanges();

                    selectedTab.List = dataTable;

                    selectedTab.Order.VALUE += Data.price;

                    if (selectedTab.IndexIdentifiers == null)
                    {
                        selectedTab.IndexIdentifiers = new List<string>();
                    }
                    selectedTab.IndexIdentifiers.Add(Data.id);

                    foreach (Label l in selectedTab.Controls.OfType<Label>())
                    {
                        if (l.Name == "priceshow")
                        {
                            l.Text = selectedTab.Order.VALUE + "€";
                        }
                    }

                    if (selectedTab.Order.EVENTS == null)
                    {
                        selectedTab.Order.EVENTS = log.IDENTIFIER + " adicionou um produto não listado " + Data.name + " no valor de " + Data.price + "€" + " (" + Time.get() + ")";
                    }
                    else
                    {
                        selectedTab.Order.EVENTS = String.Concat(selectedTab.Order.EVENTS, "§" + log.IDENTIFIER + " adicionou um produto não listado " + Data.name + " no valor de " + Data.price + "€" + " (" + Time.get() + ")");
                    }
                }
                Data.done = false;
            }
        }

        private void infoTab()
        {
            TabPage sel = tabControl1.SelectedTab;
            OrderTab o = (OrderTab)sel;

            OrderInfo d = new OrderInfo(o.Order);
            d.ShowDialog();
        }

        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font b = new Font("Arial", 8, FontStyle.Bold);
            //e.Graphics.DrawString("X", b, Brushes.Red, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 5, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        private void PedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                TabPage tab = tabControl1.SelectedTab;
                OrderTab selectedTab = (OrderTab)tab;
                OrderManager.deleteOrder(selectedTab.Order);

                this.tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhum pedido foi iniciado.", 1);
                n.ShowDialog();
            }
        }

        private void ProdutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                foreach (DataGridView dv in tabControl1.SelectedTab.Controls.OfType<DataGridView>())
                {
                    Int32 selectedRowCount = dv.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        for (int i = 0; i < selectedRowCount; i++)
                        {
                            int selectedrowindex = dv.SelectedCells[0].RowIndex;

                            DataGridViewRow selectedRow = dv.Rows[selectedrowindex];

                            dv.Rows.RemoveAt(dv.SelectedRows[0].Index);

                            TabPage tab = tabControl1.SelectedTab;
                            OrderTab selectedTab = (OrderTab)tab;

                            if (dv.Rows.GetRowCount(DataGridViewElementStates.Visible) == 0)
                            {
                                selectedTab.Empty = true;
                            }

                            Product p = ProductManager.getProduct(selectedTab.IndexIdentifiers[selectedrowindex]);

                            selectedTab.IndexIdentifiers.RemoveAt(selectedrowindex);

                            selectedTab.List = (DataTable)dv.DataSource;

                            selectedTab.Order.VALUE -= p.PRICE;

                            foreach (Label l in selectedTab.Controls.OfType<Label>())
                            {
                                if (l.Name == "priceshow")
                                {
                                    l.Text = selectedTab.Order.VALUE + "€";
                                }
                            }

                            if (p.unlimitedSTOCK == false)
                            {
                                p.STOCK += 1;
                                ProductManager.editStockNumb(p);
                            }

                            selectedTab.Order.EVENTS = String.Concat(selectedTab.Order.EVENTS, "§" + log.IDENTIFIER + " removeu " + p.NAME + " (" + Time.get() + ")");
                        }
                    }
                }
            }
            else
            {
                Notification n = new Notification(Classes.Enum.AlertType.ERROR, "Nenhum pedido foi iniciado.", 1);
                n.ShowDialog();
            }
        }

        private void ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            addFatura(false, null);
        }
    }
}
