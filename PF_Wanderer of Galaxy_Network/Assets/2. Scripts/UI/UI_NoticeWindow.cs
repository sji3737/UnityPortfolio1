using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NoticeWindow : MonoBehaviour
{
    [SerializeField] Text _contentText;

    public void SetContentText(string content)
    {
        _contentText.text = content;
    }

    public void ClickExitBtn()
    {
        gameObject.SetActive(false);
    }
}
