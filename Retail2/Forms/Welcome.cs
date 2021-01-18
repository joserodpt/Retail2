using Retail2.Forms.Admin;
using Retail2.Forms.Admin.Products;
using Retail2.Forms.Admin.Settings;
using Retail2.Managers;
using Retail2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;

namespace Retail2.Forms
{
    public partial class Welcome : Form
    {
        public Welcome(Boolean d)
        {
            debug = d;
            InitializeComponent();
            buttop.Visible = false;
        }

        Boolean debug;

        int slidenumb = 0;
        Object del;
        Label del2;
        Label del3;

        Dictionary<Image, String> slides = new Dictionary<Image, String>();

        private void Welcome_Load(object sender, EventArgs e)
        {
            slides.Add(Resources.Screenshot_1, "Interface simplística.|Sem complicações e perdas de tempo, direto ao assunto.");
            slides.Add(Resources.Screenshot_2, "Mais utilizadores, mais eficiência.|Está colocado um sistema simples e flexível de utilizadores, permitindo acesso ao sistema em qualquer altura.");
            slides.Add(Resources.Screenshot_3, "Perfis.|Crie e Guarde facilmente perfis dos seus clientes.");
            slides.Add(Resources.Screenshot_4, "Produtos. Muitos Produtos.|Adicione os produtos que precisar, com a maior flexibilidade possível.");
            slides.Add(Resources.Screenshot_5, "Pedidos.|Visualize informações acerca de pedidos abertos e fechados.");
            slides.Add(Resources.Screenshot_6, "Mais Controlo.|Adicione e remova produtos e pedidos quantas vezes quizer.");
            slides.Add(Resources.Screenshot_7, "Sistema avançado de mesas.|Com um espaço dedicado, permite-lhe ver os estados operacionais das mesas num relance.");
            slides.Add(Resources.info, "Está quase lá.|Precisamos que configure umas definições primeiro. Clique nas caixas para configurar.");

            PictureBox pic = getSlide(slidenumb, false);
            del = pic;

            var t = new Transition(new TransitionType_EaseInEaseOut(3000));
            t.add(next, "Top", buttop.Top);
            t.add(label1, "Left", txtStay.Right);
            t.add(getLabel1(slidenumb, false), "Left", txtStay.Right);
            t.add(getLabel2(slidenumb, false), "Left", txtStay.Right);
            t.add(pic, "Left", txtStay.Right);
            t.run();

            if (debug == true)
            {
                label3.Visible = true;
                timer1.Start();
            }
        }

        public PictureBox getSlide(int i, Boolean back)
        {
            PictureBox p = new PictureBox();
            p.Image = slides.ElementAt(i).Key;
            if (back == true)
            {
                p.Location = new System.Drawing.Point(880, 75);
            }
            else
            {
                p.Location = new System.Drawing.Point(-880, 75);
            }
            p.Name = "image";
            p.Size = new System.Drawing.Size(758, 426);
            p.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Controls.Add(p);
            del = p;
            return p;
        }

        public Label getLabel1(int i, Boolean back)
        {
            Label p = new Label();
            string[] s = slides.ElementAt(i).Value.Split('|');
            p.Text = s[0];
            p.AutoSize = true;
            if (back == true)
            {
                p.Location = new System.Drawing.Point(880, 34);
            }
            else
            {
                p.Location = new System.Drawing.Point(-480, 34);
            }
            this.Controls.Add(p);

            del2 = p;


            return p;
        }

        public Label getLabel2(int i, Boolean back)
        {
            Label p = new Label();
            string[] s = slides.ElementAt(i).Value.Split('|');
            p.Text = s[1];
            p.AutoSize = true;
            if (back == true)
            {
                p.Location = new System.Drawing.Point(880, 49);
            }
            else
            {
                p.Location = new System.Drawing.Point(-580, 49);
            }
            this.Controls.Add(p);

            del3 = p;


            return p;
        }


