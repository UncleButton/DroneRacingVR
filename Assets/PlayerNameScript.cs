using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameScript : MonoBehaviour
{
    private GameObject playerToLookAt;
    void Start()
    {
        playerToLookAt = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(playerToLookAt.transform.position,Vector3.up);
    }
}
