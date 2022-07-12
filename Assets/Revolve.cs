using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour
{
    private SyncTime networkedTime;
    private TimerScript nonNetworkedTime;

    private bool networkTimerFound = false;
    private bool nonNetworkTimerFound = false;

    public float speed;

    void FixedUpdate()
    {
        if(GameObject.FindGameObjectWithTag("SyncTime") != null)
        {
            networkedTime = GameObject.FindGameObjectWithTag("SyncTime").GetComponent<SyncTime>();
            networkTimerFound = true;
        }
        else if (GameObject.FindGameObjectWithTag("Timer") != null)
        {
            nonNetworkedTime = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerScript>();
            nonNetworkTimerFound = true;
        }

        if (networkTimerFound)
            this.transform.rotation = Quaternion.Euler(0, 0, speed * networkedTime.clientTime);
        else if(nonNetworkTimerFound)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, speed * nonNetworkedTime.timer);
        }
    }
}
