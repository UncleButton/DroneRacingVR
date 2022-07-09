using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTrialBoardScript : MonoBehaviour
{
    private GameObject timeTrialBoard;
    public GameObject ttPanel;
    public string coursesFolderPath;

    public Vector3 startPos;
    public Vector3 slideRightIncrement = new Vector3(1.2f, 0, 0);
    public Vector3 secondRow = new Vector3(0, 0.0f, 0);

    private TimeTrialScript timeTrialScript;
    private TimeTrials timeTrials;

    void Start()
    {
        timeTrialScript = ScriptableObject.CreateInstance<TimeTrialScript>();
        timeTrials = timeTrialScript.getTimes();

        timeTrialBoard = GameObject.FindGameObjectWithTag("TimeTrialBoard");
        startPos = timeTrialBoard.transform.position + new Vector3(0f, 0.4f, -2.2f);

        coursesFolderPath = Application.dataPath+"/Resources/Courses";

        string[] dirs = Directory.GetFiles(coursesFolderPath, "*.meta", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < dirs.Length; i++)
        {   
            GameObject newCourse = Instantiate(ttPanel);
            newCourse.transform.SetParent(timeTrialBoard.transform);
            newCourse.transform.position = startPos + slideRightIncrement * (i % 5) - secondRow*Mathf.Floor(i/5);
            
            string courseName = dirs[i].Replace(".meta", "").Replace(coursesFolderPath+"\\","");
            string thumbnail = "Courses/"+courseName+"/TTThumbnail";

            if (timeTrials.getTimeTrial().ContainsKey(courseName))
            {
                TimeSpan timerSpan = TimeSpan.FromSeconds(timeTrials.getTimeTrial()[courseName].getTime());
                newCourse.GetComponent<TTCoursePanelScript>().setBestTime(timerSpan.ToString(@"mm\:ss\:fff"));
            }
            else
                newCourse.GetComponent<TTCoursePanelScript>().setBestTime("Not attempted yet.");

            newCourse.GetComponent<TTCoursePanelScript>().setCourseName(courseName);
            newCourse.GetComponent<TTCoursePanelScript>().setThumbnail(thumbnail);
            newCourse.transform.rotation = timeTrialBoard.transform.rotation;

        }

    }
}
