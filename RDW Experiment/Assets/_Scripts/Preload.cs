using System.Collections;
using UnityEngine;

public class Preload : MonoBehaviour
{

    void Start()
    {
        Manager.SayHello();
        Manager.Experiment.Initialize();
        Manager.Sound.Initialize();
        StartCoroutine(loadSceneAfterDelay(2));
    }


    IEnumerator loadSceneAfterDelay(float waitbySecs)
    {
        yield return new WaitForSeconds(waitbySecs);
        Manager.SceneSwitcher.LoadNextScene();
    }
}
