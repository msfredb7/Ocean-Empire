﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : BaseFish {

    public void OnPlayerTouch()
    {
        Game.PlayerStats.TryCapture(this);
        //Capture();
    }

    public void Update()
    {
        //print("Hello world!")
    }

}
