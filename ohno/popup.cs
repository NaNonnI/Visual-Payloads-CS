using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ohno
{
    public partial class popup : Form
    {

        Random rnd = new Random();

        public popup()
        {
            InitializeComponent();
        }

        private void popup_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, rnd.Next(0,255), rnd.Next(0, 255), rnd.Next(0, 255));
            this.TopMost = true;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.Dispose();
            this.Close();
            timer.Stop();
        }
    }
}
