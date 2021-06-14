using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustScript : MonoBehaviour
{
    public Rigidbody thrustRB;
    public float force = 3f;

    void FixedUpdate()
    {
        thrustRB.AddForceAtPosition(this.transform.up * force, this.transform.position, ForceMode.Force);
    }
}
