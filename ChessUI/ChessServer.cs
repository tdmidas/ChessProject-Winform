using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChessUI
{
    public partial class ChessServer : Form
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private Queue<TcpClient> playerQueue = new Queue<TcpClient>();
        private Thread listenThread;

        public ChessServer()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
            listenThread = new Thread(ListenForClients);
            listenThread.Start();
            buttonStart.Text = "Listening";
            richTextBox1.AppendText("Server is listening on port 8888...\n");
        }

        private void ListenForClients()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                lock (playerQueue)
                {
                    playerQueue.Enqueue(client);
                    richTextBox1.AppendText($"Player added to queue. Queue size: {playerQueue.Count}\n");

                    if (playerQueue.Count >= 2)
                    {
                        StartMatch();
                    }
                }
            }
        }

        private void StartMatch()
        {
            TcpClient player1 = playerQueue.Dequeue();
            TcpClient player2 = playerQueue.Dequeue();

            // Send "isHost:true" to the first player
            SendIsHostMessage(player1, true);
            // Send "isHost:false" to the second player
            SendIsHostMessage(player2, false);

            // Notify both players that a match has been found
            SendMatchFoundMessage(player1);
            SendMatchFoundMessage(player2);

            lock (clients)
            {
                clients.Add(player1);
                clients.Add(player2);
            }

            Thread player1Thread = new Thread(HandleClient);
            Thread player2Thread = new Thread(HandleClient);
            player1Thread.Start(player1);
            player2Thread.Start(player2);

            richTextBox1.AppendText("Match found! Players have been paired.\n");
        }

        private void SendMatchFoundMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            string message = "MatchFound";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void SendIsHostMessage(TcpClient client, bool isHost)
        {
            NetworkStream stream = client.GetStream();
            string message = isHost ? "isHost:true" : "isHost:false";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void HandleClient(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream clientStream = client.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                    if (bytesRead == 0)
                    {
                        break; // Client disconnected
                    }
                    string receivedMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                    BroadcastMessage(receivedMessage);
                    richTextBox1.AppendText(receivedMessage + "\n");
                }
                catch
                {
                    break; // Exception occurred (client disconnected)
                }
            }

            // Remove client from the list when it disconnects
            lock (clients)
            {
                clients.Remove(client);
            }
        }

        private void BroadcastMessage(string message)
        {
          
                foreach (TcpClient client in clients)
                {
                    NetworkStream clientStream = client.GetStream();
                    byte[] broadcastMessage = Encoding.ASCII.GetBytes(message);
                    clientStream.Write(broadcastMessage, 0, broadcastMessage.Length);
                }
            
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (server != null)
            {
                server.Stop();
            }
            base.OnFormClosing(e);
        }
    }
}
