using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageInfoPanel : MonoBehaviour
{
    [SerializeField] Text _storyText;
    [SerializeField] Transform _itemScrollViewContent;
    [SerializeField] GameObject _prefabItemIcon;
    RectTransform _rectTrasform;
    List<UI_ItemIcon> _listDropItem = new List<UI_ItemIcon>();
    int _stageNum;

    private void Awake()
    {
        _rectTrasform = GetComponent<RectTransform>();
    }

    public void ShowPanel(int stageNum)
    {
        if(_listDropItem.Count > 0)
        {
            foreach(UI_ItemIcon item in _listDropItem)
            {
                //나중에 Destory 말고 재활용
                Destroy(item.gameObject);
            }
        }
        _listDropItem.Clear();
        _stageNum = stageNum;
        _storyText.text = TableDataManager._Instance.Get(LowDataType.StageInfo).ToS(_stageNum, "StageStory");
        Vector2 startPos = new Vector2(0, -1000);
        Vector2 endPos = new Vector2(0, 0);
        iTween.ValueTo(gameObject, iTween.Hash("from", startPos, "to", endPos, "onupdate", "UpdatePanelPosTween", "onupdatetarget", gameObject, "time", 0.5f,
            "easetype", iTween.EaseType.easeOutCubic));
        string itemList = TableDataManager._Instance.Get(LowDataType.StageInfo).ToS(_stageNum, "DropItem");
        string[] itemWord = itemList.Split(',');
        for(int i = 0; i < itemWord.Length; i++)
        {
            int idx = int.Parse(itemWord[i]);
            UI_ItemIcon icon = Instantiate(_prefabItemIcon, _itemScrollViewContent).GetComponent<UI_ItemIcon>();
            icon.SetItemIdx(idx);
            _listDropItem.Add(icon);
        }

    }

    void UpdatePanelPosTween(Vector2 pos)
    {
        _rectTrasform.anchoredPosition = pos;
    }

    public void ExitPanel()
    {
        Vector2 hidePos = new Vector2(0, -10000);
        _rectTrasform.anchoredPosition = hidePos;
    }

    public void GameStart()
    {
        //GameDataManager._Instacne._SelectStage = _stageNum;
        NetworkGameDataManager._Instance._SelectStage = _stageNum;
        SceneControlManager._Instacne.ConvertScene("InGameScene");
    }
}
