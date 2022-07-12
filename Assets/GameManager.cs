using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public int minPlayersToStart;
    private int numPlayers = 1;

    public NetworkManager networkManager;
    

    public void OnEnable()
    {
        networkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if (IsServer)
        {
            Debug.Log("New player: "+obj);
            //ClientConnectedClientRpc(obj);
        }
    }

    [ClientRpc]
    private void ClientConnectedClientRpc(ulong obj)
    {
        
        //Debug.Log("Players: " + networkManager.ConnectedClientsList.Count);
        
    }

}
