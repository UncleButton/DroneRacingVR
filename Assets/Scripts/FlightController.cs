using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class FlightController : MonoBehaviour
{
    public XRNode tiltInputController;
    private Vector2 tiltAxis;

    public XRNode liftSpinInputController;
    private float liftMultiplier;
    private Vector2 spinAxis;
    private bool restart;

    public GameObject flightDeck;
    private Transform flightDeckT;
    private Rigidbody flightDeckRB;

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

    private float posXThrust = 0f;
    private float negXThrust = 0f;
    private float posZThrust = 0f;
    private float negZThrust = 0f;


    // Start is called before the first frame update
    void Start()
    {
        thrusterPosXPosZThrust = thrusterPosXPosZ.GetComponent<ThrustScript>();
        thrusterNegXNegZThrust = thrusterNegXNegZ.GetComponent<ThrustScript>();
        thrusterPosXNegZThrust = thrusterPosXNegZ.GetComponent<ThrustScript>();
        thrusterNegXPosZThrust = thrusterNegXPosZ.GetComponent<ThrustScript>();
        masterThrustCopy = masterThrust;

        flightDeckRB = flightDeck.GetComponent<Rigidbody>();
        flightDeckT = flightDeck.transform;
    }

    private void FixedUpdate()
    {
        InputDevice tiltDevice = InputDevices.GetDeviceAtXRNode(tiltInputController);
        tiltDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out tiltAxis);

        InputDevice liftSpinDevice = InputDevices.GetDeviceAtXRNode(liftSpinInputController);
        liftSpinDevice.TryGetFeatureValue(CommonUsages.trigger, out liftMultiplier);
        liftSpinDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out spinAxis);
        liftSpinDevice.TryGetFeatureValue(CommonUsages.primaryButton, out restart);

        posXThrust = 0f;
        negXThrust = 0f;
        posZThrust = 0f;
        negZThrust = 0f;

        //tilting left/right/forward/backward
        if (tiltAxis.x > 0)
            negXThrust = tiltAxis.x * correctionThrust;
        if (tiltAxis.x < 0)
            posXThrust = -tiltAxis.x * correctionThrust;
        if (tiltAxis.y > 0)
            negZThrust = tiltAxis.y * correctionThrust;
        if (tiltAxis.y < 0)
            posZThrust = -tiltAxis.y * correctionThrust;

        //if not controlling, level out
        if (tiltAxis.x < 0.1 && tiltAxis.x > -0.1 && tiltAxis.y < 0.1 && tiltAxis.y > -0.1)
        {
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(0, flightDeckT.rotation.eulerAngles.y, 0), Time.deltaTime * stabilizationSpeed);
        }

        //spinning
        if (Mathf.Abs(spinAxis.x) > 0.1)
            flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y + spinAxis.x * spinMultiplier, flightDeckT.rotation.eulerAngles.z);

        //lift based on lift trigger
        masterThrust = liftMultiplier * masterThrustCopy;

        if (masterThrust < minimumThrust)
            masterThrust = minimumThrust;

        //do thrust after all the above calculations
        thrusterPosXPosZThrust.force = (masterThrust + posXThrust + posZThrust) * flightDeckRB.mass;
        thrusterNegXNegZThrust.force = (masterThrust + negXThrust + negZThrust) * flightDeckRB.mass;
        thrusterPosXNegZThrust.force = (masterThrust + posXThrust + negZThrust) * flightDeckRB.mass;
        thrusterNegXPosZThrust.force = (masterThrust + negXThrust + posZThrust) * flightDeckRB.mass;

        if (restart)
        {
            this.transform.position = new Vector3(0, 3, 0);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            SceneManager.LoadScene("SampleScene");
        }
    }

}
