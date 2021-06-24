using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdateDroneScript : MonoBehaviour
{
    public GameObject drone;
    public Text spinText;

    private FlightController flightController = null;
    private FlightControllerSinglePlayer flightControllerSinglePlayer = null;

    private void Start()
    {
        if (drone.GetComponent<FlightController>() != null)
            flightController = drone.GetComponent<FlightController>();
        else
            flightControllerSinglePlayer = drone.GetComponent<FlightControllerSinglePlayer>();
    }

    public void UpdateMultiplier()
    {
        if (flightController != null)
            flightController.spinMultiplier = GetComponent<Slider>().value;
        else
            flightControllerSinglePlayer.spinMultiplier = GetComponent<Slider>().value;

        spinText.text = GetComponent<Slider>().value.ToString("F1");
    }

}
