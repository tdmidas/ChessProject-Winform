using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessUI
{
    public partial class MainMenu : Form
    {
        bool sideBarExpanded = false;
        private Form childForm;
        public string username;
        public MainMenu()
        {
            InitializeComponent();
        }
        public void LoadForm(Form Child)
        {
            if (childForm != null)
            {
                childForm.Close();
            }
            childForm = Child;
            Child.TopLevel = false;
            Child.FormBorderStyle = FormBorderStyle.None;
            Child.Dock = DockStyle.Fill;
            // make the child form center
            Child.Location = new Point((MainPanel.Width - Child.Width) / 2, (MainPanel.Height - Child.Height) / 2);
            MainPanel.Controls.Add(Child);
            MainPanel.Tag = Child;
            Child.BringToFront();
            Child.Show();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            var temp = new DashboardForm.Home();
            LoadForm(temp);
            temp.ChildPvEButton_Click += new EventHandler(PvEButton_Click);
            temp.ChildPvPButton_Click += new EventHandler(PvPButton_Click);
        }
        private void SideBarTimer_Tick(object sender, EventArgs e)
        {


            if (sideBarExpanded)
            {
                SideBar.Width -= 20;
                if (SideBar.Width == SideBar.MinimumSize.Width)
                {
                    sideBarExpanded = false;
                    SideBarTimer.Stop();
                }
            }
            else
            {
                SideBar.Width += 20;
                if (SideBar.Width == SideBar.MaximumSize.Width)
                {
                    sideBarExpanded = true;
                    SideBarTimer.Stop();
                }
            }
        }
        private void PvEButton_Click(object sender, EventArgs e)
        {
            var temp = new DashboardForm.ChoosePVEMode();
            LoadForm(temp);
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            SideBarTimer.Start();

        }

        private void SideBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            var temp = new DashboardForm.Home();
            LoadForm(temp);

        }

        private void PvPButton_Click(object sender, EventArgs e)
        {
            var temp = new DashboardForm.ChoosePVPMode(this);
            LoadForm(temp);
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            var Login = new LoginForm();
            Login.Show();
            this.Hide();
            MessageBox.Show("You have been logged out");
        }
    }
}
