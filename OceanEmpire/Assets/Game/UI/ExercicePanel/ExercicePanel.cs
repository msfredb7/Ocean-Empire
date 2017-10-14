﻿using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExercicePanel : WindowAnimation
{
    public const string SCENENAME = "ExercicePanel";

    public void Quit()
    {
        Close(() => Scenes.UnloadAsync(SCENENAME));
    }
}