using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PreGameDroneBuilder : NetworkBehaviour
{
    private bool isNotNetworked;
    public Preset droneData;
    public GameObject parent;
    private DroneBuilderManager dbManager;
    private DroneBuilderScript dbScript;
    private GameObject drone;

    public void Start()
    {
        isNotNetworked = (GameObject.FindGameObjectWithTag("NetworkManager") == null);
        dbScript = ScriptableObject.CreateInstance<DroneBuilderScript>();
        dbManager = new DroneBuilderManager();
        dbManager.dronePlacement = this.transform;
        if (IsLocalPlayer || isNotNetworked)
        {
            SetPreset(dbScript.getPresets().data.Values[0]);
        }
        else
        {
            Debug.Log("Attempting build");
            SetPreset(new Preset(parent.GetComponent<FlightController>().currentPreset.Value));
        }
    }

    public void SetPreset(Preset preset)
    {
        this.droneData = preset;
        BuildDrone();
    }

    public void BuildDrone()
    {
        drone = dbManager.visualizePreset(droneData);
        drone.transform.SetParent(this.transform);
        drone.transform.position = this.transform.position;
        drone.transform.rotation = this.transform.rotation;
    }
}
