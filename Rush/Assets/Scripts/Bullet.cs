using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool destroyOnImpact, marker;
    public GameObject hitmarker;
    void OnCollisionEnter(Collision collision)
    {
        if(marker){
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            GameObject tempHit = Instantiate(hitmarker, position + contact.normal * 0.01f, rotation);
            tempHit.transform.SetParent(GameObject.Find("Destructables").transform);
        }
        if (destroyOnImpact){
            Destroy(gameObject);
        }
    }
}
