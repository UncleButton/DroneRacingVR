using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DroneBuilderScript : ScriptableObject
{

    private Preset defaultPreset = new Preset("Arcane", "BiProp", "#e134eb", "Matte", "#000000", "Glossy", "#e134eb", "Matte", "#000000", "Glossy");
    public bool PresetExists(string presetName)
    {
        Presets presets = GetPresets();
        if (presets.Equals(new Presets()))
            return false;

        return presets.data.ContainsKey(presetName);
    }

    private void SetPresets(Presets presets)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/DronePresets.dat");
        Presets data = new Presets(presets);
        bf.Serialize(file, data);
        file.Close();
    }

    public Presets GetPresets()
    {
        if (!File.Exists(Application.persistentDataPath + "/DronePresets.dat"))
        {
            SetPresets(new Presets("Drone 1", defaultPreset));
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/DronePresets.dat", FileMode.Open);
        Presets data = (Presets)bf.Deserialize(file);
        file.Close();
        return data;

    }

    public void UpdatePresets(string presetName, Preset presetData)
    {
        Presets currentPresets = GetPresets();

        if (currentPresets.data.ContainsKey(presetName))
        {
            currentPresets.data[presetName] = presetData;//updating existing preset
        }
        else
            currentPresets.data.Add(presetName, presetData);//new preset, add it

        SetPresets(currentPresets);//save presets
    }

    public void UpdateName(string oldName, string newName) {
        if (PresetExists(oldName))
        {
            Presets presets = GetPresets();
            presets.data.Add(newName, presets.data[oldName]);
            presets.data.Remove(oldName);
            SetPresets(presets);
        }
    }

}


[Serializable]
public class Presets
{
    public SortedList<string, Preset> data;

    public Presets()
    {
        data = new SortedList<string, Preset>();
    }
    public Presets(Presets presets)
    {
        this.data = presets.data;
    }
    public Presets(string name, Preset preset)
    {
        SortedList<string, Preset> presets = new SortedList<string, Preset>();
        presets.Add(name, preset);
        this.data = presets;
    }

}

[Serializable]
public class Preset
{
    public string body;
    public string prop;
    public string bodyCol1;
    public string bodyMat1;
    public string bodyCol2;
    public string bodyMat2;
    public string propCol1;
    public string propMat1;
    public string propCol2;
    public string propMat2;

    public Preset(string body, string prop, string bc1, string bm1, string bc2, string bm2, string pc1, string pm1, string pc2, string pm2)
    {
        this.body = body;
        this.prop = prop;
        this.bodyCol1 = bc1;
        this.bodyMat1 = bm1;
        this.bodyCol2 = bc2;
        this.bodyMat2 = bm2;
        this.propCol1 = pc1;
        this.propMat1 = pm1;
        this.propCol2 = pc2;
        this.propMat2 = pm2;
    }

    public Preset(string[] data)
    {
        this.body = data[0];
        this.prop = data[1];
        this.bodyCol1 = data[2];
        this.bodyMat1 = data[3];
        this.bodyCol2 = data[4];
        this.bodyMat2 = data[5];
        this.propCol1 = data[6];
        this.propMat1 = data[7];
        this.propCol2 = data[8];
        this.propMat2 = data[9];
    }

    public string[] toArray()
    {
        List<string> stringList = new List<string>();
        stringList.Add(body);
        stringList.Add(prop);
        stringList.Add(bodyCol1);
        stringList.Add(bodyMat1);
        stringList.Add(bodyCol2);
        stringList.Add(bodyMat2);
        stringList.Add(propCol1);
        stringList.Add(propMat1);
        stringList.Add(propCol2);
        stringList.Add(propMat2);

        return stringList.ToArray();
    }

}
