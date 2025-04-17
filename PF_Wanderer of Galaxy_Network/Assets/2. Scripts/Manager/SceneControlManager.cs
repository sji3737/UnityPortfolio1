using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneControlManager : MonoBehaviour
{
    static SceneControlManager _uniqueInstance;

    public static SceneControlManager _Instacne
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        if(_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }      
    }
    
    public void ConvertScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
