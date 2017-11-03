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
    private Feedback response;

    private GameObject path;
    private GameObject endpoint;
    private GameObject buttons;

    void Start()
    {
        Manager.Sound.SetIndex(11);
        FindObjectOfType<Controller>().SetGain(0);

        _startingEdge = LevelUtilities.ChooseRandomEdge();
        _turnLeft = LevelUtilities.GenerateRandomBool();
        Manager.Spawn.PurpleFeet(_startingEdge);
        FeetObject.OnCollision += Feet;
        Pointer.Click += Touchpad;
        
        useIndividualized = LevelUtilities.GenerateRandomBool();
        usePositive = LevelUtilities.GenerateRandomBool();
        Manager.Sound.PlayNextVoiceover(2.0f); //#13 position & follow path
        count = 0;
        totalCount = 0;
        Manager.Experiment.GetWalkthroughAlgorithm(out algorithm);
        Manager.Experiment.GetThreshold(algorithm, out positiveAlg, out negativeAlg);
        negativeAvg = -0.2f;
        positiveAvg = 0.4f;
        SetupFMS();
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        SetupInitialPath();
    }

    private void Endpoint()
    {
        ++count;
        ++totalCount;
        EndpointObject.OnCollision -= Endpoint;
        endpoint.SetActive(false);
        Manager.Spawn.MoveDiscernmentButtons(buttons, _endingEdge);
        buttons.SetActive(true);
        Manager.Sound.PlaySpecificVoiceover(12);
    }

    private void SetupInitialPath()
    {
        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        Manager.Spawn.Path(_turnLeft, _startingEdge, out path);
        Manager.Spawn.Endpoint(_endingEdge, out endpoint);

        Manager.Spawn.DiscernmentButtons(_endingEdge, out buttons);
        buttons.SetActive(false);
        EndpointObject.OnCollision += Endpoint;

        float gain = 0;
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
        Manager.Sound.PlaySpecificVoiceover(13);
        _turnLeft = !_turnLeft;

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

        float gain = 0;
        if (useIndividualized)
        {
            gain = usePositive ? positiveAlg : negativeAlg;
        }
        else
            gain = usePositive ? positiveAvg : negativeAvg;

        FindObjectOfType<Controller>().SetGain(gain);
        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        Manager.Spawn.MoveEndpoint(_endingEdge, endpoint);
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
                break;
            case ObjectType.SameButton:
                response = Feedback.Same;
                if (totalCount == 4 && !isFinal)
                {
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);

                }
                buttons.SetActive(false);
                SetupPath();
                break;
            case ObjectType.DifferentButton:
                response = Feedback.Different;
                if (totalCount == 4 && !isFinal)
                {
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                }
                buttons.SetActive(false);
                SetupPath();
                break;
            case ObjectType.ContinueButton:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private void SetupFMS()
    {
        System.DateTime now = System.DateTime.Now;
        string line = "\nWALKTHROUGH PHASE\n" +
                      "Algorithm: " + (algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n\n";
        Manager.Experiment.WriteToFMS(line);
    }

    private void UpdateFMS(int FMS)
    {
        float gain;
        FindObjectOfType<Controller>().GetGain(out gain);
        string line = "Individualized: " + (useIndividualized ? "Yes" : "No") + "\n" +
                      "Positive Threshold: " + (usePositive ? "Yes" : "No") + "\n" +
                      "Gain Applied: " + gain.ToString() + "\n" +
                      "Response: " + (response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "FMS: " + FMS + "\n";
        Manager.Experiment.WriteToFMS(line);
    }

}
