using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] Text _hp;
    [SerializeField] Text _atk;
    [SerializeField] Text _missileAtk;
    [SerializeField] Text _maxSpeed;
    [SerializeField] Text _accSpeed;
    [SerializeField] Text _rotateSpeed;

    RectTransform _rectTrasform;

    Image _shadowBG;

    private void Awake()
    {
        _rectTrasform = GetComponent<RectTransform>();
    }

    public void Init(Image bg)
    {
        _shadowBG = bg;
        _shadowBG.gameObject.SetActive(true);

        //int baseHp = GameDataManager._Instacne.GetBaseHp();
        //int hpUpgrade = GameDataManager._Instacne.GetHpUpgrade();
        int baseHp = NetworkGameDataManager._Instance.GetBaseHp();
        int hpUpgrade = NetworkGameDataManager._Instance.GetHpUpgrade();
        int increaseHp = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(hpUpgrade, "HP") * baseHp);
        Debug.Log(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(hpUpgrade, "HP"));
        _hp.text = "체력: " + baseHp + " + " + "<color=#FF0000> " + increaseHp + "</color>";

        //int baseAtk = GameDataManager._Instacne.GetBaseAtk();
        //int atkUpgrade = GameDataManager._Instacne.GetAtkUpgrade();
        int baseAtk = NetworkGameDataManager._Instance.GetBaseAtk();
        int atkUpgrade = NetworkGameDataManager._Instance.GetAtkUpgrade();
        int increaseAtk = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(atkUpgrade, "BasicWeaponDamage") * baseAtk);
        _atk.text = "기본무기 공격력: " + baseAtk + " + " + "<color=#FF0000> " + increaseAtk + "</color>";

        //int baseMissileAtk = GameDataManager._Instacne.GetBaseMissileAtk();
        //int missileAtkUpgrade = GameDataManager._Instacne.GetMissileAtkUpgrade();
        int baseMissileAtk = NetworkGameDataManager._Instance.GetBaseMissileAtk();
        int missileAtkUpgrade = NetworkGameDataManager._Instance.GetMissileAtkUpgrade();
        int increaseMissileAtk = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(missileAtkUpgrade, "MissileWeaponDamage") * baseMissileAtk);
        _missileAtk.text = "유도무기 공격력: " + baseMissileAtk + " + " + "<color=#FF0000> " + increaseMissileAtk + "</color>";

        //float baseMaxSpeed = GameDataManager._Instacne.GetBaseMaxSpeed();
        //int maxSpeedUpgrade = GameDataManager._Instacne.GetMaxSpeedUpgrade();
        float baseMaxSpeed = NetworkGameDataManager._Instance.GetBaseMaxSpeed();
        int maxSpeedUpgrade = NetworkGameDataManager._Instance.GetMaxSpeedUpgrade();
        int increaseMaxSpeed = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(maxSpeedUpgrade, "MaxSpeed") * (int)baseMaxSpeed);
        _maxSpeed.text = "최대 속도: " + baseMaxSpeed + " + " + "<color=#FF0000> " + increaseMaxSpeed + "</color>";

        //float baseAccSpeed = GameDataManager._Instacne.GetBaseAccSpeed();
        //int accSpeedUpgrade = GameDataManager._Instacne.GetAccSpeedUpgrade();
        float baseAccSpeed = NetworkGameDataManager._Instance.GetBaseAccSpeed();
        int accSpeedUpgrade = NetworkGameDataManager._Instance.GetAccSpeedUpgrade();
        int increaseAccSpeed = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(accSpeedUpgrade, "Acceleration") * (int)baseAccSpeed);
        _accSpeed.text = "기체 가속도: " + baseAccSpeed + " + " + "<color=#FF0000> " + increaseAccSpeed + "</color>";

        //float baseRotateSpeed = GameDataManager._Instacne.GetBaseRotateSpeed();
        //int rotateSpeedUpgrade = GameDataManager._Instacne.GetRotateSpeedUpgrade();
        float baseRotateSpeed = NetworkGameDataManager._Instance.GetBaseRotateSpeed();
        int rotateSpeedUpgrade = NetworkGameDataManager._Instance.GetRotateSpeedUpgrade();
        int increaseRotate = (int)(TableDataManager._Instance.Get(LowDataType.UpgradeIncrement).ToF(rotateSpeedUpgrade, "Rotate") * (int)baseRotateSpeed);
        _rotateSpeed.text = "기체 기동성: " + baseRotateSpeed + " + " + "<color=#FF0000> " + increaseRotate + "</color>";

        Vector2 startPos = new Vector2(0, -1000);
        Vector2 endPos = new Vector2(0, 30);
        iTween.ValueTo(gameObject, iTween.Hash("from", startPos, "to", endPos, "onupdate", "UpdatePanelPosTween", "onupdatetarget", gameObject, "time", 0.5f,
                        "easetype", iTween.EaseType.easeOutCubic));
    }

    void UpdatePanelPosTween(Vector2 pos)
    {
        _rectTrasform.anchoredPosition = pos;
    }

    public void ExitPanel()
    {
        _rectTrasform.anchoredPosition = new Vector2(0, -1000);
        _shadowBG.gameObject.SetActive(false);
    }
}
