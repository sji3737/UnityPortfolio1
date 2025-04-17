using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoginWindow : MonoBehaviour
{
    InputField _idInputField;
    InputField _pwInputField;

    private void Awake()
    {
        _idInputField = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<InputField>();
        _pwInputField = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<InputField>();
    }

    public void ClickLoginButton()
    {
        if(_idInputField.text == string.Empty)
        {
            LoginUIManager._Instance.OpenNoticeWindow("ID를 입력하세요");
        }
        else if(_pwInputField.text == string.Empty)
        {
            LoginUIManager._Instance.OpenNoticeWindow("PW를 입력하세요");
        }
        else
        {
            TCPClient._Instace.SendLoginPacket(_idInputField.text, _pwInputField.text);
        }
    }

    public void ClickCancelButton()
    {
        _idInputField.text = string.Empty;
        _pwInputField.text = string.Empty;
        gameObject.SetActive(false);
    }
}
