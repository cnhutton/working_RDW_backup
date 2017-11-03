using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    private bool _turnLeft;

    private Edge _startingEdge;
    private Edge _endingEdge;

    private GameObject button;
    private bool _completed;
    
    void Start()
    {
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Sound.SetIndex(2);
        _completed = false;

        _startingEdge = LevelUtilities.ChooseRandomEdge();
        Manager.Spawn.PurpleFeet(_startingEdge);
        FeetObject.OnCollision += Feet;
        Pointer.Click += Touchpad;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        Manager.Sound.PlayNextVoiceover(2.0f);
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        StartCoroutine(SetupPath(0.0f));
        Manager.Sound.PlayNextVoiceover(1.0f); //voiceover #3
    }

    private void Endpoint()
    {
        EndpointObject.OnCollision -= Endpoint;
        if (_completed)
        {
            Manager.Spawn.ContinueButton(_endingEdge, out button);
            Manager.Sound.PlayNextVoiceover(); //voiceover #5
        }
        else
        {
            FindObjectOfType<Controller>().SetGain(0);
            Manager.Sound.PlayNextVoiceover(); //voiceover #4 turn to center
            _turnLeft = !_turnLeft;
            _startingEdge = _endingEdge;
            _completed = true;
            StartCoroutine(SetupPath(1.0f));
        }
    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                break;
            case ObjectType.SameButton:
                break;
            case ObjectType.DifferentButton:
                break;
            case ObjectType.ContinueButton:
                Manager.SceneSwitcher.LoadNextScene(SceneName.Three);
                Debug.LogError("Scene 2 continue reached");
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private IEnumerator SetupPath(float delay)
    {
        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        yield return new WaitForSeconds(delay);
        Manager.Spawn.Path(_turnLeft, _startingEdge);
        Manager.Spawn.Endpoint(_endingEdge);
        EndpointObject.OnCollision += Endpoint;
        StartCoroutine(SetGain(1.0f));
    }

    private IEnumerator SetGain(float delay)
    {
        yield return new WaitForSeconds (delay);
        FindObjectOfType<Controller>().SetGain(_completed ? 0.5f : -0.5f);
    }


}
