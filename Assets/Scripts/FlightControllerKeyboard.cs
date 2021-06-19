using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MLAPI;

public class FlightControllerKeyboard : NetworkBehaviour
{

    public GameObject flightDeck;
    public Transform flightDeckT;
    private Rigidbody flightDeckRB;
    public Transform cameraTransform;

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
    public float spinPerFrame = 0.5f;
    public float hoverThrust = 9.81f / 4f;
    public float tiltLerpSpeed = 2f;
    public float tiltMaxAngle = 30f;

    public float posXThrust = 0f;
    public float negXThrust = 0f;
    public float posZThrust = 0f;
    public float negZThrust = 0f;
    // Start is called before the first frame update
    void Start()
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
        }
        else
        {
            cameraTransform.GetComponent<AudioListener>().enabled = false;
            cameraTransform.GetComponent<Camera>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            droneMovement();
        }
            
    }

    void droneMovement()
    {
        posXThrust = 0f;
        negXThrust = 0f;
        posZThrust = 0f;
        negZThrust = 0f;

        //tilting left/right/forward/backward
        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["w"]).isPressed)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(tiltMaxAngle, flightDeckT.rotation.eulerAngles.y, flightDeckT.rotation.eulerAngles.z), Time.deltaTime * tiltLerpSpeed);
        else if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["s"]).isPressed)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(-tiltMaxAngle, flightDeckT.rotation.eulerAngles.y, flightDeckT.rotation.eulerAngles.z), Time.deltaTime * tiltLerpSpeed);
        else
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(0, flightDeckT.rotation.eulerAngles.y, flightDeckT.rotation.eulerAngles.z), Time.deltaTime * tiltLerpSpeed);

        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["d"]).isPressed)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y, -tiltMaxAngle), Time.deltaTime * tiltLerpSpeed);
        else if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["a"]).isPressed)
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y, tiltMaxAngle), Time.deltaTime * tiltLerpSpeed);
        else
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y, 0), Time.deltaTime * tiltLerpSpeed);


        //if not controlling, level out
        if (!((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["w"]).isPressed
            && !((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["a"]).isPressed
            && !((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["s"]).isPressed
            && !((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["d"]).isPressed)
        {
            flightDeckT.rotation = Quaternion.Lerp(flightDeckT.rotation, Quaternion.Euler(0, flightDeckT.rotation.eulerAngles.y, 0), Time.deltaTime * stabilizationSpeed);
        }

        //spinning
        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current[KeyCode.RightArrow.ToString()]).isPressed)
            flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y + spinPerFrame, flightDeckT.rotation.eulerAngles.z);

        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current[KeyCode.LeftArrow.ToString()]).isPressed)
            flightDeckT.rotation = Quaternion.Euler(flightDeckT.rotation.eulerAngles.x, flightDeckT.rotation.eulerAngles.y - spinPerFrame, flightDeckT.rotation.eulerAngles.z);

        //lift based on lift joystick
        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current[KeyCode.UpArrow.ToString()]).isPressed)
            masterThrust = hoverThrust + masterThrustCopy;
        else if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current[KeyCode.DownArrow.ToString()]).isPressed)
            masterThrust = hoverThrust - masterThrustCopy;
        else
            masterThrust = hoverThrust;


        //do thrust after all the above calculations
        thrusterPosXPosZThrust.force = (masterThrust + posXThrust + posZThrust) * flightDeckRB.mass;
        thrusterNegXNegZThrust.force = (masterThrust + negXThrust + negZThrust) * flightDeckRB.mass;
        thrusterPosXNegZThrust.force = (masterThrust + posXThrust + negZThrust) * flightDeckRB.mass;
        thrusterNegXPosZThrust.force = (masterThrust + negXThrust + posZThrust) * flightDeckRB.mass;

        if (((UnityEngine.InputSystem.Controls.KeyControl)Keyboard.current["r"]).isPressed)
        {
            flightDeckT.position = new Vector3(0, 3, 0);
            flightDeckT.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            SceneManager.LoadScene("Demo Scene");
        }
    }
}
