﻿using System;
using UnityEngine;

[RequireComponent(typeof(FishInfo)), DisallowMultipleComponent]
public class Capturable : MonoBehaviour
{
    public delegate void CapturableEvent(Capturable capturable);

    public event CapturableEvent OnAllCaptures;
    public event CapturableEvent OnNextCapture;
    [NonSerialized] public FishInfo info;

    void Awake()
    {
        info = GetComponent<FishInfo>();
    }

    public void Capture()
    {
        // Raise Events
        if (OnNextCapture != null)
            OnNextCapture(this);
        OnNextCapture = null;

        if (OnAllCaptures != null)
            OnAllCaptures(this);

        // Kill
        var killable = GetComponent<BaseKillableUnit>();
        if (killable != null)
            killable.Kill();
    }

    public void ClearLifeEvents()
    {
        OnNextCapture = null;
    }
}