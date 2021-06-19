using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualCheckpoint : MonoBehaviour
{
    public bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
        GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointChecker>().areAllCheckpointsTriggered();
    }
}
