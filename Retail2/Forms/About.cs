using Retail2.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;
using Transitions;

namespace Retail2.Forms
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        int selected = 0;
        Control curr;

        private void About_Load(object sender, EventArgs e)
        {
            label5.Text = "Este programa foi criado ás\r\n12:27 do dia 27 de julho de 2019,\r\nhá exatamente " + Time.calculateTimeAbout("27/07/2019 12:27:05");

            displayInfo(selected);

            webBrowser1.DocumentText = Retail2.Properties.Resources.agreement;
        }

        private void displayInfo(int selected)
        {
            if (selected == 0)
            {
                var t = new Transition(new TransitionType_EaseInEaseOut(1000));
                t.add(retail2, "Top", up.Top);
                t.run();
                curr = retail2;
            }
            if (selected == 1)
            {
                var t = new Transition(new TransitionType_EaseInEaseOut(1000));
                t.add(inf, "Top", up.Top);
                t.run();
                curr = inf;
            }
            if (selected == 2)
            {
                var t = new Transition(new TransitionType_EaseInEaseOut(1000));
                t.add(legal, "Top", up.Top);
                t.run();
                curr = legal;
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            selected = 0;
            Label l = (Label)sender;
            selectWindow(l);
        }

        private void selectWindow(Label l)
        {
            //others

            label1.Font =
                  new Font
                     (
                        label1.Font,
                        FontStyle.Regular
                     );
            label2.Font =
                  new Font
                     (
                        label1.Font,
                        FontStyle.Regular
                     );
            label3.Font =
                  new Font
                     (
                        label1.Font,
                        FontStyle.Regular
                     );

            l.Font =
      new Font
         (
            l.Font,
            FontStyle.Underline
         );

            var t = new Transition(new TransitionType_EaseInEaseOut(1000));
            t.add(curr, "Top", down.Top);
            t.run();

            displayInfo(selected);
        }


        private void Label2_Click(object sender, EventArgs e)
        {
            selected = 1;
            Label l = (Label)sender;
            selectWindow(l);
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            selected = 2;
            Label l = (Label)sender;
            selectWindow(l);
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
    }
}
