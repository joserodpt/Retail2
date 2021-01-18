using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Classes.UI
{
    public class StockViewer : Panel
    {
        public Product prod;

        public StockViewer(Product asd, int mode)
        {
            prod = asd;

            Label l1 = new Label();
            l1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            l1.AutoSize = true;
            l1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l1.Location = new System.Drawing.Point(74, 5);
            l1.Name = "label1";
            l1.Size = new System.Drawing.Size(55, 16);
            l1.TabIndex = 2;
            l1.Text = prod.NAME;
            l1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            TextBox tb = new TextBox();
            tb.Anchor = System.Windows.Forms.AnchorStyles.None;
            tb.Location = new System.Drawing.Point(15, 32);
            tb.Name = "textBox1";
            tb.Text = prod.STOCK + "";
            tb.Size = new System.Drawing.Size(166, 20);
            tb.TabIndex = 4;

            CheckBox cb = new CheckBox();
            cb.Text = "Ilimitado";
            cb.Location = new System.Drawing.Point(36, 64);
            cb.Checked = prod.unlimitedSTOCK;
            cb.CheckedChanged += (sender2, e2) => chec(cb, tb);

            Button b1 = new Button();
            b1.Location = new System.Drawing.Point(2, 33);
            b1.Name = "button1";
            b1.Size = new System.Drawing.Size(30, 30);
            b1.TabIndex = 3;
            b1.Text = "-";
            b1.UseVisualStyleBackColor = true;
            b1.Click += (sender2, e2) => remove(cb, tb);

            Button b2 = new Button();
            b2.Location = new System.Drawing.Point(214, 33);
            b2.Name = "button2";
            b2.Size = new System.Drawing.Size(30, 30);
            b2.TabIndex = 5;
            b2.Text = "+";
            b2.Click += (sender2, e2) => add(cb, tb);
            b2.UseVisualStyleBackColor = true;

            PictureBox pb = new PictureBox();
            pb.Location = new System.Drawing.Point(38, 5);
            pb.Name = "pictureBox1";
            pb.Size = new System.Drawing.Size(30, 28);
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pb.TabIndex = 1;
            pb.TabStop = false;
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Image = Databases.getImage(prod.IDENTIFIER);

            Button b3 = new Button();
            b3.Location = new System.Drawing.Point(2, 87);
            b3.Name = "button3";
            b3.Size = new System.Drawing.Size(241, 23);
            b3.TabIndex = 6;
            b3.Text = "Confirmar";
            b3.UseVisualStyleBackColor = true;
            b3.Click += (sender2, e2) => save();

            if (prod.unlimitedSTOCK == true)
            {
                tb.Text = "Ilimitado";
            }

            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(pb);
            Controls.Add(l1);
            Controls.Add(b1);
            Controls.Add(b2);
            Controls.Add(tb);
            Controls.Add(cb);
            Controls.Add(b3);
            //p.Location = new System.Drawing.Point(196, 39);
            Size = new System.Drawing.Size(248, 115);
            TabIndex = 1;

            if (mode == 1)
            {
                Timer timer = new Timer();
                timer.Tag = "Timer";
                timer.Interval = 500; 
                timer.Tick += (sender2, e2) => vanish(timer);
                timer.Start();
                BackColor = SystemColors.Highlight;
            }
        }

        int s = 0;
        private void vanish(Timer t)
        {
            s += 1;
            if (s == 1)
            {
                t.Stop();
               BackColor = SystemColors.Control;
            }
        }

        private void save()
        {
            ProductManager.editStock(prod);
            Notification n = new Notification(Enum.AlertType.INFO, "Stock atualizado.", 1);
            n.ShowDialog();
        }

        private void add(CheckBox cb, TextBox tb)
        {
            if (cb.Checked != true)
            {
                prod.STOCK += 1;
                tb.Text = prod.STOCK + "";
            }
        }

        private void remove(CheckBox cb, TextBox tb)
        {
            if (cb.Checked != true)
            {
                prod.STOCK -= 1;
                tb.Text = prod.STOCK + "";
            }
        }

        private void chec(CheckBox cb, TextBox tb)
        {
            if (cb.Checked != true)
            {
                tb.Text = "0";
                prod.STOCK = 0;
                prod.unlimitedSTOCK = false;
            }
            else
            {
                tb.Text = "Ilimitado";
                prod.STOCK = -1;
                prod.unlimitedSTOCK = true;
            }
        }
    }
}
