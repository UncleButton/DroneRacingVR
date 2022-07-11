using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MLAPI;
using MLAPI.Messaging;

public class SyncTime : NetworkBehaviour
{
    public float clientTime = 0;
    public DateTime serverTime;
    private float latency;
    private bool invokedRepeating = false;

    private bool isSpawned = false;
    public void SyncMe()
    {
        if (!IsServer)
        {
            Debug.Log("Recalculating latency");
            returnTimeServerRPC();
        }
    }

    //ask server to send timestamp
    [ServerRpc]
    void returnTimeServerRPC()
    {
        returnTimeClientRPC(DateTime.Now);
    }

    //take server-sent timestamp and calculate latency
    [ClientRpc]
    void returnTimeClientRPC(DateTime serverTime){
        latency = (serverTime - DateTime.Now).Seconds + (serverTime - DateTime.Now).Milliseconds / 1000;
        clientTime = NetworkManager.NetworkTime + latency;
    }

    private void FixedUpdate()
    {
        //increase timer at same rate as the server
        clientTime += Time.deltaTime;

        if(invokedRepeating == false)
        {
            //resync time every 5 seconds
            InvokeRepeating("SyncMe", 0, 5);
            invokedRepeating = true;
        }
        
        if (IsServer)
        {
            //if on server machine, just set client timer to server timer and forget about latency
            clientTime = NetworkManager.NetworkTime;
        }
            
    }

}
