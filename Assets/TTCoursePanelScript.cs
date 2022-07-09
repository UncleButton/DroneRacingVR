using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TTCoursePanelScript : MonoBehaviour
{
    public UnityEngine.UI.Text courseName;
    public UnityEngine.UI.Text bestTime;
    public GameObject imageOnPanel;
    private RawImage img;
    public void setThumbnail(string path)
    {
        Texture thumbnail = Resources.Load<Texture2D>(path);
        img = (RawImage)imageOnPanel.GetComponent<RawImage>();
        img.texture = (Texture)thumbnail;
    }

    public void setCourseName(string name)
    {
        courseName.text = "Course: " + name;
    }

    public void setBestTime(string time)
    {
        bestTime.text = time;
    }

    public void loadCourse()
    {
        SceneManager.LoadScene(courseName.text.Replace("Course: ",""));
    }

}
