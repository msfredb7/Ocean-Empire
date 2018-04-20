﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack_MapManager : MonoBehaviour
{
    [SerializeField] Text mapNameText;
    [SerializeField] DataSaver dataSaver;
    [SerializeField] PrebuiltMapData _defaultMapData;
    [ReadOnly, SerializeField] int _mapIndex;
    public int MapIndex { get { return _mapIndex; } private set { _mapIndex = value; } }

    private const string SAVEKEY_MAPDATA = "MapData";
    private const string SAVEKEY_MAPINDEX = "MapIndex";
    private const string PREBUILT_MAPDATA_NAME = "Maps/PrebuiltMapData_";
    public MapData MapData { get; private set; }

    public event Action<int, MapData> OnMapSet = (x, y) => { };

    void OnEnable()
    {
        dataSaver.OnReassignData += Pull;

        if (!dataSaver.HasEverLoaded)
            dataSaver.LateLoad();
        else
            Pull();
    }

    void OnDisable()
    {
        dataSaver.OnReassignData -= Pull;
    }

    #region Data Saver Interactions
    void Pull()
    {
        MapIndex = dataSaver.GetInt(SAVEKEY_MAPINDEX);
        MapData = (MapData)dataSaver.GetObjectClone(SAVEKEY_MAPDATA);

        if (MapData == null)
            MapData = GetMapDataFromIndex(MapIndex);

        OnMapSet(MapIndex, MapData);
    }
    void PushAndSave()
    {
        dataSaver.SetObjectClone(SAVEKEY_MAPDATA, MapData);
        dataSaver.SetInt(SAVEKEY_MAPINDEX, MapIndex);
        dataSaver.LateSave();
    }
    #endregion
    
    public void SetMap(int mapIndex)
    {
        // Get Data
        MapIndex = mapIndex;
        MapData = GetMapDataFromIndex(MapIndex);

        if (mapNameText != null)
        {
            mapNameText.text = MapData.Name;
            mapNameText.enabled = true;
        }

        // Save to disc
        PushAndSave();

        // Event
        OnMapSet(MapIndex, MapData);
    }
    public void SetMap_Next()
    {
        SetMap(MapIndex + 1);
    }
    public void SetMap_Previous()
    {
        SetMap(MapIndex - 1);
    }

    private MapData GetMapDataFromIndex(int index)
    {
        // Load from path. If null, pick default

        var path = PREBUILT_MAPDATA_NAME + index.ToString();
        var prebuiltMapData = Resources.Load<PrebuiltMapData>(path);
        if (prebuiltMapData == null)
        {
            Debug.LogWarning("Aucune ressource nommée: " + path + ". Normalement, on génèrerait une map avec un algo," +
                " mais pour l'instant, nous allons prendre la map par défaut");
            return _defaultMapData.MapData;
        }
        else
        {
            return prebuiltMapData.MapData;
        }
    }
}
