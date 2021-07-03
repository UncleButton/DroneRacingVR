using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlipOverText : MonoBehaviour
{
    public GameObject drone;
    public Text flipOverText;

    private void Update()
    {
        if ((drone.transform.rotation.eulerAngles.x > 100 && drone.transform.rotation.eulerAngles.x < 260) || (drone.transform.rotation.eulerAngles.z > 100 && drone.transform.rotation.eulerAngles.z < 260))
            flipOverText.enabled = true;
         else
            flipOverText.enabled = false;
    }

}
