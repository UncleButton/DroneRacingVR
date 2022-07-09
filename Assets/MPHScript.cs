using UnityEngine;

public class MPHScript : MonoBehaviour
{
    public UnityEngine.UI.Text txt;
    public Rigidbody DroneRB;

    // Update is called once per frame
    void Update()
    {
        double metersPerSecond = Mathf.RoundToInt(DroneRB.velocity.magnitude);
        double kmPerHour = metersPerSecond * 0.360;
        //double milesPerHour = kmPerHour * 0.621371;
        txt.text = "KPH: " + Mathf.RoundToInt((float)kmPerHour);
    }
}
