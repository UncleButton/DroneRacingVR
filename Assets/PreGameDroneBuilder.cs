using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PreGameDroneBuilder : NetworkBehaviour
{
    private bool isNotNetworked;
    public NetworkVariable<Preset> droneData = new NetworkVariable<Preset>();
    public GameObject parent;
    private DroneBuilderManager dbManager;
    private DroneBuilderScript dbScript;
    private GameObject drone;

    public void Start()
    {
        droneData.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        isNotNetworked = (GameObject.FindGameObjectWithTag("NetworkManager") == null);
        dbScript = ScriptableObject.CreateInstance<DroneBuilderScript>();
        dbManager = new DroneBuilderManager();
        dbManager.dronePlacement = this.transform;
        if (IsLocalPlayer || isNotNetworked)
        {
            this.droneData.Value = dbScript.getPresets().data.Values[0];
        }
    }

    public void OnEnable()
    {
        droneData.OnValueChanged += PresetChanged;
    }

    public void PresetChanged(Preset oldValue, Preset newValue)
    {
        BuildDrone();
    }

    public void BuildDrone()
    {
        drone = dbManager.VisualizePreset(droneData.Value);
        drone.transform.SetParent(this.transform);
        drone.transform.position = this.transform.position;
        drone.transform.rotation = this.transform.rotation;
    }
}
