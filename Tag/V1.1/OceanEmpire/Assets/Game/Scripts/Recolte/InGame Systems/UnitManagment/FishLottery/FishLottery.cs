﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLottery : MonoBehaviour
{
    public MapLayout MapLayout { get; private set; }

    [Header("Settings")]
    public AnimationCurve generalDensity = new AnimationCurve(new Keyframe(0, .7f), new Keyframe(1, .7f));
    public float densityMultiplier = 1;
    public List<FishRepartition> fishRepartitions = new List<FishRepartition>();

    private Lottery<GameObject> lottery;

    void Awake()
    {
        RebuildLottery();
    }

    public void Init(MapLayout mapLayout)
    {
        MapLayout = mapLayout;
    }

    void RebuildLottery()
    {
        lottery = new Lottery<GameObject>(fishRepartitions.Count);
    }


    public GameObject DrawAtHeight(float height)
    {
        return DrawAtPosition01(MapLayout.GetMapPosition01(height));
    }
    public GameObject DrawAtPosition01(float pos01)
    {
        pos01 = Mathf.Clamp(pos01, 0, 1);
        lottery.Clear();

        for (int i = 0; i < fishRepartitions.Count; i++)
        {
            FishRepartition repartition = fishRepartitions[i];

            // Too deep or too shallow ?
            if (pos01 < repartition.shallowestSpawn || pos01 > repartition.deepestSpawn)
                continue;

            float depthRatio = (pos01 - repartition.shallowestSpawn) / (repartition.deepestSpawn - repartition.shallowestSpawn);
            float fishProportion = repartition.populationCurve.Evaluate(depthRatio) * repartition.weight;

            lottery.Add(repartition.prefab, Mathf.Max(0, fishProportion));
        }

        if (lottery.Count > 0)
            return lottery.Pick();
        else
            return null;
    }

    public float GetDensityAt(float height)
    {
        return GetDensityAt01(MapLayout.GetMapPosition01(height));
    }

    public float GetDensityAt01(float position01)
    {
        return generalDensity.Evaluate(position01) * densityMultiplier;
    }
}
