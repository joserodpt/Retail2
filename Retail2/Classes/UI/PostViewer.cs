using Retail2.Classes.MessageBoard;
using Retail2.Managers;
using Retail2.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Retail2.Classes.UI
{
    public partial class PostViewer : UserControl
    {
        Post linked;
        User v;
        List<String> inters;
        List<String> comments;
        int inter;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public PostViewer(Post p, Boolean debug, User viewer)
        {
            v = viewer;

            InitializeComponent();

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            label1.Text = p.NAME;
            label2.Text = "🕐 " + p.POSTDATE;

            metroCheckBox1.CheckedChanged -= MetroCheckBox1_CheckedChanged;
            metroCheckBox2.CheckedChanged -= MetroCheckBox2_CheckedChanged;

            inters = Databases.uncompactList(p.INTERACTION1);
            comments = Databases.uncompactList(p.COMMENTS);

            foreach (String s in inters)
            {
                string[] data = s.Split('-');
                String ID = data[0];

                if (ID == viewer.IDENTIFIER)
                {
                    inter = Int32.Parse(data[1]);

                    if (inter == 0) { inter = 0; }
                    if (inter == 1)
                    {
                        metroCheckBox1.Checked = true;
                        inter = 1;
                    }
                    if (inter == 2)
                    {
                        metroCheckBox2.Checked = true;
                        inter = 2;
                    }
                }

            }

            metroCheckBox1.CheckedChanged += MetroCheckBox1_CheckedChanged;
            metroCheckBox2.CheckedChanged += MetroCheckBox2_CheckedChanged;

            label3.Text = "Likes: " + p.LIKES + " | Dislikes: " + p.DISLIKES;
            listBox1.Items.AddRange(Databases.uncompactList(p.TEXT).ToArray());

            foreach(String s in comments)
            {
                if (string.IsNullOrWhiteSpace(s) == false)
                {
                    int pFrom = s.IndexOf("[") + "[".Length;
                    int pTo = s.LastIndexOf("]");

                    String result = s.Substring(pFrom, pTo - pFrom);

                    listBox2.DisplayMember = "txt";

                    CommentUI c = new CommentUI { txt = s.Replace("[" + result + "]", ""), date = result };
                    listBox2.Items.Add(c);
                }
            }

            if (debug == true)
            {
                groupBox1.Visible = true;
                debugger.Enabled = true;
                debugger.Start();
            }

            linked = p;
        }

        private void MetroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (inter == 2)
            {
                metroCheckBox2.Checked = false;
            }
            if (inter == 1)
            {
                inter = 0;
                linked.LIKES -= 1;
                PostManager.valueManager(Enum.ValueInfo.LIKE, Enum.ValueInfo.REMOVE, linked.ID);
            }
            else
            {
                inter = 1;
                linked.LIKES += 1;
                PostManager.valueManager(Enum.ValueInfo.LIKE, Enum.ValueInfo.ADD, linked.ID);
            }

            int count = 0;
            for (int i = 0; i < inters.Count; i++)
            {
                String s = inters[i];

                if (s.Contains(v.IDENTIFIER))
                {
                    inters[count] = v.IDENTIFIER + "-" + inter;
                    break;
                }
                    count += 1;
            }

            if (count == inters.Count)
            {
                inters.Add(v.IDENTIFIER + "-" + inter);
            }

            linked.INTERACTION1 = Databases.compactList(inters);
            PostManager.updateInteractions(linked, false);

            uiTrigger(0);
        }

        private void uiTrigger(int v)
        {
            if (v == 0)
            {
                label3.Text = "Likes: " + linked.LIKES + " | Dislikes: " + linked.DISLIKES;
            }
        }

        private void MetroCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (inter == 1)
            {
                metroCheckBox1.Checked = false;
            }

            if (inter == 2)
            {
                inter = 0;
                linked.DISLIKES -= 1;
                PostManager.valueManager(Enum.ValueInfo.DISLIKE, Enum.ValueInfo.REMOVE, linked.ID);
            }
            else
            {
                inter = 2;
                linked.DISLIKES += 1;
                PostManager.valueManager(Enum.ValueInfo.DISLIKE, Enum.ValueInfo.ADD, linked.ID);
            }

            int count = 0;
            for (int i = 0; i < inters.Count; i++)
            {
                String s = inters[i];

                if (s.Contains(v.IDENTIFIER))
                {
                    inters[count] = v.IDENTIFIER + "-" + inter;
                    break;
                }
                count += 1;
            }

            if (count == inters.Count)
            {
                inters.Add(v.IDENTIFIER + "-" + inter);
            }

            linked.INTERACTION1 = Databases.compactList(inters);
            PostManager.updateInteractions(linked, false);

            uiTrigger(0);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            label7.Text = "Interaction: " + inter;
            listBox3.Text = Databases.compactList(inters);
        }

        private void PostViewer_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            String text = metroTextBox1.Text;
            if (string.IsNullOrWhiteSpace(text) == false)
            {
                text = v.FIRSTNAME + " " + v.LASTNAME + ": " + metroTextBox1.Text;

                CommentUI u = new CommentUI { txt = text, date = Time.get() };

                listBox2.DisplayMember = "txt";

                listBox2.Items.Add(u);
                comments.Add(text + " [" + Time.get() + "]");

                linked.COMMENTS = Databases.compactList(comments);

                PostManager.updateComments(linked);
            }

            metroTextBox1.Text = "";
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                CommentUI c = (CommentUI)listBox2.SelectedItem;
                MessageBox.Show(c.date, "Data de Publicação");
            }
        }

        private void onesecond_Tick(object sender, EventArgs e)
        {
            label2.Text = "Postado há " + Time.calculateTimeElapsed(linked.POSTDATE);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            if (v.IDENTIFIER == linked.POSTERID)
            {
                PostManager.deletePost(linked);
                this.Dispose();
            } else
            {
                Notification n = new Notification(Enum.AlertType.ERROR, "Não têns permissão para eliminar o post.", 3);
                n.scroll = true;
                n.scrollspeedTicks = 400;
                n.ShowDialog();
            }
        }
    }
}
