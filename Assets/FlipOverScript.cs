using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FlipOverScript : MonoBehaviour
{
    public GameObject drone;
    public XRNode flipButtonController;
    private bool xPressed;
    private bool flipping;

    public float timer;
    private float timerCopy;

    private FlightController flightController = null;
    private FlightControllerSinglePlayer flightControllerSinglePlayer = null;
    private float droneYSnapshot;

    private void Start()
    {
        timerCopy = timer;
        if (drone.GetComponent<FlightController>() != null)
            flightController = drone.GetComponent<FlightController>();
        else
            flightControllerSinglePlayer = drone.GetComponent<FlightControllerSinglePlayer>();
    }
    private void FixedUpdate()
    {
        InputDevice flipDevice = InputDevices.GetDeviceAtXRNode(flipButtonController);
        flipDevice.TryGetFeatureValue(CommonUsages.primaryButton, out xPressed);
        
        if ((transform.rotation.eulerAngles.x > 100 && transform.rotation.eulerAngles.x < 260) || (transform.rotation.eulerAngles.z > 100 && transform.rotation.eulerAngles.z < 260))
        {
            if (xPressed)
            {
                flipping = true;
                droneYSnapshot = drone.transform.position.y+2;
            }
        }
        if (flipping)
        {
            timer -= 1;
            if (flightController != null)
                flightController.enabled = false;
            if (flightControllerSinglePlayer != null)
                flightControllerSinglePlayer.enabled = false;

            drone.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if(timer<100)
                drone.transform.rotation = Quaternion.Lerp(drone.transform.rotation, Quaternion.Euler(0, drone.transform.rotation.eulerAngles.y, 0), Time.deltaTime * 10);
            else
                drone.transform.position = Vector3.Lerp(drone.transform.position, new Vector3(drone.transform.position.x, droneYSnapshot, drone.transform.position.z), Time.deltaTime * 3);




        }
        if (timer<0)
        {
            if (flightController != null)
                flightController.enabled = true;
            if (flightControllerSinglePlayer != null)
                flightControllerSinglePlayer.enabled = true;
            timer = timerCopy;
            flipping = false;
        }
    }
    
}
