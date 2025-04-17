using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] int _maxSpawnIdx = 1;
    static InGameManager _uniqueInstance;
    PlayerControl _player;
    CrossHairControl _crossHair;

    PublicDefine.PlayState _gameState;
    PublicDefine.GameEndState _gameEndState;
    Vector3 _currentSpawnPos;

    float _indicatorStartTime = 2.5f;

    int _killScore = 0;
    int _areaNum;
    int _stageNum;
    int _clearStageNum;

    public PublicDefine.PlayState _GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    int _currentSpawnIdx = 0;
    int _currentSpawnEnemyCnt;
    int _killEnemyCnt = 0;

    public static InGameManager _Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;       
        _crossHair = GameObject.FindGameObjectWithTag("CrossHair").GetComponent<CrossHairControl>();
        _gameState = PublicDefine.PlayState.INIT;
    }

    private void Start()
    {
        SetCurrentStageNumber();
    }

    private void LateUpdate()
    {
        switch (_gameState)
        {
            case PublicDefine.PlayState.INIT:
                _player = GameObject.FindGameObjectWithTag("PlayerRoot").GetComponent<PlayerControl>();
                InGameUIManager._Instance.SetPlayTime(180);
                InGameUIManager._Instance.StartUITween();
                _gameState = PublicDefine.PlayState.INIT_WAIT;
                break;
            case PublicDefine.PlayState.IDLE:
                _indicatorStartTime -= Time.deltaTime;
                _player.SetPlayerStatePlay();
                if(_indicatorStartTime <= 0)
                {
                    //_currentSpawnPos = SpawnManager._Instance.GetCurrentSpawnPoint();
                    _currentSpawnPos = SpawnInfoManager._Instance.GetCurrentSpawnPos(_areaNum, _stageNum, _currentSpawnIdx);
                    InGameUIManager._Instance.StartNextSpawnPosIndicator(_currentSpawnPos);
                    _gameState = PublicDefine.PlayState.SPAWN_READY;
                }                
                break;
            case PublicDefine.PlayState.SPAWN_READY:
                if (Vector3.Distance(_currentSpawnPos, _player.gameObject.transform.position) < 150)
                {
                    _gameState = PublicDefine.PlayState.SPAWN;
                    InGameUIManager._Instance.StopNextSpawnPosIndicator();
                    _indicatorStartTime = 2.5f;
                    SpawnNextEnemy();
                }
                break;
            case PublicDefine.PlayState.SPAWN:
                if (Vector3.Distance(_currentSpawnPos, _player.gameObject.transform.position) < 100)
                {
                    _gameState = PublicDefine.PlayState.SPAWN_PassType;
                    SpawnPassTypeEnemy(_player.gameObject.transform.position);                                  
                }
                break;
            case PublicDefine.PlayState.GameEnd:
                InGameUIManager._Instance.SetMissionResultImg(_gameEndState);
                SpawnManager._Instance.SetEnemyStateEnd();
                _gameState = PublicDefine.PlayState.ShowEndImg;
                break;
            case PublicDefine.PlayState.Result:
                InGameUIManager._Instance.SetResultWindow();
                _player.SetPlayerStateEnd();
                SpawnManager._Instance.RemoveEnemyAll();
                _gameState = PublicDefine.PlayState.ShowResult;
                break;

        }

        if(_gameState != PublicDefine.PlayState.ShowEndImg || _gameState != PublicDefine.PlayState.ShowResult || _gameState != PublicDefine.PlayState.GameEnd)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //if(Time.timeScale != 0)
                //{
                //    SetGamePause();
                //}
                //else
                //{
                //    SetGameResume();
                //}
                InGameUIManager._Instance.SetReturnLobbyWindow();
            }
        }
    }

    public void SetCurrentStageNumber()
    {
        //int stageNum = GameDataManager._Instacne._SelectStage;
        int stageNum = NetworkGameDataManager._Instance._SelectStage;
        _areaNum = stageNum / 10;
        _stageNum = stageNum % 10;
        _maxSpawnIdx = SpawnInfoManager._Instance.GetCurrentWaveMaxCount(_areaNum, _stageNum);
    }

    public void SetPlayerLockOn()
    {
        Time.timeScale = 0.1f;
        _crossHair.SetLockOnMode();
    }

    public void CancelPlayerLockOn()
    {
        Time.timeScale = 1.0f;
        _crossHair.CancelLockOnMode();
    }

    public void EnemyKill()
    {
        _killEnemyCnt++;
        if (_killEnemyCnt >= _currentSpawnEnemyCnt)
        {
            if (_currentSpawnIdx >= _maxSpawnIdx)
            {
                _gameState = PublicDefine.PlayState.GameEnd;
                _gameEndState = PublicDefine.GameEndState.Clear;
                //if(GameDataManager._Instacne.GetClearStageData() == GameDataManager._Instacne._SelectStage)
                //{
                //    GameDataManager._Instacne.SetClearStageData(GameDataManager._Instacne._SelectStage + 1);
                //}
                if (NetworkGameDataManager._Instance.GetClearStageData() == NetworkGameDataManager._Instance._SelectStage)
                {
                    _clearStageNum = NetworkGameDataManager._Instance._SelectStage + 1;
                }
            }
            else
            {
                _killEnemyCnt = 0;
                _gameState = PublicDefine.PlayState.IDLE;
            }
        }
    }

    public void PlayerDead()
    {
        _clearStageNum = NetworkGameDataManager._Instance._SelectStage;
        _gameState = PublicDefine.PlayState.GameEnd;
        _gameEndState = PublicDefine.GameEndState.Fail;        
    }

    public void AddKillScore(int score)
    {
        _killScore += score;
        InGameUIManager._Instance.SetScoreText(score);
    }

    public void SpawnNextEnemy()
    {
        _currentSpawnEnemyCnt = SpawnManager._Instance.SpawnEnemy(_areaNum, _stageNum);
        _currentSpawnIdx++;
    }

    public void SpawnPassTypeEnemy(Vector3 playerPos)
    {
        SpawnManager._Instance.SpawnPassTypeEnemy(playerPos);
        _gameState = PublicDefine.PlayState.GamePlay;
    }

    public void SendGameResultPack()
    {
        int creditValue = _killScore * 5;

        TCPClient._Instace.SendGameResultPacket(creditValue, _clearStageNum);
    }

    public void SetEscapeMode()
    {
        _gameState = PublicDefine.PlayState.Escape;
    }

    public void SetGamePause()
    {
        Time.timeScale = 0;
    }

    public void SetGameResume()
    {
        Time.timeScale = 1f;
    }
}
