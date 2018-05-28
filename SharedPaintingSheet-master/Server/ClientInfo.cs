using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ptlab12Server
{
    class ClientInfo
    {
        
        public byte ID { get; set; }
        
        public IPEndPoint IP { get; set; }
        public int Port { get; set; }
        public bool NewLine { get; set; }
        
        public ClientInfo(IPEndPoint ipend, byte cntr, int port)
        {
            IP = ipend;
            ID = cntr;
            Port = port;
            NewLine = true;
        }



    }
}
