
using UnityEngine;

public class FeetObject : MonoBehaviour
{
    public delegate void Collided();

    public static event Collided OnCollision;

    void OnTriggerEnter(Collider collision)
    {
        if (OnCollision != null && collision.tag == "MainCamera")
        {
            Debug.LogWarning("Trigger " + collision.name);
            OnCollision();
            Destroy(gameObject.GetComponent<Collider>());
            DestroyObject();
        }
        
    }

    public void DestroyObject()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
