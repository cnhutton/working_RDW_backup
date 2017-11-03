
/*
 * Adapted from: 
 * http://www.moddb.com/games/gnome-more-war/tutorials/unity-and-the-grid-utility 
 * 
 */
using UnityEngine;

static class Manager
{
    public static SoundManager Sound;
    public static ExperimentManager Experiment;
    public static SceneSwitcher SceneSwitcher;
    public static SpawnManager Spawn;
    public static AlgorithmManager Algorithm;

    static Manager()
    {
        GameObject g = SafeFind("_managers");

        Sound = (SoundManager) SafeComponent(g, "SoundManager");
        Experiment = (ExperimentManager) SafeComponent(g, "ExperimentManager");
        SceneSwitcher = (SceneSwitcher) SafeComponent(g, "SceneSwitcher");
        Spawn = (SpawnManager) SafeComponent(g, "SpawnManager");
        Algorithm = (AlgorithmManager) SafeComponent(g, "AlgorithmManager");
    }
    
    public static void SayHello()
    {
        Debug.Log("Confirming to developer that the GameManager is working fine.");
    }

    private static GameObject SafeFind(string s)
    {
        GameObject g = GameObject.Find(s);
        if (g == null) BigProblem("The " + s + " game object is not in this scene. You're stuffed.");
        return g;
    }
    private static Component SafeComponent(GameObject g, string s)
    {
        Component c = g.GetComponent(s);
        if (c == null) BigProblem("The " + s + " component is not there. You're stuffed.");
        return c;
    }
    private static void BigProblem(string error)
    {
        for (int i = 10; i > 0; --i) Debug.LogError(" >>>>>>>>>>>> Cannot proceed... " + error);
        for (int i = 10; i > 0; --i) Debug.LogError(" !!! Is it possible you just forgot to launch from scene zero, the preload scene.");
        Debug.Break();
    }
}
