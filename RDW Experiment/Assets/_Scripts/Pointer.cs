using UnityEngine;
using Valve.VR;

public class Pointer : MonoBehaviour
{

    public GameObject CursorPrefab;
    private GameObject _cursor;
    private RaycastHit _hit;
    private bool _cursorVisible;
    private SteamVR_TrackedObject _trackedObj;

    public delegate void TouchpadHover(bool entered);
    public static event TouchpadHover Hover;

    public delegate void TouchpadClick(ObjectType type);
    public static event TouchpadClick Click;

    public delegate void TouchpadSlide(Vector3 position);
    public static event TouchpadSlide BeginSlide;
    public static event TouchpadSlide UpdateSlide;


    private bool hovering;
    private bool sliding;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
        _cursor = Instantiate(CursorPrefab);
        _cursor.SetActive(false);
        hovering = false;
    }

    void Update()
    {
        if (Controller != null)
        {
            if (sliding)
            {
                if (UpdateSlide != null)
                    UpdateSlide(_trackedObj.transform.position);
            }

            if (Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                if (Physics.Raycast(_trackedObj.transform.position, transform.forward, out _hit, 100))
                {
                    if (_hit.transform.GetComponent<FMS>())
                    {
                        sliding = true;
                        if (BeginSlide != null)
                            BeginSlide(_trackedObj.transform.position);
                    }
                    if (_hit.transform.GetComponent<ButtonObject>())
                    {
                        if (Click != null)
                        {
                            Click(_hit.transform.GetComponent<ButtonObject>().type);
                        }
                    }
                }
            }

            if (Controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                sliding = false;
                if (Hover != null)
                {
                    Hover(false);
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

            if (Physics.Raycast(_trackedObj.transform.position, transform.forward, out _hit, 100))
            {
                if (_hit.transform.GetComponent<FMS>())
                {
                    if (_cursor.activeSelf)
                    {
                        hovering = true;
                        if (Hover != null)
                        {
                            Hover(true);
                        }
                    }
                }
                else if (!sliding)
                {
                    hovering = false;
                    if (Hover != null)
                    {
                        Hover(false);
                    }
                }
            }


            if (sliding)
            {
                _cursorVisible = false;
                _cursor.SetActive(false);
            }

            if (_cursorVisible)
            {
                _cursor.transform.position = _hit.point;
                _cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, _hit.normal);
            }
        }
    }

}
