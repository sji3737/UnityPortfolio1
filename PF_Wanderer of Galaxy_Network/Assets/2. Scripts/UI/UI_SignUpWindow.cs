using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SignUpWindow : MonoBehaviour
{
    InputField _idInputField;
    InputField _pwInputField;
    InputField _pwVInputField;
    InputField _nickNameInputField;

    //static UI_SignUpWindow _uniqueInstance;

    private void Awake()
    {
        _idInputField = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<InputField>();
        _pwInputField = transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<InputField>();
        _pwVInputField = transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<InputField>();
        _nickNameInputField = transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<InputField>();
    }

    public void ClickSignUpButton()
    {
        string id = _idInputField.text;
        string pw = _pwInputField.text;
        string pwv = _pwVInputField.text;
        string name = _nickNameInputField.text;

        if(pw != pwv)
        {
            LoginUIManager._Instance.OpenNoticeWindow("비밀번호가 일치하지 않습니다");
        }
        else
        {
            TCPClient._Instace.SendJoinPacket(id, pw, name);
        }
    }

    public void ClickCancleButton()
    {
        _idInputField.text = string.Empty;
        _pwInputField.text = string.Empty;
        _pwVInputField.text = string.Empty;
        _nickNameInputField.text = string.Empty;
        gameObject.SetActive(false);
    }
}
