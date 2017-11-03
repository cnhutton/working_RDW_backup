using System;
using UnityEngine;

public class Level3 : MonoBehaviour
{

    public Transform Player;
    public GameObject Room;
    public GameObject NorthWall;
    public GameObject EastWall;
    public GameObject SouthWall;
    public GameObject WestWall;
    public Transform PurpleFeetSpawn;

    private GameObject _paintingEast;
    private GameObject _paintingWest;
    private GameObject _discernmentEast;
    private GameObject _discernmentWest;
    private GameObject _arrowLeft;
    private GameObject _arrowRight;
    private GameObject _proceedEast;
    private GameObject _proceedWest;

    private bool _turnLeft;
    private bool _trainingComplete;
    

    void Start()
    {
        Manager.Sound.SetIndex(6);
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
        FeetObject.OnCollision += Feet;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        _trainingComplete = false;
        Manager.Sound.PlayNextVoiceover(1.0f); //#6 position purple
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        Pointer.Click += Touchpad;
        Manager.Sound.PlayNextVoiceover(); //#7 calibration info
        SetupInitialCalibration();
        Manager.Sound.PlayNextVoiceover(5.5f); //#8 after turning
    }

    private void SetupInitialCalibration()
    {
        Manager.Spawn.ArrowButton(true, out _arrowLeft);
        Manager.Spawn.ArrowButton(false, out _arrowRight);

        Manager.Spawn.Painting(Direction.East, out _paintingEast);
        Manager.Spawn.DiscernmentButtons(Direction.East, out _discernmentEast);
        Manager.Spawn.ProceedButton(Direction.East, out _proceedEast);

        Manager.Spawn.Painting(Direction.West, out _paintingWest);
        Manager.Spawn.DiscernmentButtons(Direction.West, out _discernmentWest);
        Manager.Spawn.ProceedButton(Direction.West, out _proceedWest);

        _arrowLeft.transform.SetParent(NorthWall.transform);
        _arrowRight.transform.SetParent(NorthWall.transform);

        _paintingEast.transform.SetParent(EastWall.transform);
        _discernmentEast.transform.SetParent(EastWall.transform);
        _proceedEast.transform.SetParent(EastWall.transform);

        _paintingWest.transform.SetParent(WestWall.transform);
        _discernmentWest.transform.SetParent(WestWall.transform);
        _proceedWest.transform.SetParent(WestWall.transform);

        _arrowLeft.SetActive(_turnLeft);
        _arrowRight.SetActive(!_turnLeft);

        _paintingEast.SetActive(_turnLeft);
        _paintingWest.SetActive(!_turnLeft);

        _discernmentEast.SetActive(_turnLeft);
        _discernmentWest.SetActive(!_turnLeft);

        _proceedEast.SetActive(false);
        _proceedWest.SetActive(false);

    }

    private void SetupCalibration()
    {
        _turnLeft = !_turnLeft;
        if (!_trainingComplete)
        {
            Manager.Sound.PlayNextVoiceover(1.0f); //#9 practice until comfortable then continue
            _trainingComplete = true;
        }

        _arrowLeft.SetActive(_turnLeft);
        _arrowRight.SetActive(!_turnLeft);
        _paintingEast.SetActive(_turnLeft);
        _paintingWest.SetActive(!_turnLeft);
        _discernmentEast.SetActive(_turnLeft);
        _discernmentWest.SetActive(!_turnLeft);
        _proceedEast.SetActive(_turnLeft);
        _proceedWest.SetActive(!_turnLeft);

        RotateRoom(!_turnLeft);
    }

    private void Touchpad(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.FMS:
                break;
            case ObjectType.SameButton:
                SetupCalibration();
                break;
            case ObjectType.DifferentButton:
                SetupCalibration();
                break;
            case ObjectType.ContinueButton:
                Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    public void RotateRoom(bool turn)
    {
        SteamVR_Fade.Start(Color.black, 1f, true);
        Vector3 axis = new Vector3(0, 1, 0);
        float angle = turn ? 90f : -90f;
        Room.transform.RotateAround(Vector3.zero, axis, angle);
        SteamVR_Fade.Start(Color.clear, 1.2f);
    }
}


