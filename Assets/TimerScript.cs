using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
    }
}
