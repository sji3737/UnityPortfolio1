using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageBar : MonoBehaviour
{
    [SerializeField] Text _stageNumText;
    [SerializeField] GameObject _lockImg;
    UI_StageInfoPanel _stageInfoPanel;


    int _stageNum;



    public void SetStageNum(int area, int stage, UI_StageInfoPanel panel)
    {
        if(_stageInfoPanel == null)
        {
            _stageInfoPanel = panel;
        }
        string stageText = string.Format("{0}-{1}", area, stage);
        _stageNumText.text = stageText;
        _lockImg.SetActive(true);
        _stageNum = area * 10 + stage;
    }

    public void StageUnlock()
    {
        _lockImg.SetActive(false);
    }

    public void ClickAccessButton()
    {
        _stageInfoPanel.ShowPanel(_stageNum);
    }
}
