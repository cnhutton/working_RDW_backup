using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AlgorithmManager : MonoBehaviour
{
    public Feedback Response;
    public delegate void AlgorithmComplete();
    public static event AlgorithmComplete Complete;

    private bool _negativeTest;
    private float _currentGain;

    private float _positiveThreshold;

    //PEST variables
    private static int _numLevels = 20;
    private int _stimulusLevel;
    private float _maxStimulusRange;
    private float _std;
    private int _count;
    private int _maxCount;
    private List<float> gains = new List<float>();
    private List<Feedback> feedback = new List<Feedback>();

    //Staircase specific variables
    private float _stepSize;
    public static uint MaxReversals;           //Keep this value at 6 or 8. must be an even number for balance.
    public static uint ReversalNum;
    public static float Zn;
    public static List<float> ReversalList = new List<float>();


    float[] _prob = new float[_numLevels * 2 + 1];    //cumulative probability that threshold is at each possible value of independent variable based on user responses
    float[] _plgit = new float[_numLevels * 2 + 1];   //probability of a positive response
    float[] _mlgit = new float[_numLevels * 2 + 1];   //probability of a negative response

    //Initialize all variables to correct values
    public void Initialize(AlgorithmType algorithm)
    {
        _currentGain = _negativeTest ? -0.4f : 0.79f;

        switch (algorithm)
        {
            case AlgorithmType.Staircase:
                Zn = _negativeTest ? 0.0f : 1.0f;
                _stepSize = 0.1f;
                MaxReversals = 6;
                ReversalNum = 0;
                ReversalList.Clear();
                break;

            case AlgorithmType.PEST:
                _count = 0;
                _maxCount = 9;
                _maxStimulusRange = _currentGain * 2;    //Max gain value to try
                _std = _numLevels / 5;                //Reasonable estimation of slope (can be adjusted)

                //Initialize prob of positive response and negative responses from the psychometric function
                for (int i = 0; i < _numLevels * 2; ++i)
                {
                    _prob[i] = 0;
                    float lgit = 1 / (1 + Mathf.Exp((_numLevels - i) / _std));

                    _plgit[i] = lgit;
                    _mlgit[i] = 1 - lgit;
                }

                _stimulusLevel = 20;
                Response = Feedback.Different;

                PESTSubroutine();                   //Initialize probability array for the highest stim value

                _stimulusLevel = 1;
                Response = Feedback.Same;
                break;
            default:
                throw new ArgumentOutOfRangeException("algorithm", algorithm, null);
        }
        Manager.Algorithm.Run(algorithm);
    }

    public void Run(AlgorithmType algorithm)
    {
        switch (algorithm)
        {
            case AlgorithmType.Staircase:
                Staircase();
                FindObjectOfType<Controller>().SetGain(_currentGain);
                break;
            case AlgorithmType.PEST:
                gains.Add(_currentGain);
                feedback.Add(Response);
                PEST();
                FindObjectOfType<Controller>().SetGain(_currentGain);
                ++_count;
                break;
            default:
                throw new ArgumentOutOfRangeException("algorithm", algorithm, null);
        }
    }

    private void PESTSubroutine()
    {
        float max = float.MinValue; //Value of max probability
        float p1 = 0f, p2 = 0f;     //Indices of max probability (can be 2 if they are the same)

        //iterate through posible values and adjust cumulative prob based on response
        for (int i = 1; i < _numLevels; ++i)
        {
            //update probabilities based on response
            switch (Response)
            {
                case Feedback.Different:
                    _prob[i] += _plgit[_numLevels + _stimulusLevel - i];
                    break;
                case Feedback.Same:
                    _prob[i] += _mlgit[_numLevels + _stimulusLevel - i];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //Debug.Log(_prob[i]);

            //find maximum liklihood
            if (_prob[i] > max)
            {
                max = _prob[i];
                p1 = i;
            }
            else if (_prob[i] == max)
            { //when multiple indices have same max prob set p2
                p2 = i;
            }
            
        }
        //average in case there are multiple max probs
        _stimulusLevel = (int)Mathf.Floor((p1 + p2) / 2);
        
    }

    private void PEST()
    {
        PESTSubroutine();
        _currentGain = _stimulusLevel * (_maxStimulusRange / _numLevels);
        Debug.Log(_currentGain);
        //TERMINATION CRITERIA//
        float fullSum = 0;
        float sum = 0;

        for (int i = 0; i < _numLevels * 2; ++i)
        {
            fullSum += _prob[i];
        }

        //Get sum of probabilities 3 standard dev away
        for (int i = -3; i < 3; ++i)
        {
            if (_stimulusLevel - i >= 0)
            {
                sum += _prob[_stimulusLevel - i];
            }
        }

        if (Mathf.Log(fullSum) * 0.775f <= Mathf.Log(sum))
            CompletePest(false);

        if (_count >= _maxCount)
            CompletePest(true);

        ///if (Mathf.Log(fullSum) * 0.775f </= Mathf.Log(sum) || _count >= _maxCount)
        ///{
        ///    if (_negativeTest)
        ///    {
        ///        Debug.Log("Negative PEST complete");
        ///        Manager.Experiment.SetThresholdPEST(_positiveThreshold, _currentGain);
        ///        if (Complete != null)
        ///        {
        ///            Complete();
        ///        }
        ///        _negativeTest = false;
        ///    }
        ///    else
        ///    {
        ///        Debug.Log("Positive PEST complete");
        ///        _positiveThreshold = _currentGain;
        ///        _negativeTest = true;
        ///        Initialize(AlgorithmType.PEST);
        ///    }
        ///}
    }

    private void CompletePest(bool countExceeded)
    {
        if (countExceeded)
        {
            if (_negativeTest)
            {
                Debug.Log("Negative PEST complete, maxCount exceeded");
                Debug.Log(Response);
                Manager.Experiment.SetThresholdPEST(_positiveThreshold, _currentGain);
                if (Complete != null)
                {
                    Complete();
                }
                _negativeTest = false;
            }
            else
            {
                Debug.Log("Positive PEST complete, maxCount exceeded");
                Debug.Log(Response);
                _positiveThreshold = _currentGain;
                _negativeTest = true;
                Initialize(AlgorithmType.PEST);
            }
        }
        else
        {
            if (_negativeTest)
            {
                Debug.Log("Negative PEST complete");
                Manager.Experiment.SetThresholdPEST(_positiveThreshold, _currentGain);
                if (Complete != null)
                {
                    Complete();
                }
                _negativeTest = false;
            }
            else
            {
                Debug.Log("Positive PEST complete");
                _positiveThreshold = _currentGain;
                _negativeTest = true;
                Initialize(AlgorithmType.PEST);
            }
        }
    }

    /// <summary>
    /// Staircase Method
    /// The staircase adjusts the stimulus level according to the formula
    /// which steps at a fixed step size, and changes direction if the
    /// response changes.  It stops after a predetermined number of 
    /// reversals. The threshold estimate is the average of the reveral points.
    /// Stepping rule:  Xn+1 = Xn - d(2 * Zn - 1)
    /// Xn is the stimulus level at trial n,
    /// d is a fixed step size
    /// Zn is the response at trial n.
    /// Zn = 1 -> stimulus detected/correct
    /// Zn = 0 -> stimulus not detected
    /// </summary>
    private void Staircase()
    {
        float newZ;
        if (_negativeTest)
        {
            newZ = (Response == Feedback.Different) ? 0.0f : 1.0f;
        }
        else
        {
            newZ = (Response == Feedback.Same) ? 0.0f : 1.0f;
        }

        if (newZ != Zn)
        {
            ReversalList.Add(_currentGain);
            if (ReversalList.Count == MaxReversals)
            {
                if (_negativeTest)
                {
                    Debug.Log("Negative staircase complete");
                    if (Complete != null)
                    {
                        Complete();
                    }
                    _negativeTest = false;
                    Manager.Experiment.SetThresholdStaircase(_positiveThreshold, ReversalList.Average());
                    return;
                }
                else
                {
                    Debug.Log("Positive staircase complete");
                    _positiveThreshold = ReversalList.Average();
                    _negativeTest = true;
                    Initialize(AlgorithmType.Staircase);
                }
            }
        }

        Zn = newZ;
        _currentGain -= _stepSize * (2 * Zn - 1);
        Debug.Log(_currentGain);
    }

    public void GetData(out float currentGain, out bool negativeTest)
    {
        currentGain = _currentGain;
        negativeTest = _negativeTest;
    }
}
