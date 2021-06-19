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
        if(other.tag == "Player")//only local player has "player" tag, enemies have "EnemyPlayer"
        {
            isTriggered = true;
            Ring.GetComponent<MeshRenderer>().material = triggeredMat;
            GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointChecker>().areAllCheckpointsTriggered();
        }
        
    }
}
