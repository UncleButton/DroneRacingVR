using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.Serialization;

public class FlightController : NetworkBehaviour
{
    private bool isNotNetworked;
    public bool elevationStabilization = true;
    public XRNode tiltInputController;
    private Vector2 tiltAxis;

    public XRNode liftSpinInputController;
    private Vector2 liftSpinAxis;
    public bool settingsOn;

    public GameObject settingsMenu;

    public GameObject flightDeck;
    private Transform flightDeckT;
    private Rigidbody flightDeckRB;
    public Transform cameraTransform;

    public AudioSource droneSound;

    public Canvas playerName;

    public GameObject thruster;

    private ThrustScript thrusterThrust;

    public float masterThrust = 5f;
    private float masterThrustCopy;
    public float minimumThrust = 2.2f;
    public float stabilizationSpeed = 1f;
    public float spinMultiplier = 1.25f;
    public float hoverThrust = 9.81f / 2f;
    public float tiltLerpSpeed = 2f;
    public float tiltMaxAngle = 40f;
    public float tiltDeadzone = 0.1f;
    public float spinDeadzone = 0.25f;
    public float liftDeadzone = 0.1f;

    // Start is called before the first frame update
    public void Start()
    {
        isNotNetworked = (GameObject.FindGameObjectWithTag("NetworkManager") == null);
        if (!IsLocalPlayer && !isNotNetworked)
        {
            cameraTransform.GetComponent<AudioListener>().enabled = false;
            cameraTransform.GetComponent<Camera>().enabled = false;
            this.tag = "EnemyPlayer";
            Destroy(this);
        }

        DroneBuilderScript dbScript = ScriptableObject.CreateInstance<DroneBuilderScript>();

        thrusterThrust = thruster.GetComponent<ThrustScript>();
        masterThrustCopy = masterThrust;

        flightDeckRB = flightDeck.GetComponent<Rigidbody>();
        flightDeckT = flightDeck.transform;
        playerName.enabled = false;
        this.tag = "Player";

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
        droneSound.pitch = 1f + liftSpinAxis.y / 1.5f;
        PlayerInputActions();
        NonPlayerCalculations();
    }

    void PlayerInputActions()
    {
        //////////////////////////////////////////////////////////////////////////////////
        ///                                 tilt                                       ///
        //////////////////////////////////////////////////////////////////////////////////
        InputDevice tiltDevice = InputDevices.GetDeviceAtXRNode(tiltInputController);
        tiltDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out tiltAxis);

        if (Mathf.Abs(tiltAxis.y) > tiltDeadzone)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(tiltMaxAngle * tiltAxis.y, flightDeckT.rotation.eulerAngles.y, flightDeckT.rotation.eulerAngles.z), Time.deltaTime * tiltLerpSpeed);

        if (Mathf.Abs(tiltAxis.x) > tiltDeadzone)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y, -tiltMaxAngle * tiltAxis.x), Time.deltaTime * tiltLerpSpeed);


        //////////////////////////////////////////////////////////////////////////////////
        ///                              lift and spin                                 ///
        //////////////////////////////////////////////////////////////////////////////////
        InputDevice liftSpinDevice = InputDevices.GetDeviceAtXRNode(liftSpinInputController);
        liftSpinDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out liftSpinAxis);

        //lift
        if (liftSpinAxis.y >= liftDeadzone)
            masterThrust = hoverThrust + masterThrustCopy * liftSpinAxis.y;
        else
        {
            masterThrust = 0f;
            droneSound.pitch = 0.8f;
        }

        //spin
        if (Mathf.Abs(liftSpinAxis.x) > spinDeadzone)
            flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y + liftSpinAxis.x * spinMultiplier, flightDeckT.rotation.eulerAngles.z);


        //////////////////////////////////////////////////////////////////////////////////
        ///                             settings button                                ///
        //////////////////////////////////////////////////////////////////////////////////
        liftSpinDevice.TryGetFeatureValue(CommonUsages.menuButton, out settingsOn);
        if (settingsOn)
            settingsMenu.GetComponent<SettingsPanel>().setActive();
    }

    void NonPlayerCalculations()
    {
        //if not controlling, level out tilt
        if (tiltAxis.x < tiltDeadzone && tiltAxis.x > -tiltDeadzone && tiltAxis.y < tiltDeadzone && tiltAxis.y > -tiltDeadzone)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(0, flightDeckT.rotation.eulerAngles.y, 0), Time.deltaTime * stabilizationSpeed);

        //keep elevation stable while tilting if desired
        if (elevationStabilization)
            hoverThrust = ((9.81f / 0.5f) / Mathf.Cos(Vector3.Angle(-flightDeckT.up, Vector3.down) * Mathf.Deg2Rad));
        else
            hoverThrust = (9.81f / 0.5f);

        //do thrust after all the above calculations
        thrusterThrust.force = masterThrust * flightDeckRB.mass;
    }


}
