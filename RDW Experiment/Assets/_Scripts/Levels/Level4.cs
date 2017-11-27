using System;
using JetBrains.Annotations;
using UnityEngine;
// ReSharper disable SpecifyACultureInStringConversionExplicitly
// ReSharper disable RedundantNameQualifier

public class Level4 : MonoBehaviour
{

    public GameObject Player;
    public GameObject Room;
    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;
    public Transform PurpleFeetSpawn;

    private bool _turnLeft;
    private GameObject _paintingEast;
    private GameObject _paintingWest;
    private GameObject _discernmentEast;
    private GameObject _discernmentWest;
    private GameObject _arrowLeft;
    private GameObject _arrowRight;
    private GameObject fms;

    private AlgorithmType _algorithm;

    private int _turnCount;

    private bool _completed;

    [UsedImplicitly]
    // ReSharper disable once ArrangeTypeMemberModifiers
    void Start()
    {
        _completed = false;
        FindObjectOfType<Controller>().SetGain(0);
        AlgorithmManager.Complete += Completed;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        Manager.Experiment.GetAlgorithm(out _algorithm);
        Debug.Log(_algorithm);
        Manager.Algorithm.Initialize(_algorithm);

        bool isFinal;
        Manager.Experiment.WalkthroughStatus(out isFinal);

        if (isFinal)
        {
            Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
            FeetObject.OnCollision += Feet;
            Manager.Sound.PlaySpecificVoiceover(3); // move to purple footprints
            Manager.Sound.SetIndex(14);
        }
        else
        {
            Manager.Sound.SetIndex(13);
            SetupInitialCalibration();
            Manager.Sound.PlayNextVoiceover(); // Turn to face the arrow
            Manager.Sound.PlayNextVoiceover(2.3f); //The calibration will now begin
        }
        
        Pointer.Click += Touchpad;
        Manager.Experiment.GetCalibrationCompleted(out _completed);
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Feet()
    {
        Manager.Sound.PlayNextVoiceover(1.0f); //The calibration will now begin
        FeetObject.OnCollision -= Feet;
        SetupInitialCalibration();
    }

    private void SetupInitialCalibration()
    {
        _turnCount = 1;
        SetupFile();

        Manager.Spawn.ArrowButton(true, out _arrowLeft);
        Manager.Spawn.ArrowButton(false, out _arrowRight);

        Manager.Spawn.Painting(Direction.East, out _paintingEast);
        Manager.Spawn.DiscernmentButtons(Direction.East, out _discernmentEast);

        Manager.Spawn.Painting(Direction.West, out _paintingWest);
        Manager.Spawn.DiscernmentButtons(Direction.West, out _discernmentWest);

        _arrowLeft.transform.SetParent(NorthWall.transform);
        _arrowRight.transform.SetParent(NorthWall.transform);

        _paintingEast.transform.SetParent(EastWall.transform);
        _discernmentEast.transform.SetParent(EastWall.transform);

        _paintingWest.transform.SetParent(WestWall.transform);
        _discernmentWest.transform.SetParent(WestWall.transform);

        _arrowLeft.SetActive(_turnLeft);
        _arrowRight.SetActive(!_turnLeft);

        _paintingEast.SetActive(_turnLeft);
        _paintingWest.SetActive(!_turnLeft);

        _discernmentEast.SetActive(_turnLeft);
        _discernmentWest.SetActive(!_turnLeft);
        
    }

    private void SetupCalibration()
    {
        _turnLeft = !_turnLeft;
        
        _arrowLeft.SetActive(_turnLeft);
        _arrowRight.SetActive(!_turnLeft);
        _paintingEast.SetActive(_turnLeft);
        _paintingWest.SetActive(!_turnLeft);
        _discernmentEast.SetActive(_turnLeft);
        _discernmentWest.SetActive(!_turnLeft);

        Manager.Algorithm.Run(_algorithm);
        ++_turnCount;
        UpdateFile();
        RotateRoom(!_turnLeft);
    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                UpdateFMS();
                Pointer.Click -= Touchpad;
                FindObjectOfType<FMS>().Shutdown();
                if (!_completed)
                {
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Break1);
                }
                else
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Break2);

                break;
            case ObjectType.SameButton:
                Manager.Algorithm.Response = Feedback.Same;
                SetupCalibration();
                break;
            case ObjectType.DifferentButton:
                Manager.Algorithm.Response = Feedback.Different;
                SetupCalibration();
                break;
            case ObjectType.ContinueButton:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private void Completed()
    {
        AlgorithmManager.Complete -= Completed;
        _paintingEast.SetActive(false);
        _paintingWest.SetActive(false);
        _discernmentEast.SetActive(false);
        _discernmentWest.SetActive(false);
        _arrowLeft.SetActive(false);
        _arrowRight.SetActive(false);

        CompleteFile();
        SetupFMS();
        Manager.Sound.PlayNextVoiceover(); //Please submit your rating
    }

    private void SetupFile()
    {
        System.DateTime now = System.DateTime.Now;
        float gain;
        bool negative;
        Manager.Algorithm.GetData(out gain, out negative);
        string line = "CALIBRATION PHASE \n" +
                      "Algorithm: " + (_algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Negative Threshold: " + (negative ? "Yes" : "No") + "\n" +
                      "Turn direction: " + (_turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + _turnCount + "\n" +
                      "Current Gain: " + gain.ToString() + "\n";
        Manager.Experiment.WriteToFile(line);
    }

    private void UpdateFile()
    {
        System.DateTime now = System.DateTime.Now;
        float gain;
        bool negative;
        Manager.Algorithm.GetData(out gain, out negative);
        string line = "Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Previous response: " + (Manager.Algorithm.Response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "Negative Threshold: " + (negative ? "Yes" : "No") + "\n" +
                      "Turn direction: " + (_turnLeft ? "Left" : "Right") + "\n" +
                      "Turn count: " + _turnCount + "\n" +
                      "Current Gain: " + gain.ToString() + "\n";
        Manager.Experiment.WriteToFile(line);
    }
   
    private void CompleteFile()
    {
        System.DateTime now = System.DateTime.Now;
        float pos, neg;
        Manager.Experiment.GetThreshold(_algorithm, out pos, out neg);
        string line = "Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n" +
                      "Previous response: " + (Manager.Algorithm.Response == Feedback.Same ? "Same" : "Different") + "\n" +
                      "Total turn count: " + _turnCount + "\n" +
                      "Final gain, positive: " + pos.ToString() + "\n" +
                      "Final gain, negative: " + neg.ToString() + "\n\n";
        Manager.Experiment.WriteToFile(line);
    }

    public void RotateRoom(bool turn)
    {
        SteamVR_Fade.Start(Color.black, 1f, true);
        Vector3 axis = new Vector3(0, 1, 0);
        float angle = turn ? 90f : -90f;
        Room.transform.RotateAround(Vector3.zero, axis, angle);
        SteamVR_Fade.Start(Color.clear, 1.2f);
    }

    private void SetupFMS()
    {
        Manager.Spawn.MotionSicknessUI(out fms);
        FindObjectOfType<FMS>().Initialize();

        System.DateTime now = System.DateTime.Now;
        string line = "\nCALIBRATION PHASE\n" +
                      "Algorithm: " + (_algorithm == AlgorithmType.Staircase ? "Staircase" : "PEST") + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n\n";
        Manager.Experiment.WriteToFMS(line);
    }

    private void UpdateFMS()
    {
        float rating;
        FindObjectOfType<FMS>().GetRating(out rating);
        string line = "FMS: " + rating + "\n";
        Manager.Experiment.WriteToFMS(line);
    }

}
