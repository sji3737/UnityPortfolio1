using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineProtocol
{ 
    public enum eClient
    {
        Recv_Connect = 0,
        Send_Connect,
        Recv_Join_Result,
        Send_Join,
        Recv_Login_Result,
        Send_Login,
        Recv_UserInfo,
        Send_UserInfo_Request,
        Recv_ChangeGold_Result,
        Send_ChangeGold,
        Recv_ChangeCash_Result,
        Send_ChangeCash,
        Recv_ChangeStageNum_Result,
        Send_ChangeStageNum,
        Recv_IncreaseUpgraed_Result,
        Send_IncreaseUpgrade_Request,
        Recv_GameResultHandel_Result,
        Send_GameResultHandle_Request
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
