using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] UI_PlayerInfoPanel _playerInfoPanel;
    [SerializeField] Image _shadowBG;
    [SerializeField] Text _creditText;

    private void Start()
    {
        //int credit = GameDataManager._Instacne.GetCredit();
        int credit = NetworkGameDataManager._Instance.GetCredit();
        if(credit == 0)
        {
            _creditText.text = "0";
        }
        else
        {
            _creditText.text = string.Format("{0:#,###}", credit);
        }
        
    }

    public void ConvertToStageSelectScene()
    {
        SceneControlManager._Instacne.ConvertScene("StageSelectScene");
    }

    public void ConvertToResearchScene()
    {
        SceneControlManager._Instacne.ConvertScene("ResearchScene");
    }

    public void OpenInfoPanel()
    {
        _playerInfoPanel.Init(_shadowBG);
    }
}
