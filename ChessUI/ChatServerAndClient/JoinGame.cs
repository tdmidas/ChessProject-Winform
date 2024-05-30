using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessUI.DashboardForm
{
    public partial class JoinGame : Form
    {
        MainMenu parentForm;
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;

        public JoinGame(MainMenu parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 8888); // Change this to your server IP and port
                stream = client.GetStream();
                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true; // Ensure the thread is a background thread
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to the server: " + ex.Message);
            }
        }

        private void ReceiveData()
        {
            byte[] data = new byte[1024];
            int bytesRead;

            while (true)
            {
                try
                {
                    bytesRead = stream.Read(data, 0, data.Length);
                    if (bytesRead == 0)
                    {
                        // Connection closed
                        break;
                    }

                    string message = Encoding.ASCII.GetString(data, 0, bytesRead);
                    HandleMessage(message);
                }
                catch (Exception ex)
                {
                    // Ensure this does not block the UI thread
                    BeginInvoke(new Action(() => MessageBox.Show("Error receiving data from the server: " + ex.Message)));
                    break;
                }
            }
        }

        private void HandleMessage(string message)
        {
            // Handle messages from the server
            if (message == "MatchFound")
            {
                // Invoke on the UI thread to show the new form
                BeginInvoke(new Action(ShowTwoPlayerOnlineForm));
            }
        }

        private void ShowTwoPlayerOnlineForm()
        {
            TwoPlayerOnline twoPlayerOnlineForm = new TwoPlayerOnline(TimeSpan.FromMinutes(10), "Medium");
            twoPlayerOnlineForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parentForm.LoadForm(new ChoosePVPMode(parentForm));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            receiveThread?.Abort(); // Ensure the receive thread is terminated when the form closes
            client?.Close();
        }
    }
}
