using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class FlightController : NetworkBehaviour
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

    public bool tiltIsClamped = false;
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
        if (IsLocalPlayer)
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
        else
        {
            cameraTransform.GetComponent<AudioListener>().enabled = false;
            cameraTransform.GetComponent<Camera>().enabled = false;
            this.tag = "EnemyPlayer";
        }
        
        Vector3 positionToSpwan = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerScript>().getSpawnLocation().position;
        Quaternion rotationToSpawn = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerScript>().getSpawnLocation().rotation;
        SpawnClientRPC(positionToSpwan, rotationToSpawn);
    }

    [ClientRpc]
    void SpawnClientRPC(Vector3 position, Quaternion rotation)
    {
        this.gameObject.SetActive(false);
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        droneSound.pitch = 1f + liftSpinAxis.y/1.5f;
        if (IsLocalPlayer)
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


        posXThrust = 0f;
        negXThrust = 0f;
        posZThrust = 0f;
        negZThrust = 0f;

        //tilting left/right/forward/backward
        if (!tiltIsClamped)
        {
            if (tiltAxis.x > tiltDeadzone)
                negXThrust = tiltAxis.x * correctionThrust;
            if (tiltAxis.x < -tiltDeadzone)
                posXThrust = -tiltAxis.x * correctionThrust;
            if (tiltAxis.y > tiltDeadzone)
                negZThrust = tiltAxis.y * correctionThrust;
            if (tiltAxis.y < -tiltDeadzone)
                posZThrust = -tiltAxis.y * correctionThrust;

            masterThrust = hoverThrust + masterThrustCopy * liftSpinAxis.y;

            if (masterThrust < minimumThrust)
                masterThrust = minimumThrust;
        }
        else
        {
            if (Mathf.Abs(tiltAxis.y) > tiltDeadzone)
                flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(tiltMaxAngle * tiltAxis.y, flightDeckT.rotation.eulerAngles.y, flightDeckT.rotation.eulerAngles.z), Time.deltaTime * tiltLerpSpeed);

            if (Mathf.Abs(tiltAxis.x) > tiltDeadzone)
                flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y, -tiltMaxAngle * tiltAxis.x), Time.deltaTime * tiltLerpSpeed);

            //lift based on lift joystick
            if (liftSpinAxis.y >= liftDeadzone)
                masterThrust = hoverThrust + masterThrustCopy * liftSpinAxis.y;
            else
            {
                masterThrust = 0f;
                droneSound.pitch = 0.8f;
            }

        }



        //if not controlling, level out
        if (tiltAxis.x < tiltDeadzone && tiltAxis.x > -tiltDeadzone && tiltAxis.y < tiltDeadzone && tiltAxis.y > -tiltDeadzone)
        {
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(0, flightDeckT.rotation.eulerAngles.y, 0), Time.deltaTime * stabilizationSpeed);
        }

        //spinning
        if (Mathf.Abs(liftSpinAxis.x) > spinDeadzone)
            flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y + liftSpinAxis.x * spinMultiplier, flightDeckT.rotation.eulerAngles.z);



        //do thrust after all the above calculations
        thrusterPosXPosZThrust.force = (masterThrust + posXThrust + posZThrust) * flightDeckRB.mass;
        thrusterNegXNegZThrust.force = (masterThrust + negXThrust + negZThrust) * flightDeckRB.mass;
        thrusterPosXNegZThrust.force = (masterThrust + posXThrust + negZThrust) * flightDeckRB.mass;
        thrusterNegXPosZThrust.force = (masterThrust + negXThrust + posZThrust) * flightDeckRB.mass;

    }

}
