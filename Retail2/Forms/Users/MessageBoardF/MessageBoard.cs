using Retail2.Classes;
using Retail2.Classes.MessageBoard;
using Retail2.Classes.UI;
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

namespace Retail2.Forms.Users
{
    public partial class MessageBoard : UI1
    {
        User user;
        private bool formmax;

        public MessageBoard(User u)
        {
            user = u;
            InitializeComponent();
        }

        private void MessageBoard_Load(object sender, EventArgs e)
        {
            //ps.Add(new Post { NAME = "batatas", DISLIKES = 0, LIKES = 3, POSTDATE = Time.get(), LIKED = 1, TEXT = new List<string> { "Quinta feira reunião ás 5 e meia na casa da ana maria por isso bazem ok textoooooooooooooooooooo aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa 12344444444444444444", "   espero por todos", "" , "ola", "", "lollllssss,", "@sapooooo", "kanguruh"} });

            refreshPosts();
        }

        public void refreshPosts()
        {
            flowLayoutPanel1.Controls.Clear();

            List<Post> ps = PostManager.loadPosts();

            List<Post> rev = ps.OrderByDescending(x => x.POSTDATE)
           .ToList();

            foreach (Post p in rev)
            {
                flowLayoutPanel1.Controls.Add(new PostViewer(p, false, user));
            }

            if (rev.Count == 0)
            {
                flowLayoutPanel1.Controls.Add(new Label { Text = "Não há nenhum post. Clique no + para criar um novo post." , Size = new Size(500, 20) });
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            List<String> toAdd = new List<string>();
            foreach(String s in richTextBox1.Lines)
            {
                toAdd.Add(s);
            }

            if(user.ADMIN == true)
            {
                PostManager.savePost(new Post { NAME = user.FIRSTNAME + " " + user.LASTNAME + " [Admin]", DISLIKES = 0, LIKES = 0, POSTDATE = Time.get(), INTERACTION1 = "", TEXT = Databases.compactList(toAdd), POSTERID = user.IDENTIFIER });
            }
            else
            {
                PostManager.savePost(new Post { NAME = user.FIRSTNAME + " " + user.LASTNAME, DISLIKES = 0, LIKES = 0, POSTDATE = Time.get(), INTERACTION1 = "", TEXT = Databases.compactList(toAdd), POSTERID = user.IDENTIFIER });
            }

            richTextBox1.Clear();
            panel1.Visible = false;

            refreshPosts();
        }

        private void min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void max_Click(object sender, EventArgs e)
        {
            if (formmax == false)
            {
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
                formmax = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                formmax = false;
            }
        }

        private void contastrip_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
