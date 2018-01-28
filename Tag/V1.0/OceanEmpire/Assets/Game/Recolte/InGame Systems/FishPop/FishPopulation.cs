using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using FullInspector;

public class FishPopulation : BaseManager<FishPopulation>
{

    private const string SAVE_KEY_POPULATION = "population";

    [SerializeField, ReadOnly]
    private const float limitPopulation = 200;
    private TimeSpan refreshingTime = new TimeSpan(0, 0, 80, 0);

    [InspectorTooltip("Densit� decroit exponentiellement en fonction du nombre")]
    public float PopulationDensityVariation = 4;

    [SerializeField, ReadOnly]
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

    public static TimeSpan GetTimeToRefill()
    {
        return new TimeSpan( (long)((float)instance.refreshingTime.Ticks * (1.0f - PopulationRate)) );
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
        PopulationRate = Mathf.Min(value + PopulationRate, 1);
    }

    public float FishNumberToRate(float fishes)
    {
        return fishes / limitPopulation;
    }

    public static float PopulationRate
    {
        private set { Population = value * limitPopulation ; }
        get { return Population / limitPopulation; }
    }

    public static float FishDensity
    {
        get { return Mathf.Pow(FishPopulation.PopulationRate, instance.PopulationDensityVariation); }
    }

    public static float GetFishDensityFromRate(float rate)
    {
       return Mathf.Pow(rate, instance.PopulationDensityVariation); 
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

    public static void LowerRate(float value)
    {
        PopulationRate = (PopulationRate - value).Raised(0);
    }

    public void RefreshPopulation()
    {
        DateTime now = System.DateTime.Now;

        TimeSpan deltaTime = now - LastUpdate;
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