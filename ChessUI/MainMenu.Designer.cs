namespace ChessUI
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            SideBar = new System.Windows.Forms.FlowLayoutPanel();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            panel5 = new System.Windows.Forms.Panel();
            MenuButton = new System.Windows.Forms.Button();
            HomePanel = new System.Windows.Forms.Panel();
            HomeButton = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            PvEButton = new System.Windows.Forms.Button();
            panel2 = new System.Windows.Forms.Panel();
            PvPButton = new System.Windows.Forms.Button();
            panel7 = new System.Windows.Forms.Panel();
            LogoutButton = new System.Windows.Forms.Button();
            SideBarTimer = new System.Windows.Forms.Timer(components);
            MainPanel = new System.Windows.Forms.Panel();
            SideBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel5.SuspendLayout();
            HomePanel.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // SideBar
            // 
            SideBar.BackColor = System.Drawing.SystemColors.Info;
            SideBar.Controls.Add(pictureBox2);
            SideBar.Controls.Add(panel5);
            SideBar.Controls.Add(HomePanel);
            SideBar.Controls.Add(panel1);
            SideBar.Controls.Add(panel2);
            SideBar.Controls.Add(panel7);
            SideBar.Dock = System.Windows.Forms.DockStyle.Left;
            SideBar.Location = new System.Drawing.Point(0, 0);
            SideBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            SideBar.MaximumSize = new System.Drawing.Size(206, 516);
            SideBar.MinimumSize = new System.Drawing.Size(52, 516);
            SideBar.Name = "SideBar";
            SideBar.Size = new System.Drawing.Size(206, 516);
            SideBar.TabIndex = 3;
            SideBar.Paint += SideBar_Paint;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new System.Drawing.Point(3, 2);
            pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(230, 75);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // panel5
            // 
            panel5.Controls.Add(MenuButton);
            panel5.Location = new System.Drawing.Point(3, 81);
            panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(185, 46);
            panel5.TabIndex = 6;
            // 
            // MenuButton
            // 
            MenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            MenuButton.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            MenuButton.Image = (System.Drawing.Image)resources.GetObject("MenuButton.Image");
            MenuButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            MenuButton.Location = new System.Drawing.Point(-16, -12);
            MenuButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MenuButton.Name = "MenuButton";
            MenuButton.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            MenuButton.Size = new System.Drawing.Size(214, 73);
            MenuButton.TabIndex = 0;
            MenuButton.Text = "Menu";
            MenuButton.UseVisualStyleBackColor = true;
            MenuButton.Click += MenuButton_Click;
            // 
            // HomePanel
            // 
            HomePanel.Controls.Add(HomeButton);
            HomePanel.Location = new System.Drawing.Point(3, 131);
            HomePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HomePanel.Name = "HomePanel";
            HomePanel.Size = new System.Drawing.Size(185, 46);
            HomePanel.TabIndex = 4;
            // 
            // HomeButton
            // 
            HomeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            HomeButton.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            HomeButton.Image = (System.Drawing.Image)resources.GetObject("HomeButton.Image");
            HomeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            HomeButton.Location = new System.Drawing.Point(-16, -12);
            HomeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            HomeButton.Name = "HomeButton";
            HomeButton.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            HomeButton.Size = new System.Drawing.Size(222, 73);
            HomeButton.TabIndex = 0;
            HomeButton.Text = "Home";
            HomeButton.UseVisualStyleBackColor = true;
            HomeButton.Click += HomeButton_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(PvEButton);
            panel1.Location = new System.Drawing.Point(3, 181);
            panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(185, 46);
            panel1.TabIndex = 2;
            // 
            // PvEButton
            // 
            PvEButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            PvEButton.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            PvEButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            PvEButton.Location = new System.Drawing.Point(-36, -12);
            PvEButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            PvEButton.Name = "PvEButton";
            PvEButton.Size = new System.Drawing.Size(242, 73);
            PvEButton.TabIndex = 0;
            PvEButton.Text = "🤖 PvE 🤖 ";
            PvEButton.UseVisualStyleBackColor = true;
            PvEButton.Click += PvEButton_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(PvPButton);
            panel2.Location = new System.Drawing.Point(3, 231);
            panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(185, 46);
            panel2.TabIndex = 3;
            // 
            // PvPButton
            // 
            PvPButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            PvPButton.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            PvPButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            PvPButton.Location = new System.Drawing.Point(-44, -12);
            PvPButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            PvPButton.Name = "PvPButton";
            PvPButton.Size = new System.Drawing.Size(250, 73);
            PvPButton.TabIndex = 0;
            PvPButton.Text = "⚔ PvP ⚔️";
            PvPButton.UseVisualStyleBackColor = true;
            PvPButton.Click += PvPButton_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(LogoutButton);
            panel7.Location = new System.Drawing.Point(3, 281);
            panel7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            panel7.Name = "panel7";
            panel7.Size = new System.Drawing.Size(185, 46);
            panel7.TabIndex = 7;
            // 
            // LogoutButton
            // 
            LogoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            LogoutButton.Font = new System.Drawing.Font("Showcard Gothic", 18F, System.Drawing.FontStyle.Bold);
            LogoutButton.Image = (System.Drawing.Image)resources.GetObject("LogoutButton.Image");
            LogoutButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            LogoutButton.Location = new System.Drawing.Point(-16, -12);
            LogoutButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            LogoutButton.Name = "LogoutButton";
            LogoutButton.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            LogoutButton.Size = new System.Drawing.Size(222, 73);
            LogoutButton.TabIndex = 0;
            LogoutButton.Text = "Logout";
            LogoutButton.UseVisualStyleBackColor = true;
            // 
            // SideBarTimer
            // 
            SideBarTimer.Interval = 10;
            SideBarTimer.Tick += SideBarTimer_Tick;
            // 
            // MainPanel
            // 
            MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            MainPanel.Location = new System.Drawing.Point(0, 0);
            MainPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new System.Drawing.Size(962, 519);
            MainPanel.TabIndex = 4;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(962, 519);
            Controls.Add(SideBar);
            Controls.Add(MainPanel);
            Name = "MainMenu";
            Text = "MainMenu";
            Load += MainMenu_Load;
            SideBar.ResumeLayout(false);
            SideBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel5.ResumeLayout(false);
            HomePanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel SideBar;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button MenuButton;
        private System.Windows.Forms.Panel HomePanel;
        private System.Windows.Forms.Button HomeButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button PvEButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button PvPButton;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button LogoutButton;
        private System.Windows.Forms.Timer SideBarTimer;
        private System.Windows.Forms.Panel MainPanel;
    }
}