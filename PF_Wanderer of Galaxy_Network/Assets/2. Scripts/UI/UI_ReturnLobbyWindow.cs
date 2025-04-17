using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ReturnLobbyWindow : UI_TwoButtonBaseWindow
{
    public override void ClickOkButton()
    {
        Time.timeScale = 1f;
        InGameManager._Instance.SetEscapeMode();
        SpawnManager._Instance.SetEscapeMode();
        InGameUIManager._Instance.SetEscapeToLobby();
        base.ClickOkButton();
    }

    public override void ClickCancleButton()
    {
        Time.timeScale = 1f;
        base.ClickCancleButton();
    }
}
