using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public  ObjectType type;

    public void Level3Setup()
    {
       // Level3.reset += DestroyObject;
    }

    public void Level4Setup()
    {
        //Level4.reset += DestroyObject;
    }

    public void DestroyObject()
    {
        //Destroy(gameObject.GetComponent<Collider>());
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
