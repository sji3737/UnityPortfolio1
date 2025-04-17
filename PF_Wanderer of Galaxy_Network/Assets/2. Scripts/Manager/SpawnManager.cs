using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Vector3[] _spawnPoints;
    [SerializeField] PublicDefine.stEnemySpawnInfo[] _spawnInfo;
    [SerializeField] GameObject _prefabFixedType1Enemy;
    [SerializeField] GameObject _prefabFixedType2Enemy;
    [SerializeField] GameObject _prefabPassType1Enemy;
    [SerializeField] GameObject _prefabFightType1Enemy;

    int _currentSpawnIdx = 0;
    static SpawnManager _uniqueInstance;

    List<Enemy> _spawnEnemyList = new List<Enemy>();

    public static SpawnManager _Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
        
    }

    public int SpawnEnemy(int area, int stage)
    {
        int spawnCnt = 0;
        int spawnFight1 = SpawnInfoManager._Instance.GetCurrentFightType1SpawnCount(area, stage, _currentSpawnIdx);
        int spawnFixed1Cnt = SpawnInfoManager._Instance.GetCurrentFixedType1SpawnCount(area, stage, _currentSpawnIdx);
        int spawnFixed2Cnt = SpawnInfoManager._Instance.GetCurrentFixedType2SpawnCount(area, stage, _currentSpawnIdx);
        Vector3 spawnPos = SpawnInfoManager._Instance.GetCurrentSpawnPos(area, stage, _currentSpawnIdx);
        //for (int i = 0; i < _spawnInfo[_currentSpawnIdx].fightType1; i++)
        for (int i = 0; i < spawnFight1; i++)
        {
            float xRan = Random.Range(-60, 60);
            float zRan = Random.Range(-10, 10);
            //Vector3 spawnPos = _spawnPoints[_currentSpawnIdx];
            spawnPos.x += xRan;
            spawnPos.z += zRan;
            GameObject obj = Instantiate(_prefabFightType1Enemy, spawnPos, Quaternion.identity);
            _spawnEnemyList.Add(obj.GetComponent<Enemy>());
            spawnCnt++;
        }
        //for (int i = 0; i < _spawnInfo[_currentSpawnIdx].fixedType1; i++)
        for (int i = 0; i < spawnFixed1Cnt; i++)
        {
            //Vector3 spawnPos = _spawnInfo[_currentSpawnIdx].fixed1SpawnPos[i];
            spawnPos = SpawnInfoManager._Instance.GetCurrentFixedType1SpawnPos(area, stage, _currentSpawnIdx, i);
            GameObject obj = Instantiate(_prefabFixedType1Enemy, spawnPos, Quaternion.identity);
            _spawnEnemyList.Add(obj.GetComponent<Enemy>());
            spawnCnt++;
        }
        //for (int i = 0; i < _spawnInfo[_currentSpawnIdx].fixedType2; i++)
        for (int i = 0; i < spawnFixed2Cnt; i++)
        {
            //Vector3 spawnPos = _spawnInfo[_currentSpawnIdx].fixed2SpawnPos[i];
            spawnPos = SpawnInfoManager._Instance.GetCurrentFixedType2SpawnPos(area, stage, _currentSpawnIdx, i);
            GameObject obj = Instantiate(_prefabFixedType2Enemy, spawnPos, Quaternion.identity);
            _spawnEnemyList.Add(obj.GetComponent<Enemy>());
            spawnCnt++;
        }
        _currentSpawnIdx++;
        return spawnCnt;
    }

    public void SpawnPassTypeEnemy(Vector3 playerPos)
    {
        StartCoroutine(IESpawnPassTypeEnemy(playerPos));
    }

    public Vector3 GetCurrentSpawnPoint()
    {
        return _spawnPoints[_currentSpawnIdx];
    }

    public void RemoveEnemy(Enemy e)
    {
        _spawnEnemyList.Remove(e);
    }

    public void RemoveEnemyAll()
    {
        foreach(Enemy e in _spawnEnemyList)
        {
            Destroy(e.gameObject);
        }
        _spawnEnemyList.Clear();
    }

    public void SetEnemyStateEnd()
    {
        foreach (Enemy e in _spawnEnemyList)
        {
            e.SetStateEnd();
        }
    }

    public void SetEscapeMode()
    {
        foreach(Enemy e in _spawnEnemyList)
        {
            e.SetStateEscape();
        }
    }

    IEnumerator IESpawnPassTypeEnemy(Vector3 playerPos)
    {
        int pSpawnCnt = 0;

        Vector3 SpawnPos = playerPos;
        SpawnPos.x -= 100;
        SpawnPos.z += 120;

        Vector3 lookPos = playerPos;
        lookPos.x += 50;

        Quaternion lookAngle = Quaternion.LookRotation(lookPos - SpawnPos);



        while (pSpawnCnt < 3)
        {
            Instantiate(_prefabPassType1Enemy, SpawnPos, lookAngle);
            pSpawnCnt++;
            yield return new WaitForSeconds(0.5f);
        }
               
    }
    
}
