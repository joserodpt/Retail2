using Dapper;
using Retail2.Classes;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Retail2.Managers
{
    class TableManager
    {
        public static String con = ConfigurationManager.ConnectionStrings["Orders"].ConnectionString;
        public static List<Table> orderCache = new List<Table>();

        public static List<TableUI> makeTables()
        {
            List<TableUI> fin = new List<TableUI>();

            loadTables();
            foreach (Table t in orderCache)
            {
                TableUI tab = new TableUI();
                tab.Size = Databases.getSize(t.SIZE);
                tab.Location = Databases.getLocation(t.LOC);
                tab.Table = t;
                tab.BorderStyle = BorderStyle.None;
                
                if (t.AVAILABLE == 0)
                {
                    tab.BackColor = Color.LightGreen;
                }
                if (t.AVAILABLE == 1)
                {
                    tab.BackColor = Color.IndianRed;
                }
                if (t.AVAILABLE == 2)
                {
                    tab.BackColor = Color.FromArgb(254, 255, 111);
                }

                tab.Paint += new PaintEventHandler((sender, e) =>
                {
                    string n = t.NUMBER + "";
                    using (Font font = new Font("Arial", 20, System.Drawing.FontStyle.Regular, GraphicsUnit.Point))
                    {
                        TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
                        TextRenderer.DrawText(e.Graphics, n, font, tab.ClientRectangle, Color.Black, flags);
                    }

                    Pen p = new Pen(Color.Black);
                    SolidBrush sb = new SolidBrush(Color.Gray);
                    if (t.STATUS == 0)
                    {
                        sb = new SolidBrush(Color.LawnGreen);
                    }
                    if (t.STATUS == 1)
                    {
                        sb = new SolidBrush(Color.Red);
                    }
                    if (t.STATUS == 2)
                    {
                        sb = new SolidBrush(Color.Yellow);
                    }

                    int x = 3;
                    int y = tab.Size.Height - 17;
                    int r = 11;
                    e.Graphics.DrawEllipse(p, x, y, r, r);
                    e.Graphics.FillEllipse(sb, x, y, r, r);
                });

                foreach(Order o in OrderManager.loadOrders())
                {
                    if (o.ORDERTYPE == 1)
                    {
                        if (o.DONE != true)
                        {
                            if (o.TABLE == t.NUMBER)
                            {
                                tab.Table.ORDER = o;
                            }
                        }
                    }
                }

                fin.Add(tab);
            }

            return fin;
        }

        public static Table getTable(int number)
        {
            foreach(Table t in orderCache)
            {
                if (t.NUMBER == number)
                {
                    return t;
                }
            }
            return null;
        }

        public static List<TableUIAdmin> makeTablesAdmin()
        {
            List<TableUIAdmin> fin = new List<TableUIAdmin>();

            if (orderCache.Count == 0)
            {
                loadTables();
            }
            foreach (Table t in orderCache)
            {
                TableUIAdmin tab = new TableUIAdmin();
                tab.Size = Databases.getSize(t.SIZE);
                tab.Location = Databases.getLocation(t.LOC);
                tab.Table = t;
                tab.Text = t.NUMBER + "";
                tab.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                tab.MinimumSize = new Size(10, 10);

                fin.Add(tab);
            }

            return fin;
        }

        public static List<Table> loadTables()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Table>("select * from Tables", new DynamicParameters());
                orderCache = output.ToList();
                return orderCache;
            }
        }

        public static int getTableNumber()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var count = cnn.ExecuteScalar<int>("SELECT COUNT(*) FROM Tables");
                return count;
            }
        }

        public static void saveTable(Table t)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Tables (NUMBER, SIZE, LOC, AVAILABLE, STATUS, INFO) values (@NUMBER, @SIZE, @LOC, @AVAILABLE, @STATUS, @INFO)", t);
            }
        }

        public static Boolean tableExists(Table t)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var check = cnn.ExecuteScalar<Boolean>("SELECT 1 WHERE EXISTS (SELECT 1 FROM Tables WHERE NUMBER= @NUMBER)", t);
                return check;
            }
        }

        public static void updateTable(Table t)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Tables 
SET  SIZE = @SIZE
,LOC = @LOC 
,AVAILABLE = @AVAILABLE
,STATUS = @STATUS
,INFO = @INFO
WHERE NUMBER = @NUMBER";
                connection.Execute(sqlStatement, t);
            }
        }

        public static void updateStatus(Table t)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Tables 
SET AVAILABLE = @AVAILABLE
,STATUS = @STATUS
WHERE NUMBER = @NUMBER";
                connection.Execute(sqlStatement, t);
            }
        }

        public static void deleteTable(Table t)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Tables WHERE NUMBER='" + t.NUMBER + "'");
            }
        }




        //Zones


        public static List<Zone> loadZones()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Zone>("select * from Zones", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveZone(Zone t)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Zones (NAME, INFO) values (@NAME, @INFO)", t);
            }
        }

        public static Boolean zoneExists(String n)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var check = cnn.ExecuteScalar<Boolean>("SELECT 1 WHERE EXISTS (SELECT 1 FROM Zones WHERE ID='" + n + "')");
                return check;
            }
        }

        public static void updateZone(Zone t)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Zones
SET NAME = @NAME
,INFO = @INFO
WHERE NAME= @NAME";
                connection.Execute(sqlStatement, t);
            }
        }

        public static void flushZones()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("DELETE FROM Zones");
            }
        }
    }
}
