using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointChecker : MonoBehaviour
{
    private GameObject[] allCheckpoints;
    public bool levelComplete = false;
    private void Start()
    {
        allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    public void areAllCheckpointsTriggered()
    {
        for(int i=0; i<allCheckpoints.Length; i++)
        {
            if (!allCheckpoints[i].GetComponent<IndividualCheckpoint>().isTriggered)
                return;
        }
        levelComplete = true;
    }

}
