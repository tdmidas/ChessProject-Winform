using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessUI.DashboardForm
{
    public partial class Home : Form
    {
        public event EventHandler ChildPvEButton_Click;
        public event EventHandler ChildPvPButton_Click;

        public Home()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void PvEButton_Click(object sender, EventArgs e)
        {
            ChildPvEButton_Click?.Invoke(this, e);

        }

        private void PvPButton_Click(object sender, EventArgs e)
        {
            ChildPvPButton_Click?.Invoke(this, e);

        }
    }
}
