using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualCheckpoint : MonoBehaviour
{
    public bool isTriggered = false;
    public Transform Ring;

    public Material triggeredMat;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isTriggered)//only local player has "player" tag, enemies have "EnemyPlayer"
        {
            isTriggered = true;
            Ring.GetComponent<MeshRenderer>().material = triggeredMat;
            GameObject.FindGameObjectWithTag("LapManager").GetComponent<LapManager>().AreAllCheckpointsTriggered();
        }
    }
}
