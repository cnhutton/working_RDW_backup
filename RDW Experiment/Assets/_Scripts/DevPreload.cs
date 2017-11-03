using UnityEngine;

public class DevPreload : MonoBehaviour
{
    void Awake()
    {
        GameObject check = GameObject.Find("_managers");
        if (check == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("0 Preload");
        }

    }
}