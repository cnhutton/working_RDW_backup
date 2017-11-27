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
        Manager.Sound.SetIndex(3);
        _completed = false;

        _startingEdge = Edge.East;
        _turnLeft = true;

        Manager.Spawn.PurpleFeet(_startingEdge);
        FeetObject.OnCollision += Feet;
        Pointer.Click += Touchpad;
        
        Manager.Sound.PlayNextVoiceover(1.0f); //Please position yourself on the purple footprints
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        StartCoroutine(SetupPath(19.0f, true));
        Manager.Sound.PlayNextVoiceover(1.0f); // This experiment conerns redirected walking
    }

    private void Endpoint()
    {
        EndpointObject.OnCollision -= Endpoint;
        if (_completed)
        {
            Manager.Spawn.ContinueButton(_endingEdge, out button);
            Manager.Sound.PlayNextVoiceover(); //Select continue to proceed
        }
        else
        {
            FindObjectOfType<Controller>().SetGain(0);
            Manager.Sound.PlayNextVoiceover(); //Please turn to the center
            Manager.Sound.PlayNextVoiceover(2.3f); //Redirection will again be applied 
            _turnLeft = !_turnLeft;
            _startingEdge = _endingEdge;
            _completed = true;
            StartCoroutine(SetupPath(9.0f, false));
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
                Pointer.Click -= Touchpad;
                Manager.SceneSwitcher.LoadNextScene(SceneName.Three);
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private IEnumerator SetupPath(float delay, bool first)
    {
        _endingEdge = LevelUtilities.EndpointEdge(_startingEdge, _turnLeft);
        yield return new WaitForSeconds(1.0f);
        if (first)
        {
            Manager.Spawn.Path(_turnLeft, _startingEdge);
        }
        Manager.Spawn.Endpoint(_endingEdge);
        EndpointObject.OnCollision += Endpoint;
        StartCoroutine(SetGain(delay));
    }

    private IEnumerator SetGain(float delay)
    {
        yield return new WaitForSeconds (delay);
        FindObjectOfType<Controller>().SetGain(_completed ? 0.5f : -0.5f);
    }


}
