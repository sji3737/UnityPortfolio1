using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUIManager : MonoBehaviour
{
    [SerializeField] Text _areaNumText;
    [SerializeField] Text _areaTitleText;
    [SerializeField] UI_StageInfoPanel _stageInfoPanel;
    [SerializeField] UI_StageBar[] _stageBars;

    int _currentArea = 1;
    int _clearStage;

    private void Start()
    {
        _areaNumText.text = _currentArea.ToString();
        // _areaTitleText.text = _areaTitle[_currentArea - 1];
        _areaTitleText.text = TableDataManager._Instance.Get(LowDataType.AreaInfo).ToS(_currentArea, "AreaTitle");
        //_clearStage = GameDataManager._Instacne.GetClearStageData();
        _clearStage = NetworkGameDataManager._Instance.GetClearStageData();
        if(_clearStage == 0)
        {
            _clearStage = 14;
        }
        int area = _clearStage / 10;
        int stage = _clearStage % 10;
        for(int i = 0; i < _stageBars.Length; i++)
        {
            _stageBars[i].SetStageNum(_currentArea, i + 1, _stageInfoPanel);
        }
        if(area >= _currentArea)
        {
            for(int i = 0; i < stage; i++)
            {
                _stageBars[i].StageUnlock();
            }
        }
    }

    public void SelectPrevArea()
    {
        if (_currentArea <= 1) return;
        _currentArea--;
        _areaNumText.text = _currentArea.ToString();
        //_areaTitleText.text = _areaTitle[_currentArea - 1];
        _areaTitleText.text = TableDataManager._Instance.Get(LowDataType.AreaInfo).ToS(_currentArea, "AreaTitle");
        int area = _clearStage / 10;
        int stage = _clearStage % 10;
        for (int i = 0; i < _stageBars.Length; i++)
        {
            _stageBars[i].SetStageNum(_currentArea, i + 1, _stageInfoPanel);
        }
        if (area >= _currentArea)
        {
            for (int i = 0; i < stage; i++)
            {
                _stageBars[i].StageUnlock();
            }
        }
    }

    public void SelectNextArea()
    {
        if (_currentArea >= 4) return;
        _currentArea++;
        _areaNumText.text = _currentArea.ToString();
        //_areaTitleText.text = _areaTitle[_currentArea - 1];
        _areaTitleText.text = TableDataManager._Instance.Get(LowDataType.AreaInfo).ToS(_currentArea, "AreaTitle");
        int area = _clearStage / 10;
        int stage = _clearStage % 10;
        for (int i = 0; i < _stageBars.Length; i++)
        {
            _stageBars[i].SetStageNum(_currentArea, i + 1, _stageInfoPanel);
        }
        if (area >= _currentArea)
        {
            for (int i = 0; i < stage; i++)
            {
                _stageBars[i].StageUnlock();
            }
        }
    }

    public void ExitStageSelectScene()
    {
        SceneControlManager._Instacne.ConvertScene("LobbyScene");
    }

}
