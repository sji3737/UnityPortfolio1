using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    const string _ip = "127.0.0.1";
    const int _port = 80;
    Socket _server;

    long _clientID;
    long _uuid;

    string _id;
    string _pw;
    string _nickName;

    bool _isConnect;

    Queue<PacketClass> _receiveQueue= new Queue<PacketClass>();
    Queue<PacketClass> _sendQueue = new Queue<PacketClass>();

    static TCPClient _uniqueInstance;

    public static TCPClient _Instace
    {
        get { return _uniqueInstance; }
    }

    public bool _IsConnect
    {
        get { return _isConnect; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ProcessSendPacket());
        StartCoroutine(ProcessReceivePacket());
    }

    // Update is called once per frame
    void Update()
    {
        if(_server != null && _server.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int recvLen = _server.Receive(buffer);
            if(recvLen > 0)
            {
                PacketClass pack = new PacketClass();
                pack = (PacketClass)ConvertPacket.ReceiveToStructure(buffer, pack.GetType(), Marshal.SizeOf(pack));
                _receiveQueue.Enqueue(pack);
            }
        }
    }

    IEnumerator ProcessSendPacket()
    {
        while (true)
        {
            if (_sendQueue.Count > 0)
            {
                PacketClass pack = _sendQueue.Dequeue();
                Debug.Log("SendPacket Size: " + Marshal.SizeOf(pack));
                byte[] buffer = ConvertPacket.SendToByteArray(pack);
                _server.Send(buffer);
            }
            yield return null;
        }
    }

    IEnumerator ProcessReceivePacket()
    {
        while (true)
        {
            if(_receiveQueue.Count > 0)
            {
                PacketClass pack = _receiveQueue.Dequeue();
                switch (pack._protocolID)
                {
                    case (int)DefineProtocol.eClient.Recv_Connect:
                        ReceiveConnectPack(pack);
                        break;
                    case (int)DefineProtocol.eClient.Recv_Join_Result:
                        ReceiveJoinPack(pack);
                        break;
                    case (int)DefineProtocol.eClient.Recv_Login_Result:
                        ReceiveLoginPack(pack);
                        break;
                    case (int)DefineProtocol.eClient.Recv_IncreaseUpgraed_Result:
                        ReceiveIncreaseUpgradeResult(pack);
                        break;
                    case (int)DefineProtocol.eClient.Recv_GameResultHandel_Result:
                        ReceiveGameResultHandelResult(pack);
                        break;
                }
            }
            yield return null;
        }
    }

    public bool ConnectServer()
    {
        try
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Connect(_ip, _port);
            _isConnect = true;
            return true;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        return false;
    }

    public void SendJoinPacket(string id, string pw, string nickName)
    {
        PacketClass pack = new PacketClass();
        DefinePacket.Send_Join joinPack = new DefinePacket.Send_Join();
        joinPack.ModData(id, pw, nickName);

        pack._protocolID = (int)DefineProtocol.eClient.Send_Join;
        pack._clientID = _clientID;
        pack._totalSize = Marshal.SizeOf(joinPack);
        pack._data = ConvertPacket.SendToByteArray(joinPack);

        _sendQueue.Enqueue(pack);
    }

    public void SendLoginPacket(string id, string pw)
    {
        PacketClass pack = new PacketClass();
        DefinePacket.Send_Login loginPack = new DefinePacket.Send_Login();
        loginPack.ModData(id, pw);

        pack._protocolID = (int)DefineProtocol.eClient.Send_Login;
        pack._clientID = _clientID;
        pack._totalSize = Marshal.SizeOf(loginPack);
        pack._data = ConvertPacket.SendToByteArray(loginPack);

        _sendQueue.Enqueue(pack);
    }

    public void SendIncreaseUpgradeRequest(PublicDefine.eUpgradeType type, int requireCredit)
    {
        PacketClass pack = new PacketClass();
        DefinePacket.Send_IncreaseUpgradeRequest requsetPack = new DefinePacket.Send_IncreaseUpgradeRequest();
        requsetPack._upgradeType = (int)type;
        requsetPack._credit = requireCredit;
        requsetPack._uuid = _uuid;

        pack._protocolID = (int)DefineProtocol.eClient.Send_IncreaseUpgrade_Request;
        pack._clientID = _clientID;
        pack._totalSize = Marshal.SizeOf(requsetPack);
        pack._data = ConvertPacket.SendToByteArray(requsetPack);

        _sendQueue.Enqueue(pack);
    }

    public void SendGameResultPacket(int credit, int stage)
    {
        PacketClass pack = new PacketClass();
        DefinePacket.Send_GameResult gameResultPack = new DefinePacket.Send_GameResult();
        gameResultPack._credit = credit;
        gameResultPack._stage = stage;
        gameResultPack._uuid = _uuid;

        pack._protocolID = (int)DefineProtocol.eClient.Send_GameResultHandle_Request;
        pack._clientID = _clientID;
        pack._totalSize = Marshal.SizeOf(gameResultPack);
        pack._data = ConvertPacket.SendToByteArray(gameResultPack);

        _sendQueue.Enqueue(pack);
    }

    void ReceiveConnectPack(PacketClass pack)
    {
        DefinePacket.Receive_Connect connPack = new DefinePacket.Receive_Connect();
        connPack = (DefinePacket.Receive_Connect)ConvertPacket.ReceiveToStructure(pack._data, connPack.GetType(), pack._totalSize);
        if (connPack._isSuccess)
        {
            Debug.Log("접속 성공");
            Debug.Log("client id: " + connPack._clientID);
        }
        _clientID = connPack._clientID;
    }

    void ReceiveJoinPack(PacketClass pack)
    {
        DefinePacket.Receive_JoinResult joinPack = new DefinePacket.Receive_JoinResult();
        joinPack = (DefinePacket.Receive_JoinResult)ConvertPacket.ReceiveToStructure(pack._data, joinPack.GetType(), pack._totalSize);
        Debug.Log("JoinPack Result Code: " + joinPack._resultCode);
        if (joinPack._resultCode == 0)
        {
            LoginUIManager._Instance.OpenNoticeWindow("가입성공했습니다.");
            LoginUIManager._Instance.CloseSignUpPanel();
        }
        else if (joinPack._resultCode == 1)
        {
            LoginUIManager._Instance.OpenNoticeWindow("ID 중복!");
        }
        else if (joinPack._resultCode == 2)
        {
            LoginUIManager._Instance.OpenNoticeWindow("닉네임 중복!");
        }
    }

    void ReceiveLoginPack(PacketClass pack)
    {
        DefinePacket.Receive_LoginResult loginPack = new DefinePacket.Receive_LoginResult();
        loginPack = (DefinePacket.Receive_LoginResult)ConvertPacket.ReceiveToStructure(pack._data, loginPack.GetType(), pack._totalSize);
        if(loginPack._resultCode == 0)
        {
            _uuid = loginPack._uuid;
            NetworkGameDataManager._Instance.SetUserinfo(loginPack._gold, loginPack._stage, loginPack._baseHP, loginPack._baseAtk, loginPack._missileAtkUpgrade,
                                                         loginPack._baseMaxSpeed, loginPack._baseAccSpeed, loginPack._baseRotateSpeed,
                                                         loginPack._hpUpgrade, loginPack._atkUpgrade, loginPack._missileAtkUpgrade, loginPack._speedUpgrade,
                                                         loginPack._accSpeedUpgrade, loginPack._rotateSpeedUpgrade);

            SceneControlManager._Instacne.ConvertScene("LobbyScene");

        }
        else if(loginPack._resultCode == 1)
        {
            LoginUIManager._Instance.OpenNoticeWindow("로그인 정보 틀림!");
        }
        else
        {
            LoginUIManager._Instance.OpenNoticeWindow("-기타- 이유로 실패");
        }
    }

    void ReceiveIncreaseUpgradeResult(PacketClass pack)
    {
        DefinePacket.Receive_IncreaseUpgradeResult resultPack = new DefinePacket.Receive_IncreaseUpgradeResult();
        resultPack = (DefinePacket.Receive_IncreaseUpgradeResult)ConvertPacket.ReceiveToStructure(pack._data, resultPack.GetType(), pack._totalSize);

        if (resultPack._success)
        {
            NetworkGameDataManager._Instance.IncreaseUpgrade((PublicDefine.eUpgradeType)resultPack._upgradeType, resultPack._spendedCredit);
        }
        else
        {
            UpgradeUIManager._Instance.OpenNoticeWindow("네트워크 오류! 업그레이드 실패");
        }

    }

    void ReceiveGameResultHandelResult(PacketClass pack)
    {
        DefinePacket.Receive_GameResultHandle handlePack = new DefinePacket.Receive_GameResultHandle();
        handlePack = (DefinePacket.Receive_GameResultHandle)ConvertPacket.ReceiveToStructure(pack._data, handlePack.GetType(), pack._totalSize);
        if (handlePack._success)
        {
            NetworkGameDataManager._Instance.SetClearStageData(handlePack._stage);
            NetworkGameDataManager._Instance.UpdateCreditValue(handlePack._credit);

            Debug.Log("TCPClient ReceiveGameResultHandelResult " + handlePack._success);
            InGameUIManager._Instance.GotoLobbyScene();
        }
        else
        {
            Debug.Log("게임결과처리 오류");
            //InGameUIManager._Instance.GotoLobbyScene();
            InGameUIManager._Instance.SetGameResultHandleFailWindow();
        }
    }

    public void GetJoinInfo(string id, string pw, string nickName)
    {
        _id = id;
        _pw = pw;
        _nickName = nickName;
    }

    public long GetCurrentUUID()
    {
        return _uuid;
    }
}
