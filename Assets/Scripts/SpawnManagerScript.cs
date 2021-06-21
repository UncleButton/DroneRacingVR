using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{

    public Transform[] spawns;

    public Transform getSpawnLocation()
    {
        int numPlayers = GameObject.FindGameObjectsWithTag("EnemyPlayer").Length;
        return spawns[numPlayers];
    }

}
