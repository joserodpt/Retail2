using Retail2.Classes;
using Retail2.Classes.UI;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Retail2.Forms.Read
{
    public partial class Orders : UI3
    {
        public Orders()
        {
            InitializeComponent();
        }

        List<Order> displaying = new List<Order>();
        List<OrderViewer> uidisp = new List<OrderViewer>();

        Boolean read = true;
        Boolean pedido = false;

        int prevSize;
        Boolean prevBol;
        private bool prevState;

        private void filterOrders(Boolean read, Boolean pedido)
        {
            TableManager.loadTables();
            displaying.Clear();
            foreach (Order o in OrderManager.loadOrders())
            {
                if (pedido == false)
                {
                    if (o.ORDERTYPE == 1)
                    {
                        if (o.DONE == false)
                        {
                            if (TableManager.getTable(o.TABLE).STATUS == 2)
                            {
                                displaying.Add(o);
                            }
                        }
                    }
                }

                if (pedido == true)
                {
                    if (o.ORDERTYPE == 0)
                    {
                        if (o.DONE == false)
                        {
                                displaying.Add(o);
                        }
                    }
                }
            }

            if (prevSize != displaying.Count)
            {
                isplayOrders();
                prevSize = displaying.Count;
            }

            if (prevBol != read)
            {
                isplayOrders();
                prevBol = read;
            }

            if (prevState != pedido)
            {
                isplayOrders();
                prevState = pedido;
            }
        }
        private void Orders_Load(object sender, EventArgs e)
        {
            this.Size = Databases.getSize(SettingsManager.getWindowSize(1));

            filterOrders(read, pedido);
        }

        private void isplayOrders()
        {
            foreach (Control o in panel1.Controls)
            {
                o.Dispose();
            }

            Point[] p = new Point[displaying.Count];

            OrderViewer[] ord = new OrderViewer[displaying.Count];
            for (int i = 0; i < ord.GetLength(0); i++)
            {
                ord[i] = new OrderViewer(displaying[i], read, pedido);
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

        private void Timer1_Tick(object sender, EventArgs e)
        {
            filterOrders(read, pedido);
        }

        private void ToolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripStatusLabel6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripStatusLabel5_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel5.Text == "Modo: Read-Only")
            {
                read = false;
                toolStripStatusLabel5.Text = "Modo: Editar";
            }
            else
            {
                read = true;
                toolStripStatusLabel5.Text = "Modo: Read-Only";
            }
            filterOrders(read, pedido);
        }

        private void ToolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel3.Text == "Mesas")
            {
                pedido = true;
                toolStripStatusLabel3.Text = "Faturas";
            } else
            {
                pedido = false;
                toolStripStatusLabel3.Text = "Mesas";
            }
            filterOrders(read, pedido);
        }
    }
}
