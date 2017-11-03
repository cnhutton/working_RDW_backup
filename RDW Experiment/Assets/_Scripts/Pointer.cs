using UnityEngine;
using Valve.VR;

public class Pointer : MonoBehaviour
{

    public GameObject CursorPrefab;
    private GameObject _cursor;
    private RaycastHit _hit;
    private bool _cursorVisible;
    private SteamVR_TrackedObject _trackedObj;

    public delegate void TouchpadClick(ObjectType type);

    public static event TouchpadClick Click;

    private bool _FMS;
    private bool notChanged = true;

    private Vector2 lastValue;


    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
        _cursor = Instantiate(CursorPrefab);
        _cursor.SetActive(false);
        _FMS = false;
    }

    void Update()
    {
        if (Controller != null)
        {
            if (Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                if (Physics.Raycast(_trackedObj.transform.position, transform.forward, out _hit, 100))
                {
                    if (_hit.transform.GetComponent<ButtonObject>())
                    {
                        if (Click != null)
                        {
                            Click(_hit.transform.GetComponent<ButtonObject>().type);
                        }
                    }
                }
            }
            if (Controller.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                _cursor.SetActive(true);
                _cursorVisible = true;
            }

            if (Controller.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                _cursor.SetActive(false);
                _cursorVisible = false;
            }
            if (_trackedObj != null)
            {
                Physics.Raycast(_trackedObj.transform.position, transform.forward, out _hit, 100);

            }

            if (_cursorVisible)
            {
                _cursor.transform.position = _hit.point;
                _cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, _hit.normal);
            }
        }


    }





    private void FMS()
    {
        Debug.Log("REACHED");
        _FMS = true;
    }
}
