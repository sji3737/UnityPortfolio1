using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultWindow : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Text _creditText;

    int _score;
    int _goalCredit;

    public void SetWindowInit(int score)
    {
        _score = score;
        _goalCredit = score * 5;
        _scoreText.text = _score.ToString();
        _creditText.text = "0";

        StartCoroutine(DecreaseScoreText());
        StartCoroutine(IncreaceCreditText());
    }

    public void ClickGotoLobbyButton()
    {
        InGameManager._Instance.SendGameResultPack();
    }

    public void GotoLobbyScene()
    {
        //GameDataManager._Instacne.AddCredit(_goalCredit);
        //GameDataManager._Instacne.SaveUserData();
        SceneControlManager._Instacne.ConvertScene("LobbyScene");      
    }

    IEnumerator DecreaseScoreText()
    {
        yield return new WaitForSeconds(1.5f);
        float offset = _score / 0.5f;
        float currValue = _score;
        while (currValue > 0)
        {
            currValue -= offset * Time.deltaTime;
            _scoreText.text = ((int)currValue).ToString();
            yield return null;
        }
        _scoreText.text = "0";
    }

    IEnumerator IncreaceCreditText()
    {
        yield return new WaitForSeconds(1.5f);
        float offset = _goalCredit / 0.5f;
        float currValue = 0;
        while(currValue < _goalCredit)
        {
            currValue += offset * Time.deltaTime;
            _creditText.text = ((int)currValue).ToString();
            yield return null;
        }
        _creditText.text = ((int)_goalCredit).ToString();
    }
}
