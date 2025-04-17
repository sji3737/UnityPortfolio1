using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential)]
[Serializable]
public class PacketClass
{
    [MarshalAs(UnmanagedType.U4)]
    public int _protocolID;
    [MarshalAs(UnmanagedType.U4)]
    public int _totalSize;
    [MarshalAs(UnmanagedType.U8)]
    public long _clientID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1008)]
    public byte[] _data;

    public void modData(int id, int clientID, int size, byte[] data)
    {
        _protocolID = id;
        _clientID = clientID;
        _totalSize = size;
        _data = data;
    }
}
