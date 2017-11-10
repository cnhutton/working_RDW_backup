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
    private GameObject fms;

    private bool _turnLeft;
    private bool _trainingComplete;
    

    void Start()
    {
        Manager.Sound.SetIndex(8);
        FindObjectOfType<Controller>().SetGain(0);
        Manager.Spawn.PurpleFeet(PurpleFeetSpawn.position);
        FeetObject.OnCollision += Feet;
        _turnLeft = LevelUtilities.GenerateRandomBool();
        _trainingComplete = false;
        Manager.Sound.PlayNextVoiceover(1.0f); //Position on purple
    }

    private void Feet()
    {
        FeetObject.OnCollision -= Feet;
        Pointer.Click += Touchpad;
        Manager.Sound.PlayNextVoiceover(); //To determine how much redirection to apply
        SetupInitialCalibration();
        Manager.Sound.PlayNextVoiceover(5f); //After turning, make selection
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
            Manager.Sound.PlayNextVoiceover(1.0f); //#Practice until comfortable then continue
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
                UpdateFMS();
                Pointer.Click -= Touchpad;
                FindObjectOfType<FMS>().Shutdown();
                Manager.SceneSwitcher.LoadNextScene(SceneName.Four);
                break;
            case ObjectType.SameButton:
                SetupCalibration();
                break;
            case ObjectType.DifferentButton:
                SetupCalibration();
                break;
            case ObjectType.ContinueButton:
                SetupFMS();
                Manager.Sound.PlayNextVoiceover(); //Please submit your rating
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

    private void SetupFMS()
    {
        _paintingEast.SetActive(false);
        _paintingWest.SetActive(false);
        _arrowLeft.SetActive(false);
        _arrowRight.SetActive(false);
        _discernmentEast.SetActive(false);
        _discernmentWest.SetActive(false);
        _proceedEast.SetActive(false);
        _proceedWest.SetActive(false);

        Manager.Spawn.MotionSicknessUI(out fms);
        FindObjectOfType<FMS>().Initialize();

        System.DateTime now = System.DateTime.Now;
        string line = "\nEND OF TRAINING\n" +
                      "Start Time: " + now.Hour.ToString() + ":" + now.Minute.ToString() + "\n";
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


