using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneSwitcher : MonoBehaviour
{

    private int _index = 0;

    public void LoadNextScene()
    {
        if (_index >= SceneManager.sceneCountInBuildSettings) return;
        SceneManager.LoadScene(_index + 1, LoadSceneMode.Single);
        
        ++_index;
        Debug.Log("Loaded scene # " + _index);
    }

    public int CurrentSceneIndex()
    {
        return _index;
    }

    public void LoadNextScene(SceneName name)
    {
        switch (name)
        {
            case SceneName.Zero:
                SceneManager.LoadScene("0 Preload", LoadSceneMode.Single);
                break;
            case SceneName.One:
                SceneManager.LoadScene("1 Intro", LoadSceneMode.Single);
                break;
            case SceneName.Two:
                SceneManager.LoadScene("2 RDW Training", LoadSceneMode.Single);
                break;
            case SceneName.Three:
                SceneManager.LoadScene("3 Calibration Training", LoadSceneMode.Single);
                break;
            case SceneName.Four:
                SceneManager.LoadScene("4 Calibration", LoadSceneMode.Single);
                break;
            case SceneName.Five:
                SceneManager.LoadScene("5 Walkthrough", LoadSceneMode.Single);
                break;
            default:
                throw new ArgumentOutOfRangeException("name", name, null);
        }
    }

}
