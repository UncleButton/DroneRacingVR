using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerRotate : MonoBehaviour
{
    public float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, -180*Time.deltaTime*rotateSpeed*2);
    }
}
