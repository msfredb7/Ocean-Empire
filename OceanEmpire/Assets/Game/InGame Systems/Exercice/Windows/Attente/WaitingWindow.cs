﻿using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingWindow : MonoBehaviour {

    public const string SCENE_NAME = "WaitWindow";

    public Text title;
    public Text exercice;
    public Text enAttente;
    public WaitAnimation waitAnimation;
    public WindowAnimation windowAnim;
    public bool debug = false;

    private Action onCompleteEvent;

    void Start()
    {
        if (debug)
            InitDisplay("À faire: Courrir ta vie");
    }

    public void InitDisplay(string exerciceDescription, Action onComplete = null, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        this.title.text = title;
        exercice.text = exerciceDescription;
        this.enAttente.text = enAttente;
        onCompleteEvent = onComplete;
        windowAnim.Open(delegate() {
            waitAnimation.DoAnimation();
        });
    }

    public void Hide()
    {
        windowAnim.Close();
        Scenes.UnloadAsync(SCENE_NAME);
        if(onCompleteEvent != null)
            onCompleteEvent.Invoke();
    }
    
	public void DebugComplete()
    {
        Hide();
    }
}