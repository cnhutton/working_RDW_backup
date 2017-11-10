using System;
using System.Collections;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public Transform PurpleFeetSpawn;
    public Transform ContinueSpawn;

    private GameObject _fms;

    // ReSharper disable once UnusedMember.Local
    // ReSharper disable once ArrangeTypeMemberModifiers
    void Start()
    {
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
        FeetObject.OnCollision += Feet;
        Manager.Sound.PlayNextVoiceover(2.0f); //Welcome to the VE 
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        Pointer.Click += Touchpad;
        Manager.Sound.PlayNextVoiceover(); //In your hand is your controller
        StartCoroutine(WaitToSpawn(17.5f));

    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                FindObjectOfType<FMS>().Shutdown();
                Pointer.Click -= Touchpad;
                Manager.SceneSwitcher.LoadNextScene(SceneName.Two);
                break;
            case ObjectType.SameButton:
                break;
            case ObjectType.DifferentButton:
                break;
            case ObjectType.ContinueButton:
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private IEnumerator WaitToSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        Manager.Sound.PlayNextVoiceover(); //At some points, you will see this interface
        Manager.Spawn.MotionSicknessUI(out _fms);
        FindObjectOfType<FMS>().Initialize();
    }
}
