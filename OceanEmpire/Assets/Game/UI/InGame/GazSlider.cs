﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GazSlider : MonoBehaviour
{
    public Slider gageMeter;
    public Image bg;

    private bool startAlert = false;

    void Start()
    {
        UpdateMeter();
    }

    void Update()
    {
        UpdateMeter();
        if (gageMeter.value < 0.2f)
        {
            Alerte();
        }
    }

    public void UpdateMeter()
    {
        SubmarinParts parts;
        if (Game.Instance != null && (parts = Game.Instance.SubmarinParts) != null && parts.GazTank != null)
        {
            gageMeter.value = parts.GazTank.GetGazRatio();
        }
        else
        {
            gageMeter.value = 1;
        }
    }

    public void Alerte()
    {
        if (startAlert)
            return;

        startAlert = true;
        bg.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
