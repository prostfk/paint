using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace prostrmk
{
    class UDPConnecter
    {
        IPEndPoint endPoint;
        public UDPConnecter()
        {
            InitializeSender();
            receivingClient = new UdpClient();
            receivingClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            receivingClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            endPoint = new IPEndPoint(IPAddress.Any, port);
        }


        const int port = 54545;
        const string broadcastAddress = "255.255.255.255";

        UdpClient receivingClient;
        UdpClient sendingClient;

        private void InitializeSender()
        {
            sendingClient = new UdpClient(broadcastAddress, port);
            sendingClient.EnableBroadcast = true;
        }
        
        public byte[] Receiver()
        {
            return receivingClient.Receive(ref endPoint);
        }

        public void Send(byte[] DataToSend)
        {
            if (DataToSend != null)
            {
                sendingClient.Send(DataToSend, DataToSend.Length);
            }
        }
    }
}
