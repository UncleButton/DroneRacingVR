using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    public bool settingsOpen = false;

    public GameObject gameVRRig;
    public GameObject settingsVRRig;
    public GameObject drone;

    private FlightController flightController = null;

    public GameObject tracers;

    private void Start()
    {
        flightController = drone.GetComponent<FlightController>();
        tracers = GameObject.FindGameObjectWithTag("Tracers");
    }

    public void SetInactive()
    {
        Time.timeScale = 1;
        flightController.enabled = true;
        settingsVRRig.SetActive(false);
        gameVRRig.SetActive(true);
    }

    public void SetActive()
    {
        Time.timeScale = 0;
        gameVRRig.SetActive(false);
        settingsVRRig.SetActive(true);
        flightController.enabled = false;
    }

    public void LoadGarage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Garage");
    }

    public void ToggleTracers()
    {
        tracers.SetActive(!tracers.activeSelf);
    }

    public void ToggleElevationStabilization()
    {
        flightController.elevationStabilization = !flightController.elevationStabilization;
    }
}
