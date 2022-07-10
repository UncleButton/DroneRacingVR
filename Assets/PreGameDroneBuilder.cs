using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PreGameDroneBuilder : NetworkBehaviour
{
    private bool isNotNetworked;
    public NetworkVariable<string[]> droneData = new NetworkVariable<string[]>();
    public GameObject parent;
    private DroneBuilderManager dbManager;
    private DroneBuilderScript dbScript;
    private GameObject drone;
    public void OnEnable()
    {
        droneData.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        droneData.Settings.ReadPermission = NetworkVariablePermission.Everyone;
        isNotNetworked = (GameObject.FindGameObjectWithTag("NetworkManager") == null);
        dbScript = ScriptableObject.CreateInstance<DroneBuilderScript>();
        dbManager = ScriptableObject.CreateInstance<DroneBuilderManager>();
        dbManager.dronePlacement = this.transform;
        if (isNotNetworked || IsOwner)
        {
            droneData.Value = dbScript.GetPresets().data.Values[0].toArray();
        }
        droneData.OnValueChanged += PresetChanged;
    }

    public void PresetChanged(string[] oldValue, string[] newValue)
    {
        BuildDrone();
    }

    public void BuildDrone()
    {
        drone = dbManager.VisualizePreset(new Preset(droneData.Value));
        drone.transform.SetParent(this.transform);
        drone.transform.position = this.transform.position;
        drone.transform.rotation = this.transform.rotation;
    }
}
