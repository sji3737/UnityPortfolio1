using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace WoG_Server
{
    class SocketClass
    {
        Socket _socket;
        public int _socketID;
        public long _clientID;

        public SocketClass(Socket socket, int sID, long clientID)
        {
            _socket = socket;
            _socketID = sID;
            _clientID = clientID;
        }

        public void SendPacket(PacketClass pack)
        {
            byte[] data = ConvertPacket.SendToByteArray(pack);
            _socket.Send(data);
        }

        public PacketClass ReceivePacket()
        {
            PacketClass pack = new PacketClass();
            if(_socket.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[1024];
                int recvLen = _socket.Receive(buffer);
                if(recvLen > 0)
                {
                    pack = (PacketClass)ConvertPacket.ReciveToStructure(buffer, pack.GetType(), Marshal.SizeOf(pack));
                }
            }
            return pack;
        }

        public void Bind(EndPoint ep)
        {
            _socket.Bind(ep);
        }

        public void Listen(int time)
        {
            _socket.Listen(time);
        }

        public bool Poll(int time, SelectMode mode)
        {
            return _socket.Poll(time, mode);
        }

        public Socket Accept()
        {
            return _socket.Accept();
        }
    }
}
