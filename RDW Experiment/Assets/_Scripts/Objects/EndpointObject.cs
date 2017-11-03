using UnityEngine;

public class EndpointObject : MonoBehaviour
{
    public delegate void Collided();

    public static event Collided OnCollision;

    void OnTriggerEnter(Collider collision)
    {
        if (OnCollision != null)
        {
            OnCollision();
            Destroy(gameObject.GetComponent<Collider>());
            DestroyObject();
        }

    }
    public void DestroyObject()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}