using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break2 : MonoBehaviour {

    void Start()
    {
        Pointer.Click += Touchpad;
        Manager.Sound.PlayBreakVoiceover();
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
                Debug.Log("Break 2 Complete");
                Manager.SceneSwitcher.LoadNextScene(SceneName.Five);
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }
    
}
