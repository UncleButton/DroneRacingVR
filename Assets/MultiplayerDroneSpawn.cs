using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MultiplayerDroneSpawn : NetworkBehaviour
{
    public Transform cameraTransform;
    public GameObject helmetGUI;
    public void Start()
    {
        if (!IsLocalPlayer)
        {
            cameraTransform.GetComponent<AudioListener>().enabled = false;
            cameraTransform.GetComponent<Camera>().enabled = false;
            this.transform.tag = "EnemyPlayer";
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(helmetGUI);
            Destroy(this.GetComponent<FlightController>());
        }

        Transform spawnLocation = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerScript>().getSpawnLocation();
        SpawnClientRPC(spawnLocation.position, spawnLocation.rotation);
    }

    [ClientRpc]
    void SpawnClientRPC(Vector3 position, Quaternion rotation)
    {
        this.gameObject.SetActive(false);
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.gameObject.SetActive(true);
    }
}
