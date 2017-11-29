using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public class FishPopulation : BaseManager<FishPopulation>
{

    private const string SAVE_KEY_POPULATION = "population";

    private const float limitPopulation = 150;
    private TimeSpan refreshingTime = new TimeSpan(0, 0, 30, 0);

    private float population;
    private DateTime lastUpdate;

    private float time = 0;

    public bool periodicLogs = false;

    public void Update()
    {
        if (!periodicLogs)
            return;

        if (time < 0)
        {
            print(population);
            time = 4;
        }
        time -= Time.deltaTime;
    }



    public override void Init()
    {
        population = GameSaves.instance.GetFloat(GameSaves.Type.FishPop, SAVE_KEY_POPULATION, limitPopulation);
        lastUpdate = GameSaves.instance.GetDateTime(GameSaves.Type.FishPop, SAVE_KEY_POPULATION, System.DateTime.Now);

        CompleteInit();
    }

    private static void Save()
    {
        GameSaves.instance.SetFloat(GameSaves.Type.FishPop, SAVE_KEY_POPULATION, Population);
        GameSaves.instance.SetDateTime(GameSaves.Type.FishPop, SAVE_KEY_POPULATION, LastUpdate);

        GameSaves.instance.SaveData(GameSaves.Type.FishPop);
    }

    /// <summary>
    /// 0 = 0%      1 = 100%
    /// </summary>
    public void AddRate(float value)
    {
        PopulationRate += value;
    }

    public float FishNumberToRate(float fishes)
    {
        return fishes / limitPopulation;
    }

    public static float PopulationRate
    {
        private set { Population = value * limitPopulation; }
        get { return Population / limitPopulation; }
    }

    public static float Population
    {
        private set
        {
            instance.population = value.Clamped(0, limitPopulation);
        }
        get { return instance.population; }
    }

    public static DateTime LastUpdate
    {
        private set { instance.lastUpdate = value; }
        get { return instance.lastUpdate; }

    }

    public void RefreshPopulation()
    {
        DateTime now = System.DateTime.Now;

        TimeSpan deltaTime = now.Subtract(LastUpdate);
        float refreshRate = (float)( deltaTime.TotalSeconds / refreshingTime.TotalSeconds );

        population = (population += (refreshRate * limitPopulation)).Capped(limitPopulation);
        LastUpdate = now;
    }


    void OnApplicationQuit()
    {
        RefreshPopulation();
        Save();
    }

    public void UpdateOnFishing(float capturedFishes)
    {
        LastUpdate = System.DateTime.Now;
        Population -= capturedFishes.Raised(0.0f);

        Save();
    }



    public void UpdateOnExercise(float exerciseRateValue)
    {
        RefreshPopulation();
        PopulationRate += exerciseRateValue.Capped(limitPopulation);

        Save();
    }

    public void DebugInGameExercice()
    {
        UpdateOnExercise(50);
        if (Game.instance != null)
            if (Game.FishSpawner != null)
                Game.FishSpawner.SetPalierFishLimit();
    }

    public static void Reload()
    {
        instance.Init();
    }
}
