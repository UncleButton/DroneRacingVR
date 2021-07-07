using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FlightControllerSinglePlayer : MonoBehaviour
{
    public XRNode tiltInputController;
    private Vector2 tiltAxis;

    public XRNode liftSpinInputController;
    private Vector2 liftSpinAxis;
    public bool settingsOn;

    public GameObject settingsMenu;

    public GameObject flightDeck;
    public Transform flightDeckT;
    private Rigidbody flightDeckRB;
    public Transform cameraTransform;

    public AudioSource droneSound;

    public GameObject thrusterPosXNegZ;
    public GameObject thrusterNegXNegZ;
    public GameObject thrusterPosXPosZ;
    public GameObject thrusterNegXPosZ;

    private ThrustScript thrusterPosXPosZThrust;
    private ThrustScript thrusterNegXNegZThrust;
    private ThrustScript thrusterPosXNegZThrust;
    private ThrustScript thrusterNegXPosZThrust;

    public float masterThrust = 2.2f;
    private float masterThrustCopy;
    public float correctionThrust = 0.2f;
    public float minimumThrust = 2.2f;
    public float stabilizationSpeed = 1f;
    public float spinMultiplier = 0.5f;
    public float hoverThrust = 9.81f / 4f;
    public float tiltLerpSpeed = 2f;
    public float tiltMaxAngle = 30f;
    public float tiltDeadzone = 0.1f;
    public float spinDeadzone = 0.1f;
    public float liftDeadzone = 0.1f;

    public float posXThrust = 0f;
    public float negXThrust = 0f;
    public float posZThrust = 0f;
    public float negZThrust = 0f;

    // Start is called before the first frame update
    public void Start()
    {
        
        thrusterPosXPosZThrust = thrusterPosXPosZ.GetComponent<ThrustScript>();
        thrusterNegXNegZThrust = thrusterNegXNegZ.GetComponent<ThrustScript>();
        thrusterPosXNegZThrust = thrusterPosXNegZ.GetComponent<ThrustScript>();
        thrusterNegXPosZThrust = thrusterNegXPosZ.GetComponent<ThrustScript>();
        masterThrustCopy = masterThrust;

        flightDeckRB = flightDeck.GetComponent<Rigidbody>();
        flightDeckT = flightDeck.transform;
        this.tag = "Player";
    }

   

    private void FixedUpdate()
    {
        droneSound.pitch = 1f + liftSpinAxis.y / 1.5f;
        DroneMovement();
    }

    void DroneMovement()
    {
        InputDevice tiltDevice = InputDevices.GetDeviceAtXRNode(tiltInputController);
        tiltDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out tiltAxis);

        InputDevice liftSpinDevice = InputDevices.GetDeviceAtXRNode(liftSpinInputController);
        liftSpinDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out liftSpinAxis);
        liftSpinDevice.TryGetFeatureValue(CommonUsages.menuButton, out settingsOn);

        if (settingsOn)
            settingsMenu.GetComponent<SettingsPanel>().settingsOpen = true;


        //tilting left/right/forward/backward
        if (Mathf.Abs(tiltAxis.y) > tiltDeadzone)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(tiltMaxAngle * tiltAxis.y, flightDeckT.rotation.eulerAngles.y, flightDeckT.rotation.eulerAngles.z), Time.deltaTime * tiltLerpSpeed);

        if (Mathf.Abs(tiltAxis.x) > tiltDeadzone)
        {
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y, -tiltMaxAngle * tiltAxis.x), Time.deltaTime * tiltLerpSpeed);
            //flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y + tiltAxis.x * spinMultiplier / 2, flightDeckT.rotation.eulerAngles.z);
        }
            

        //lift based on lift joystick
        if (liftSpinAxis.y >= liftDeadzone)
            masterThrust = hoverThrust + masterThrustCopy * liftSpinAxis.y;
        else
        {
            masterThrust = 0f;
            droneSound.pitch = 0.8f;
        }



        //if not controlling, level out
        if (tiltAxis.x < tiltDeadzone && tiltAxis.x > -tiltDeadzone && tiltAxis.y < tiltDeadzone && tiltAxis.y > -tiltDeadzone)
        {
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(0, flightDeckT.rotation.eulerAngles.y, 0), Time.deltaTime * stabilizationSpeed);
        }

        //spinning
        if (Mathf.Abs(liftSpinAxis.x) > spinDeadzone)
            flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y + liftSpinAxis.x * spinMultiplier/1, flightDeckT.rotation.eulerAngles.z);



        //do thrust after all the above calculations
        thrusterPosXPosZThrust.force = masterThrust * flightDeckRB.mass;
        thrusterNegXNegZThrust.force = masterThrust * flightDeckRB.mass;
        thrusterPosXNegZThrust.force = masterThrust * flightDeckRB.mass;
        thrusterNegXPosZThrust.force = masterThrust * flightDeckRB.mass;

    }
}
