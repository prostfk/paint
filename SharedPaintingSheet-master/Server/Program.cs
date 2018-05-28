using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ptlab12Server
{
    class Program
    {
        static int receiverPort = 1234;
        static int drawingPort = 1000;
        static int sendingPort = 2000;
        static UdpClient newClientReceiver = new UdpClient(receiverPort);
        static byte clientCounter = 0;
        static private ConcurrentDictionary<byte, ClientInfo> clients = new ConcurrentDictionary<byte, ClientInfo>();
        static private ConcurrentQueue<DrawingData> dataQueue = new ConcurrentQueue<DrawingData>();
        static void Main(string[] args)
        {
            Task listenForNewClients = Task.Factory.StartNew(StartListening);
            Task listenForDataFromClients = Task.Factory.StartNew(ReceiveDrawingData);
            Task sendDataToClients = Task.Factory.StartNew(SendToAll);
            Console.ReadKey();
        }

        private static void SendToAll()
        {
            IPEndPoint newEndPoint;
            DrawingData data;
            Byte[] bytes;
            ClientInfo owner;
            UdpClient sender = new UdpClient(sendingPort);
            while (true)
            {
                dataQueue.TryDequeue(out data);
                if (data != null)
                {
                    owner = FindClientByID(data.ClientID);
                    if (owner != null && owner.NewLine)
                    {
                        owner.NewLine = false;
                        bytes = data.ToBytesAll();
                        foreach (ClientInfo client in clients.Values)
                        {
                            if (data.ClientID != client.ID)
                            {
                                newEndPoint = new IPEndPoint(client.IP.Address, client.Port);
                                sender.SendAsync(bytes, bytes.Length, newEndPoint);
                            }
                        }
                    }
                    else if (owner != null && !owner.NewLine)
                    {
                        bytes = data.ToBytesPoints();
                        foreach (ClientInfo client in clients.Values)
                        {
                            if (data.ClientID != client.ID)
                            {
                                newEndPoint = new IPEndPoint(client.IP.Address, client.Port);
                                sender.SendAsync(bytes, bytes.Length, newEndPoint);
                            }
                        }
                    }
                }
            }
        }

        private static void ReceiveDrawingData()
        {
            UdpClient dataReceiver = new UdpClient(drawingPort);
            dataReceiver.BeginReceive(ProcessData, dataReceiver);
        }

        private static void ProcessData(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;
            IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);
            
            DrawingData receivedData = DrawingData.ToData(receivedBytes);
            
            ClientInfo client = FindClient(receivedIpEndPoint);
            if (client != null)
            {
                receivedData.ClientID = client.ID;
                if (receivedBytes.Length > 17)
                    client.NewLine = true;
                dataQueue.Enqueue(receivedData);
            }
            c.BeginReceive(ProcessData, ar.AsyncState);
        }

        private static void StartListening()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            var localIP = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            Console.WriteLine("Listening for new clients IP {0} on port {1} ", localIP, receiverPort);
            newClientReceiver.BeginReceive(DataReceived, newClientReceiver);
        }
        private static void DataReceived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;
            IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
            Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);
            string command = Encoding.ASCII.GetString(receivedBytes);
            if (command == "disconnect")
            {
                ClientInfo client = FindClient(receivedIpEndPoint);
                if(client != null)
                {
                    Console.WriteLine("Client ID {0} disconnected", client.ID);
                    clients.TryRemove(client.ID, out client);
                    Console.WriteLine("{0} clients connected", clients.Count);
                }
            }
            else if(command=="connect")
            {
                int clientPort = receivedIpEndPoint.Port + 1;
                clients.TryAdd(clientCounter, new ClientInfo(receivedIpEndPoint, clientCounter, clientPort));
                Console.WriteLine(receivedIpEndPoint + " New client ID {0} is connected", clientCounter);
                Console.WriteLine("{0} clients connected",clients.Count);
                clientCounter++;
                Byte[] portToSend = Encoding.ASCII.GetBytes(drawingPort.ToString());
                newClientReceiver.SendAsync(portToSend, portToSend.Length, receivedIpEndPoint);
            }
            c.BeginReceive(DataReceived, ar.AsyncState);
        }

        private static ClientInfo FindClient(IPEndPoint newIP)
        {
            foreach(ClientInfo info in clients.Values)
            {
                if (info.IP.Equals(newIP)) return info;
            }
            return null;
        }
        private static ClientInfo FindClientByID(byte id)
        {
            foreach (ClientInfo info in clients.Values)
            {
                if (info.ID.Equals(id)) return info;
            }
            return null;
        }
    }
}


