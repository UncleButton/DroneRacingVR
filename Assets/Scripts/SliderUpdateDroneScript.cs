using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdateDroneScript : MonoBehaviour
{
    public GameObject drone;
    public Text spinText;

    private FlightController flightController = null;

    private void Start()
    {
        flightController = drone.GetComponent<FlightController>();
    }

    public void UpdateMultiplier()
    {
        flightController.spinMultiplier = GetComponent<Slider>().value;
        spinText.text = GetComponent<Slider>().value.ToString("F1");
    }

}
