using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LapManager : MonoBehaviour
{
    public GameObject[] allCheckpoints;
    public bool lapComplete = false;
    public bool courseComplete = false;
    public int lapNum = 1;
    public int lapsRequired = 0;
    public Material untriggeredMat;

    public void areAllCheckpointsTriggered()
    {
        allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            if (allCheckpoints[i].GetComponent<IndividualCheckpoint>().isTriggered == false)
            {
                Debug.Log(allCheckpoints[i].transform.parent.name);
                return;
            }
                
        }
        Debug.Log("Lap complete");
        lapComplete = true;
        if (lapComplete)
        {
            lapComplete = false;
            if (lapNum == lapsRequired)
                courseComplete = true;
            else
            {
                lapNum++;
                for (int i = 0; i < allCheckpoints.Length; i++)
                {
                    allCheckpoints[i].GetComponent<IndividualCheckpoint>().isTriggered = false;
                    allCheckpoints[i].transform.parent.GetChild(3).GetComponent<MeshRenderer>().material = untriggeredMat;
                }
            }
        }
    }
}
