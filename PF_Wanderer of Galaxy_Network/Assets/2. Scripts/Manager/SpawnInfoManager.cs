using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInfoManager : MonoBehaviour
{
    public Spawn_AreaInfo[] _areaInfo;

    static SpawnInfoManager _uniqueInstance;

    public static SpawnInfoManager _Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
    }

    public int GetCurrentWaveMaxCount(int area, int stage)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo.Length;
    }

    public Vector3 GetCurrentSpawnPos(int area, int stage, int waveCnt)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo[waveCnt]._spawnPos;
    }

    public int GetCurrentFightType1SpawnCount(int area, int stage, int waveCnt)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo[waveCnt]._fightType1SpawnCnt;
    }

    public Vector3 GetCurrentFixedType1SpawnPos(int area, int stage, int waveCnt, int spawnCnt)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo[waveCnt]._fixedType1SpawnPos[spawnCnt];
    }

    public int GetCurrentFixedType1SpawnCount(int area, int stage, int waveCnt)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo[waveCnt]._fixedType1SpawnCnt;
    }

    public Vector3 GetCurrentFixedType2SpawnPos(int area, int stage, int waveCnt, int spawnCnt)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo[waveCnt]._fixedType2SpawnPos[spawnCnt];
    }

    public int GetCurrentFixedType2SpawnCount(int area, int stage, int waveCnt)
    {
        return _areaInfo[area - 1]._stageInfo[stage - 1]._waveInfo[waveCnt]._fixedType2SpawnCnt;
    }

}
[Serializable]
public class Spawn_AreaInfo
{
    public Spawn_StageInfo[] _stageInfo;
}
[Serializable]
public class Spawn_StageInfo
{
    public Spawn_WaveInfo[] _waveInfo;
}
[Serializable]
public class Spawn_WaveInfo
{
    public Vector3 _spawnPos;
    public int _fixedType1SpawnCnt;
    public Vector3[] _fixedType1SpawnPos;
    public int _fixedType2SpawnCnt;
    public Vector3[] _fixedType2SpawnPos;
    public int _passType1SpawnCnt;
    public int _passType1SpawnNum;
    public int _fightType1SpawnCnt;
}
