using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoG_Server
{
    class SocketManager
    {
        List<SocketClass> _ltSocket = new List<SocketClass>();
        TCPServer _server;

        public void Init(TCPServer sever)
        {
            _server = sever;
        }

        public void AddSocket(SocketClass socket)
        {
            _ltSocket.Add(socket);
        }

        public SocketClass FindSocketWithClientID(long clientID)
        {
            return _ltSocket.Find(x => x._clientID == clientID);
        }

        public void ReceivePacket()
        {

            foreach(SocketClass s in _ltSocket)
            {
                PacketClass pack = s.ReceivePacket();
                if(pack._clientID != 0)
                {
                    Console.WriteLine("소켓매니저: {0}", pack._protocolID);
                }

                if(pack!=null && pack._totalSize > 0)
                {
                    _server.EnqueueReceivePacket(pack);
                }
            }
        }
    }
}
