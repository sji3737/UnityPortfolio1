using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    [SerializeField] GameObject _prefabNoticeWnd;
    [SerializeField] GameObject _prefabSignUpWnd;
    [SerializeField] GameObject _prefabLoginWnd;
    [SerializeField] GameObject _connectButton;
    [SerializeField] GameObject _signUpButton;
    [SerializeField] GameObject _loginButton;

    UI_NoticeWindow _noticeWindow;
    UI_SignUpWindow _signUpWindow;
    UI_LoginWindow _loginWindow;


    static LoginUIManager _uniqueInstance;

    public static LoginUIManager _Instance
    {
        get
        {
            return _uniqueInstance;
        }      
    }

    private void Awake()
    {
        _uniqueInstance = this;
        _signUpButton.SetActive(false);
        _loginButton.SetActive(false);
    }

    public void ConvertToLobbyScene()
    {
        SceneControlManager._Instacne.ConvertScene("LobbyScene");
    }

    public void Connect()
    {
        if (TCPClient._Instace.ConnectServer())
        {
            _connectButton.SetActive(false);
            _signUpButton.SetActive(true);
            _loginButton.SetActive(true);
        }
    }

    public void OpenSignUpPanel()
    {
        if (_signUpWindow == null)
        {
            _signUpWindow = Instantiate(_prefabSignUpWnd).GetComponent<UI_SignUpWindow>();
        }
        _signUpWindow.gameObject.SetActive(true);
    }

    public void CloseSignUpPanel()
    {
        _signUpWindow.ClickCancleButton();
    }

    public void OpenLoginPanel()
    {
        if(_loginWindow == null)
        {
            _loginWindow = Instantiate(_prefabLoginWnd).GetComponent<UI_LoginWindow>();
        }
        _loginWindow.gameObject.SetActive(true);
    }

    public void OpenLoginFailPanel()
    {

    }

    public void OpenNoticeWindow(string content)
    {
        if(_noticeWindow == null)
        {
            _noticeWindow = Instantiate(_prefabNoticeWnd).GetComponent<UI_NoticeWindow>();
        }
        _noticeWindow.gameObject.SetActive(true);
        _noticeWindow.SetContentText(content);
    }

}
