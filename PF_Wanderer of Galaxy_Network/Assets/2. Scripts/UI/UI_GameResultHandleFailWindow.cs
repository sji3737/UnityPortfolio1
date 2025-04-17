using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameResultHandleFailWindow : UI_TwoButtonBaseWindow
{
    public override void ClickOkButton()
    {
        InGameManager._Instance.SendGameResultPack();
        base.ClickOkButton();
    }

    public override void ClickCancleButton()
    {
        SceneControlManager._Instacne.ConvertScene("LobbyScene");
    }
}