        private void Next_Click(object sender, EventArgs e)
        {
            if (preClick() == 1)
            {
                slidenumb += 1;

                var fg = new Transition(new TransitionType_EaseInEaseOut(1500));
                fg.add(del, "Left", txtImgHide.Right);
                fg.add(del2, "Left", txtImgHide.Right);
                fg.add(del3, "Left", txtImgHide.Right);

                fg.add(getLabel1(slidenumb, false), "Left", txtStay.Right);
                fg.add(getLabel2(slidenumb, false), "Left", txtStay.Right);
                fg.add(panel1, "Left", txtStay.Right);
                fg.run();

                del = panel1;

                var t = new Transition(new TransitionType_EaseInEaseOut(1200));
                t.add(next, "Top", baixobut.Top);
                t.run();

                return;
            }

            slidenumb += 1;
            var t2 = new Transition(new TransitionType_EaseInEaseOut(1000));
            t2.add(del, "Left", txtImgHide.Right);
            t2.add(del2, "Left", txtImgHide.Right);
            t2.add(del3, "Left", txtImgHide.Right);
            t2.add(getSlide(slidenumb, false), "Left", txtStay.Right);
            t2.add(getLabel1(slidenumb, false), "Left", txtStay.Right);
            t2.add(getLabel2(slidenumb, false), "Left", txtStay.Right);
            t2.run();

            var t3 = new Transition(new TransitionType_EaseInAndOut(1200));
            t3.add(next, "Top", baixobut.Top);
            t3.run();

            if (1 == slidenumb)
            {
                var ts = new Transition(new TransitionType_EaseInEaseOut(1200));
                ts.add(back, "Top", buttop.Top);
                ts.run();
            }
        }

        public int preClick()
        {
            if ((slides.Count - 2) == slidenumb)
            {
                return 1;
            }

            return 0;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (slidenumb > 0)
            {
                slidenumb -= 1;

                var t2 = new Transition(new TransitionType_EaseInEaseOut(1000));
                t2.add(del, "Left", txtimghideback.Right);
                t2.add(del2, "Left", txtimghideback.Right);
                t2.add(del3, "Left", txtimghideback.Right);
                t2.add(getSlide(slidenumb, true), "Left", txtStay.Right);
                t2.add(getLabel1(slidenumb, true), "Left", txtStay.Right);
                t2.add(getLabel2(slidenumb, true), "Left", txtStay.Right);
                t2.run();
            }

            var t = new Transition(new TransitionType_EaseInAndOut(1200));
            t.add(back, "Top", baixobut.Top);
            t.run();

            if (6 == slidenumb)
            {
                var t2 = new Transition(new TransitionType_EaseInEaseOut(1200));
                t2.add(next, "Top", buttop.Top);
                t2.run();
            }

            if (0 == slidenumb)
            {
                var ts = new Transition(new TransitionType_EaseInEaseOut(1200));
                ts.add(back, "Top", baixobut.Top);
                ts.run();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = slidenumb + "";
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            AddUser a = new AddUser();
            a.FormClosing += (sender2, e2) => check(sender2, e2, 1);
            a.ShowDialog();
        }

        int prodadd;
        private void check(object sender, FormClosingEventArgs e, int v)
        {
            if (v == 1)
            {
                p1 = true;
                pictureBox1.Image = Resources.boxchecked;
                checkDone();
            }
            if (v == 2)
            {
                p2 = true;
                pictureBox2.Image = Resources.boxchecked;
                checkDone();
            }
            if (v == 3)
            {
                prodadd += 1;
                p3 = true;
                label12.Visible = true;
                label12.Text = prodadd + "";
                checkDone();
            }
            if (v == 4)
            {
                p4 = true;
                pictureBox4.Image = Resources.boxchecked;
                checkDone();
            }
        }
        int counter = 0;
        Boolean done = false;
        private void checkDone()
        {
            if (p1 == true)
            {
                if (p2 == true)
                {
                    if (p3 == true)
                    {
                        if (p4 == true)
                        {
                            label15.Visible = true;
                            pictureBox6.Image = Properties.Resources.ok;
                            timer2.Start();
                            done = true;
                        }
                    }
                }
            }
        }

        Boolean p1 = false;
        Boolean p2 = false;
        Boolean p3 = false;
        Boolean p4 = false;

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            AddCategory a = new AddCategory();
            a.FormClosing += (sender2, e2) => check(sender2, e2, 2);
            a.ShowDialog();
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            AddProduct a = new AddProduct();
            a.FormClosing += (sender2, e2) => check(sender2, e2, 3);
            a.ShowDialog();
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            MesasAdmin a = new MesasAdmin();
            a.FormClosing += (sender2, e2) => check(sender2, e2, 4);
            a.ShowDialog();
        }

        private void Label12_Click(object sender, EventArgs e)
        {
            AddProduct a = new AddProduct();
            a.FormClosing += (sender2, e2) => check(sender2, e2, 3);
            a.ShowDialog();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter == 100)
                timer2.Stop();
            pictureBox6.Image = Properties.Resources.okstill;
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            SettingsManager.setDataPath(AppDomain.CurrentDomain.BaseDirectory + @"Data");

            SettingsForm f = new SettingsForm();
            f.ShowDialog();
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            if (done == true)
            {
                this.Close();
            }
        }

        private void Welcome_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (done == false)
            {
                e.Cancel = true;
            }
        }
    }
}