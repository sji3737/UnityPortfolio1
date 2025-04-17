using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    [SerializeField] GameObject _prefabUpgradePanel;
    [SerializeField] GameObject _prefabNoticeWnd;
    [SerializeField] Transform _scollContent;
    [SerializeField] Text _currentUpgradeLv;
    [SerializeField] Text _currentUpgradeDetail;
    [SerializeField] Text _nextUpgradeLv;
    [SerializeField] Text _nextUpgradeDetail;
    [SerializeField] Text _requireCreditText;
    [SerializeField] Text _currentCreditText;

    int _selectUpgradeIndex;
    int _selectUpgradeIdxLv;

    UI_NoticeWindow _noticeWindow;

    static UpgradeUIManager _uniqueInstacne;

    public static UpgradeUIManager _Instance
    {
        get { return _uniqueInstacne; }
    }

    private void Awake()
    {
        if(_uniqueInstacne == null)
        {
            _uniqueInstacne = this;
            int maxCnt = TableDataManager._Instance.Get(LowDataType.UpgradeList).MaxCount();
            for (int i = 0; i < maxCnt; i++)
            {
                GameObject obj = Instantiate(_prefabUpgradePanel, _scollContent);
                UI_UpgradeIdxPanel panel = obj.GetComponent<UI_UpgradeIdxPanel>();
                panel.Init(i + 1);
                //int credit = GameDataManager._Instacne.GetCredit();
                int credit = NetworkGameDataManager._Instance.GetCredit();
                if (credit != 0)
                {
                    _currentCreditText.text = string.Format("{0:#,###}", credit);
                }
                else
                {
                    _currentCreditText.text = "0";
                }
            }
        }       
    }

    public void SelectUpgradeItem(int idx)
    {
        _selectUpgradeIndex = idx;
        int currentLv = 0;
        switch (idx)
        {
            case (int)UpgradeIncrementTable.eIndex.HP:
                //currentLv = GameDataManager._Instacne.GetHpUpgrade();
                currentLv = NetworkGameDataManager._Instance.GetHpUpgrade();
                break;
            case (int)UpgradeIncrementTable.eIndex.BasicWeaponDamage:
                //currentLv = GameDataManager._Instacne.GetAtkUpgrade();
                currentLv = NetworkGameDataManager._Instance.GetAtkUpgrade();
                break;
            case (int)UpgradeIncrementTable.eIndex.MissileWeaponDamage:
                //currentLv = GameDataManager._Instacne.GetMissileAtkUpgrade();
                currentLv = NetworkGameDataManager._Instance.GetMissileAtkUpgrade();
                break;
            case (int)UpgradeIncrementTable.eIndex.MaxSpeed:
                //currentLv = GameDataManager._Instacne.GetMaxSpeedUpgrade();
                currentLv = NetworkGameDataManager._Instance.GetMaxSpeedUpgrade();
                break;
            case (int)UpgradeIncrementTable.eIndex.Acceleration:
                //currentLv = GameDataManager._Instacne.GetAccSpeedUpgrade();
                currentLv = NetworkGameDataManager._Instance.GetAccSpeedUpgrade();
                break;
            case (int)UpgradeIncrementTable.eIndex.Rotate:
                //currentLv = GameDataManager._Instacne.GetRotateSpeedUpgrade();
                currentLv = NetworkGameDataManager._Instance.GetRotateSpeedUpgrade();
                break;

        }
        _selectUpgradeIdxLv = currentLv;
        //int currentLv = 1;
        _currentUpgradeLv.text = string.Format("Lv. {0}", currentLv);
        string detail = TableDataManager._Instance.Get(LowDataType.UpgradeList).ToS(idx, "Detail");
        UpgradeIncrementTable.eIndex col = (UpgradeIncrementTable.eIndex)idx;
        float inc = 0;
        if (currentLv != 0)
        {
            inc = TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(currentLv, col.ToString()) * 100;
        }       
        detail = string.Format(detail, inc);
        _currentUpgradeDetail.text = detail;

        if(currentLv < 10)
        {
            int credit = TableDataManager._Instance.Get(LowDataType.UpgradeCredit).ToI(currentLv + 1, col.ToString());
            _requireCreditText.text = string.Format("{0:#,###.###}", credit);
            //if(credit > GameDataManager._Instacne.GetCredit())
            if (credit > NetworkGameDataManager._Instance.GetCredit())
            {
                _requireCreditText.color = Color.red;
            }
            else
            {
                _requireCreditText.color = Color.black;
            }
        }
        else
        {
            _requireCreditText.text = string.Empty;
        }
        

        if(currentLv < 10)
        {
            _nextUpgradeLv.text = string.Format("Lv. {0}", currentLv + 1);
            inc = TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF((currentLv + 1), col.ToString()) * 100;
            detail = TableDataManager._Instance.Get(LowDataType.UpgradeList).ToS(idx, "Detail");
            detail = string.Format(detail, inc);
            _nextUpgradeDetail.text = detail;
        }
        else
        {
            _currentUpgradeLv.text = "Lv. Max";
            _nextUpgradeLv.text = string.Empty;
            _nextUpgradeDetail.text = string.Empty;
            _requireCreditText.text = string.Empty;
        }
    }

    public void Upgrade()
    {
        UpgradeIncrementTable.eIndex idx = (UpgradeIncrementTable.eIndex)_selectUpgradeIndex;
        int requireCredit = TableDataManager._Instance.Get(LowDataType.UpgradeCredit).ToI(_selectUpgradeIdxLv + 1, idx.ToString());
        //if (requireCredit > GameDataManager._Instacne.GetCredit()) return;
        if (requireCredit > NetworkGameDataManager._Instance.GetCredit()) return;
        if (_selectUpgradeIdxLv < 10)
        {
            switch (_selectUpgradeIndex)
            {
                case (int)UpgradeIncrementTable.eIndex.HP:
                    //GameDataManager._Instacne.IncreaseHpUpgrade();
                    //NetworkGameDataManager._Instance.RequestIncreaseUpgrade(PublicDefine.eUpgradeType.Hp, requireCredit);
                    TCPClient._Instace.SendIncreaseUpgradeRequest(PublicDefine.eUpgradeType.Hp, requireCredit);
                    break;
                case (int)UpgradeIncrementTable.eIndex.BasicWeaponDamage:
                    //GameDataManager._Instacne.IncreaseAtkUpgrade();
                    NetworkGameDataManager._Instance.RequestIncreaseUpgrade(PublicDefine.eUpgradeType.Atk, requireCredit);
                    break;
                case (int)UpgradeIncrementTable.eIndex.MissileWeaponDamage:
                    //GameDataManager._Instacne.IncreaseMissileAtkUpgrade();
                    NetworkGameDataManager._Instance.RequestIncreaseUpgrade(PublicDefine.eUpgradeType.MissileAtk, requireCredit);
                    break;
                case (int)UpgradeIncrementTable.eIndex.MaxSpeed:
                    //GameDataManager._Instacne.IncreaseMaxSpeedUpgrade();
                    NetworkGameDataManager._Instance.RequestIncreaseUpgrade(PublicDefine.eUpgradeType.Speed, requireCredit);
                    break;
                case (int)UpgradeIncrementTable.eIndex.Acceleration:
                    //GameDataManager._Instacne.IncreaseAccSpeedUpgrade();
                    NetworkGameDataManager._Instance.RequestIncreaseUpgrade(PublicDefine.eUpgradeType.AccSpeed, requireCredit);
                    break;
                case (int)UpgradeIncrementTable.eIndex.Rotate:
                    //GameDataManager._Instacne.IncreaseRotateSpeedUpgrade();
                    NetworkGameDataManager._Instance.RequestIncreaseUpgrade(PublicDefine.eUpgradeType.RotateSpeed, requireCredit);
                    break;
            }
            //GameDataManager._Instacne.SubstractCredit(requireCredit);
            //Debug.Log(GameDataManager._Instacne.GetCredit());
            //_currentCreditText.text = string.Format("{0:#,###}", NetworkGameDataManager._Instance.GetCredit());
            SelectUpgradeItem(_selectUpgradeIndex);
            //GameDataManager._Instacne.SaveUserData();
        }
    }

    public void UpdateCreditValue()
    {
        if(NetworkGameDataManager._Instance.GetCredit() != 0)
        {
            _currentCreditText.text = string.Format("{0:#,###}", NetworkGameDataManager._Instance.GetCredit());
        }
        else
        {
            _currentCreditText.text = "0";
        }
        
    }

    public void OpenNoticeWindow(string content)
    {
        if (_noticeWindow == null)
        {
            _noticeWindow = Instantiate(_prefabNoticeWnd).GetComponent<UI_NoticeWindow>();
        }
        _noticeWindow.gameObject.SetActive(true);
        _noticeWindow.SetContentText(content);
    }

    public void ExitUpgradeScene()
    {
        SceneControlManager._Instacne.ConvertScene("LobbyScene");   
    }
}
