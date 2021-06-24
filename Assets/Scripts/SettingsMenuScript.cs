using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuScript : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject menuVRRig;
    public GameObject settingsMenu;

    public void ExitSettings()
    {
        settingsMenu.GetComponent<SettingsPanel>().settingsOpen = false;
    }
}
