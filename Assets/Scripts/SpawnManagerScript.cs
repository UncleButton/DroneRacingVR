using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : NetworkBehaviour
{
    public Transform[] spawns;

    public Transform getSpawnLocation()
    {
        return spawns[GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().ConnectedClientsList.Count];

    }

}
