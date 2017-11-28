﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ToRecolteMessage : SceneMessage
{
    private MapDescription mapDescription;

    public ToRecolteMessage(MapDescription mapDescription)
    {
        this.mapDescription = mapDescription;
    }

    public void OnLoaded(Scene scene)
    {
        scene.FindRootObject<GameBuilder>().Init(mapDescription.sceneToLoad);
    }

    public void OnOutroComplete()
    {

    }
}