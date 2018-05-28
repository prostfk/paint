using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace prostrmk
{
    public partial class MainWindow : Window
    {
        public delegate void AddLineToCanvasDelegate(DrawingData receivedData);
        private Color currentColor;
        private SolidColorBrush currentBrush;
        private Point currentPoint = new Point();
        private int drawingServerPort;
        private ushort myPort, serverStartPort;
        private UdpClient sender, receiver;
        private DrawingData[] lastPoints = new DrawingData[255];
        private Color[] lastColors = new Color[255];
        private Thread receiveFromServer;
        private bool shouldStop = false, isConnected = false;
        private string serverIP;

        public MainWindow()
        {
            InitializeComponent();
            currentColor = colorPicker.SelectedColor ?? Colors.Black;
            currentBrush = new SolidColorBrush(currentColor);
            connectButton.IsEnabled = true;
            disconnectButton.IsEnabled = false;
        }

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DrawingData data;
                currentPoint = e.GetPosition(this);
                if (sender != null && isConnected)
                {
                    data = new DrawingData(currentPoint.X, currentPoint.Y, currentColor.A, currentColor.R, currentColor.G, currentColor.B);
                    Byte[] data1 = data.ToBytesAll();
                    this.sender.Send(data1, data1.Length);
                }
            }
        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawingData data;
                Line line = new Line();
                line.Stroke = currentBrush;

                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;
                currentPoint = e.GetPosition(this);
                if (sender != null && isConnected)
                {
                    data = new DrawingData(line.X1, line.Y1);
                    Byte[] data1 = data.ToBytesPoints();
                    this.sender.Send(data1, data1.Length);

                    data = new DrawingData(line.X2, line.Y2);
                    Byte[] data2 = data.ToBytesPoints();
                    this.sender.Send(data2, data2.Length);
                }
                paintSurface.Children.Add(line);
            }
        }
        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color newColor = colorPicker.SelectedColor ?? Colors.Black;
            SolidColorBrush newBrush = new SolidColorBrush(newColor);
            currentBrush = newBrush;
            currentColor = newColor;
        }
        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //myPort = ushort.Parse(myPortTextBox.Text);
                myPort = (ushort)new Random().Next(1000, 9000);
                serverIP = serverIPTextBox.Text != "" ? serverIPTextBox.Text : null;
                serverStartPort = ushort.Parse(serverPortTextBox.Text);
                this.sender = new UdpClient(myPort);
                this.sender.Connect(serverIP, serverStartPort);
                Byte[] sendBytes = Encoding.ASCII.GetBytes("connect");
                this.sender.Send(sendBytes, sendBytes.Length);
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = this.sender.Receive(ref RemoteIpEndPoint);
                drawingServerPort = int.Parse(Encoding.ASCII.GetString(receiveBytes));
                Console.WriteLine("Port for drawing data " + drawingServerPort);

                this.sender.Connect(serverIP, drawingServerPort);
                isConnected = true;
                shouldStop = false;
                receiveFromServer = new Thread(new ThreadStart(ReceiveData));
                receiveFromServer.Start();
                connectButton.IsEnabled = false;
                disconnectButton.IsEnabled = true;
                statusLabel.Content = "Connected";
            }
            catch (SocketException)
            {
                System.Windows.MessageBox.Show("   There's no listening server under that address");
            }
            catch (FormatException)
            {
                System.Windows.MessageBox.Show("    Bad format of data", "Port");
            }
            catch (ArgumentNullException)
            {
                System.Windows.MessageBox.Show("    Bad format of data", "Server IP");
            }
            catch (OverflowException)
            {
                System.Windows.MessageBox.Show("    Bad format of data", "Port");
            }
        }
        
        private void ReceiveData()
        {
            Console.WriteLine("Waiting for data from server");
            receiver = new UdpClient(myPort + 1);
            receiver.BeginReceive(ProcessData, receiver);

        }

        private void ProcessData(IAsyncResult ar)
        {
            if (!shouldStop)
            {
                UdpClient c = (UdpClient)ar.AsyncState;
                IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

                DrawingData receivedData = DrawingData.ToData(receivedBytes);
                if (receivedBytes.Length > 17)
                {
                    lastPoints[receivedData.ClientID] = null;
                    lastColors[receivedData.ClientID] = Color.FromArgb(receivedData.A, receivedData.R, receivedData.G, receivedData.B);
                }
                paintSurface.Dispatcher.Invoke(new AddLineToCanvasDelegate(AddLineToCanvas), new DrawingData[] { receivedData });
                c.BeginReceive(ProcessData, ar.AsyncState);
            }
            else
                return;
        }

        private void AddLineToCanvas(DrawingData receivedData)
        {
            Line line = new Line();
            SolidColorBrush brush = new SolidColorBrush(lastColors[receivedData.ClientID]);
            line.Stroke = brush;
            if (lastPoints[receivedData.ClientID] != null)
            {
                line.X1 = lastPoints[receivedData.ClientID].X;
                line.Y1 = lastPoints[receivedData.ClientID].Y;
                line.X2 = receivedData.X;
                line.Y2 = receivedData.Y;
                lastPoints[receivedData.ClientID] = receivedData;
                paintSurface.Children.Add(line);
            }
            else
                lastPoints[receivedData.ClientID] = receivedData;
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            connectButton.IsEnabled = true;
            disconnectButton.IsEnabled = false;
            statusLabel.Content = "Disconnected";
            shouldStop = true;
            isConnected = false;
            Byte[] sendBytes = Encoding.ASCII.GetBytes("disconnect");
            this.sender.Connect(serverIP, serverStartPort);
            this.sender.Send(sendBytes, sendBytes.Length);
            this.sender.Close();
            receiver.Close();
        }
    }
}