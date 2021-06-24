using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SettingsPanel : MonoBehaviour
{
    public bool settingsOpen = false;

    public GameObject gameVRRig;
    public GameObject settingsVRRig;
    public GameObject drone;

    private FlightController flightController = null;
    private FlightControllerSinglePlayer flightControllerSinglePlayer = null;

    private void Start()
    {
        if (drone.GetComponent<FlightController>() != null)
            flightController = drone.GetComponent<FlightController>();
        else
            flightControllerSinglePlayer = drone.GetComponent<FlightControllerSinglePlayer>();
    }

    void Update()
    {
        if (settingsOpen)
        {
            gameVRRig.SetActive(false);
            settingsVRRig.SetActive(true);
            if (flightController != null)
                flightController.enabled = false;
            else
                flightControllerSinglePlayer.enabled = false;
        }
        else
        {
            if (flightController != null)
                flightController.enabled = true;
            else
                flightControllerSinglePlayer.enabled = true;
            settingsVRRig.SetActive(false);
            gameVRRig.SetActive(true);
        }

    }
}
