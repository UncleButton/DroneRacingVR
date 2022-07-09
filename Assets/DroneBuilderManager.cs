using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DroneBuilderManager : MonoBehaviour
{
    public Transform droneBuilderManager;
    private DroneBuilderScript dbScript;
    public Transform dronePlacement;

    public Preset currentPreset;
    public string currentName;
    public int currentIndex;

    public Text nameText;
    Preset defaultPreset = new Preset("Arcane", "BiProp", "#e1e1eb", "Shiny", "#000000", "Matte", "#e134eb", "Shiny", "#0ff000", "Matte");

    public void Start()
    {
        dbScript = ScriptableObject.CreateInstance<DroneBuilderScript>();
        

        currentIndex = 0;
        currentName = dbScript.getPresets().data.Keys[0];
        rebuildPresetAtIndex(currentIndex);        
    }

    public Preset rebuildPresetAtIndex(int index)
    {
        currentName = dbScript.getPresets().data.Keys[index];
        destroyAndVisualize(dbScript.getPresets().data.Values[index]);
        return currentPreset;
    }

    public Preset rebuildPresetByName(string name)
    {
        currentName = name;
        destroyAndVisualize(dbScript.getPresets().data[name]);
        return currentPreset;
    }

    public Preset destroyAndVisualize(Preset preset)
    {
        if (GameObject.FindGameObjectWithTag("BuiltDrone"))
            Destroy(GameObject.FindGameObjectWithTag("BuiltDrone"));

        visualizePreset(preset);

        return currentPreset;
    }


    public GameObject visualizePreset(Preset preset)
    {
        GameObject droneInProgress = new GameObject("Drone");
        droneInProgress.transform.tag = "BuiltDrone";

        /////////////////////////////////////////             DRONE BODY                  /////////////////////////////////////////////
        GameObject body = Instantiate(Resources.Load<GameObject>("DroneParts/Bodies/"+ preset.body + "/" + preset.body));//get body
        body.transform.SetParent(droneInProgress.transform);//set parent to droneInProgress

        /////////////////////////////////////////             Body Materials                  /////////////////////////////////////////////
        Material bodyMat1 = Instantiate(Resources.Load<Material>("DroneParts/Body Materials (Primary)/" + preset.bodyMat1 + "/" + preset.bodyMat1));//get color 1
        Material bodyMat2 = Instantiate(Resources.Load<Material>("DroneParts/Body Materials (Secondary)/" + preset.bodyMat2 + "/" + preset.bodyMat2));//get color 2
        Material[] bodyMats = { bodyMat1, bodyMat2 };
        body.GetComponent<MeshRenderer>().materials = bodyMats;

        setColor(bodyMat1, preset.bodyCol1);
        setColor(bodyMat2, preset.bodyCol2);


        /////////////////////////////////////////             Propeller Materials                  /////////////////////////////////////////////
        Material propMat1 = Instantiate(Resources.Load<Material>("DroneParts/Propeller Materials (Primary)/" + preset.propMat1 + "/" + preset.propMat1));//get color 1
        Material propMat2 = Instantiate(Resources.Load<Material>("DroneParts/Propeller Materials (Secondary)/" + preset.propMat2 + "/" + preset.propMat2));//get color 2

        setColor(propMat1, preset.propCol1);
        setColor(propMat2, preset.propCol2);

        Material[] propMats = { propMat1, propMat2 };
        foreach (Transform t in body.GetComponentsInChildren<Transform>())
        {
            if (t.CompareTag("PropPlaceholder"))
            {
                GameObject prop = Instantiate(Resources.Load<GameObject>("DroneParts/Propellers/" + preset.prop + "/" + preset.prop));//get prop
                prop.transform.SetParent(t);
                prop.transform.position = t.position;
                prop.GetComponent<MeshRenderer>().materials = propMats;
            }
        }

        droneInProgress.transform.position = dronePlacement.position;
        droneInProgress.transform.rotation = dronePlacement.rotation;

        if (currentName == null)
            currentIndex = 0;
        else
            currentIndex = dbScript.getPresets().data.IndexOfKey(currentName);


        currentPreset = preset;

        if (nameText!=null)
            nameText.text = currentName;
        
        return droneInProgress;
    }

    public void setColor(Material material, string color)
    {
        Color newCol;
        ColorUtility.TryParseHtmlString(color, out newCol);
        material.color = newCol;
    }

    public void savePreset()
    {
        dbScript.updatePresets(currentName, currentPreset);
    }

    public void nextPreset()
    {
        if (currentIndex < dbScript.getPresets().data.Count - 1)
            rebuildPresetAtIndex(++currentIndex);
        else
            rebuildPresetAtIndex(0);
    }
    public void previousPreset()
    {
        if (currentIndex > 0)
            rebuildPresetAtIndex(--currentIndex);
        else
            rebuildPresetAtIndex(dbScript.getPresets().data.Count-1);
    }

    public void newPreset()
    {
        dbScript.updatePresets("Drone " + (dbScript.getPresets().data.Count+1), defaultPreset);
        rebuildPresetAtIndex(dbScript.getPresets().data.Count - 1);
    }

}


