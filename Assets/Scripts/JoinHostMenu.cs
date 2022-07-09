using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.InputSystem;

public class JoinHostMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject menuVRRig;

    public void Update()
    {
        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["h"]).isPressed)
            Host();

        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["j"]).isPressed)
            Join();
    }

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
