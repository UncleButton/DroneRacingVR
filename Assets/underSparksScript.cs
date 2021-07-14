using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class underSparksScript : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            // Visualize the contact point
            Debug.DrawRay(contact.point, contact.normal, Color.red);
        }
        
    }
}
