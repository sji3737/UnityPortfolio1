using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;


public class ConvertPacket
{
    //obj = 구조체. 패킷의 종류는 다양하기 때문에 다양하게 받을 수 있는 object로 받음
    public static byte[] SendToByteArray(object obj)
    {
        //구조체에 할당된 메모리의 크기를 구함
        int datasize = Marshal.SizeOf(obj);
        Debug.Log(datasize);
        //비관리 메모리 영역에 구조체 크기만큼의 메모리를 할당
        IntPtr buff = Marshal.AllocHGlobal(datasize);
        //할당된 구조체 객체의 주소를 구함
        Marshal.StructureToPtr(obj, buff, false);
        //구조체가 복사될 배열을 할당
        byte[] data = new byte[datasize];
        //구조체 객체를 배열에 복사
        Marshal.Copy(buff, data, 0, datasize);
        //비관리 메모리 영역에 할당된 메모리를 해제
        Marshal.FreeHGlobal(buff);
        //배열을 반환
        return data;
    }

    public static object ReceiveToStructure(byte[] data, Type type, int size)
    {
        //배열의 크기만큼 비관리 메모리 영역에 메모리를 할당
        IntPtr buff = Marshal.AllocHGlobal(data.Length);
        //배열에 저장된 데이터를 위해서 할당한 메모리 영역에 복사
        Marshal.Copy(data, 0, buff, data.Length);
        // 복사된 데이터를 구조체 객체로 변환
        object obj = Marshal.PtrToStructure(buff, type);
        //비관리 메모리 영역에 할당했던 메모리를 해제
        Marshal.FreeHGlobal(buff);
        //구조체와 원래 데이터 크기를 비교
        if (Marshal.SizeOf(obj) != size)
        {
            return null;
        }
        //구조체 반환
        return obj;
    }
}
