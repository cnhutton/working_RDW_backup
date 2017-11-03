using System;
using System.Collections;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public Transform PurpleFeetSpawn;
    public Transform ContinueSpawn;

    private GameObject _button;

    // ReSharper disable once UnusedMember.Local
    // ReSharper disable once ArrangeTypeMemberModifiers
    void Start()
    {
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
        FeetObject.OnCollision += Feet;
        Manager.Sound.PlayNextVoiceover(2.0f);
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        Pointer.Click += Touchpad;
        Manager.Sound.PlayNextVoiceover();
        StartCoroutine(WaitToSpawn());

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
                    Manager.SceneSwitcher.LoadNextScene(SceneName.Two);
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(10.0f);
        Manager.Spawn.ContinueButton(Direction.North, out _button);
    }
}
