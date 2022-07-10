using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerDroneSpawn : MonoBehaviour
{
    public void Start()
    {
        Transform spawnLocation = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerScript>().getSpawnLocation();
        this.gameObject.SetActive(false);
        this.transform.position = spawnLocation.position;
        this.transform.rotation = spawnLocation.rotation;
        this.gameObject.SetActive(true);
    }
}
