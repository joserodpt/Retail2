using Retail2.Classes;
using Retail2.Forms.Users.Mesas;
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
    public partial class MesasFo : UI2
    {
        User log;
        public MesasFo(User u)
        {
            log = u;
            InitializeComponent();
            count = OrderManager.getOrderNumber();
        }

        private void Mesas_Load(object sender, EventArgs e)
        {
            this.Size = Databases.getSize(SettingsManager.getWindowSize(1));

            panel1.Controls.Clear();
            panel1.AutoScroll = false;
            panel1.VerticalScroll.Enabled = true;
            panel1.AutoScroll = true;

            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBox2.SelectedIndex = 0;

            loadZones();

            loadTables1();
        }

        private void loadZones()
        {
            label1.BringToFront();
            comboBox1.BringToFront();

            foreach(Zone z in TableManager.loadZones())
            {
                toolStripDropDownButton1.DropDownItems.Add(z.NAME);
            }

            toolStripStatusLabel4.Text = toolStripDropDownButton1.DropDownItems[0].Text;
        }

        List<TableUI> shown = new List<TableUI>();

        private void loadTables1()
        {
            foreach (Control c in shown)
            {
                c.Dispose();
            }
            shown.Clear();

            List<TableUI> list = TableManager.makeTables();
            if (c1 != 4)
            {
                var co = from TableUI t in list
                         where t.Table.AVAILABLE == c1 || t.Table.ZONE.NAME == toolStripStatusLabel4.Text
                         select t;

                foreach (TableUI t in co)
                {
                    t.MouseDown += (sender2, e2) => mouseClick(e2, t);
                    t.DoubleClick += (sender2, e2) => openTable(t);
                    panel1.Controls.Add(t);
                    shown.Add(t);
                }
            }
            else
            {
                foreach (TableUI t in list)
                {
                    t.MouseDown += (sender2, e2) => mouseClick(e2, t);
                    t.DoubleClick += (sender2, e2) => openTable(t);
                    panel1.Controls.Add(t);
                    shown.Add(t);
                }
            }
        }

        int count;
        private void openTable(TableUI t)
        {
            if (t.Table.ORDER == null)
            {
                Order o2 = new Order();
                o2.CREATORUSERID = log.IDENTIFIER;
                o2.ORDERTYPE = 1;
                count += 1;
                o2.ID = count;
                o2.DATECREATED = Time.get();
                o2.DONE = false;
                o2.TABLE = t.Table.NUMBER;
                o2.IDENTIFIER = Databases.getIdentifier(Classes.Enum.IdentifierType.ORDER);

                t.Table.ORDER = o2;

                MesaFatura f = new MesaFatura(log, t);
                //f.FormClosed += (sender2, e2) => refreshDisplay(t);
                f.ShowDialog();
                return;
            } else
            {
                MesaFatura f = new MesaFatura(log, t);
                //f.FormClosed += (sender2, e2) => refreshDisplay(t);
                f.ShowDialog();
                return;
            }
        }

        private void refreshDisplay()
        {
            foreach (Control u in shown)
            {
                u.Invalidate();
            }
        }

        TableUI clicked;
        private void mouseClick(MouseEventArgs e, TableUI t)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        clicked = t;
                        mtext.Text = "Mesa " + t.Table.NUMBER;
                        rclik.Show(Cursor.Position.X, Cursor.Position.Y + 5);
                    }
                    break;
            }
        }

        private void loadTables2()
        {
            foreach (Control c in shown)
            {
                c.Dispose();
            }
            shown.Clear();

            List<TableUI> list = TableManager.makeTables();
            if (c2 != 4)
            {
                var co = from TableUI t in list
                         where t.Table.AVAILABLE == c2 || t.Table.ZONE.NAME == toolStripStatusLabel4.Text
                         select t;

                foreach (TableUI t in co)
                {

                    t.MouseDown += (sender2, e2) => mouseClick(e2, t);
                    t.DoubleClick += (sender2, e2) => openTable(t);
                    panel1.Controls.Add(t);
                    shown.Add(t);
                }
            }
            else
            {
                foreach (TableUI t in list)
                {
                    t.MouseDown += (sender2, e2) => mouseClick(e2, t);
                    t.DoubleClick += (sender2, e2) => openTable(t);
                    panel1.Controls.Add(t);
                    shown.Add(t);
                }
            }
        }

        private void SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastClick == 0)
            {
                loadTables1();
            }
            if (lastClick == 1)
            {
                loadTables2();
            }
        }

        int c1 = 4;
        int c2 = 4;

        private void ToolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastClick = 0;
            if (toolStripComboBox1.SelectedIndex == 0)
            {
                c1 = 4;
            }
            if (toolStripComboBox1.SelectedIndex == 1)
            {
                c1 = 0;
            }
            if (toolStripComboBox1.SelectedIndex == 3)
            {
                c1 = 1;
            }
            if (toolStripComboBox1.SelectedIndex == 2)
            {
                c1 = 2;
            }
            loadTables1();
        }

        private void ToolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastClick = 1;
            if (toolStripComboBox2.SelectedIndex == 0)
            {
                c2 = 4;
            }
            if (toolStripComboBox2.SelectedIndex == 1)
            {
                c2 = 0;
            }
            if (toolStripComboBox2.SelectedIndex == 3)
            {
                c2 = 1;
            }
            if (toolStripComboBox2.SelectedIndex == 2)
            {
                c2 = 2;
            }
            if (toolStripComboBox2.SelectedIndex == 4)
            {
                c2 = 3;
            }
            loadTables2();
        }

        private void LivreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.AVAILABLE = 0;
            TableManager.updateStatus(clicked.Table);
            clicked.BackColor = Color.LightGreen;
        }

        private void IndisponívelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.AVAILABLE = 2;
            TableManager.updateStatus(clicked.Table);
            clicked.BackColor = Color.FromArgb(254, 255, 111);

        }

        private void OcupadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.AVAILABLE = 1;
            TableManager.updateStatus(clicked.Table);
            clicked.BackColor = Color.IndianRed;
        }

        private void OKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.STATUS = 0;
            TableManager.updateStatus(clicked.Table);
            clicked.Invalidate();
        }

        private void AguardandoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.STATUS = 2;
            TableManager.updateStatus(clicked.Table);
            clicked.Invalidate();
        }

        private void VermelhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.STATUS = 1;
            TableManager.updateStatus(clicked.Table);
            clicked.Invalidate();
        }

        int lastClick = 0;
        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex > 0)
            {
                toolStripComboBox1.SelectedIndex = toolStripComboBox1.SelectedIndex - 1;
            }
            lastClick = 0;
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex < toolStripComboBox1.Items.Count - 1)
            {
                toolStripComboBox1.SelectedIndex = toolStripComboBox1.SelectedIndex + 1;
            }
            lastClick = 0;
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox2.SelectedIndex > 0)
            {
                toolStripComboBox2.SelectedIndex = toolStripComboBox2.SelectedIndex - 1;
            }
            lastClick = 1;
        }
        private void ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox2.SelectedIndex < toolStripComboBox2.Items.Count - 1)
            {
                toolStripComboBox2.SelectedIndex = toolStripComboBox2.SelectedIndex + 1;
            }
            lastClick = 1;
        }

        private void SemPedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clicked.Table.STATUS = 3;
            TableManager.updateStatus(clicked.Table);
            clicked.Invalidate();
        }

        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            clicked.BackColor = Color.LightGreen;
            clicked.Table.AVAILABLE = 0;
            clicked.Table.STATUS = 3;
            clicked.Invalidate();
            TableManager.updateStatus(clicked.Table);
            if (OrderManager.orderExistsNormal(clicked.Table.ORDER) == true)
            {
                OrderManager.deleteOrder(clicked.Table.ORDER);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if(lastClick == 0)
            {
                loadTables1();
            }
            if(lastClick == 1)
            {
                loadTables2();
            }
        }
    }
}
