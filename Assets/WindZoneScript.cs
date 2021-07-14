using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneScript : MonoBehaviour
{
    public Transform drone;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && other.attachedRigidbody!= null)
        {
            other.attachedRigidbody.AddForce((other.transform.position - drone.transform.position)*5 + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), Random.Range(-5f, 5f)));
        }
    }
}
