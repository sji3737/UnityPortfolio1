using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    static InGameUIManager _uniqueInstacne;

    [SerializeField] Image _nextSpawnPosIndicator;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _timerText;
    [SerializeField] Image _hpBar;
    [SerializeField] Image _missileIconBG;
    [SerializeField] Image _missionResultImg;
    [SerializeField] Sprite[] _missionResultSprites;
    [SerializeField] Image _backgroundImg;
    [SerializeField] GameObject _resultWindow;
    [SerializeField] GameObject _returnLobbyWindow;
    [SerializeField] GameObject _gameResultHandleWindowObj;
    [SerializeField] Text _debugText;
    PlayerControl _player;

    bool _isUIWork;

    bool _isSpawnIndicatorOn;
    Coroutine _spawnArrowRotate;
    Coroutine _spawnArrowFill;

    float _missileDelayTime;
    float _missileDelayTimer;
    bool _startMissileDelayTimer = false;

    float _killScore = 0;
    float _remainTime;

    UI_ReturnLobbyWindow _returnLobbyWnd;
    UI_GameResultHandleFailWindow _gameResultHandelFailWnd;

    public static InGameUIManager _Instance
    {
        get
        {
            return _uniqueInstacne;
        }
    }

    private void Awake()
    {
        _uniqueInstacne = this;
        _player = GameObject.FindGameObjectWithTag("PlayerRoot").GetComponent<PlayerControl>();
        _isSpawnIndicatorOn = false;
        _nextSpawnPosIndicator.gameObject.SetActive(false);
        _resultWindow.SetActive(false);
        _scoreText.text = "0";
        _isUIWork = false;
    }

    private void LateUpdate()
    {
        if (_startMissileDelayTimer)
        {
            Image shadowBG = _missileIconBG.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _missileDelayTimer -= Time.deltaTime;
            shadowBG.fillAmount = _missileDelayTimer / _missileDelayTime;
            if (_missileDelayTimer <= 0)
            {
                _startMissileDelayTimer = false;
            }
        }
        if (_isUIWork)
        {
            _remainTime -= Time.deltaTime;
            int m = (int)_remainTime / 60;
            int s = (int)_remainTime % 60;
            if (s < 10)
            {
                _timerText.text = string.Format("{0}:0{1}", m, s);
            }
            else
            {
                _timerText.text = string.Format("{0}:{1}", m, s);
            }
        }      
    }

    void TweenMoveHpBar()
    {
        Vector2 startPos = new Vector2(-1300, 400);
        Vector2 endPos = new Vector2(-600, 400);
        GameObject obj = _hpBar.transform.parent.gameObject;
        iTween.ValueTo(obj, iTween.Hash("from", startPos, "to", endPos, "onupdate", "UpdateHpBarTween", "onupdatetarget", gameObject, "time", 1.0f,
            "easetype", iTween.EaseType.easeOutCubic));
    }

    void UpdateHpBarTween(Vector2 targetPos)
    {
        Image obj = _hpBar.transform.parent.gameObject.GetComponent<Image>();
        obj.rectTransform.anchoredPosition = targetPos;
    }

    void TweenTimerText()
    {
        Vector2 startPos = new Vector2(0, 600);
        Vector2 endPos = new Vector2(0, 400);
        iTween.ValueTo(gameObject, iTween.Hash("from", startPos, "to", endPos, "onupdate", "UpdateTimerTween", "onupdatetarget", gameObject, "time", 1.0f,
            "easetype", iTween.EaseType.easeOutCubic));
    }

    void UpdateTimerTween(Vector2 targetPos)
    {
        _timerText.rectTransform.anchoredPosition = targetPos;
    }

    void TweenScoreBoard()
    {
        Vector2 startPos = new Vector2(1200, 400);
        Vector2 endPos = new Vector2(700, 400);
        iTween.ValueTo(gameObject, iTween.Hash("from", startPos, "to", endPos, "onupdate", "UpdateScoreBoardTween", "onupdatetarget", gameObject, "time", 1.0f,
            "easetype", iTween.EaseType.easeOutCubic));
    }

    void UpdateScoreBoardTween(Vector2 targetPos)
    {
        Image obj = _scoreText.transform.parent.gameObject.GetComponent<Image>();
        obj.rectTransform.anchoredPosition = targetPos;
    }

    void TweenSkillGroup()
    {
        Vector2 startPos = new Vector2(0, -650);
        Vector2 endPos = new Vector2(0, -400);
        iTween.ValueTo(gameObject, iTween.Hash("from", startPos, "to", endPos, "onupdate", "UpdateSkillGroupTween", "onupdatetarget", gameObject, "time", 1.0f,
            "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "EndGameStartTween", "oncompletetarget", gameObject));
    }

    void UpdateSkillGroupTween(Vector2 targetPos)
    {
        RectTransform obj = _missileIconBG.transform.parent.GetComponent<RectTransform>();
        obj.anchoredPosition = targetPos;
    }

    void EndGameStartTween()
    {
        _isUIWork = true;
        InGameManager._Instance._GameState = PublicDefine.PlayState.IDLE;
    }

    void TweenClearImage()
    {
        _missionResultImg.gameObject.SetActive(true);
        _missionResultImg.transform.localScale = new Vector3(0, 0, 0);
        float startScale = 0;
        float EndScale = 1;
        iTween.ValueTo(gameObject, iTween.Hash("from", startScale, "to", EndScale, "onupdate", "UpdateClearImage", "onupdatetarget", gameObject, "time", 1.0f,
            "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "EndResultImgTween", "oncompletetarget", gameObject));
    }

    void UpdateClearImage(float targetScale)
    {
        _missionResultImg.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
    }

    void TweenFailImage()
    {
        _missionResultImg.gameObject.SetActive(true);
        _missionResultImg.color = new Color(1, 1, 1, 0);
        float startAlpha = 0;
        float EndAlpha = 1;
        iTween.ValueTo(gameObject, iTween.Hash("from", startAlpha, "to", EndAlpha, "onupdate", "UpdateFailImage", "onupdatetarget", gameObject, "time", 1.0f,
            "easetype", iTween.EaseType.easeOutCubic, "oncomplete", "EndResultImgTween", "oncompletetarget", gameObject));
    }

    void UpdateFailImage(float targetAlpha)
    {
        _missionResultImg.color = new Color(1, 1, 1, targetAlpha);
    }

    void EndResultImgTween()
    {
        StartCoroutine(SetBackgorundDark());
    }

    public void SetPlayTime(float second)
    {
        _remainTime = second;
        int m = (int)second / 60;
        int s = (int)second % 60;
        if (s < 10)
        {
            _timerText.text = string.Format("{0}:0{1}", m, s);
        }
        else
        {
            _timerText.text = string.Format("{0}:{1}", m, s);
        }
    }

    public void SetDebugText(string a)
    {
        _debugText.text = a;
    }

    public void StartUITween()
    {
        TweenMoveHpBar();
        TweenTimerText();
        TweenScoreBoard();
        TweenSkillGroup();
    }

    public void SetHpBarValue(float value)
    {
        _hpBar.fillAmount = value;
    }

    public void StartNextSpawnPosIndicator(Vector3 targetPos)
    {
        if (!_isSpawnIndicatorOn)
        {
            _spawnArrowRotate = StartCoroutine(PlayNextSpawnPosIndicator(targetPos));
            _spawnArrowFill = StartCoroutine(FillNextSpawnPosIndicatorArrow());
            _nextSpawnPosIndicator.gameObject.SetActive(true);
            _isSpawnIndicatorOn = true;
        }
        
    }

    public void StopNextSpawnPosIndicator()
    {
        _isSpawnIndicatorOn = false;
        _nextSpawnPosIndicator.gameObject.SetActive(false);
        if(_spawnArrowRotate != null)
        {
            StopCoroutine(_spawnArrowRotate);
        }
        if(_spawnArrowFill != null)
        {
            StopCoroutine(_spawnArrowFill);
        }      
    }

    public void SelectMissileSkill()
    {
        _missileIconBG.color = new Color(255 / 255, 150 / 255, 0);
        Text missileText = _missileIconBG.transform.GetChild(1).GetComponent<Text>();
        missileText.color = new Color(255 / 255, 150 / 255, 0);
    }

    public void DeSelectMissileSkill()
    {
        _missileIconBG.color = new Color(1, 1, 1);
        Text missileText = _missileIconBG.transform.GetChild(1).GetComponent<Text>();
        missileText.color = new Color(0, 1, 1);
    }

    public void SetMissileDelayTimer(float delay)
    {
        _missileDelayTime = delay;
        _missileDelayTimer = delay;
        _startMissileDelayTimer = true;
    }

    public void SetScoreText(int score)
    {
        StartCoroutine(IncreaseScoreText(score));
    }

    public void SetMissionResultImg(PublicDefine.GameEndState state)
    {
        _missionResultImg.sprite = _missionResultSprites[(int)state];
        
        _backgroundImg.gameObject.SetActive(true);
        if (state == PublicDefine.GameEndState.Clear)
        {
            TweenClearImage();
        }
        else
        {
            TweenFailImage();
        }
    }

    public void SetResultWindow()
    {
        _backgroundImg.gameObject.SetActive(false);
        _missionResultImg.gameObject.SetActive(false);
        _hpBar.transform.parent.gameObject.SetActive(false);
        _scoreText.transform.parent.gameObject.SetActive(false);
        _timerText.gameObject.SetActive(false);
        _missileIconBG.transform.parent.gameObject.SetActive(false);
        _resultWindow.SetActive(true);
        UI_ResultWindow resultWnd = _resultWindow.GetComponent<UI_ResultWindow>();
        resultWnd.SetWindowInit((int)_killScore);
    }

    public void GotoLobbyScene()
    {
        Debug.Log("InGameUIManger GotoLobbyScene()");
        SceneControlManager._Instacne.ConvertScene("LobbyScene");
    }

    public void SetReturnLobbyWindow()
    {
        Time.timeScale = 0f;
        if(_returnLobbyWnd == null)
        {
            _returnLobbyWnd = Instantiate(_returnLobbyWindow).GetComponent<UI_ReturnLobbyWindow>();
        }
        else
        {
            _returnLobbyWnd.transform.gameObject.SetActive(true);
        }
    }

    public void SetGameResultHandleFailWindow()
    {
        if(_gameResultHandelFailWnd == null)
        {
            _gameResultHandelFailWnd = Instantiate(_gameResultHandleWindowObj).GetComponent<UI_GameResultHandleFailWindow>();
        }
        else
        {
            _gameResultHandleWindowObj.transform.gameObject.SetActive(true);
        }
    }

    public void SetEscapeToLobby()
    {
        _isUIWork = false;
        StartCoroutine(SetEscapeBackgorundDark());
    }

    IEnumerator PlayNextSpawnPosIndicator(Vector3 targetPos)
    {
        while (true)
        {
            Vector3 dir = _player.gameObject.transform.position - targetPos;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            _nextSpawnPosIndicator.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            yield return null;
        }
    }

    IEnumerator FillNextSpawnPosIndicatorArrow()
    {
        while (true)
        {
            _nextSpawnPosIndicator.fillAmount += Time.deltaTime * 2;
            yield return null;
            if (_nextSpawnPosIndicator.fillAmount >= 1)
            {
                yield return new WaitForSeconds(1.0f);
                _nextSpawnPosIndicator.fillAmount = 0;
            }           
        }
    }

    IEnumerator IncreaseScoreText(int score)
    {
        float offset = score / 0.5f;
        float currentScore = _killScore;
        _killScore += score;
        while (_killScore >= currentScore)
        {
            currentScore += offset * Time.deltaTime;
            _scoreText.text = ((int)currentScore).ToString();
            yield return null;
        }
        _scoreText.text = ((int)_killScore).ToString();
    }

    IEnumerator SetBackgorundDark()
    {
        yield return new WaitForSeconds(2f);
        float alpha = 0;
        while(alpha < 1)
        {
            alpha += Time.deltaTime;
            _backgroundImg.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        InGameManager._Instance._GameState = PublicDefine.PlayState.Result;
    }

    IEnumerator SetEscapeBackgorundDark()
    {
        yield return new WaitForSeconds(2f);
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            _backgroundImg.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SceneControlManager._Instacne.ConvertScene("LobbyScene");
    }
}
