using System;
using System.IO;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    private string _participantID;
    private bool _beginWithStaircase;
    private const string Location = "C:/Users/hutton/Desktop";
    private string _path;
    private string _FMS;

    private float _thresholdPESTPositive;
    private float _thresholdPESTNegative;
    private float _thresholdStaircasePositive;
    private float _thresholdStaircaseNegative;

    private bool _firstCalibrationCompleted;
    private bool _firstWalkthroughCompleted;
    private bool _finalWalkthrough;

    private int valueFMS;

    public void Initialize()
    {
        System.DateTime now = System.DateTime.Now;
        _participantID = now.Day.ToString() + now.Month.ToString() + now.Year.ToString() + now.Hour.ToString() +
                         now.Minute.ToString();
        //_beginWithStaircase = LevelUtilities.GenerateRandomBool();
        _beginWithStaircase = false;
        _firstCalibrationCompleted = false;
        _firstWalkthroughCompleted = false;
        CreateFilePath();
        SetupFiles();
    }

    public void GetAlgorithm(out AlgorithmType algorithm)
    {
        if (_firstCalibrationCompleted)
        {
            algorithm = _beginWithStaircase ? AlgorithmType.PEST : AlgorithmType.Staircase;
        }
        else
        {
            algorithm = _beginWithStaircase ? AlgorithmType.Staircase : AlgorithmType.PEST;
        }
    }

    public void GetWalkthroughAlgorithm(out AlgorithmType algorithm)
    {
        if (_firstWalkthroughCompleted)
        {
            algorithm = _beginWithStaircase ? AlgorithmType.PEST : AlgorithmType.Staircase;
        }
        else
        {
            algorithm = _beginWithStaircase ? AlgorithmType.Staircase : AlgorithmType.PEST;
            _firstWalkthroughCompleted = true;
        }
    }

    public void GetParticipantID(out string participantID)
    {
        participantID = _participantID;
    }

    public void WriteToFile(string info)
    {
        StreamWriter writer = new StreamWriter(_path, true);
        writer.WriteLine(info);
        writer.Close();
    }

    public void WriteToFMS(string info)
    {
        StreamWriter writer = new StreamWriter(_FMS, true);
        writer.WriteLine(info);
        writer.Close();
    }

    private void CreateFilePath()
    {
        _path = Location + _participantID + "_Results.txt";
        _FMS = Location + _participantID + "_FMS.txt";
    }

    public void SetThresholdPEST(float positive, float negative)
    {
        _thresholdPESTPositive = positive;
        _thresholdPESTNegative = negative;
        _firstCalibrationCompleted = true;
    }

    public void SetThresholdStaircase(float positive, float negative)
    {
        _thresholdStaircasePositive = positive;
        _thresholdStaircaseNegative = negative;
        _firstCalibrationCompleted = true;
    }
    
    public void GetThreshold(AlgorithmType algorithm, out float positive, out float negative)
    {
        switch (algorithm)
        {
            case AlgorithmType.Staircase:
                positive = _thresholdStaircasePositive;
                negative = _thresholdStaircaseNegative;
                break;
            case AlgorithmType.PEST:
                positive = _thresholdPESTPositive;
                negative = _thresholdPESTNegative;
                break;
            default:
                throw new ArgumentOutOfRangeException("algorithm", algorithm, null);
        }
    }

    private void SetupFiles()
    {
        System.DateTime now = System.DateTime.Now;
        string line = "Participant: " + _participantID + "\n" +
                      "Date: " + now.Day.ToString() + "/" + now.Month.ToString() + "/" + now.Year.ToString() + "\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n\n";
        WriteToFile(line);
        WriteToFMS(line);
    }

    private void ResponseFMS(int response)
    {
        valueFMS = response;
    }

    public void GetFMSResponse(out int response)
    {
        response = valueFMS;
    }

    public void WalkthroughCompleted()
    {
        _finalWalkthrough = true;
    }

    public void WalkthroughStatus(out bool isFinal)
    {
        isFinal = _finalWalkthrough;
    }
    
}
