using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Retail2.Forms.Admin.Settings
{
    public partial class DummyWindow : Form
    {
        public DummyWindow()
        {
            InitializeComponent();
        }

        private void DummyWindow_SizeChanged(object sender, EventArgs e)
        {
            label1.Text = "Tamanho desta Janela: " + this.Size.Width + " por " + this.Size.Height;
        }

        private void DummyWindow_Load(object sender, EventArgs e)
        {
            label1.Text = "Tamanho desta Janela: " + this.Size.Width + " por " + this.Size.Height;
        }
    }
}
