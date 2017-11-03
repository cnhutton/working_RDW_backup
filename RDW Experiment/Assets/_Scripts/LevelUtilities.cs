using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelUtilities
{
    public static void WriteToFile(string path, string text)
    {
        StreamWriter writer = new StreamWriter(path, true); //
        writer.WriteLine(text);
        writer.Close();
    }

    //min(inclusive) max(exclusive)
    public static int GenerateRandomInt(int minRange, int maxRange)
    {
        return Random.Range(minRange, maxRange);
    }

    public static bool GenerateRandomBool()
    {
        return Random.Range(0, 2) > 0;
    }

    public static Vector3 FlattenVector3(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public static Edge ChooseRandomEdge()
    {
        int value = GenerateRandomInt(0, 4);
        switch (value)
        {
            case 0:
                return Edge.North;
            case 1:
                return Edge.West;
            case 2:
                return Edge.East;
            case 3:
                return Edge.South;
            default:
                Debug.LogError("Random int Generator failed, using default value");
                return Edge.West;
        }

    }

    public static Edge EndpointEdge(Edge start, bool turnLeft)
    {
        switch (start)
        {
            case Edge.North:
                if (turnLeft)
                    return Edge.East;
                return Edge.West;

            case Edge.East:
                if (turnLeft)
                    return Edge.South;
                return Edge.North;

            case Edge.South:
                if (turnLeft)
                    return Edge.West;
                return Edge.East;

            case Edge.West:
                if (turnLeft)
                    return Edge.North;
                return Edge.South;

            default:
                throw new ArgumentOutOfRangeException("start", start, null);
        }
    }


}
