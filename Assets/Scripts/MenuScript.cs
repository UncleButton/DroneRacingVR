using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class MenuScript : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject menuVRRig;
    
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        menuPanel.SetActive(false);
        menuVRRig.SetActive(false);
}

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        menuPanel.SetActive(false);
        menuVRRig.SetActive(false);
    }

}
