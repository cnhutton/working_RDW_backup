using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMS : MonoBehaviour {

    public Transform StartPosition;
    public Transform EndPosition;
    public GameObject Slider;
    public UnityEngine.UI.Text label;


    private Vector3 startPoint;

    private float value;
    private float initialOffset;

    
    void Start()
    {
        Pointer.Hover += Hovering;
        Pointer.BeginSlide += StartSlide;
        Pointer.UpdateSlide += UpdateUI;

        value = 0;
    }

    private void Hovering(bool entered)
    {
        Slider.GetComponent<Renderer>().material.color = entered ? Color.red : Color.black;
    }

    private void StartSlide(Vector3 position)
    {
        startPoint = position;
        initialOffset = value;
    }

    private void UpdateUI(Vector3 position)
    {
        Vector3 displacement = position - startPoint;
        float change = Mathf.Floor(displacement.magnitude * 20f);
        if (displacement.x < 0)
        {
            change = change * -1;
        }
        value = initialOffset + change;

        if (value < 0)
            value = 0;
        if (value > 20)
            value = 20;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        label.text = value.ToString();
        if (value == 0)
        {
            Slider.transform.position = StartPosition.position;
            return;
        }
        else if (value == 20)
        {
            Slider.transform.position = EndPosition.position;
            return;
        }
        else
        {
            Vector3 newPosition = new Vector3(StartPosition.position.x + (value / 10), StartPosition.position.y, StartPosition.position.z);
            Slider.transform.position = newPosition;
        }
    }
    
    public void GetRating(out float rating)
    {
        rating = value;
    }
}
