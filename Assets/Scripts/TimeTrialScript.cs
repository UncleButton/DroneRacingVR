﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TimeTrialScript : ScriptableObject
{
    public bool trialExists(string courseName)
    {
        TimeTrials timeTrials = getTimes();
        if (timeTrials.Equals(new TimeTrials()))
            return false;

        return timeTrials.getTimeTrial().ContainsKey(courseName);
    }

    private void setTimes(TimeTrials times)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/CourseTimes.dat");
        TimeTrials data = new TimeTrials(times);
        bf.Serialize(file, data);
        file.Close();
    }

    public TimeTrials getTimes()
    {
        if (File.Exists(Application.persistentDataPath + "/CourseTimes.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CourseTimes.dat", FileMode.Open);
            TimeTrials data = (TimeTrials)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
            return new TimeTrials();
    }

    public void updateTimeTrialTimes(string courseName, List<GhostRiderData> ghostRiderData)
    {
        TimeTrials currentBest = getTimes();
        float timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerScript>().timer;
        if (currentBest.getTimeTrial().ContainsKey(courseName))
        {
            if (timer < currentBest.getTimeTrial()[courseName].getTime())
            {
                currentBest.getTimeTrial()[courseName].setTime(timer);//time is beat, replace saved time with new time
                currentBest.getTimeTrial()[courseName].setData(ghostRiderData);
            }
        }
        else
            currentBest.getTimeTrial().Add(courseName, new TimeTrialData(timer, ghostRiderData));//first attempt.  add course to time trial mapping

        setTimes(currentBest);//save times
    }
}

[Serializable]
public class TimeTrials
{
    private SortedDictionary<string, TimeTrialData> data;

    public TimeTrials(TimeTrials copy)
    {
        data = copy.data;
    }

    public TimeTrials(SortedDictionary<string, TimeTrialData> copy)
    {
        data = copy;
    }

    public TimeTrials()
    {
        data = new SortedDictionary<string, TimeTrialData>();
    }

    public SortedDictionary<string, TimeTrialData>  getTimeTrial()
    {
        return this.data;
    }
}

[Serializable]
public class GhostRiderData
{

    private SerializableVector3 position;
    private SerializableQuaternion rotation;
    private float timeStamp;

    public GhostRiderData(Vector3 pos, Quaternion rot, float timeStamp)
    {
        this.position = pos;
        this.rotation = rot;
        this.timeStamp = timeStamp;
    }

    public Vector3 getPos()
    {
        return this.position;
    }

    public Quaternion getRot()
    {
        return this.rotation;
    }

    public float getTimeStamp()
    {
        return this.timeStamp;
    }

}

[Serializable]
public class TimeTrialData
{
    private float time;
    private List<GhostRiderData> data;

    public TimeTrialData(float time, List<GhostRiderData> data)
    {
        this.time = time;
        this.data = data;
    }

    public float getTime()
    {
        return this.time;
    }

    public void setTime(float newTime)
    {
        this.time = newTime;
    }

    public List<GhostRiderData> getData()
    {
        return this.data;
    }

    public void setData(List<GhostRiderData> newData)
    {
        this.data = newData;
    }

}

[Serializable]
public struct SerializableVector3
{
    public float x;

    public float y;

    public float z;

    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

[Serializable]
public struct SerializableQuaternion
{
    public float x;

    public float y;

    public float z;

    public float w;

    public SerializableQuaternion(float rX, float rY, float rZ, float rW)
    {
        x = rX;
        y = rY;
        z = rZ;
        w = rW;
    }

    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
    }

    public static implicit operator Quaternion(SerializableQuaternion rValue)
    {
        return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }

    public static implicit operator SerializableQuaternion(Quaternion rValue)
    {
        return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }
}
