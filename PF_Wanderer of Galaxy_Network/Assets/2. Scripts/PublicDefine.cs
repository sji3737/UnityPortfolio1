using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicDefine
{
    [Serializable]
    public struct stEnemySpawnInfo
    {
        public int fixedType1;
        public Vector3[] fixed1SpawnPos;
        public int fixedType2;
        public Vector3[] fixed2SpawnPos;
        public int passType1;
        public int passSpawnNum;
        public int fightType1;
    }
    public enum eEnemyState
    {
        IDLE = 0,
        PlayerTrack,
        ATTACK,
        DODGE,
        DEAD,
        ESCAPE
    }
    public enum eEnemyType
    {
        PASS = 0,
        FIXED1,
        FIXED2,
        FIGHT1
    }
    public enum PlayState
    {
        INIT = 0,
        INIT_WAIT,
        IDLE,
        SPAWN_READY,
        SPAWN,
        SPAWN_PassType,
        GamePlay,
        GameEnd,
        ShowEndImg,
        Result,
        ShowResult,
        Escape
    }

    public enum GameEndState
    {
        Clear = 0,
        Fail,
        TimeOver
    }

    public enum eUpgradeType
    {
        Hp = 0,
        Atk,
        MissileAtk,
        Speed,
        AccSpeed,
        RotateSpeed
    }
}
