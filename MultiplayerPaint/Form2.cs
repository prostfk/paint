using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace prostrmk
{

    public partial class Form2 : Form
    {
        delegate void AddMessage(string message);

        //string userName;

        const int port = 54545;
        const string broadcastAddress = "255.255.255.255";//"127.0.0.1";

        UdpClient receivingClient;
        UdpClient sendingClient;

        Thread receivingThread;


        public Form2()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        private void StartGame()
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }
        private void InitializeSender()
        {
            sendingClient = new UdpClient(broadcastAddress, port);
            sendingClient.EnableBroadcast = true;
        }
        private void InitializeReceiver()
        {
            receivingClient = new UdpClient(port);
            ThreadStart start = new ThreadStart(Receiver);
            receivingThread = new Thread(start);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void Receiver()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            AddMessage messageDelegate = MessageReceived;

            while (true)
            {
                byte[] data = receivingClient.Receive(ref endPoint);
                string message = Encoding.ASCII.GetString(data);
                FileStream fs = new FileStream(Environment.GetFolderPath(
                         System.Environment.SpecialFolder.DesktopDirectory) + "\\Text.exe", FileMode.Append, FileAccess.Write);
                fs.Write(data, 0, data.Length);
                fs.Close();

                Invoke(messageDelegate, message);
            }
        }

        private void MessageReceived(string message)
        {
            txtMessage.Text += message + "\n";
            txtMessage.SelectionStart = txtMessage.Text.Length;
            txtMessage.ScrollToCaret();

        }

        void btnSend_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void Send()
        {
            txtSend.Text = txtSend.Text;//.TrimEnd();

            if (!string.IsNullOrEmpty(txtSend.Text))
            {
                //string toSend = userName + ": " + txtSend.Text;
                //byte[] data = Encoding.ASCII.GetBytes(toSend);
                //sendingClient.Send(data, data.Length);
                txtSend.Text = "";
            }

            txtSend.Focus();
        }


        private void txtSend_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
