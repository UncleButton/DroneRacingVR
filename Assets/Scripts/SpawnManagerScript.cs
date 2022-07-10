using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : NetworkBehaviour
{
    public Transform[] spawns;

    public Transform getSpawnLocation()
    {
        if (GameObject.FindGameObjectWithTag("NetworkManager") == null)
            return spawns[1];
        else
            return spawns[GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().ConnectedClientsList.Count];

    }

}
