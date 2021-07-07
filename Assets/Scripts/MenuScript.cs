using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject menuVRRig;

    public void PlayOnline()
    {
        SceneManager.LoadScene("Demo Scene");
    }

    public void Training()
    {
        SceneManager.LoadScene("Downtown Scene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}
