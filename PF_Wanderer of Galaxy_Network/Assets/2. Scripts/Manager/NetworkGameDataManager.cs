using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameDataManager : MonoBehaviour
{
    static NetworkGameDataManager _uniqueInstance;

    int _selectStage;

    UserData _userData;

    public static NetworkGameDataManager _Instance
    {
        get { return _uniqueInstance; }
    }

    public int _SelectStage
    {
        get { return _selectStage; }
        set { _selectStage = value; }
    }

    private void Awake()
    {
        if(_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);

            _userData = new UserData();
        }        
    }

    public void SetUserinfo(int credit, int clearStage, int baseHp, int baseAtk, int baseMissileAtk, float baseMaxSpeed, float baseAccSpeed, float baseRotateSpeed,
        int hpUpgrade, int atkUpgrade, int missileAtkUpgrade, int MaxSpeedUpgrade, int accSpeedUpgrade, int rotateSpeedUpgrade)
    {
        _userData.Init(baseHp, baseAtk, baseMissileAtk, baseMaxSpeed, baseAccSpeed, baseRotateSpeed);

        _userData._Credit = credit;
        _userData._ClearStage = clearStage;

        _userData._HpUpgrade = hpUpgrade;
        _userData._AtkUpgrade = atkUpgrade;
        _userData._MissileAtkUpgrade = missileAtkUpgrade;
        _userData._MaxSpeedUpgrade = MaxSpeedUpgrade;
        _userData._accSpeedUpgrade = accSpeedUpgrade;
        _userData._rotateSpeedUpgrade = rotateSpeedUpgrade;

        Debug.Log("_Credit: " + _userData._Credit + "/n ClearStage: " + _userData._ClearStage + "");
    }

    public int GetCredit()
    {
        return _userData._Credit;
    }

    public int GetBaseHp()
    {
        return _userData._BaseHP;
    }

    public int GetBaseAtk()
    {
        return _userData._BaseAtk;
    }

    public int GetBaseMissileAtk()
    {
        return _userData._BaseMissileAtk;
    }

    public float GetBaseMaxSpeed()
    {
        return _userData._BaseMaxSpeed;
    }

    public float GetBaseRotateSpeed()
    {
        return _userData._BaseRotateSpeed;
    }

    public float GetBaseAccSpeed()
    {
        return _userData._BaseAccSpeed;
    }

    public int GetHpUpgrade()
    {
        return _userData._HpUpgrade;
    }

    public int GetAtkUpgrade()
    {
        return _userData._AtkUpgrade;
    }

    public int GetMissileAtkUpgrade()
    {
        return _userData._MissileAtkUpgrade;
    }

    public int GetMaxSpeedUpgrade()
    {
        return _userData._MaxSpeedUpgrade;
    }

    public int GetAccSpeedUpgrade()
    {
        return _userData._accSpeedUpgrade;
    }

    public int GetRotateSpeedUpgrade()
    {
        return _userData._rotateSpeedUpgrade;
    }

    public int GetClearStageData()
    {
        return _userData._ClearStage;
    }

    public void SetClearStageData(int stage)
    {
        _userData._ClearStage = stage;
    }

    public void RequestIncreaseUpgrade(PublicDefine.eUpgradeType type, int requireCredit)
    {
        TCPClient._Instace.SendIncreaseUpgradeRequest(type, requireCredit);
    }

    public void IncreaseUpgrade(PublicDefine.eUpgradeType type, int requireCredit)
    {
        switch (type)
        {
            case PublicDefine.eUpgradeType.Hp:
                _userData._HpUpgrade++;
                _userData._Credit -= requireCredit;
                break;
            case PublicDefine.eUpgradeType.Atk:
                _userData._AtkUpgrade++;
                _userData._Credit -= requireCredit;
                break;
            case PublicDefine.eUpgradeType.MissileAtk:
                _userData._MissileAtkUpgrade++;
                _userData._Credit -= requireCredit;
                break;
            case PublicDefine.eUpgradeType.Speed:
                _userData._MaxSpeedUpgrade++;
                _userData._Credit -= requireCredit;
                break;
            case PublicDefine.eUpgradeType.AccSpeed:
                _userData._accSpeedUpgrade++;
                _userData._Credit -= requireCredit;
                break;
            case PublicDefine.eUpgradeType.RotateSpeed:
                _userData._rotateSpeedUpgrade++;
                _userData._Credit -= requireCredit;
                break;
        }
        UpgradeUIManager._Instance.UpdateCreditValue();
        UpgradeUIManager._Instance.SelectUpgradeItem((int)type + 1);
    }

    public void UpdateCreditValue(int credit)
    {
        _userData._Credit = credit;
    }

    public void ChangeCashValue(int value)
    {

    }
}
