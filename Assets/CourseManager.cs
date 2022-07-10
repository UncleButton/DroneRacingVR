﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseManager : MonoBehaviour
{
    public bool isMultiplayer;
    
    //singleplayer
    public GameObject timeTrialManager;
    public GameObject singlePlayerDrone;

    //multiplayer
    public GameObject networkManager;
    public GameObject join_hostMenu;
    void Start()
    {
        if (isMultiplayer)
        {
            Instantiate(networkManager);
            Instantiate(join_hostMenu);
        }
        else
        {
            Instantiate(timeTrialManager);
            Instantiate(singlePlayerDrone);
        }
    }
}
