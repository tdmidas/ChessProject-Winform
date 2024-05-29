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
    public partial class ChoosePVPMode : Form
    {
        MainMenu parentForm;
        public ChoosePVPMode(MainMenu parentf)
        {
            InitializeComponent();
            parentForm = parentf;
        }

        private void ChoosePVPMode_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                parentForm.LoadForm(new WaitingRoom(parentForm));
            }

        }
    }
}
