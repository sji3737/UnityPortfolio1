using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoG_Server
{
    class DefineProtocol
    {
        public enum eServer
        {
            Send_Connect = 0,
            Recv_Connect,
            Send_Join_Result,
            Recv_Join,
            Send_Login_Result,
            Recv_Login,
            Send_UserInfo,
            Recv_UserInfo_Request,
            Send_ChangeGold_Result,
            Recv_ChangeGold,
            Send_ChangeCash_Result,
            Recv_ChangeCash,
            Send_ChangeStageNum_Result,
            Recv_ChangeStageNum,
            Send_IncreaseUpgraed_Result,
            Recv_IncreaseUpgrade,
            Send_GameResultHandel_Result,
            Recv_GameResultHandle_Request
        }

        public enum eJoinResult
        {
            Success = 0,
            ID_Fail,
            ETC_Fail
        }

        public enum eLoginResult
        {
            Success = 0,
            ID_Fail,
            ETC_Fail
        }
    }
}
