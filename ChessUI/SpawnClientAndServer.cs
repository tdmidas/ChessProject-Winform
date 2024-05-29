﻿using System;
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
    public partial class SpawnClientAndServer : Form
    {
        public SpawnClientAndServer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TwoPlayerLAN twoPlayerLAN = new TwoPlayerLAN(TimeSpan.FromMinutes(10),"Medium");
            twoPlayerLAN.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChessServer chessServer = new ChessServer();
            chessServer.Show();
        }
    }
}
