using UnityEngine;

public class LapTextScript : MonoBehaviour
{
    public UnityEngine.UI.Text txt;
    public LapManager lm;

    private void Start()
    {
        lm = GameObject.FindGameObjectWithTag("LapManager").GetComponent<LapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Lap: " + lm.lapNum + "/" + lm.lapsRequired;
    }
}
