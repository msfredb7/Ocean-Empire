﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    [Header("Environement")]
    [SerializeField] Shack_Environment shack_Environment;

    [Header("Recolte")]
    [SerializeField] FishingFrenzyWidget fishingFrenzyWidget;
    [SerializeField] Shack_CallToAction recolteCallToAction;
    [SerializeField] Shack_MapManager shack_MapManager;

    [Header("Calendar")]
    [SerializeField]
    SceneInfo calendarScene;

    public void OpenCalendar()
    {
        Scenes.Load(calendarScene, (scene) =>
        {
            scene.FindRootObject<CalendarScroll_Controller>().OnEntranceComplete(() => Scenes.UnloadAsync(gameObject.scene));
        });
    }

    void OnEnable()
    {
        CheckFishingFrenzy();
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.AddListener(CheckFishingFrenzy);

        shack_MapManager.OnChangeMap += OnMapChange;
    }

    private void OnMapChange(MapData obj)
    {
        shack_Environment.SetSeveral(obj);
    }

    void OnDisable()
    {
        if (fishingFrenzyWidget != null)
            fishingFrenzyWidget.OnStateUpdated.RemoveListener(CheckFishingFrenzy);

        if (shack_MapManager != null)
            shack_MapManager.OnChangeMap -= OnMapChange;
    }

    void CheckFishingFrenzy()
    {
        recolteCallToAction.enabled = FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available;
    }

    public void LaunchGame()
    {
        var mapData = shack_MapManager.GetMapData();
        GameSettings gameSettings = new GameSettings(mapData, true);

        if (FishingFrenzy.Instance != null && FishingFrenzy.Instance.State == FishingFrenzy.EffectState.Available)
        {
            FishingFrenzy.Instance.Activate();
        }
        LoadingScreen.TransitionTo(GameBuilder.SCENENAME, new ToRecolteMessage(gameSettings), true);
    }
}
