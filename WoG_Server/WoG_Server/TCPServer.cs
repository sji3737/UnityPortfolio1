using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace WoG_Server
{
    class TCPServer
    {
        const int PACK_DATA_SIZE = 1008;
        const int JOIN_SUCCESS = 0;
        const int JOIN_FAIL_IDOVERLAP = 1;
        const int JOIN_FAIL_ETC = 2;
        const int LOGIN_SUCCESS = 0;
        const int LOGIN_FAIL_PWMISMATCH = 1;
        const int LOGIN_FAIL_ETC = 2;

        short _port;
        int _nowSocketID = 1000;
        long _nowClientID = 100000000;
        long _nowUUID = 1;

        SocketClass _waitClient;

        SocketManager _socketManager = new SocketManager();
        DBManager _dBManager = new DBManager();

        Queue<PacketClass> _sendQueue = new Queue<PacketClass>();
        Queue<PacketClass> _receiveQueue = new Queue<PacketClass>();

        Thread _sendThread;
        Thread _receiveThread;

        public TCPServer(short port)
        {
            _socketManager.Init(this);
            _port = port;

            Socket _waitSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _waitClient = new SocketClass(_waitSocket, 0, 0);
            //소켓에 주소와 포트 바인드
            _waitClient.Bind(new IPEndPoint(IPAddress.Any, _port));
            //대기 상태로 변경
            _waitClient.Listen(30);
            _sendThread = new Thread(SendProcess);
            _receiveThread = new Thread(() => RecevieProcess());

            _sendThread.Start();
            _receiveThread.Start();

            _dBManager.ConnectDB();
            _dBManager.SetNextUUID();

            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("[{0}] Server Start!", date);
        }

        ~TCPServer()
        {

        }
        
        public bool MainProcess()
        {
            //대기소켓에 클라이언트가 붙는가
            if(_waitClient.Poll(0, SelectMode.SelectRead))
            {
                Socket s = _waitClient.Accept();
                SocketClass newClient = new SocketClass(s, _nowSocketID, _nowClientID);
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Console.WriteLine("[{0}] 새로운 클라이언트가 접속했습니다. 할당 SocketID: {1}", date, _nowSocketID);
                _socketManager.AddSocket(newClient);
                SendConnectSuccessPacket(_nowClientID);
                _nowSocketID++;
                _nowClientID++;
            }

            _socketManager.ReceivePacket();

            return true;
        }

        public void EnqueueReceivePacket(PacketClass pack)
        {
            _receiveQueue.Enqueue(pack);
        }

        public void SendConnectSuccessPacket(long clientID)
        {
            DefinePacket.Send_Connect connPack = new DefinePacket.Send_Connect();
            connPack._isSuccess = true;
            connPack._clientID = clientID;
            byte[] buffer = ConvertPacket.SendToByteArray(connPack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_Connect, clientID, Marshal.SizeOf(connPack), bytePack);          
            _sendQueue.Enqueue(pack);

        }

        public void SendJoinResultPacket(int resultCode, long clientID)
        {
            DefinePacket.Send_JoinResult joinResultPack = new DefinePacket.Send_JoinResult();
            joinResultPack._resultCode = resultCode;
            byte[] buffer = ConvertPacket.SendToByteArray(joinResultPack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_Join_Result, clientID, Marshal.SizeOf(joinResultPack), bytePack);
            _sendQueue.Enqueue(pack);
        }

        public void SendLoginResultPack(int resultCode, long uuid, long clientID)
        {
            DefinePacket.Send_LoginResult loginPack = new DefinePacket.Send_LoginResult();
            loginPack._resultCode = resultCode;
            if (resultCode == LOGIN_SUCCESS)
            {
                loginPack._uuid = uuid;
                loginPack._gold = _dBManager.GetGoldValue(uuid);
                loginPack._cash = _dBManager.GetCashValue(uuid);
                loginPack._stage = _dBManager.GetClearStageInfo(uuid);

                loginPack._baseHP = _dBManager.GetBaseHPInfo(uuid);
                loginPack._baseAtk = _dBManager.GetBaseAtkInfo(uuid);
                loginPack._baseMissileAtk = _dBManager.GetBaseMissileAtkInfo(uuid);
                loginPack._baseMaxSpeed = _dBManager.GetBaseMaxSpeedInfo(uuid);
                loginPack._baseAccSpeed = _dBManager.GetBaseAccSpeedInfo(uuid);
                loginPack._baseRotateSpeed = _dBManager.GetBaseRotateSpeedInfo(uuid);

                loginPack._hpUpgrade = _dBManager.GetHpUpgradeInfo(uuid);
                loginPack._atkUpgrade = _dBManager.GetAtkUpgradeInfo(uuid);
                loginPack._missileAtkUpgrade = _dBManager.GetMissileAtkUpgradeInfo(uuid);
                loginPack._speedUpgrade = _dBManager.GetSpeedUpgradeInfo(uuid);
                loginPack._accSpeedUpgrade = _dBManager.GetAccSpeedUpgradeInfo(uuid);
                loginPack._rotateSpeedUpgrade = _dBManager.GetRotateSpeedUpgradeInfo(uuid);
                loginPack.ModData(_dBManager.GetNickName(uuid));
            }

            byte[] buffer = ConvertPacket.SendToByteArray(loginPack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_Login_Result, clientID, Marshal.SizeOf(loginPack), bytePack);
            _sendQueue.Enqueue(pack);
        }


        public void SendGoldValuePacket(int gold, long clientID)
        {
            DefinePacket.Send_GoldValue goldPack = new DefinePacket.Send_GoldValue();
            goldPack._gold = gold;
            byte[] buffer = ConvertPacket.SendToByteArray(goldPack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_ChangeGold_Result, clientID, Marshal.SizeOf(goldPack), bytePack);
            _sendQueue.Enqueue(pack);
        }

        public void SendCashValuePacket(int cash, long clientID)
        {
            DefinePacket.Send_CashValue cashPack = new DefinePacket.Send_CashValue();
            cashPack._cash = cash;
            byte[] buffer = ConvertPacket.SendToByteArray(cashPack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_ChangeCash_Result, clientID, Marshal.SizeOf(cashPack), bytePack);
            _sendQueue.Enqueue(pack);
        }

        public void SendStageClearPacket(int stage, long clientID)
        {
            DefinePacket.Send_CurrentClearStage stagePack = new DefinePacket.Send_CurrentClearStage();
            stagePack._stage = stage;
            byte[] buffer = ConvertPacket.SendToByteArray(stagePack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_ChangeStageNum_Result, clientID, Marshal.SizeOf(stagePack), bytePack);
            _sendQueue.Enqueue(pack);
        }

        public void SendIncreaseUpgradeResult(bool isSuccess, int upgradeType, int spendedCredit, long clientID)
        {
            DefinePacket.Send_IncreaseUpgradeResult upgradeResultPack = new DefinePacket.Send_IncreaseUpgradeResult();
            upgradeResultPack._success = isSuccess;
            upgradeResultPack._upgradeType = upgradeType;
            upgradeResultPack._spendedCredit = spendedCredit;
            byte[] buffer = ConvertPacket.SendToByteArray(upgradeResultPack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_IncreaseUpgraed_Result, clientID, Marshal.SizeOf(upgradeResultPack), bytePack);
            _sendQueue.Enqueue(pack);
        }

        public void SendGameResultHandleRequestResultPack(bool isSuccess, int credit, int stage, long clientID)
        {
            DefinePacket.Send_GameResultHandle gameResultHandlePack = new DefinePacket.Send_GameResultHandle();
            gameResultHandlePack._success = isSuccess;
            gameResultHandlePack._credit = credit;
            gameResultHandlePack._stage = stage;
            byte[] buffer = ConvertPacket.SendToByteArray(gameResultHandlePack);
            byte[] bytePack = new byte[PACK_DATA_SIZE];
            Array.Copy(buffer, 0, bytePack, 0, buffer.Length);
            PacketClass pack = new PacketClass();
            pack.modData((int)DefineProtocol.eServer.Send_GameResultHandel_Result, clientID, Marshal.SizeOf(gameResultHandlePack), bytePack);
            _sendQueue.Enqueue(pack);
        }

        void ReceiveJoinPacket(PacketClass pack)
        {
            DefinePacket.Receive_Join joinPack = new DefinePacket.Receive_Join();
            joinPack = (DefinePacket.Receive_Join)ConvertPacket.ReciveToStructure(pack._data, joinPack.GetType(), pack._totalSize);
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string id = Encoding.UTF8.GetString(joinPack._id);
            string pw = Encoding.UTF8.GetString(joinPack._pw);
            string nickName = Encoding.UTF8.GetString(joinPack._nickName);

            //이 밑으로 DB처리
            if (_dBManager.CheckIDOverlap(id))
            {
                SendJoinResultPacket(JOIN_FAIL_IDOVERLAP, pack._clientID);
                return;
            }
            long uuid = _dBManager.GetNextUUID();
            _dBManager.IncreaseUUID();
            UserInfo info = new UserInfo(uuid, id, pw, nickName);
            if (_dBManager.InsertUserInfo(info))
            {
                SendJoinResultPacket(JOIN_SUCCESS, pack._clientID);
            }
            else
            {
                SendJoinResultPacket(JOIN_FAIL_ETC, pack._clientID);
            }
        }

        void ReceiveLoginPacket(PacketClass pack)
        {
            DefinePacket.Receive_Login loginPack = new DefinePacket.Receive_Login();
            loginPack = (DefinePacket.Receive_Login)ConvertPacket.ReciveToStructure(pack._data, loginPack.GetType(), pack._totalSize);
            string id = Encoding.UTF8.GetString(loginPack._id);
            string pw = Encoding.UTF8.GetString(loginPack._pw);

            long uuid = _dBManager.GetUUID(id);

            SendLoginResultPack(_dBManager.CheckMatchLoginInfo(id, pw), uuid, pack._clientID);
            //SendLoginTestPacket(pack._clientID);
        }

        void ReceiveGoldChangePacket(PacketClass pack)
        {
            DefinePacket.Receive_GoldChangeValue goldPack = new DefinePacket.Receive_GoldChangeValue();
            goldPack = (DefinePacket.Receive_GoldChangeValue)ConvertPacket.ReciveToStructure(pack._data, goldPack.GetType(), pack._totalSize);

            _dBManager.UpdateGoldValue(goldPack._uuid, goldPack._goldChange);
            int resultVal = _dBManager.GetGoldValue(goldPack._uuid);

            SendGoldValuePacket(resultVal, pack._clientID);
        }

        void ReceiveCashChangePacket(PacketClass pack)
        {
            DefinePacket.Receive_CashChangeValue cashPack = new DefinePacket.Receive_CashChangeValue();
            cashPack = (DefinePacket.Receive_CashChangeValue)ConvertPacket.ReciveToStructure(pack._data, cashPack.GetType(), pack._totalSize);

            _dBManager.UpdateCashValue(cashPack._uuid, cashPack._cashChange);
            int resultVal = _dBManager.GetCashValue(cashPack._uuid);

            SendCashValuePacket(resultVal, pack._clientID);
        }

        void ReceiveStageChangePacket(PacketClass pack)
        {
            DefinePacket.Receive_StageClear stagePack = new DefinePacket.Receive_StageClear();
            stagePack = (DefinePacket.Receive_StageClear)ConvertPacket.ReciveToStructure(pack._data, stagePack.GetType(), pack._totalSize);

            _dBManager.UpdateClearStage(stagePack._uuid);
            int resultVal = _dBManager.GetClearStageInfo(stagePack._uuid);

            SendStageClearPacket(resultVal, pack._clientID);
        }

        void ReceiveIncreaseUpgradeRequestPacket(PacketClass pack)
        {
            Console.WriteLine("ReceiveIncreaseUpgradeRequestPacket");

            DefinePacket.Receive_IncreaseUpgradeRequest requestPack = new DefinePacket.Receive_IncreaseUpgradeRequest();
            requestPack = (DefinePacket.Receive_IncreaseUpgradeRequest)ConvertPacket.ReciveToStructure(pack._data, requestPack.GetType(), pack._totalSize);

            bool success = false;

            if(_dBManager.UpdateGoldValue(requestPack._uuid, -requestPack._credit))
            {
                success = _dBManager.IncreaseUpgrade(requestPack._uuid, (eUpgradeType)requestPack._upgradeType, requestPack._credit);
            }
  
            SendIncreaseUpgradeResult(success, requestPack._upgradeType, requestPack._credit, pack._clientID);
 
        }

        void ReceiveGameResultHandleRequestPack(PacketClass pack)
        {
            DefinePacket.Receive_GameResult gameResultPack = new DefinePacket.Receive_GameResult();
            gameResultPack = (DefinePacket.Receive_GameResult)ConvertPacket.ReciveToStructure(pack._data, gameResultPack.GetType(), pack._totalSize);

            bool success = _dBManager.GameResultHandle(gameResultPack._uuid, gameResultPack._credit, gameResultPack._stage);
            int credit = _dBManager.GetGoldValue(gameResultPack._uuid);
            int stage = _dBManager.GetClearStageInfo(gameResultPack._uuid);

            SendGameResultHandleRequestResultPack(success, credit, stage, pack._clientID);
        }

        #region [SendQ]
        void SendProcess()
        {
            while (true)
            {
                if(_sendQueue.Count > 0)
                {
                    PacketClass pack = _sendQueue.Dequeue();
                    SocketClass clientSocket = _socketManager.FindSocketWithClientID(pack._clientID);
                    clientSocket.SendPacket(pack);
                }
            }
        }
        #endregion

        #region [ReceiveQ]
        void RecevieProcess()
        {
            while (true)
            {             
                if (_receiveQueue.Count > 0)
                {
                    PacketClass pack;
                    pack = _receiveQueue.Dequeue();
                    Console.WriteLine("Packet protocol number: {0}", pack._protocolID);
                    switch (pack._protocolID)
                    {
                        case (int)DefineProtocol.eServer.Recv_Connect:
                            break;
                        case (int)DefineProtocol.eServer.Recv_Join:
                            ReceiveJoinPacket(pack);
                            break;
                        case (int)DefineProtocol.eServer.Recv_Login:
                            ReceiveLoginPacket(pack);
                            break;
                        case (int)DefineProtocol.eServer.Recv_ChangeGold:
                            ReceiveGoldChangePacket(pack);
                            break;
                        case (int)DefineProtocol.eServer.Recv_ChangeStageNum:
                            ReceiveStageChangePacket(pack);
                            break;
                        case (int)DefineProtocol.eServer.Recv_IncreaseUpgrade:
                            ReceiveIncreaseUpgradeRequestPacket(pack);
                            break;
                        case (int)DefineProtocol.eServer.Recv_GameResultHandle_Request:
                            ReceiveGameResultHandleRequestPack(pack);
                            break;
                    }
                }
                

                
            }
        }

        #endregion
    }
}
