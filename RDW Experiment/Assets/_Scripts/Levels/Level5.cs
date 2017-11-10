using System;
using UnityEngine;

public class Level5 : MonoBehaviour
{
    private bool _turnLeft;
    private bool useIndividualized;
    private bool usePositive;
    private int count;
    private int totalCount;
    private Edge _startingEdge;
    private Edge _endingEdge;

    private AlgorithmType algorithm; //
    private float positiveAvg;
    private float negativeAvg;
    private float positiveAlg;
    private float negativeAlg;
    private float gain;
    private Feedback response;

    private GameObject path;
    private GameObject endpoint;
    private GameObject buttons;
    private GameObject fms;

    private bool playonce;

    void Start()
    {
        Manager.Sound.SetIndex(16);
        FindObjectOfType<Controller>().SetGain(0);

        _startingEdge = LevelUtilities.ChooseRandomEdge();
        _turnLeft = LevelUtilities.GenerateRandomBool();

        Manager.Spawn.PurpleFeet(_startingEdge);
        FeetObject.OnCollision += Feet;
        Pointer.Click += Touchpad;

        useIndividualized = LevelUtilities.GenerateRandomBool();
        usePositive = LevelUtilities.GenerateRandomBool();
        Manager.Sound.PlayNextVoiceover(2.0f); //Position yourself on the footprints then follow path
        count = 0;
        totalCount = 0;

        Manager.Experiment.GetWalkthroughAlgorithm(out algorithm);
        Manager.Experiment.GetThreshold(algorithm, out positiveAlg, out negativeAlg);

        negativeAvg = -0.2f;
        positiveAvg = 0.4f;
        gain = 0;
        SetupFMSFile();
        playonce = true;
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        SetupInitialPath();
    }

    private void Endpoint()
    {
        FindObjectOfType<Controller>().SetGain(0);
        gain = 0;
        ++count;
        ++totalCount;
        EndpointObject.OnCollision -= Endpoint;
        endpoint.SetActive(false);

        Manager.Spawn.MoveDiscernmentButtons(buttons, _endingEdge);
        buttons.SetActive(true);
        if (playonce)
        {
            Manager.Sound.PlaySpecificVoiceover(17); //If your virtual rotation matched
            playonce = false;
        }

    }

    private void SetupInitialPath()
    {
        SetupFile();
        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        Manager.Spawn.Path(_turnLeft, _startingEdge, out path);
        Manager.Spawn.Endpoint(_endingEdge, out endpoint);
        Manager.Spawn.DiscernmentButtons(_endingEdge, out buttons);
        Manager.Spawn.MotionSicknessUI(out fms);
        fms.SetActive(false);
        buttons.SetActive(false);
        EndpointObject.OnCollision += Endpoint;
        

        if (useIndividualized)
        {
            gain = usePositive ? positiveAlg : negativeAlg;
        }
        else
            gain = usePositive ? positiveAvg : negativeAvg;

        FindObjectOfType<Controller>().SetGain(gain);
    }

    private void SetupPath()
    {
        path.SetActive(true);
        Manager.Sound.PlaySpecificVoiceover(18); // Please turn to the center of the room
        Manager.Sound.PlaySpecificVoiceover(19, 2.2f); // Position yourself on the purple footprints
        Manager.Sound.PlaySpecificVoiceover(20, 3.2f); // Follow the path to the endpoint
        _turnLeft = !_turnLeft;
        _startingEdge = _endingEdge;

        if (count == 2)
        {
            useIndividualized = !useIndividualized;
            usePositive = !usePositive;
            count = 0;
        }
        else
        {
            usePositive = !usePositive;
        }

        if (useIndividualized)
        {
            gain = usePositive ? positiveAlg : negativeAlg;
        }
        else
            gain = usePositive ? positiveAvg : negativeAvg;

        Debug.Log(gain);
        FindObjectOfType<Controller>().SetGain(gain);

        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        Manager.Spawn.Endpoint(_endingEdge, out endpoint);
        endpoint.SetActive(true);
        EndpointObject.OnCollision += Endpoint;
    }

    private void Touchpad(ObjectType type)
    {
        bool isFinal;
        Manager.Experiment.WalkthroughStatus(out isFinal);
        switch (type)
        {
            case ObjectType.FMS:
                UpdateFMS();
                FindObjectOfType<FMS>().Shutdown();
                if (totalCount == 4 && !isFinal)
                {
                    Pointer.Click -= Touchpad;
                    Manager.Experiment.WalkthroughCompleted();
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                }
                if (totalCount == 4 & isFinal)
                {
                    Pointer.Click -= Touchpad;
                    Debug.Log("EXPERIMENT COMPLETE");
                    return;
                }
                fms.SetActive(false);
                SetupPath();
                break;
            case ObjectType.SameButton:
                response = Feedback.Same;
                UpdateFile();
                SetupFMS();
                break;
            case ObjectType.DifferentButton:
                response = Feedback.Different;
                SetupFMS();
                break;
            case ObjectType.ContinueButton:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private void SetupFMS()
    {
        path.SetActive(false);
        endpoint.SetActive(false);
        buttons.SetActive(false);

        fms.SetActive(true);
        FindObjectOfType<FMS>().Initialize();
        Manager.Sound.PlaySpecificVoiceover(21); //Please submit your rating
    }

    private void SetupFMSFile()
    {
        System.DateTime now = System.DateTime.Now;
        string line = "\nWALKTHROUGH PHASE\n" +
                      "Algorithm: " + (algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n\n";
        Manager.Experiment.WriteToFMS(line);
    }

    private void UpdateFMS()
    {
        float rating;
        FindObjectOfType<FMS>().GetRating(out rating);
        string line = "Individualized: " + (useIndividualized ? "Yes" : "No") + "\n" +
                      "Positive Threshold: " + (usePositive ? "Yes" : "No") + "\n" +
                      "Gain Applied: " + gain.ToString() + "\n" +
                      "Response: " + (response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "FMS: " + rating + "\n";
        Manager.Experiment.WriteToFMS(line);
    }

    private void SetupFile()
    {
        System.DateTime now = System.DateTime.Now;
        string line = "Walkthrough PHASE \n" +
                      "Algorithm: " + (algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Turn direction: " + (_turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + totalCount + "\n";
        Manager.Experiment.WriteToFile(line);
    }

    private void UpdateFile()
    {
        System.DateTime now = System.DateTime.Now;
        string line = "Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Response: " + (Manager.Algorithm.Response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "Gain applied: " + gain.ToString() + "\n" +
                      "Individualized gain used: " + (useIndividualized ? "Yes" : "No") + "\n" +
                      "Turn direction: " + (_turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + totalCount + "\n";
        Manager.Experiment.WriteToFile(line);
    }
}
