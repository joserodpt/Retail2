using ControlManager;
using Retail2.Classes;
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

namespace Retail2.Forms.Admin
{
    public partial class MesasAdmin : UI2
    {
        public MesasAdmin()
        {
            InitializeComponent();

            this.Size = Databases.getSize(SettingsManager.getWindowSize(1));
            this.CenterToScreen();
        }

        private void MesasAdmin_Load(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.AutoScroll = false;
            panel1.VerticalScroll.Enabled = true;
            panel1.AutoScroll = true;

            loadTables();
            i = TableManager.getTableNumber();
        }

        List<TableUIAdmin> tables = new List<TableUIAdmin>();

        private void loadTables()
        {
            ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize;
            foreach (TableUIAdmin t in TableManager.makeTablesAdmin())
            {
                t.DoubleClick += (sender2, e2) => removebut(sender2, e2, t);
                panel1.Controls.Add(t);
                tables.Add(t);
                ControlMoverOrResizer.Init(t);
            }
        }

        private void removebut(object sender2, EventArgs e2, TableUIAdmin t)
        {
            tables.Remove(t);
            TableManager.deleteTable(t.Table);
            t.Hide();
            t.Dispose();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            addTable();
        }

        int i;

        private void addTable()
        {
            i += 1;
            Table t = new Table();
            t.NUMBER = i;
            t.AVAILABLE = 0;
            t.SIZE = "65;65";
            t.LOC = "5;5";
            t.STATUS = 3;

            TableUIAdmin tab = new TableUIAdmin();
            tab.Size = Databases.getSize(t.SIZE);
            tab.Location = Databases.getLocation(t.LOC);
            tab.Table = t;
            tab.Text = t.NUMBER + "";
            tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tab.MinimumSize = new Size(10, 10);
            tab.DoubleClick += (sender2, e2) => removebut(sender2, e2, tab);

            ControlMoverOrResizer.Init(tab);

            panel1.Controls.Add(tab);

            TableManager.saveTable(t);
            tables.Add(tab);
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            foreach (TableUIAdmin c in tables)
            {
                if (TableManager.tableExists(c.Table))
                {
                    c.Table.SIZE = c.Size.Width + ";" + c.Size.Height;
                    c.Table.LOC = c.Location.X + ";" + c.Location.Y;

                    TableManager.updateTable(c.Table);
                } else
                {
                    c.Table.SIZE = c.Size.Width + ";" + c.Size.Height;
                    c.Table.LOC = c.Location.X + ";" + c.Location.Y;

                    TableManager.saveTable(c.Table);
                }
            }
            this.Close();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            foreach (TableUIAdmin c in tables)
            {
                TableManager.deleteTable(c.Table);
                c.Hide();
                c.Dispose();
            }
            tables.Clear();
            i = 0;
        }
    }
}
