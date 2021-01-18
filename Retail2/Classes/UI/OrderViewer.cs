using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Classes.UI
{
    public class OrderViewer : TableLayoutPanel
    {
        public Order o;
        public Boolean pedido;

        public OrderViewer(Order asd, Boolean read, Boolean pedido)
        {
            o = asd;
            this.pedido = pedido;

            DataGridView d = new DataGridView();
            d.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            d.Dock = System.Windows.Forms.DockStyle.Fill;
            d.Location = new System.Drawing.Point(3, 38);
            d.Name = "dataGridView1";
            d.Size = new System.Drawing.Size(168, 240);
            d.TabIndex = 0;
            d.AllowUserToAddRows = false;
            d.AllowUserToDeleteRows = false;

            DataTable t = Databases.uncompactTable(asd.CONTENTLIST);
            t.Columns.Remove("Valor");

            var query = t.AsEnumerable()
                       .GroupBy(r => new { Name = r.Field<string>("Produto") })
                       .Select(grp => new
                       {
                           Name = grp.Key.Name,
                           Count = grp.Count()
                       });

            DataTable fin = new DataTable();
            fin.Columns.Add("Produto", typeof(string));
            fin.Columns.Add("Quantidade", typeof(string));

            foreach(var item in query)
            {
                fin.Rows.Add(item.Name, item.Count);
            }

            d.DataSource = fin;
            d.ColumnHeadersVisible = true;
            d.RowHeadersVisible = false;
            d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            Label l1 = new Label();
            l1.AutoSize = true;
            l1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l1.Location = new System.Drawing.Point(3, 0);
            l1.Name = "label1";
            l1.Size = new System.Drawing.Size(80, 17);
            l1.TabIndex = 1;
            
            if (pedido == false)
            {
                l1.Text = "Mesa " + asd.TABLE;
            } else
            {
                l1.Text = "#" + asd.ID + " ID: " + asd.IDENTIFIER.Substring(0, 7);
            }

            Label l2 = new Label();
            l2.AutoSize = true;
            l2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l2.Location = new System.Drawing.Point(3, 17);
            l2.Name = "label2";
            l2.Size = new System.Drawing.Size(48, 15);
            l2.TabIndex = 2;
            l2.Text = "Tempo: " + Time.calculateTimeElapsed(o.DATECREATED);

            ColumnCount = 1;
            ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            Location = new System.Drawing.Point(12, 12);
            Name = "tableLayoutPanel1";
            RowCount = 3;
            CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));

            if (read == false)
            {
                RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            }

            Size = new System.Drawing.Size(180, 270);
            TabIndex = 0;
            Controls.Add(l1, 0, 0);
            Controls.Add(l2, 0, 1);
            Controls.Add(d, 0, 2);

            if (read == false)
            {
                Button b = new Button();
                b.Text = "Concluir";
                b.Dock = DockStyle.Fill;
                b.Click += (sender2, e2) => doneOrder(sender2, e2, asd);
                Controls.Add(b, 0, 3);
                Size = new System.Drawing.Size(180, 290);
            }

            Timer timer = new Timer();
            timer.Tag = "Timer";
            timer.Interval = 1000; 
            timer.Tick += (sender2, e2) => updTime(sender2, e2, asd, l2);
            timer.Start();

        }

        private void doneOrder(object sender2, EventArgs e2, Order asd)
        {
            if (pedido == false)
            {
                Table t = TableManager.getTable(asd.TABLE);
                t.STATUS = 0;
                TableManager.updateStatus(t);
            } else
            {
                o.DONE = true;
                OrderManager.editOrder(o);
            }
        }

        private void updTime(object sender2, EventArgs e2, Order asd, Label l)
        {
            l.Text = "Tempo: " + Time.calculateTimeElapsed(asd.DATECREATED);
        }
    }
}
