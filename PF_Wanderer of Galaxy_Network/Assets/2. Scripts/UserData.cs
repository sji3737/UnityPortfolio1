using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    int _baseHP;
    int _baseAtk;
    int _baseMissileAtk;
    float _baseMaxSpeed;
    float _baseAccSpeed;
    float _baseRotateSpeed;

    public int _BaseHP
    {
        get { return _baseHP; }
    }

    public int _BaseAtk
    {
        get { return _baseAtk; }
    }

    public int _BaseMissileAtk
    {
        get { return _baseMissileAtk; }
    }

    public float _BaseMaxSpeed
    {
        get { return _baseMaxSpeed; }
    }

    public float _BaseAccSpeed
    {
        get { return _baseAccSpeed; }
    }

    public float _BaseRotateSpeed
    {
        get { return _baseRotateSpeed; }
    }

    public int _HpUpgrade
    {
        get;set;
    }
    public int _AtkUpgrade
    {
        get;set;
    }
    public int _MissileAtkUpgrade
    {
        get;set;
    }
    public int _MaxSpeedUpgrade
    {
        get;set;
    }
    public int _accSpeedUpgrade
    {
        get;set;
    }
    public int _rotateSpeedUpgrade
    {
        get;set;
    }

    public int _Credit
    {
        get;set;
    }

    public int _ClearStage
    {
        get;set;
    }

    public void Init(int baseHp, int baseAtk, int baseMissileAtk, float baseMaxSpeed, float baseAccSpeed, float baseRotateSpeed)
    {
        {
            //_baseHP = 200;
            //_baseAtk = 25;
            //_baseMissileAtk = 120;
            //_baseMaxSpeed = 90f;
            //_baseAccSpeed = 300f;
            //_baseRotateSpeed = 300f;

            //_HpUpgrade = 0;
            //_AtkUpgrade = 0;
            //_MissileAtkUpgrade = 0;
            //_speedUpgrade = 0;
            //_accSpeedUpgrade = 0;
            //_rotateSpeedUpgrade = 0;

            //_Credit = 1500;
            //_ClearStage = 11;
        }

        _baseHP = baseHp;
        _baseAtk = baseAtk;
        _baseMissileAtk = baseMissileAtk;
        _baseMaxSpeed = baseMaxSpeed;
        _baseAccSpeed = baseAccSpeed;
        _baseRotateSpeed = baseRotateSpeed;

    }
}
