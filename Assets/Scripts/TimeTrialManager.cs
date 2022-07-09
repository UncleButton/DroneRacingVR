using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTrialManager : MonoBehaviour
{
    private TimeTrialScript timeTrialScript;
    private Transform drone;
    private List<GhostRiderData> droneData = new List<GhostRiderData>();
    private LapManager lapManager;
    private string courseName;

    private bool saved = false;

    private List<GhostRiderData> ghostRiderData;
    private int gdIndex = 0;
    public GameObject ghostRiderDrone;
    private GameObject ghostRider;
    private bool trialExists = false;

    private float timeValue = 0;
    private float timer = 0;
    public int recordFrequency;
    private int index1;
    private int index2;

    private void Start()
    {
        courseName = SceneManager.GetActiveScene().name.Replace(" Scene", "");
        lapManager = GameObject.FindGameObjectWithTag("LapManager").GetComponent<LapManager>();
        timeTrialScript = ScriptableObject.CreateInstance<TimeTrialScript>();
        drone = GameObject.FindGameObjectWithTag("Player").transform;
        if (timeTrialScript.trialExists(courseName))
        {
            trialExists = true;
            ghostRiderData = timeTrialScript.getTimes().getTimeTrial()[courseName].getData();
            ghostRider = Instantiate(ghostRiderDrone);
            ghostRider.transform.position = ghostRiderData[0].getPos();
            ghostRider.transform.rotation = ghostRiderData[0].getRot();
        }
    }

    private void FixedUpdate()
    {
        timeValue += Time.deltaTime;
        timer += Time.deltaTime;
        
        if (timer >= (1f/recordFrequency) && droneData.Count < recordFrequency*300)
        {
            droneData.Add(new GhostRiderData(drone.position, drone.rotation, timeValue));
            timer = 0;
        }

        if (trialExists)
        {
            GetIndex();
            SetTransform();
        }

        if (lapManager.courseComplete && saved == false)
        {
            saved = true;
            timeTrialScript.updateTimeTrialTimes(courseName, droneData);
        }

    }

    private void GetIndex()
    {
        for (int i = 0; i<ghostRiderData.Count-2; i++)
        {
            if(ghostRiderData[i].getTimeStamp() == timeValue)
            {
                index1 = i;
                index2 = i;
                return;
            }
            else if(ghostRiderData[i].getTimeStamp()<timeValue && timeValue < ghostRiderData[i + 1].getTimeStamp())
            {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = ghostRiderData.Count - 1;
        index2 = ghostRiderData.Count - 1;

    }

    private void SetTransform()
    {
        if (index1 == index2)
        {
            ghostRider.transform.position = ghostRiderData[index1].getPos();
            ghostRider.transform.rotation = ghostRiderData[index1].getRot();
        }
        else
        {
            float interpolationFactor = (timeValue - ghostRiderData[index1].getTimeStamp()) / (ghostRiderData[index2].getTimeStamp() - ghostRiderData[index1].getTimeStamp());

            ghostRider.transform.position = Vector3.Lerp(ghostRiderData[index1].getPos(), ghostRiderData[index2].getPos(), interpolationFactor);
            ghostRider.transform.rotation = Quaternion.Lerp(ghostRiderData[index1].getRot(), ghostRiderData[index2].getRot(), interpolationFactor);
        }
    }

}



