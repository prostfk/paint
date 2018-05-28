using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;

namespace ptlab12Server
{
    [Serializable]
    public class DrawingData
    {
        public byte ClientID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }


        public DrawingData()
        {

        }
        public DrawingData(byte ID, double x, double y)
        {
            ClientID = ID;
            X = x;
            Y = y;
        }
        public DrawingData(double x, double y)
        {
            X = x;
            Y = y;
        }
        public DrawingData(double x, double y, byte a, byte r, byte g, byte b)
        {
            X = x;
            Y = y;
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public DrawingData(byte id, double x, double y, byte a, byte r, byte g, byte b)
        {
            ClientID = id;
            X = x;
            Y = y;
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public byte[] ToBytesAll()
        {
            List<byte> bytesToSend = new List<byte>();
            bytesToSend.Add(ClientID);
            bytesToSend.AddRange(BitConverter.GetBytes(X));
            bytesToSend.AddRange(BitConverter.GetBytes(Y));
            bytesToSend.Add(A);
            bytesToSend.Add(R);
            bytesToSend.Add(G);
            bytesToSend.Add(B);

            return bytesToSend.ToArray();
        }
        public byte[] ToBytesPoints()
        {
            List<byte> bytesToSend = new List<byte>();
            bytesToSend.Add(ClientID);
            bytesToSend.AddRange(BitConverter.GetBytes(X));
            bytesToSend.AddRange(BitConverter.GetBytes(Y));

            return bytesToSend.ToArray();
        }
        public static DrawingData ToData(byte[] bytes)
        {
            DrawingData result = new DrawingData();
            result.ClientID = bytes[0];
            result.X = BitConverter.ToDouble(bytes, 1);
            result.Y = BitConverter.ToDouble(bytes, 9);
            if (bytes.Length > 17)
            {
                result.A = bytes[17];
                result.R = bytes[18];
                result.G = bytes[19];
                result.B = bytes[20];
            }
            return result;
        }

    }
}
