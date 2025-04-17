using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameDataManager : MonoBehaviour
{
    static GameDataManager _uniqueInstance;

    int _selectStage;

    UserData _userData;

    public static GameDataManager _Instacne
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
            Debug.Log("GameDataManager Awake");
            LoadUserData();
            DontDestroyOnLoad(gameObject);
        }       
    }

    void LoadUserData()
    {
        string fileName = "UserData";
        string path = Application.dataPath + "/" + fileName + ".dat";
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            _userData = bf.Deserialize(fs) as UserData;
        }
        else
        {
            _userData = new UserData();
            //_userData.Init();
            Debug.Log("GameData FirstLoad");
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _userData);
            fs.Close();
        }
    }

    public void SaveUserData()
    {
        string fileName = "UserData";
        string path = Application.dataPath + "/" + fileName + ".dat";
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, _userData);
        fs.Close();
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

    public float GetBaseAccSpeed()
    {
        return _userData._BaseAccSpeed;
    }

    public float GetBaseRotateSpeed()
    {
        return _userData._BaseRotateSpeed;
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

    public void IncreaseHpUpgrade()
    {
        _userData._HpUpgrade += 1;
    }

    public void IncreaseAtkUpgrade()
    {
        _userData._AtkUpgrade += 1;
    }

    public void IncreaseMissileAtkUpgrade()
    {
        _userData._MissileAtkUpgrade += 1;
    }

    public void IncreaseMaxSpeedUpgrade()
    {
        _userData._MaxSpeedUpgrade += 1;
    }

    public void IncreaseAccSpeedUpgrade()
    {
        _userData._accSpeedUpgrade += 1;
    }

    public void IncreaseRotateSpeedUpgrade()
    {
        _userData._rotateSpeedUpgrade += 1;
    }

    public int GetCredit()
    {
        return _userData._Credit;
    }

    public int GetClearStageData()
    {
        return _userData._ClearStage;
    }

    public void AddCredit(int credit)
    {
        _userData._Credit += credit;
    }

    public void SubstractCredit(int credit)
    {
        _userData._Credit -= credit;
    }

    public void SetClearStageData(int stageNum)
    {
        _userData._ClearStage = stageNum;
    }
}
